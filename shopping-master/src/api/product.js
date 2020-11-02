
import request from '../request'

export function getPageList(data) {
    return request({
      url: '/ProductInfo/GetPageList',
      method: 'post',
      data
    })
  }
  