
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

/**
 * 启动实例
 * @param {*} gId 
 */
export function addJob(gId){
  return request({
    url: "/Jobs/AddJob",
    method: "get",
    params:{gId}
  });
}

/**
 * 停止实例
 * @param {*} gId 
 */
export function stopJob(gId){
  return request({
    url: "/Jobs/StopJob",
    method: "get",
    params:{gId}
  });
}

/**
 * 恢复实例
 * @param {*} gId 
 */
export function resumeJob(gId){
  return request({
    url: "/Jobs/ResumeJob",
    method: "get",
    params:{gId}
  });
}

/** 单个post 传值
 * 删除job
 * @param {*} data 
 */
export function del(data)
{
  return request({
    url: "/Jobs/DeleteTaskJob",
    method: "post",
    headers: {
      'content-type': 'application/json;charset=UTF-8'
    }, 
    data
  });
}
