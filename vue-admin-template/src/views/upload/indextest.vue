<template>
  <uploader :options="options"
            class="uploader-example"
            ref="uploader"
            :autoStart="false"
            @file-added="onFileAdded"
            @file-success="onFileSuccess"
            @file-progress="onFileProgress"
            @file-error="onFileError"
            style="width: 100%;">
    <uploader-unsupport></uploader-unsupport>
    <uploader-drop>
      <p>欢迎来到上传界面</p>
      <uploader-btn :attrs="attrs">选择文件</uploader-btn>
      <uploader-btn :attrs="attrs">选择图片</uploader-btn>
      <uploader-btn :attrs="attrs" :directory="true">选择目录</uploader-btn>
    </uploader-drop>
    <uploader-list>
      <ul class="file-list" slot-scope="props">
        <li v-for="file in props.fileList" :key="file.id">
          <uploader-file :class="'file_' + file.id" ref="files" :file="file" :list="true"></uploader-file>
        </li>
        <div class="no-file" v-if="!props.fileList.length"><i class="nucfont inuc-empty-file"></i> 暂无待上传文件</div>
      </ul>
    </uploader-list>
    <div class="login-container">
      <el-button type="primary" style="width:100%;margin-bottom:30px;" @click.native.prevent="testdownload">下载</el-button>
    </div>
    <div id="dcontent" class="dcontent">
      <el-button type="primary" style="width:100%;margin-bottom:30px;" @click.native.prevent="testdownload2">下载2222</el-button>
      <br />
      <el-progress :text-inside="true" :stroke-width="24" :percentage="percentage" status="success"></el-progress>
    
      <el-button type="primary" style="width:100%;margin-bottom:30px;" @click.native.prevent="convertFile">合成文件流</el-button>
      <br />
    </div>
  </uploader>

