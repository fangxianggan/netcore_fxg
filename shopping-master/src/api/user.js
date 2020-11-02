
import request from '../request'

export function logon(data) {
  return request({
    url: '/Login/GetUserLogonData',
    method: 'post',
    data
  })
}

export function login(data) {
  return request({
    url: '/Login/GetValidateLogon',
    method: 'post',
    data
  })
}