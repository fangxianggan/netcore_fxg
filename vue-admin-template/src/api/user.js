import request from '@/utils/request'

export function login(data) {
  return request({
    url: '/Login/GetValidateLogon',
    method: 'post',
    data
  })
}

export function getInfo() {
  return request({
    url: '/Login/GetUserInfoData',
    method: 'get',
    params: {  }
  })
}


export function getRefreshTokenData(refreshToken) {
  return request({
    url: '/Login/GetRefreshToken',
    method: 'get',
    params: { refreshToken }
  })
}

export function logout(data) {
  return request({
    url: '/Login/GetLogout',
    method: 'post',
    headers: {
      'content-type': 'application/json;charset=UTF-8'
    }, 
    data
  })
}
