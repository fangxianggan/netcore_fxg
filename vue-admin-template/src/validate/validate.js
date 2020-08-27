



/* 合法uri*/
export function validateURL(rule, value, callback) {
  const reg = /^(https?|ftp):\/\/([a-zA-Z0-9.-]+(:[a-zA-Z0-9.&%$-]+)*@)*((25[0-5]|2[0-4][0-9]|1[0-9]{2}|[1-9][0-9]?)(\.(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[1-9]?[0-9])){3}|([a-zA-Z0-9-]+\.)*[a-zA-Z0-9-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{2}))(:[0-9]+)*(\/($|[a-zA-Z0-9.,?'\\+&%$#=~_-]+))*$/
  if (value == '' || value == undefined || value == null) {
    callback();
  } else {
    if (!reg.test(value)) {
      callback(new Error('输入的不合法'));
    } else {
      callback();
    }
  }
}

/* 小写字母*/
export function validateLowerCase(rule, value, callback) {
  const reg = /^[a-z]+$/
  if (value == '' || value == undefined || value == null) {
    callback();
  } else {
    if (!reg.test(value)) {
      callback(new Error('输入的不合法'));
    } else {
      callback();
    }
  }
}

/* 大写字母*/
export function validateUpperCase(rule, value, callback) {
  const reg = /^[A-Z]+$/
  if (value == '' || value == undefined || value == null) {
    callback();
  } else {
    if (!reg.test(value)) {
      callback(new Error('输入的不合法'));
    } else {
      callback();
    }
  }
}

/* 大小写字母*/
export function validatAlphabets(rule, value, callback) {
  const reg = /^[A-Za-z]+$/
  if (value == '' || value == undefined || value == null) {
    callback();
  } else {
    if (!reg.test(value)) {
      callback(new Error('输入的不合法'));
    } else {
      callback();
    }
  }
}

/*判断输入的EMAIL格式是否正确 */
export function validateIsEmail(rule, value, callback) {

    const reg = /[\w!#$%&'*+/=?^_`{|}~-]+(?:\.[\w!#$%&'*+/=?^_`{|}~-]+)*@(?:[\w](?:[\w-]*[\w])?\.)+[\w](?:[\w-]*[\w])?/;
    if (value == '' || value == undefined || value == null) {
      callback();
    } else {
      if (!reg.test(value)) {
        callback(new Error('输入的不合法'));
      } else {
        callback();
      }
    }
}
/*判断输入的手机号格式是否正确 */
export function validateIsPhone(rule, value, callback) {

  const reg = /^1[0-9]{10}$/;
  if (value == '' || value == undefined || value == null) {
    callback();
  } else {
    if (!reg.test(value)) {
      callback(new Error('输入的不合法'));
    } else {
      callback();
    }
  }

}

/*验证价格*/
export function validatePrice(rule, value, callback) {
  // const reg = /(^[1-9]([0-9]{1,4})?(\.[0-9]{1,2})?$)|(^(0){1}$)|(^[0-9]\.[0-9]([0-9])?$)/;
  const reg = /(^[1-9](\d+)?(\.\d{1,2})?$)|(^\d\.\d{1,2}$)/;
    if (!reg.test(value)) {
      callback(new Error('输入的不合法'));
    } else {
      callback();
    }
  
}

/*验证正整数*/
export function validateNum(rule, value, callback) {
  const reg = /^[1-9][0-9]*$/;
  if (value == '' || value == undefined || value == null) {
    callback();
  } else {
    if (!reg.test(value)) {
      callback(new Error('输入的不合法'));
    } else {
      callback();
    }
  }
}


/*验证1-18位数字加6位小数*/
export function validateNum18(rule, value, callback) {
  const reg = /^(0|[1-9][0-9]{0,17})(\.[0-9]{1,6})?$/;
  if (value == '' || value == undefined || value == null) {
    callback();
  } else {
    if (!reg.test(value)) {
      callback(new Error('请输入数字值且不大于18位或最大六位小数'));
    } else {
      callback();
    }
  }

}

/*验证9位正整数*/
export function validateNum9(rule, value, callback) {
  const reg = /^0|[1-9][0-9]{0,9}$/;
  if (value == '' || value == undefined || value == null) {
    callback();
  } else {
    if (!reg.test(value)) {
      callback(new Error('请输入数字值且不大于10位'));
    } else {
      callback();
    }
  }
}

/*验证12位正整数*/
export function validateNum12_6(rule, value, callback) {
  const reg = /^(0|[1-9][0-9]{0,11})(\.[0-9]{1,6})?$/;
  if (value == '' || value == undefined || value == null) {
    callback();
  } else {
    if (!reg.test(value)) {
      callback(new Error('请输入数字值且不大于12位或最大六位小数'));
    } else {
      callback();
    }
  }
}
//校验输入非中文  true 是中文
export function noChinese(rule, value, callback) {
  const reg = /[\u4e00-\u9fa5]/;
  if (value == '' || value == undefined || value == null) {
    callback();
  } else {
    if (!reg.test(value)) {
      callback(new Error('请输入非中文的字符'));
    } else {
      callback();
    }
  }
}


//校验输入是否是数字（包括正负、小数点）
export function inputOrNumber(rule, value, callback) {
  const reg = /[-+]*[0-9][.][0-9]+|[-+]*[1-9][0-9]*|^[0]$/;
  if (value == '' || value == undefined || value == null) {
    callback();
  } else {
    if (!reg.test(value)) {
      callback(new Error('请输入数字'));
    } else {
      callback();
    }
  }
}

//校验输入字符是否大于6位(包含数字字母)
export function validateByte6_18(rule, value, callback) {
  const reg = /^.{6,18}$/;
  if (value == '' || value == undefined || value == null) {
    callback();
  } else {
    if (!reg.test(value)) {
      callback(new Error('请输入至少输入6位及以上任意字符'));
    } else {
      callback();
    }
  }
}

//校验输入字符是否是路径格式
export function validateUrl(rule, value, callback) {
  const reg = /^[a-zA-Z]:(((\\(?! )[^/:*?<>\""|\\]+)+\\?)|(\\)?)\s*$/;

  if (value == '' || value == undefined || value == null) {
    callback();
  } else {
    if (!reg.test(value)) {
      callback(new Error('请输入正确的文件路径'));
    } else {
      callback();
    }
  }
}


/* 是否邮箱*/
export function validateEMail(rule, value, callback) {
  const reg = /^([a-zA-Z0-9]+[-_\.]?)+@[a-zA-Z0-9]+\.[a-z]+$/;
  if (value == '' || value == undefined || value == null) {
    callback();
  } else {
    if (!reg.test(value)) {
      callback(new Error('请输入正确的邮箱地址'));
    } else {
      callback();
    }
  }
}


/* 是否身份证号码*/
export function validateIdNo(rule, value, callback) {
  const reg = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/;
  if (value == '' || value == undefined || value == null) {
    callback();
  } else {
    if ((!reg.test(value)) && value != '') {
      callback(new Error('请输入正确的身份证号码'));
    } else {
      callback();
    }
  }
}

/* 是否手机号码或者固话*/
export function validatePhoneTwo(rule, value, callback) {
  const reg = /^((0\d{2,3}-\d{7,8})|(1[34578]\d{9}))$/;;
  if (value == '' || value == undefined || value == null) {
    callback();
  } else {
    if ((!reg.test(value)) && value != '') {
      callback(new Error('请输入正确的电话号码或者固话号码'));
    } else {
      callback();
    }
  }
}
/* 是否固话*/
export function validateTelphone(rule, value, callback) {
  const reg = /0\d{2}-\d{7,8}/;
  if (value == '' || value == undefined || value == null) {
    callback();
  } else {
    if ((!reg.test(value)) && value != '') {
      callback(new Error('请输入正确的固话（格式：区号+号码,如010-1234567）'));
    } else {
      callback();
    }
  }
}
