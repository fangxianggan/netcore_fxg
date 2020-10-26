import Cookies from 'js-cookie'

const TokenKey = 'auth_token'

const RefreshTokenKey = 'auth_refresh_oken';

export function getToken() {
  return Cookies.get(TokenKey)
}

export function setToken(token, cookieExpires) {

  return Cookies.set(TokenKey, token, cookieExpires)
}

export function removeToken() {
  return Cookies.remove(TokenKey)
}


export function getRefreshToken() {
  return Cookies.get(RefreshTokenKey)
}

export function setRefreshToken(token, cookieExpires) {
  return Cookies.set(RefreshTokenKey, token, cookieExpires)
}

export function removeRefreshToken() {
  return Cookies.remove(RefreshTokenKey)
}




