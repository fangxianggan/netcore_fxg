import request from '@/utils/request'

/**
 * //获取列表
 * @param {any} data
 */

export function getPageList(data) {
  return request({
    url: '/ProductInfo/GetPageList',
    method: 'post',
    data
  })
}

/**
 * 新增或者修改
 * @param {any} data
 */
export function addOrEdit(data) {
  return request({
    url: "/ProductInfo/AddOrEdit",
    method: "post",
    data
  });
}


/** 单个post 传值
 * 删除job
 * @param {*} data 
 */
export function del(data)
{
  return request({
    url: "/ProductInfo/Delete",
    method: "post",
    headers: {
      'content-type': 'application/json;charset=UTF-8'
    }, 
    data
  });
}