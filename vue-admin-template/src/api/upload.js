import axios from 'axios'
import request from '@/utils/request'

//上传url地址
const chunkUploadUrl = process.env.VUE_APP_BASE_API+ "/FileUpload/ChunkUpload";

//合并的fun
export function mergeFiles(data) {
  return request({
    url: '/FileUpload/MergeFiles',
    method: 'post',
    data
  })
}

//拆分文件的分片信息
export function getMD5ToBurstData() {
  return request({
    url: '/FileDownload/GetMD5ToBurstData',
    method: 'get',
    params: { }
  })
}

//下载大文件测试
export function downloadBigFile() {

  return new Promise((resolve, reject) => {
    axios({
      url: process.env.VUE_APP_BASE_API+'/FileDownload/DownloadBigFile',
      method: 'get',
      params: {},
      responseType: 'blob'
    }).then(res => {
     // console.log(res);
      resolve(res)
    }).catch(err => {
      reject(err);
    });
  });
  
}

export  function downloadBigFile2(range) {

  return new Promise((resolve, reject) => {
    axios({
      url: process.env.VUE_APP_BASE_API + '/FileDownload/DownLoad2',
      method: 'get',
      params: {},
      responseType: 'blob',
      headers: {
        'range': range
        //"bytes=0-1000",
        }
    }).then(res => {
      // console.log(res);
      resolve(res)
    }).catch(err => {
      reject(err);
    });
  });

}


const apiUploadService = {
  chunkUploadUrl,
  mergeFiles,
  getMD5ToBurstData,
  downloadBigFile,
  downloadBigFile2
}

export default apiUploadService

