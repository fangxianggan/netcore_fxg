
/**
 * 上传
 * @param {any} $this
 * @param {any} id
 * @param {any} status
 */

export function statusSet($this,id, status) {
  let statusMap = {
    md5: {
      text: '校验MD5',
      bgc: '#e2eeff'
    },
    merging: {
      text: '合并中',
      bgc: '#ac3ae6'
    },
    transcoding: {
      text: '转码中',
      bgc: '#e2eeff'
    },
    uploading: {
      text: '上传中',
      bgc: '#e2eeff'
    },
    waiting: {
      text: '等待',
      bgc: '#e2eeff'
    },
    paused: {
      text: '暂停',
      bgc: '#61affe'
    },
    error: {
      text: '上传失败',
      bgc: '#f56c6c'
    },
    success:
    {
      text: '上传成功',
      bgc: '#49cc90'
    }
  }
  $this.$nextTick(() => {
    $('.file_' + id).find(".uploader-file-status span:eq(0)").css({
      'position': 'absolute',
      'top': '0',
      'left': '0',
      'right': '0',
      'bottom': '0',
      'zIndex': '1',
      'color': statusMap[status].bgc,
    }).text(statusMap[status].text);
  })
}

/**
 * 
 * @param {any} $this
 * @param {any} ext
 */
export function fileCategory($this,ext)
{
  var str = "unkown";
  $.each($this.options.categoryMap, function (i, e) {
   let len= e.filter(function (m) {
      return m == ext;
   });
    if (len.length > 0) {
      str = i;
      return false;
    }
  });
  return str;
}



