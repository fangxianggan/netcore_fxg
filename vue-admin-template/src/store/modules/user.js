import { login, logout, getInfo, getRefreshTokenData } from '@/api/user'
import {
  getToken,
  setToken, removeToken,
  setRefreshToken, removeRefreshToken,
  getTokenExpires, setTokenExpires
} from '@/utils/auth'
import { resetRouter } from '@/router'

const getDefaultState = () => {
  return {
    token: getToken(),
    refreshToken: '',
    expires: getTokenExpires(),
    refreshExpires: '',
    name: '',
    avatar: ''
  }
}

const state = getDefaultState()
const mutations = {
  RESET_STATE: (state) => {
    Object.assign(state, getDefaultState())
  },
  SET_TOKEN: (state, token) => {
    state.token = token
  },
  SET_EXPIRES: (state, expires) => {
    state.expires = expires
  },
  SET_REFRESHTOKEN: (state, refreshToken) => {
    state.refreshToken = refreshToken
  },
  SET_REFRESHEXPIRES: (state, refreshExpires) => {
    state.refreshExpires = refreshExpires
  },
  SET_NAME: (state, name) => {
    state.name = name
  },
  SET_AVATAR: (state, avatar) => {
    state.avatar = avatar
  }
}

const actions = {
  // user login
  login({ commit }, userInfo) {
    const { username, password } = userInfo
    return new Promise((resolve, reject) => {
      login({ username: username.trim(), password: password }).then(response => {
        const { data } = response
        commit('SET_TOKEN', data.accessToken.tokenContent)
        commit('SET_EXPIRES', data.accessToken.expires)
        commit('SET_REFRESHTOKEN', data.refreshToken.tokenContent)
        commit('SET_REFRESHEXPIRES', data.refreshToken.expires)
        setToken(data.accessToken.tokenContent, data.accessToken.expires);
        setRefreshToken(data.refreshToken.tokenContent, data.refreshToken.expires);
        resolve()
      }).catch(error => {
        reject(error)
      })
    })
  },

  // get user info
  getInfo({ commit, state }) {
    return new Promise((resolve, reject) => {
      getInfo().then(response => {
        const { data } = response

        if (!data) {
          return reject('Verification failed, please Login again.')
        }
        const { name, avatar } = data
        commit('SET_NAME', name)
        commit('SET_AVATAR', avatar)
        resolve(data)
      }).catch(error => {
        reject(error)
      })
    })
  },

  // user logout
  logout({ commit, state }) {
    return new Promise((resolve, reject) => {
      let data = '"' + state.token + '"';
      logout(data).then(() => {
        removeToken() // must remove  token  first
        removeRefreshToken()
        resetRouter()
        commit('RESET_STATE')
        resolve()
      }).catch(error => {
        reject(error)
      })
    })
  },

  // remove token
  resetToken({ commit }) {
    return new Promise(resolve => {
      removeToken() // must remove  token  first
      commit('RESET_STATE')
      resolve()
    })
  },



  //刷新 token 存储
  refreshToken({ commit },d) {
    return new Promise((resolve, reject) => {
      getRefreshTokenData(d).then(res => {
        if (res.statusCode === 200) {
          const data = res.data
          commit('SET_EXPIRES', data.expires)
          commit('SET_TOKEN', data.tokenContent)
          removeToken();
          setToken(data.tokenContent, data.expires);
        }
        resolve(res)
      }).catch(error => {
        reject(error)
      })
    })
  }


}



export default {
  namespaced: true,
  state,
  mutations,
  actions
}

