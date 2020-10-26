
/**
 * 返回通知消息
 * @param {*} response 
 * @param {*} $this 
 */
export function getMessageFunc(response, $this)
{
    let title = '', type = '';
    switch (response.resultSign) {
      case 0: title = "成功"; type = "success"; break;
      case 1: title = "警告"; type = "warning"; break;
      case 2: title = "消息"; type = "info"; break;
      case 3: title = "错误"; type = "error"; break;
      default: title = "消息"; type = "info"; break;
    }
    $this.$notify({
      title: title,
      message: response.message,
      type: type,
      duration: 2000
    })

}