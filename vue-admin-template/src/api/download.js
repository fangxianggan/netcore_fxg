import qs from 'qs'
import axios from 'axios'
import request from '@/utils/request'

//拆分文件的分片信息
export function getMD5ToBurstData(id) {
  return request({
    url: '/StoreFiles/GetMD5ToBurstData',
    method: 'get',
    params: { "id": id },
  
  })
}

/**
 * 下载小文件
 * @param {any} id
 */
export function downloadSmallFiles(id) {
  return new Promise((resolve, reject) => {
    axios({
      url: process.env.VUE_APP_BASE_API + '/StoreFiles/DownloadSmallFiles',
      method: 'get',
      params: {id},
      responseType: 'blob'
    }).then(res => {
      resolve(res)
    }).catch(err => {
      reject(err);
    });
  });
}



/**
 * 分片下载 大文件下载
 * @param {any} path
 * @param {any} range
 */
export function downloadBigFiles(path, range) {

  return new Promise((resolve, reject) => {
    axios({
      url: process.env.VUE_APP_BASE_API + '/StoreFiles/GetDownloadBigFiles',
      method: 'get',
      params: {path},
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
