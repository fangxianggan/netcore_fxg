
import request from '@/utils/request'

//获取存储文件列表
export function getPageList(data) {
  return request({
    url: '/StoreFiles/GetPageList',
    method: 'post',
    data
  })
}


/** 单个post 传值
 * 删除job
 * @param {*} data 
 */
export function deleteStoreFiles(data)
{
  return request({
    url: "/StoreFiles/DeleteStoreFiles",
    method: "post",
    headers: {
      'content-type': 'application/json;charset=UTF-8'
    }, 
    data
  });
}