</template>
<script>
  import SparkMD5 from 'spark-md5'
  import apiUploadService from '@/api/upload'
  import indexDBService from '@/store/indexdb'
  let { chunkUploadUrl, mergeFiles, getMD5ToBurstData, downloadBigFile, downloadBigFile2 } = apiUploadService
  let { add, queryDataBymd5, queryCount, mergeFileStream, requestFileStreamArrs, requestDB } = indexDBService

  export default {
    data() {
      return {
        recording: {},
        options: {
          target: chunkUploadUrl,
          testChunks: true, //校验
          chunkSize: '1024000',//分片的大小 1MB
          simultaneousUploads: 3,//并发个数
          fileParameterName: 'file', //上传文件时文件的参数名，默认file
          maxChunkRetries: 3,  //最大自动失败重试上传次数
          // 服务器分片校验函数，秒传及断点续传基础
          checkChunkUploadedByResponse: function (chunk, res) {
            let d = JSON.parse(res);
            if (d.flag) {
              var data = d.data;
              if (data.secondTransmission) {
                return true;
              } else {
                return (data.uploaded || []).indexOf(chunk.offset + 1) >= 0
              }
            }
          },
          //显示还剩余多少秒
          parseTimeRemaining: function (timeRemaining, parsedTimeRemaining) {
            return parsedTimeRemaining
              .replace(/\syears?/, '年')
              .replace(/\days?/, '天')
              .replace(/\shours?/, '小时')
              .replace(/\sminutes?/, '分钟')
              .replace(/\sseconds?/, '秒')
          }

        },
        attrs: {
          // 接受的文件类型，形如['.png', '.jpg', '.jpeg', '.gif', '.bmp'...] 这里我封装了一下
          accept: {
            image: ['gif', 'jpg', 'jpeg', 'png', 'bmp', 'webp'],
            video: ['mp4', 'm3u8', 'rmvb', 'avi', 'swf', '3gp', 'mkv', 'flv'],
            audio: ['mp3', 'wav', 'wma', 'ogg', 'aac', 'flac'],
            document: ['doc', 'txt', 'docx', 'pages', 'epub', 'pdf', 'numbers', 'csv', 'xls', 'xlsx', 'keynote', 'ppt', 'pptx']
          }
        },
        downloadmd5: '',
        percentage: 0,
        colors: [
          { color: '#f56c6c', percentage: 20 },
          { color: '#e6a23c', percentage: 40 },
          { color: '#5cb87a', percentage: 60 },
          { color: '#1989fa', percentage: 80 },
          { color: '#6f7ad3', percentage: 100 }
        ]
      };
    },
    methods: {
      //选择文件后，将上传的窗口展示出来，开始md5的计算工作
      onFileAdded(file) {
        // 计算MD5，下文会提到
        this.computeMD5(file);
      },
      // 文件进度的回调
      onFileProgress(rootFile, file, chunk) {
        console.log(`上传中 ${file.name}，chunk：${chunk.startByte / 1024 / 1024} ~ ${chunk.endByte / 1024 / 1024}`)
      },
      //上传成功的事件
      onFileSuccess(rootFile, file, response, chunk) {
        let res = JSON.parse(response);
        // 服务器自定义的错误，这种错误是Uploader无法拦截的
        if (!res.flag) {
          this.$message({ message: res.message, type: 'error' });
          // 文件状态设为“失败”
          this.statusSet(file.id, 'error');
          return
        }
        var d = res.data;
        // 如果服务端返回需要合并
        if (d.needMerge) {
          var param = {
            Identifier: d.identifier,
            FileName: d.fileName,
            TotalSize: file.size,
            FileType: "",
            FileExt: d.fileName.substring(d.fileName.lastIndexOf('.') + 1)

          }
          // 文件状态设为“合并中”
          this.statusSet(file.id, 'merging');
          mergeFiles(param).then(response => {
            this.statusSet(file.id, "success");
          });
        } else {
          this.statusSet(file.id, "success");
        }
      },
      onFileError(rootFile, file, response, chunk) {
        console.log(response)
      },
      /**
      * 计算md5，实现断点续传及秒传
      * @param file
      */
      computeMD5(file) {
        let fileReader = new FileReader();
        let time = new Date().getTime();
        let blobSlice = File.prototype.slice || File.prototype.mozSlice || File.prototype.webkitSlice;
        let currentChunk = 0;
        const chunkSize = 10 * 1024 * 1000;
        let chunks = Math.ceil(file.size / chunkSize);
        let spark = new SparkMD5.ArrayBuffer();
        // 文件状态设为"计算MD5"
        this.statusSet(file.id, 'md5');
        //文件暂停
        file.pause();
        loadNext();
        fileReader.onload = (e => {
          spark.append(e.target.result);
          if (currentChunk < chunks) {
            currentChunk++;
            loadNext();
            // 实时展示MD5的计算进度
            this.$nextTick(() => {
              $('.file_' + file.id).find(".uploader-file-status span:eq(0)").text('校验MD5 ' + ((currentChunk / chunks) * 100).toFixed(0) + '%');
              //不可以点击上传
              $('.file_' + file.id).find(".uploader-file-actions .uploader-file-resume").addClass("hide");
            })
          } else {
            let md5 = spark.end();
            //可以有上传的按钮
            $('.file_' + file.id).find(".uploader-file-actions .uploader-file-resume").removeClass("hide");
            this.computeMD5Success(md5, file);
            //console.log(`MD5计算完毕：${file.name} \nMD5：${md5} \n分片：${chunks} 大小:${file.size} 用时：${new Date().getTime() - time} ms`);
          }
        });
        fileReader.onerror = function () {
          // this.error(`文件${file.name}读取出错，请检查该文件`)
          file.cancel();
        };
        function loadNext() {
          let start = currentChunk * chunkSize;
          let end = ((start + chunkSize) >= file.size) ? file.size : start + chunkSize;
          fileReader.readAsArrayBuffer(blobSlice.call(file.file, start, end));
        }
      },
      computeMD5Success(md5, file) {
        // 将自定义参数直接加载uploader实例的opts上
        globalThis.uploader.opts.query = {
          //  query: {}
        }
        file.uniqueIdentifier = md5;
        file.resume();
      },
      /**
      * 新增的自定义的状态: 'md5'、'transcoding'、'failed'
      * @param id
      * @param status
      */
      statusSet(id, status) {
        let statusMap = {
          md5: {
            text: '校验MD5',
            bgc: '#fff'
          },
          merging: {
            text: '合并中',
            bgc: '#ac3ae6'
          },
          transcoding: {
            text: '转码中',
            bgc: '#e2eeff'
          },
          uploading: {
            text: '上传中',
            bgc: '#e2eeff'
          },
          waiting: {
            text: '等待',
            bgc: '#e2eeff'
          },
          paused: {
            text: '暂停',
            bgc: '#61affe'
          },
          error: {
            text: '上传失败',
            bgc: '#f56c6c'
          },
          success:
          {
            text: '上传成功',
            bgc: '#49cc90'
          }
        }
        this.$nextTick(() => {
          $('.file_' + id).find(".uploader-file-status span:eq(0)").css({
            'position': 'absolute',
            'top': '0',
            'left': '0',
            'right': '0',
            'bottom': '0',
            'zIndex': '1',
            'color': 'white',
            'background': statusMap[status].bgc
          }).text(statusMap[status].text);
        })
      },
      //测试下载大文件
      testdownload() {
        downloadBigFile().then(data => {
          console.log(data);
          // 文件名
          let fileName = "6767";
          let blob = new Blob([data.data]);
          if (window.navigator.msSaveOrOpenBlob) {
            navigator.msSaveBlob(blob);
          } else {
            let elink = document.createElement("a");
            elink.download = fileName;
            elink.style.display = "none";
            elink.href = URL.createObjectURL(blob);
            document.body.appendChild(elink);
            elink.click();
            document.body.removeChild(elink);
          }
          //console.log(res)
        });
      },
      testdownload2() {

        let $this = this;
        
        //拆分信息
        getMD5ToBurstData().then(res => {
        //  console.log(res.data);
          var d = res.data;
          $this.downloadmd5 = d.identifier;
          let totalSize = d.fileRanges.length;
          //进度条
         var t= setInterval(function () {
            //计算进度条
           queryCount(requestDB, function (count) {
             let p = (count/totalSize).toFixed(2);
            // console.log(p);
             $this.percentage = parseInt(p*100);
            // console.log(this.percentage);
             if ($this.percentage === 100) {
               clearInterval(t);
               //合并数据 
               $this.convertFile();
             }

           });
          
          }, 1000);
        

          //首先从缓存取分片流 没有的话再去请求后台流数据
          queryDataBymd5(requestDB, this.downloadmd5, function (arrs) {
            var fileRanges = requestFileStreamArrs(arrs, d.fileRanges);
            $.each(fileRanges, function (i, ee) {
              //请求分片
                downloadBigFile2(ee.range).then(res2 => {
                // console.log(res2)
                //存储数据流
                if (requestDB != null && res2.status == 206) {
                  var dd = {
                    index: ee.sliceNumber,
                    md5: d.identifier,
                    range: ee.range,
                    content: res2.data,
                  };
                  add(requestDB, dd);
                }
              });
            });
          })
        });
      },
      convertFile() {
        //查询数据 合并文件流
        queryDataBymd5(requestDB, this.downloadmd5, function (d) {
          mergeFileStream(d, "12334");
        });
      }
    },
    mounted() {
      // 获取uploader对象
      this.$nextTick(() => {
        window.uploader = this.$refs.uploader.uploader;
      });
    }
  };
</script>

<style>
  .uploader-example {
    width: 100%;
    padding: 15px;
    margin: 50px auto 0;
    font-size: 12px;
    box-shadow: 0 0 10px rgba(0, 0, 0, 0.4);
  }

    .uploader-example .uploader-btn {
      margin-right: 4px;
    }

    .uploader-example .uploader-list {
      max-height: 440px;
      overflow: auto;
      overflow-x: hidden;
      overflow-y: auto;
    }

  .hide {
    display: none !important;
  }
</style>
