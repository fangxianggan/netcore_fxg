import axios from 'axios'
import { MessageBox, Message } from 'element-ui'
import store from '@/store'



// create an axios instance
const service = axios.create({
  baseURL: process.env.VUE_APP_BASE_API, // url = base url + request url
  // withCredentials: true, // send cookies when cross-domain requests
  timeout: 300000 // request timeout
})

// 是否有请求在刷新token
window.isRefreshing = false
// 被挂起的请求数组
let requests = []

/* 请求拦截器 */
// request interceptor
service.interceptors.request.use(
  config => {
    // do something before request is sent
    if (config.method === 'post' && config.data.constructor !== FormData) {
      config.data = config.data
    } else {
      config.params = config.params || {}
    }
    // let each request carry token
    // ['X-Token'] is a custom headers key
    // please modify it according to the actual situation

    //登录和刷新token
    if (config.url.indexOf("/login") >= 0 || config.url.indexOf("/GetRefreshToken") >= 0) {
      return config;
    }

    //jwt
    config.headers['Authorization'] = "Bearer " + store.getters.token

    //token 是否在超過有效期內 但是在刷新的有效期內 獲取新的token
    if (store.getters.expires && store.getters.token) {
      // console.log(store.getters.expires);
      //  console.log(store.getters.token);
      const now = Date.now();
      const expires = new Date(store.getters.expires).getTime();
      if (now >= expires) {
        //立即刷新token
        if (!window.isRefreshing) {
          window.isRefreshing = true
          store.dispatch("user/refreshToken", store.getters.refreshToken)
            .then((res) => {
              window.isRefreshing = false;
              if (res.statusCode === 200) {
                const data = res.data
                return data.tokenContent
              }
            }).then((token) => {
              // console.log("刷新token成功 执行队列");
              requests.forEach(element => {
                element(token)
              });
              requests = [];
            }).catch((res) => {
              console.error("refesh token error", res);
            })
        }
        const retry = new Promise((resolve, reject) => {
          requests.push((token) => {
            config.headers['Authorization'] = "Bearer " + token;
            resolve(config)
          })
        })
        return retry
      }
    }
    if (!config.headers['Authorization']) {
      delete config.headers['Authorization']
    }
    return config
  },
  error => {
    // do something with request error
    //  console.log(error) // for debug
    return Promise.reject(error)
  }
)

// response interceptor
service.interceptors.response.use(
  /**
   * If you want to get http information such as headers or status
   * Please return  response => response
  */
  /**
   * Determine the request status by custom code
   * Here is just an example
   * You can also judge the status by HTTP Status Code
   */
  response => {
    const res = response.data
    const statusCode = res.statusCode
    // if the custom code is not 20000, it is judged as an error.
    if (statusCode !== 200) {
      var mes = res.message;
      var s = 5000;
      switch (statusCode) {
        case 401:
          var i = 5;
          var txt = i;
          mes = "长时间未操作页面已经失效<span style='font-size:20px;' id='aa'>" + txt + "</span>秒后跳回登录页面！"
          var dd = setInterval(function () {
            $("#aa").html(txt--);
            // console.log("ffff")
          }, 1000);
          Message({
            message: mes,
            type: 'error',
            dangerouslyUseHTMLString: true,
            duration: s
          })
          setTimeout(function () {
            clearInterval(dd);
            store.dispatch('user/resetToken').then(() => {
              location.reload()
            })
          }, i * 1000);
          break
        default:
          s = 5000;
          Message({
            message: mes,
            type: 'error',
            dangerouslyUseHTMLString: true,
            duration: s
          })
          break
      }
      return Promise.reject(res.message)
    } else {
      return res;
    }
  },
  error => {
    let msg = "";
    if (error && error.response) {
      switch (error.response.status) {
        case 400:
          msg = "错误请求";
          console.log('错误请求')
          break;
        case 401:
          msg = "未授权，请重新登录";
          console.log('未授权，请重新登录')
          break;
        case 403:
          msg = "拒绝访问";
          console.log('拒绝访问')
          break;
        case 404:
          msg = "请求错误,未找到该资源";
          console.log('请求错误,未找到该资源')
          break;
        case 405:
          msg = "请求方法未允许";
          console.log('请求方法未允许')
          break;
        case 408:
          msg = "请求超时";
          console.log('请求超时')
          break;
        case 500:
          msg = "服务器端出错";
          console.log('服务器端出错')
          break;
        case 501:
          msg = "网络未实现";
          console.log('网络未实现')
          break;
        case 502:
          msg = "网络错误";
          console.log('网络错误')
          break;
        case 503:
          msg = "服务不可用";
          console.log('服务不可用')
          break;
        case 504:
          msg = "网络超时";
          console.log('网络超时')
          break;
        case 505:
          msg = "http版本不支持该请求";
          console.log('http版本不支持该请求')
          break;
        default:
          msg = "连接错误";
          console.log(`连接错误${error.response.status}`)
      }
    } else {
      msg = "连接到服务器失败";
    }
    // console.log('err' + error) // for debug
    Message({
      message: msg,
      type: 'error',
      duration: 5 * 1000
    })
    return Promise.reject(error)
  }
)
export default service
