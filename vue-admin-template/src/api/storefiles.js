
import request from '@/utils/request'

//获取存储文件列表
export function getPageList(data) {
  return request({
    url: '/StoreFiles/GetPageList',
    method: 'post',
    data
  })
}
