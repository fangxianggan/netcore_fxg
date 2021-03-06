import request from '@/utils/request'

export function login(data) {
  return request({
    url: '/Login/GetValidateLogon',
    method: 'post',
    data
  })
}

export function getInfo(token) {
  return request({
    url: '/Login/GetUserInfoData',
    method: 'get',
    params: { token }
  })
}

export function logout(data) {
  return request({
    url: '/Login/GetLogout',
    method: 'post',
    data
  })
}
