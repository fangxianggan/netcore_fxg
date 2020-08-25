

import *as validates from '@/validate/validate'



/**
 *
 */

let myAction = {
  /**
   *设置vue页面data数据
   * */
  setBaseVueData:
  {
    //表格的key
    tableKey: 0,
    //表格的数据
    list: null,
    //总条数
    total: 0,
    //加载显示
    listLoading: true,
    //分页的设置
    listQuery: {
      page: 1,
      limit: 20
    },
    //区间时间设置
    daterangeOptions: {
      disabledDate(date) {
        const disabledDay = date.getDate();
        return disabledDay === 15;
      },
      shortcuts: [{
        text: '最近一周',
        onClick(picker) {
          const end = new Date();
          const start = new Date();
          start.setTime(start.getTime() - 3600 * 1000 * 24 * 7);
          picker.$emit('pick', [start, end]);
        }
      }, {
        text: '最近一个月',
        onClick(picker) {
          const end = new Date();
          const start = new Date();
          start.setTime(start.getTime() - 3600 * 1000 * 24 * 30);
          picker.$emit('pick', [start, end]);
        }
      }, {
        text: '最近三个月',
        onClick(picker) {
          const end = new Date();
          const start = new Date();
          start.setTime(start.getTime() - 3600 * 1000 * 24 * 90);
          picker.$emit('pick', [start, end]);
        }
      }]
    },
    //弹出框显示掩藏
    dialogFormVisible: false,
    //弹出框状态
    dialogStatus: '',
    //弹出框title
    dialogTitle: '',
    //展开
    activeName: "1"

  },
  /*
  *设置vue页面共用的methods 方法
  * **/
  setBaseVueMethods:
  {


  },
  /**
   * 自定义基础验证规则
   * */
  setBaseValidateRules: function () {

    /*
  ** 不为空
  * */

    let checkNull = { required: true, message: '不能为空', trigger: ['blur', 'change'] };
    let checkInt = {
      validator: function (rule, value, callback) {
        let flag = new RegExp("^[0-9]([0-9])*$").test(value);
        if (flag) {
          callback();
        } else {
          return callback(new Error('必须为整数'));
        }
      }, trigger: 'blur'
    };


    return {
      checkNull: [checkNull],
      checkInt: [checkInt],
      checkIntAndNull: [
        checkNull, checkInt
      ],
      validateNum: [{
        validator: validates.validateNum, trigger: 'blur'
      }],
      validatePrice: [
        {
          validator: validates.validatePrice, trigger: 'blur'
        },
        checkNull
      ],


    }


  },
  /**
   * 将对象属性第一个字母转化成大写
   * @param {any} jsonObj
   */
  convertJsonKey: function (jsonObj) {
    // 这里没有进行深度拷贝
    var result = {};
    for (var key of Object.keys(jsonObj)) {
      var val = jsonObj[key];
      key = key.replace(key[0], key[0].toUpperCase());
      result[key] = val;
    };
    return result;
  },
  /**
   * 将null转化""
   * @param {any} val
   */
  convertNULL: function (val) {
    // console.log(val);
    if (val == null || val == undefined) {
      return "";
    } else {
      return val;
    }
  },
  /**
   * format 性别
   * @param {any} val
   */
  formatGender: function (val) {
    var v = "";
    switch (val) {
      case 0: v = "男"; break;
      case 1: v = "女"; break;
      case 2: v = "未知"; break;
      default: v = "未知"; break;
    }
    return v;
  },

  /**
   * foramt 订单状态
   * @param {any} val
   */
  formatOrderState: function (val) {
    let text = "";
    switch (val) {
      case "FF01": text = "未完成"; break;
      case "FF02": text = "已锁定"; break;
      case "SS01": text = "未发货"; break;
      case "SS02": text = "已发货"; break;
      case "SS03": text = "已签收"; break;
      case "SS04": text = "已丢失"; break;
      default: text = ""; break;
    }
    return text;
  },
  /**
   * 格式化时间
   * @param {any} val
   */
  formatTime: function (val, fmt1) {
    if (arguments.length === 0) {
      return null
    }
    let fmt = fmt1 || 'yyyy-MM-dd'
    let $this = new Date(val)
    let o = {
      'M+': $this.getMonth() + 1,
      'd+': $this.getDate(),
      'h+': $this.getHours(),
      'm+': $this.getMinutes(),
      's+': $this.getSeconds(),
      'q+': Math.floor(($this.getMonth() + 3) / 3),
      'S': $this.getMilliseconds()
    }
    if (/(y+)/.test(fmt)) {
      fmt = fmt.replace(RegExp.$1, ($this.getFullYear() + '').substr(4 - RegExp.$1.length))
    }
    for (var k in o) {
      if (new RegExp('(' + k + ')').test(fmt)) {
        fmt = fmt.replace(RegExp.$1, (RegExp.$1.length === 1) ? (o[k]) : (('00' + o[k]).substr(('' + o[k]).length)))
      }
    }
    return fmt;
  },
  /**
   *
   */
  newOrderNumber: function () {
    var outTradeNo = "";  //订单号
    for (var i = 0; i < 4; i++) //6位随机数，用以加在时间戳后面。
    {
      outTradeNo += Math.floor(Math.random() * 10);
    }
    var $this = new Date();
    var Y = $this.getFullYear();
    var M = $this.getMonth() + 1; //月份
    M = M >= 10 ? M : "0" + M;
    var d = $this.getDate();//日
    d = d >= 10 ? d : "0" + d;
    var H = $this.getHours();//小时
    H = H >= 10 ? H : ("0" + H);
    var m = $this.getMinutes(); //分
    m = m >= 10 ? m : ("0" + m);
    var s = $this.getSeconds(); //秒
    s = s >= 10 ? s : ("0" + s);

    outTradeNo = Y.toString() + M.toString() + d.toString() + H.toString() + m.toString() + s.toString() + outTradeNo.toString();
    return outTradeNo;
  },
  /**
   *
   * */
  newGuid: function () {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
      var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  },
  /**
   * 获取queryModel
   * @param {any} limit
   * @param {any} page
   * @param {any} total
   * @param {any} filterModel
   * @param {any} orderArr
   * @param {any} isReport
   */

  getQueryModel: function (limit, page, total, filterModel, orderArr, isReport) {

    let items = [];
    $.each(filterModel, function (i, e) {
      if (e.value == null) {
        e.value = "";
      }
      if (typeof (e.value) === "object" && e.method == "Between") {
        $.each(e.value, function (m, n) {
          var o = {
            field: e.field,
            method: m == 0 ? "GreaterThanOrEqual" : "LessThanOrEqual",
            value: n,
            prefix: e.prefix,
            operator: e.operator
          }
          items.push(o);
        });

      } else if (typeof (e.value) === "object" && e.method == "BetweenTime") {
        let val = e.value;
        val = val.join(',');
        var o = {
          field: e.field,
          method: "BetweenTime",
          value: val,
          prefix: e.prefix,
          operator: e.operator
        }
        items.push(o);
      } else {
        items.push(e);
      }
    });
    let orders = [];
    if (orderArr.length == 0 || orderArr[0].prop == null) {
      orders = [{ field: "ID", isDesc: true }]
    } else {
      $.each(orderArr, function (i, e) {
        var isDesc = e.order == "descing" ? true : false;
        var firstVal = e.prop.substr(0, 1).toUpperCase();
        var otherVal = e.prop.substr(1, e.prop.length - 1);
        var obj = { field: firstVal + otherVal, isDesc: isDesc };
        orders.push(obj);
      });
    }
    let queryModel = {
      pageSize: limit,
      pageIndex: page,
      total: total,
      items: items,
      orderList: orders,
      isReport: isReport == undefined ? false : isReport
    };
    return queryModel;
  },

  /**
   * 通知消息封装
   * @param {any} response
   * @param {any} $this 当前vue对象
   */
  getNotifyFunc: function (response, $this) {
    let title = '', type = '';
    switch (response.resultSign) {
      case 0: title = "成功"; type = "success"; break;
      case 1: title = "警告"; type = "warning"; break;
      case 2: title = "错误"; type = "error"; break;
      case 3: title = "消息"; type = "info"; break;
      default: title = "消息"; type = "info"; break;
    }
    $this.$notify({
      title: title,
      message: response.message,
      type: type,
      duration: 2000
    })
  },
  /**
   * 大写金额
   * @param {any} num
   */
  getDigitUppercase: function (num) {
    var tmpNum = num;
    var AA = new Array("零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖");
    var BB = new Array("", "拾", "佰", "仟", "万", "亿", "元", "");
    var CC = new Array("角", "分", "厘");
    var a = ("" + num).replace(/(^0*)/g, "").split("."), k = 0, re = "";
    for (var i = a[0].length - 1; i >= 0; i--) {
      switch (k) {
        case 0:
          re = BB[7] + re;
          break;
        case 4:
          if (!new RegExp("0{4}\\d{" + (a[0].length - i - 1) + "}$").test(a[0]))
            re = BB[4] + re;
          break;
        case 8:
          re = BB[5] + re;
          BB[7] = BB[5];
          k = 0;
          break;
      }
      if (k % 4 == 2 && a[0].charAt(i + 2) != 0 && a[0].charAt(i + 1) == 0)
        re = AA[0] + re;

      if (a[0].charAt(i) != 0)
        re = AA[a[0].charAt(i)] + BB[k % 4] + re;
      k++;
    }
    if (a.length > 1) {// 加上小数部分(如果有小数部分)
      if (a[0].length == 0) {
        re += "";
      } else {
        re += BB[6];
      }
      for (var i = 0; i < a[1].length; i++) {
        re += AA[a[1].charAt(i)] + CC[i];
        if (i == 2)
          break;
      }
    }
    if (tmpNum.toString().indexOf(".") == -1) {
      re += "元整";
    }
    // 处理输入字符0的情况
    if (a == '') {
      re = "零" + re;
    }
    return re;
  },

  setRandomNumberArr: function (min, max, length) {
    var arr = [];
    if (min == undefined) min = 0;
    if (max == undefined) max = 100;
    if (length == undefined) length = 4;
    for (var i = 0; i < 4; i++) {
      var rand = parseInt(Math.random() * (max - min + 1) + min);
      arr.push(rand);
    }
    return arr;
  },
  /**
   * chart 鼠标滑过显示数值
   * @param {any} value
   * @param {any} typename
   */
  formattingNumbers: function (value, typename) {
    //text 比值不用科学计数法
    value = value.toString();
    if (value.indexOf(":") >= 0 || value.indexOf("：") >= 0) {
      return value;
    }
    var starttile = "";
    if ((typename.indexOf("费用") >= 0 || typename.indexOf("收入") >= 0) && (typename.indexOf("占比") < 0) && (typename.indexOf("高费用人次") < 0)) {
      starttile = "￥";
    }
    var result = val;
    var val = parseFloat(value.replace(/\,/g, ''));
    var isxyl = "";
    if (val < 0) {
      val = Math.abs(val);
      isxyl = "-";
    }
    if (val > 10000 && val < 10000000) {
      result = starttile + isxyl + (val / 10000).toFixed(2) + "万";
    }
    else if (val > 10000000) {
      var num = ((val / 10000).toFixed(2) || 0).toString();
      if (/^.*\..*$/.test(num)) {
        var pointIndex = num.lastIndexOf(".");
        var intPart = num.substring(0, pointIndex);
        var pointPart = num.substring(pointIndex + 1, num.length);
        intPart = intPart + "";
        var re = /(-?\d+)(\d{3})/
        while (re.test(intPart)) {
          intPart = intPart.replace(re, "$1,$2")
        }
        num = intPart + "." + pointPart;
      } else {
        num = num + "";
        var re = /(-?\d+)(\d{3})/
        while (re.test(num)) {
          num = num.replace(re, "$1,$2")
        }
      }
      result = starttile + isxyl + num + "万";
    }
    else {
      result = starttile + isxyl + value;
    }
    return result;
  }



}

export default myAction 
