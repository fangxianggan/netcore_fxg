<template>
  <div class="app-container">
    <div class="filter-container">
      <el-collapse accordion v-model="activeName">
        <el-collapse-item title="查询条件" name="1">
          <el-form :inline="true" :model="filterModel">
            <el-form-item label="配置名称">
              <el-input
                v-model="filterModel.value.value"
                placeholder="配置名称"
                class="filter-item"
                @keyup.enter.native="handleFilter"
              />
            </el-form-item>
            <el-form-item label="添加时间">
              <el-date-picker
                v-model="filterModel.createTime.value"
                :picker-options="daterangeOptions"
                value-format="yyyy-MM-dd"
                range-separator="至"
                start-placeholder="开始时间"
                end-placeholder="结束时间"
                style="width: 250px;"
                type="daterange"
                placement="bottom-end"
                @keyup.enter.native="handleFilter"
              />
            </el-form-item>
          </el-form>
        </el-collapse-item>
      </el-collapse>
    </div>
    <el-row class="el-table-header-buttom">
      <el-button
        v-waves
        class="filter-item"
        type="primary"
        icon="el-icon-search"
        @click="handleFilter"
      >查询</el-button>

      <el-button class="filter-item" type="primary" icon="el-icon-edit" @click="handleUpload">上传</el-button>
    </el-row>
    <el-table
      :key="tableKey"
      v-loading="listLoading"
      :data="list"
      border
      fit
      highlight-current-row
      style="width: 100%;"
      @sort-change="sortChange"
    >
      <el-table-column label="文件名称" prop="fileName" sortable="custom" width="180">
        <template slot-scope="{row}">
          <span>{{ row.fileName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="文件分类" prop="fileCategory" sortable="custom" width="180">
        <template slot-scope="{row}">
          <span>{{ row.fileCategory }}</span>
        </template>
      </el-table-column>
      <el-table-column label="文件类型" prop="fileType" sortable="custom" width="180">
        <template slot-scope="{row}">
          <span>{{ row.fileType }}</span>
        </template>
      </el-table-column>
      <el-table-column label="文件大小" prop="fileBytes" sortable="custom" width="180px">
        <template slot-scope="{row}">
          <span>{{ row.fileBytes}}</span>
        </template>
      </el-table-column>
      <el-table-column label="上传时间" width="180px">
        <template slot-scope="{row}">
          <span>{{ row.uploadTime|formatTime}}</span>
        </template>
      </el-table-column>
      <el-table-column label="上传人" min-width="100px">
        <template slot-scope="{row}">
          <span>{{ row.uploader }}</span>
        </template>
      </el-table-column>

      <el-table-column label="操作" width="250" class-name="small-padding fixed-width">
        <template slot-scope="{row,$index}">
          <el-button
            type="success"
            v-if="row.fileCategory!='document'"
            size="mini"
            @click="handlePreview(row)"
          >预览</el-button>
          <el-button type="primary" size="mini" @click="handleDown(row)">下载</el-button>

          <el-button
            v-if="row.status!='deleted'"
            size="mini"
            type="danger"
            @click="handleDelete(row,$index)"
          >删除</el-button>
        </template>
      </el-table-column>
    </el-table>

    <pagination
      v-show="total>0"
      :total="total"
      :page.sync="listQuery.page"
      :limit.sync="listQuery.limit"
      @pagination="getList"
    />

    <el-dialog
      v-el-drag-dialog
      :title="dialogTitle"
      :visible.sync="dialogFormVisible"
      :close-on-click-modal="false"
      fit
      width="60%"
      :destroy-on-close="true"
      :fullscreen="false"
    >
      <uploader
        :options="options"
        class="uploader-example"
        ref="uploader"
        :autoStart="false"
        @file-added="onFileAdded"
        @file-success="onFileSuccess"
        @file-progress="onFileProgress"
        @file-error="onFileError"
        style="width: 100%;"
      >
        <uploader-unsupport></uploader-unsupport>
        <uploader-drop>
          <uploader-btn :attrs="attrs">选择文件</uploader-btn>
          <!--<uploader-btn :attrs="attrs">选择图片</uploader-btn>
          <uploader-btn :attrs="attrs" :directory="true">选择目录</uploader-btn>-->
        </uploader-drop>
        <uploader-list>
          <ul class="file-list" slot-scope="props">
            <li v-for="file in props.fileList" :key="file.id">
              <uploader-file :class="'file_' + file.id" ref="files" :file="file" :list="true"></uploader-file>
            </li>
          </ul>
        </uploader-list>
      </uploader>

      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogFormVisible = false">关闭</el-button>
      </div>
    </el-dialog>

    <el-dialog
      v-el-drag-dialog
      :title="dialogprogressTitle"
      :visible.sync="dialogprogressVisible"
      :show-close="false"
      :close-on-click-modal="false"
      fit
      width="60%"
      :destroy-on-close="true"
      :fullscreen="false"
    >
      <el-progress :text-inside="true" :stroke-width="24" :percentage="percentage" status="success"></el-progress>

      <div slot="footer" class="dialog-footer"></div>
    </el-dialog>
  </div>
</template>
<script>
import SparkMD5 from "spark-md5";
import waves from "@/directive/waves"; // waves directive
import Pagination from "@/components/Pagination";
import apiUploadService from "@/api/upload";
import {
  getMD5ToBurstData,
  downloadSmallFiles,
  downloadBigFiles
} from "@/api/download";
import indexDBService from "@/store/indexdb";
import { getPageList, deleteStoreFiles } from "@/api/storefiles";
import { statusSet, fileCategory } from "@/utils/upload-download";
import myAction from "@/utils/baseutil";

let { chunkUploadUrl, mergeFiles } = apiUploadService;
let {
  add,
  queryDataBymd5,
  queryCount,
  mergeFileStream,
  requestFileStreamArrs,
  requestDB
} = indexDBService;

export default {
  name: "upload",
  directives: { waves },
  components: { Pagination },
  filters: {
    formatTime: function(val) {
      return myAction.formatTime(val);
    }
  },
  data() {
    let currentData = {
      filterModel: {
        value: {
          field: "fileName",
          method: "Contains",
          value: "",
          prefix: "",
          operator: "And"
        },
        createTime: {
          field: "UploadTime",
          method: "Between",
          value: "",
          prefix: "",
          operator: "And"
        }
      },
      temp: {
        id: "",
        fileName: "",
        fileType: "",
        fileCategory: "",
        fileBytes: 0,
        uploadTime: "",
        uploader: ""
      },
      orderArr: [],
      options: {
        target: chunkUploadUrl,
        testChunks: true, //校验
        chunkSize: "1024000", //分片的大小 1MB
        simultaneousUploads: 3, //并发个数
        fileParameterName: "file", //上传文件时文件的参数名，默认file
        maxChunkRetries: 3, //最大自动失败重试上传次数
        headers: {
          // 在header中添加的验证，请根据实际业务来
          Authorization: "Bearer "
        },
        // 服务器分片校验函数，秒传及断点续传基础
        checkChunkUploadedByResponse: function(chunk, res) {
          let d = JSON.parse(res);
          if (d.flag) {
            var data = d.data;
            //秒传  已经存在该文件
            if (d.resultSign == 2) {
              return true;
            } else {
              return (data || []).indexOf(chunk.offset + 1) >= 0;
            }
          }
        },
        //显示还剩余多少秒
        parseTimeRemaining: function(timeRemaining, parsedTimeRemaining) {
          return parsedTimeRemaining
            .replace(/\syears?/, "年")
            .replace(/\days?/, "天")
            .replace(/\shours?/, "小时")
            .replace(/\sminutes?/, "分钟")
            .replace(/\sseconds?/, "秒");
        },
        categoryMap: {
          image: ["gif", "jpg", "jpeg", "png", "bmp", "webp"],
          video: ["mp4", "m3u8", "rmvb", "avi", "swf", "3gp", "mkv", "flv"],
          audio: ["mp3", "wav", "wma", "ogg", "aac", "flac"],
          document: [
            "doc",
            "txt",
            "docx",
            "pages",
            "epub",
            "pdf",
            "numbers",
            "csv",
            "xls",
            "xlsx",
            "keynote",
            "ppt",
            "pptx",
            "zip"
          ]
        }
      },
      attrs: {
        // 接受的文件类型，形如['.png', '.jpg', '.jpeg', '.gif', '.bmp'...] 这里我封装了一下
        accept: [
          ".gif",
          ".jpg",
          ".jpeg",
          ".png",
          ".bmp",
          ".webp",
          ".mp4",
          ".m3u8",
          ".rmvb",
          ".avi",
          ".swf",
          ".3gp",
          ".mkv",
          ".flv",
          ".mp3",
          ".wav",
          ".wma",
          ".ogg",
          ".aac",
          "flac",
          ".doc",
          ".txt",
          ".docx",
          ".pages",
          ".epub",
          ".pdf",
          ".numbers",
          ".csv",
          ".xls",
          ".xlsx",
          ".keynote",
          "ppt",
          ".pptx",
          ".zip"
        ]
      },
      downloadmd5: "",
      percentage: 0,
      colors: [
        { color: "#f56c6c", percentage: 20 },
        { color: "#e6a23c", percentage: 40 },
        { color: "#5cb87a", percentage: 60 },
        { color: "#1989fa", percentage: 80 },
        { color: "#6f7ad3", percentage: 100 }
      ],
      dialogprogressVisible: false,
      dialogprogressTitle: ""
    };
    var data = $.extend(false, myAction.setBaseVueData, currentData);
    return data;
  },
  created() {
    this.getList();
  },
  methods: {
    getList() {
      this.listLoading = true;
      let param = myAction.getQueryModel(
        this.listQuery.limit,
        this.listQuery.page,
        this.total,
        this.filterModel,
        this.orderArr,
        false
      );
      getPageList(param).then(response => {
        this.list = response.data;
        this.total = response.total;

        // Just to simulate the time of the request
        setTimeout(() => {
          this.listLoading = false;
        }, 1.5 * 300);
      });
    },
    //上传弹窗
    handleUpload() {
      this.dialogStatus = "Create";
      this.dialogTitle = "上传文件";
      this.dialogFormVisible = true;

      // 获取uploader对象
      this.$nextTick(() => {
        window.uploader = this.$refs.uploader.uploader;
      });
    },
    //查询处理的事件
    handleFilter() {
      this.listQuery.page = 1;
      this.getList();
    },
    //排序事件
    sortChange(data) {
      this.orderArr = [];
      this.orderArr.push(data);
      this.handleFilter();
    },
    //预览
    handlePreview(row) {},
    //下载
    handleDown(row) {
      this.dialogprogressTitle = "下载进度";
      this.dialogprogressVisible = true;
      let id = row.id;
      var $this = this;
      //拆分信息
      getMD5ToBurstData(id).then(res => {
        var d = res.data;
        $this.downloadmd5 = d.identifier;
        let totalSize = d.fileRanges.length;
        //进度条
        var t = setInterval(function() {
          //计算进度条
          queryCount(requestDB, $this.downloadmd5, function(count) {
            let p = (count / totalSize).toFixed(2);
            $this.percentage = parseInt(p * 100);
            if ($this.percentage === 100) {
              clearInterval(t);
              //合并数据
              queryDataBymd5(requestDB, $this.downloadmd5, function(res2) {
                //等待三秒
                setTimeout(function() {
                  $this.dialogprogressVisible = false;
                  $this.percentage = 0;
                }, 1000);
                mergeFileStream(res2, d.fileName);
              });
            }
          });
        }, 1000);
        //首先从缓存取分片流 没有的话再去请求后台流数据
        queryDataBymd5(requestDB, $this.downloadmd5, function(arrs) {
          var fileRanges = requestFileStreamArrs(arrs, d.fileRanges);
          $.each(fileRanges, function(i, ee) {
            //请求分片
            downloadBigFiles(d.filePathUrl, ee.range).then(res2 => {
              //存储数据流
              if (requestDB != null && res2.status == 206) {
                var dd = {
                  index: ee.sliceNumber,
                  md5: d.identifier,
                  range: ee.range,
                  content: res2.data
                };

                add(requestDB, dd);
              }
            });
          });
        });
      });
    },
    //删除数据
    handleDelete(row, index) {
      var title = '<span style="color: red;">是否要删除这条数据?</span>';
      this.$confirm(title, "提示", {
        dangerouslyUseHTMLString: true,
        type: "warning",
        confirmButtonText: "确定",
        cancelButtonText: "取消"
      })
        .then(() => {
          let data = '"' + row.id + '"';
          deleteStoreFiles(data).then(response => {
            if (response.resultSign == 0) {
              this.list.splice(index, 1);
              this.total--;
            }
            myAction.getNotifyFunc(response, this);
          });
        })
        .catch(action => {});
    },
    //选择文件后，将上传的窗口展示出来，开始md5的计算工作
    onFileAdded(file) {
      // 计算MD5
      let fileReader = new FileReader();
      let blobSlice =
        File.prototype.slice ||
        File.prototype.mozSlice ||
        File.prototype.webkitSlice;
      let currentChunk = 0;
      const chunkSize = 10 * 1024 * 1000;
      let chunks = Math.ceil(file.size / chunkSize);
      let spark = new SparkMD5.ArrayBuffer();
      // 文件状态设为"计算MD5"
      statusSet(this, file.id, "md5");

      //文件暂停
      file.pause();
      loadNext();
      fileReader.onload = e => {
        spark.append(e.target.result);
        if (currentChunk < chunks) {
          currentChunk++;
          loadNext();
          // 实时展示MD5的计算进度
          this.$nextTick(() => {
            $(".file_" + file.id)
              .find(".uploader-file-status span:eq(0)")
              .text(
                "校验MD5 " + ((currentChunk / chunks) * 100).toFixed(0) + "%"
              );
            //不可以点击上传
            $(".file_" + file.id)
              .find(".uploader-file-actions .uploader-file-resume")
              .addClass("hide_class");
          });
        } else {
          let md5 = spark.end();

          spark.destroy(); //释放缓存
          //可以有上传的按钮
          $(".file_" + file.id)
            .find(".uploader-file-actions .uploader-file-resume")
            .removeClass("hide_class");
          //自定义的参数
          globalThis.uploader.opts.query = {
            fileExt: file.getExtension(),
            fileType: file.getType(),
            fileCategory: fileCategory(this, file.getExtension())
          };
          file.uniqueIdentifier = md5;
          file.resume();
        }
      };
      fileReader.onerror = function() {
        // this.error(`文件${file.name}读取出错，请检查该文件`)
        file.cancel();
      };
      function loadNext() {
        let start = currentChunk * chunkSize;
        let end =
          start + chunkSize >= file.size ? file.size : start + chunkSize;
        fileReader.readAsArrayBuffer(blobSlice.call(file.file, start, end));
      }
    },
    // 文件进度的回调
    onFileProgress(rootFile, file, chunk) {
      //  console.log(`上传中 ${file.name}，chunk：${chunk.startByte / 1024 / 1024} ~ ${chunk.endByte / 1024 / 1024}`)
    },
    //上传成功的事件
    onFileSuccess(rootFile, file, response, chunk) {
      //console.log(file);
      let res = JSON.parse(response);
      // 服务器自定义的错误，这种错误是Uploader无法拦截的
      if (!res.flag) {
        this.$message({ message: res.message, type: "error" });
        // 文件状态设为“失败”
        statusSet(this, file.id, "error");
        return;
      }

      // 如果服务端返回需要合并
      if (res.data) {
        //debugger;
        var param = {
          Identifier: file.uniqueIdentifier,
          FileName: file.name,
          TotalSize: file.size,
          FileType: file.getType(),
          FileExt: file.getExtension(),
          FileCategory: fileCategory(this, file.getExtension())
        };
        // 文件状态设为“合并中”
        statusSet(this, file.id, "merging");
        mergeFiles(param).then(res => {
          this.list.unshift(res.data);
          this.total++;
          statusSet(this, file.id, "success");
        });
      } else {
        statusSet(this, file.id, "success");
      }
    },
    //上传失败
    onFileError(rootFile, file, response, chunk) {
      console.log(response);
    },
    //测试下载大文件
    testdownload() {
      downloadSmallFiles().then(data => {
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
    }
  },
  mounted() {}
};
</script>

<style>
.uploader-example {
  width: 100%;
  font-size: 12px;
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

.hide_class {
  display: none !important;
}

.el-dialog__body {
  padding: 10px !important;
}
</style>
