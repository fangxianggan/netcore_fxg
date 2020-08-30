

//indexDB 数据库操作
const dbName = "downloadDB";
const tableName = "downloadfile";
const version = 1;

let IDB = window.indexedDB || window.mozIndexdDB || window.webkitIndexdDB || window.msIndexedDB
//打开数据库连接
let requestDB = IDB.open(dbName, version);
//事件表示打开数据库失败。
requestDB.onerror = function (e) {
  console.log(e);
};
//事件表示成功打开数据库
requestDB.onsucces = function (e) {
  let myDb = e.target.result;
  console.log(e);
};
//
requestDB.onupgradeneeded = function (e) {
  let myDb = e.target.result;
  //创建一张表
  if (!myDb.objectStoreNames.contains(tableName)) {
    var store = myDb.createObjectStore(tableName, { keyPath: 'id', autoIncrement: true });
    store.createIndex('downloadfile_id', 'id', { unique: true });
    store.createIndex('downloadfile_md5', 'md5', { unique: false });
    store.createIndex('downloadfile_index', 'index', { unique: false });
    //创建索引store.createIndex('索引名称','索引属性名',{unique:true}索引属性值是否唯一);
    store.createIndex('downloadfile_range', 'range', { unique: false });
    store.createIndex('downloadfile_3', ['md5', 'index', 'range'], { unique: false });
  }
}

//添加数据 重复的不要添加了
export function add(requestDB, data) {
  let myDb = requestDB.result;
  //验证是不是已经存储了
  var store = myDb.transaction("downloadfile", 'readwrite')
    .objectStore("downloadfile");
  var index = store.index("downloadfile_3");
  var range = IDBKeyRange.only([data.md5, data.index, data.range]);
  index.get(range).onsuccess = function (evt) {
    var res = evt.target.result;
    if (!res) {
      store.add(data);
    }
  };

}

//查询数据 
export function queryDataBymd5(requestDB, md5, callack) {
  try {
    let myDb = requestDB.result;
    let arss = [];
    if (myDb != null) {
      var store = myDb.transaction("downloadfile")
        .objectStore("downloadfile");
      var index = store.index("downloadfile_md5");
      index.openCursor(md5).onsuccess = function (evt) {
        var res = evt.target.result;
        if (res) {
          arss.push(res.value);
          // console.log(res.value);
          res.continue();
        } else {
          callack(arss);
        }
      };
    }
  }
  catch (e) {
    console.log(e);
  }
}

//和缓存里过滤 
export function requestFileStreamArrs(arss, allrss) {
  var rArrs = arss.map(function (x) {
    return x.range;
  });
  var arr = [];
  var flag = false;
  $.each(allrss, function (i, e) {
    if (rArrs.length > 0) {
      var len = rArrs.filter(function (x) {
        return x == e.range;
      });
      if (len.length == 0) {
        arr.push(e);
      }
    } else {
      flag = true;
      return false;
    }
  });

  if (flag) {
    arr = allrss;
  }
  return arr;
}

//合并文件流
export function mergeFileStream(arss, fileName) {
  function compare(propertyName) {
    return function (object1, object2) {
      var value1 = object1[propertyName];
      var value2 = object2[propertyName];
      if (value2 > value1) {
        return -1;
      }
      else if (value2 < value1) {
        return 1;
      }
      else {
        return 0;
      }
    }
  }
  let type = arss[0].content.type;
  arss = arss.sort(compare('index')).map(function (x) {
    return x.content;
  });

  //文件流
  let blob = new Blob(arss, { type: type });
  if (window.navigator.msSaveOrOpenBlob) {
    navigator.msSaveBlob(blob);
  } else {
    let elink = document.createElement("a");
    elink.download = fileName;
    elink.style.display = "none";
    elink.href = URL.createObjectURL(blob);
    document.body.appendChild(elink);
    elink.click();
    document.body.removeChild(elink);
  }
}

//查询条数
export function queryCount(requestDB,md5,callback)
{
  let myDb = requestDB.result;
  var store = myDb.transaction("downloadfile")
    .objectStore("downloadfile");
  var index = store.index("downloadfile_md5");
  var countRequest = index.count(md5);
  countRequest.onsuccess = function () {
     callback(countRequest.result);
  }

}


let indexDBService = {
  add,
  queryDataBymd5,
  queryCount,
  mergeFileStream,
  requestFileStreamArrs,
  requestDB
}
export default indexDBService

