
import request from '@/utils/request'

/**
 * //获取任务列表
 * @param {any} data
 */

export function getPageList(data) {
  return request({
    url: '/Jobs/GetPageList',
    method: 'post',
    data
  })
}

/**
 * 新增任务或者修改任务
 * @param {any} data
 */
export function addOrEdit(data) {
  return request({
    url: "/Jobs/AddOrEditTaskJob",
    method: "post",
    data
  });
}

