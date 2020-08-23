using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web;

namespace NetCore.Core.Util
{
    /// <summary>
    ///     文件操作
    /// </summary>
    public static class FileUtil
    {
        #region 编码方式
        /// <summary>
        ///     编码方式
        /// </summary>
        private static readonly Encoding Encoding = Encoding.GetEncoding("utf-8");

        #endregion

        #region 递归取得文件夹下文件
        /// <summary>
        ///     递归取得文件夹下文件
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="list"></param>
        public static void GetFiles(string dir,
            List<string> list)
        {
            GetFiles(dir, list, new List<string>());
        }

        /// <summary>
        ///     递归取得文件夹下文件
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="list"></param>
        /// <param name="fileExtsions"></param>
        public static void GetFiles(string dir,
            List<string> list,
            List<string> fileExtsions)
        {
            //添加文件   
            var files = Directory.GetFiles(dir);
            if (fileExtsions.Count > 0)
            {
                foreach (var file in files)
                {
                    var extension = Path.GetExtension(file);
                    if (extension != null && fileExtsions.Contains(extension))
                    {
                        list.Add(file);
                    }
                }
            }
            else
            {
                list.AddRange(files);
            }
            //如果是目录，则递归  
            var directories = new DirectoryInfo(dir).GetDirectories();
            foreach (var item in directories)
            {
                GetFiles(item.FullName, list, fileExtsions);
            }
        }
        #endregion

        #region 写入文件
        /// <summary>
        ///     写入文件
        /// </summary>
        /// <param name="filePath">文件名</param>
        /// <param name="content">文件内容</param>
        public static void WriteFile(string filePath,
            string content)
        {
            try
            {
                var directoryPath = GetDirectoryFromPath(filePath);
                if (!string.IsNullOrWhiteSpace(directoryPath) && !Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                var fs = new FileStream(filePath, FileMode.Create);
                var encode = Encoding;
                //获得字节数组  
                var data = encode.GetBytes(content);
                //开始写入  
                fs.Write(data, 0, data.Length);
                //清空缓冲区、关闭流  
                fs.Flush();
                fs.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        #region 读取文件
        /// <summary>
        ///     读取文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ReadFile(string filePath)
        {
            return ReadFile(filePath, Encoding.Default);
        }

        /// <summary>
        ///     读取文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string ReadFile(string filePath,
            Encoding encoding)
        {
            if (!File.Exists(filePath)) return string.Empty;

            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var sr = new StreamReader(fs, encoding))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        /// <summary>
        ///     读取文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static List<string> ReadFileLines(string filePath)
        {
            var str = new List<string>();
            using (var sr = new StreamReader(filePath, Encoding))
            {
                String input;
                while ((input = sr.ReadLine()) != null)
                {
                    str.Add(input);
                }
            }
            return str;
        }
        #endregion

        #region 复制文件夹
        /// <summary>
        ///     复制文件夹（及文件夹下所有子文件夹和文件）
        /// </summary>
        /// <param name="sourcePath">待复制的文件夹路径</param>
        /// <param name="destinationPath">目标路径</param>
        public static void CopyDirectory(String sourcePath,
            String destinationPath)
        {
            var info = new DirectoryInfo(sourcePath);
            Directory.CreateDirectory(destinationPath);
            foreach (var fsi in info.GetFileSystemInfos())
            {
                var destName = Path.Combine(destinationPath, fsi.Name);

                if (fsi is FileInfo) //如果是文件，复制文件  
                    File.Copy(fsi.FullName, destName);
                else //如果是文件夹，新建文件夹，递归  
                {
                    Directory.CreateDirectory(destName);
                    CopyDirectory(fsi.FullName, destName);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetDirectoryFromPath(string path)
        {
            return path.Substring(0, path.LastIndexOf(@"\", StringComparison.Ordinal));
        }
        #endregion

        #region 删除文件夹
        /// <summary>
        ///     删除文件夹（及文件夹下所有子文件夹和文件）
        /// </summary>
        /// <param name="directoryPath"></param>
        public static void DeleteFolder(string directoryPath)
        {
            foreach (var d in Directory.GetFileSystemEntries(directoryPath))
            {
                if (File.Exists(d))
                {
                    var fi = new FileInfo(d);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly", StringComparison.Ordinal) != -1)
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete(d); //删除文件     
                }
                else
                    DeleteFolder(d); //删除文件夹  
            }
            Directory.Delete(directoryPath); //删除空文件夹  
        }
        #endregion

        #region 取得文件大小，按适当单位转换
        /// <summary>
        ///     取得文件大小，按适当单位转换
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static string GetFileSize(string filepath)
        {
            var result = "0KB";
            if (File.Exists(filepath))
            {
                var size = new FileInfo(filepath).Length;
                var filelength = size.ToString().Length;
                if (filelength < 4)
                    result = size + "byte";
                else if (filelength < 7)
                    result = Math.Round(Convert.ToDouble(size / 1024d), 2) + "KB";
                else if (filelength < 10)
                    result = Math.Round(Convert.ToDouble(size / 1024d / 1024), 2) + "MB";
                else if (filelength < 13)
                    result = Math.Round(Convert.ToDouble(size / 1024d / 1024 / 1024), 2) + "GB";
                else
                    result = Math.Round(Convert.ToDouble(size / 1024d / 1024 / 1024 / 1024), 2) + "TB";
                return result;
            }
            return result;
        }

        public static long GetLength(string filepath)
        {
            long size = 0;
            if (File.Exists(filepath))
            {
                using (FileStream file = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    size = file.Length;
                } 
            }
            return size;
        }
        #endregion

        #region 文件转换成流
        /// <summary>
        ///     文件转换成流
        /// </summary>
        /// <param name="physicalPath"></param>
        /// <returns></returns>
        public static FileStream GetStreamFrom(string physicalPath)
        {
            if (!File.Exists(physicalPath))
                return null;
            return new FileStream(physicalPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        }
        #endregion

        #region 检测指定目录是否存在
        /// <summary>
        /// 检测指定目录是否存在
        /// </summary>
        /// <param name="directoryPath">目录的绝对路径</param>
        /// <returns></returns>
        public static bool IsExistDirectory(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }
        #endregion

        #region  相对目录 检测指定目录是否存在
        /// <summary>
        /// 检测指定目录是否存在
        /// </summary>
        /// <param name="directoryPath">目录的相对目录</param>
        /// <returns></returns>
        public static bool IsExistDir(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }
        #endregion

        #region 检测指定文件是否存在,如果存在返回true
        /// <summary>
        /// 检测指定文件是否存在,如果存在则返回true。
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public static bool IsExistFile(string filePath)
        {
            return File.Exists(filePath);
        }
        #endregion

        #region 获取指定目录中的文件列表
        /// <summary>
        /// 获取指定目录中所有文件列表
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>        
        public static string[] GetFileNames(string directoryPath)
        {
            //如果目录不存在，则抛出异常
            if (!IsExistDirectory(directoryPath))
            {
                throw new FileNotFoundException();
            }

            //获取文件列表
            return Directory.GetFiles(directoryPath);
        }
        #endregion

        #region 获取指定目录中所有子目录列表,若要搜索嵌套的子目录列表,请使用重载方法.
        /// <summary>
        /// 获取指定目录中所有子目录列表,若要搜索嵌套的子目录列表,请使用重载方法.
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>        
        public static string[] GetDirectories(string directoryPath)
        {
            try
            {
                return Directory.GetDirectories(directoryPath);
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 获取指定目录及子目录中所有文件列表
        /// <summary>
        /// 获取指定目录及子目录中所有文件列表
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>
        /// <param name="isSearchChild">是否搜索子目录</param>
        public static string[] GetFileNames(string directoryPath, string searchPattern, bool isSearchChild)
        {
            //如果目录不存在，则抛出异常
            if (!IsExistDirectory(directoryPath))
            {
                throw new FileNotFoundException();
            }

            try
            {
                if (isSearchChild)
                {
                    return Directory.GetFiles(directoryPath, searchPattern, SearchOption.AllDirectories);
                }
                else
                {
                    return Directory.GetFiles(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 检测指定目录是否为空
        /// <summary>
        /// 检测指定目录是否为空
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>        
        public static bool IsEmptyDirectory(string directoryPath)
        {
            try
            {
                //判断是否存在文件
                string[] fileNames = GetFileNames(directoryPath);
                if (fileNames.Length > 0)
                {
                    return false;
                }

                //判断是否存在文件夹
                string[] directoryNames = GetDirectories(directoryPath);
                if (directoryNames.Length > 0)
                {
                    return false;
                }

                return true;
            }
            catch
            {
                //这里记录日志
                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
                return true;
            }
        }
        #endregion

        #region 检测指定目录中是否存在指定的文件
        /// <summary>
        /// 检测指定目录中是否存在指定的文件,若要搜索子目录请使用重载方法.
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>        
        public static bool Contains(string directoryPath, string searchPattern)
        {
            try
            {
                //获取指定的文件列表
                string[] fileNames = GetFileNames(directoryPath, searchPattern, false);

                //判断指定文件是否存在
                if (fileNames.Length == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
            }
        }

        /// <summary>
        /// 检测指定目录中是否存在指定的文件
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param> 
        /// <param name="isSearchChild">是否搜索子目录</param>
        public static bool Contains(string directoryPath, string searchPattern, bool isSearchChild)
        {
            try
            {
                //获取指定的文件列表
                string[] fileNames = GetFileNames(directoryPath, searchPattern, true);

                //判断指定文件是否存在
                if (fileNames.Length == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
            }
        }
        #endregion

        #region 相对目录创建目录
        /// <summary>
        /// 创建相对目录  
        /// </summary>
        /// <param name="dir">要创建的目录路径包括目录名</param>
        public static string CreateDir(string dir)
        {
            if (dir.Length == 0) return "";

            var path = dir;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;
        }
        #endregion

        #region  绝对目录 创建一个目录
        /// <summary>
        /// 创建一个目录
        /// </summary>
        /// <param name="directoryPath">目录的绝对路径</param>
        public static void CreateDirectory(string directoryPath)
        {
            //如果目录不存在则创建该目录
            if (!IsExistDirectory(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
        #endregion

        #region 删除目录
        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="dir">要删除的目录路径和名称</param>
        public static void DeleteDir(string dir)
        {
            if (dir.Length == 0) return;
            if (Directory.Exists(dir))
                Directory.Delete(dir);
        }
        #endregion

        #region 删除文件
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="file">要删除的文件路径和名称</param>
        public static void DeleteFiles(string file)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }
        #endregion

        #region 创建文件
        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="dir">带后缀的文件名</param>
        /// <param name="pagestr">文件内容</param>
        public static void CreateFile(string dir, string pagestr)
        {
            dir = dir.Replace("/", "\\");
            if (dir.IndexOf("\\") > -1)
                CreateDir(dir.Substring(0, dir.LastIndexOf("\\")));
            StreamWriter sw = new StreamWriter(dir, false, System.Text.Encoding.GetEncoding("GB2312"));
            sw.Write(pagestr);
            sw.Close();
        }
        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="content">内容</param>
        public static void CreateFileContent(string path, string content)
        {
            FileInfo fi = new FileInfo(path);
            var di = fi.Directory;
            if (!di.Exists)
            {
                di.Create();
            }
            StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.GetEncoding("GB2312"));
            sw.Write(content);
            sw.Close();
        }
        #endregion

        #region 移动文件(剪贴--粘贴)
        /// <summary>
        /// 移动文件(剪贴--粘贴)
        /// </summary>
        /// <param name="dir1">要移动的文件的路径及全名(包括后缀)</param>
        /// <param name="dir2">文件移动到新的位置,并指定新的文件名</param>
        public static void MoveFile(string dir1, string dir2)
        {
            dir1 = dir1.Replace("/", "\\");
            dir2 = dir2.Replace("/", "\\");
            if (File.Exists(dir1))
                File.Move(dir1, dir2);
        }
        #endregion

        #region 复制文件
        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="dir1">要复制的文件的路径已经全名(包括后缀)</param>
        /// <param name="dir2">目标位置,并指定新的文件名</param>
        public static void CopyFile(string dir1, string dir2)
        {
            dir1 = dir1.Replace("/", "\\");
            dir2 = dir2.Replace("/", "\\");
            if (File.Exists(dir1))
            {
                File.Copy(dir1, dir2, true);
            }
        }
        #endregion

        #region 根据时间得到目录名 / 格式:yyyyMMdd 或者 HHmmssff
        /// <summary>
        /// 根据时间得到目录名yyyyMMdd
        /// </summary>
        /// <returns></returns>
        public static string GetDateDir()
        {
            return DateTime.Now.ToString("yyyyMMdd");
        }
        /// <summary>
        /// 根据时间得到文件名HHmmssff
        /// </summary>
        /// <returns></returns>
        public static string GetDateFile()
        {
            return DateTime.Now.ToString("HHmmssff");
        }
        #endregion

        #region 根据时间获取指定路径的 后缀名的 的所有文件
        /// <summary>
        /// 根据时间获取指定路径的 后缀名的 的所有文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="Extension">后缀名 比如.txt</param>
        /// <returns></returns>
        public static DataRow[] GetFilesByTime(string path, string Extension)
        {
            if (Directory.Exists(path))
            {
                string fielExts = string.Format("*{0}", Extension);
                string[] files = Directory.GetFiles(path, fielExts);
                if (files.Length > 0)
                {
                    DataTable table = new DataTable();
                    table.Columns.Add(new DataColumn("filename", Type.GetType("System.String")));
                    table.Columns.Add(new DataColumn("createtime", Type.GetType("System.DateTime")));
                    for (int i = 0; i < files.Length; i++)
                    {
                        DataRow row = table.NewRow();
                        DateTime creationTime = File.GetCreationTime(files[i]);
                        string fileName = Path.GetFileName(files[i]);
                        row["filename"] = fileName;
                        row["createtime"] = creationTime;
                        table.Rows.Add(row);
                    }
                    return table.Select(string.Empty, "createtime desc");
                }
            }
            return new DataRow[0];
        }
        #endregion

        #region 复制文件夹
        /// <summary>
        /// 复制文件夹(递归)
        /// </summary>
        /// <param name="varFromDirectory">源文件夹路径</param>
        /// <param name="varToDirectory">目标文件夹路径</param>
        public static void CopyFolder(string varFromDirectory, string varToDirectory)
        {
            Directory.CreateDirectory(varToDirectory);

            if (!Directory.Exists(varFromDirectory)) return;

            string[] directories = Directory.GetDirectories(varFromDirectory);

            if (directories.Length > 0)
            {
                foreach (string d in directories)
                {
                    CopyFolder(d, varToDirectory + d.Substring(d.LastIndexOf("\\")));
                }
            }
            string[] files = Directory.GetFiles(varFromDirectory);
            if (files.Length > 0)
            {
                foreach (string s in files)
                {
                    File.Copy(s, varToDirectory + s.Substring(s.LastIndexOf("\\")), true);
                }
            }
        }
        #endregion

        #region 检查文件,如果文件不存在则创建
        /// <summary>
        /// 检查文件,如果文件不存在则创建  
        /// </summary>
        /// <param name="FilePath">路径,包括文件名</param>
        public static void ExistsFile(string FilePath)
        {
            //if(!File.Exists(FilePath))    
            //File.Create(FilePath);    
            //以上写法会报错,详细解释请看下文.........   
            if (!File.Exists(FilePath))
            {
                FileStream fs = File.Create(FilePath);
                fs.Close();
            }
        }
        #endregion

        #region 删除指定文件夹对应其他文件夹里的文件
        /// <summary>
        /// 删除指定文件夹对应其他文件夹里的文件
        /// </summary>
        /// <param name="varFromDirectory">指定文件夹路径</param>
        /// <param name="varToDirectory">对应其他文件夹路径</param>
        public static void DeleteFolderFiles(string varFromDirectory, string varToDirectory)
        {
            Directory.CreateDirectory(varToDirectory);

            if (!Directory.Exists(varFromDirectory)) return;

            string[] directories = Directory.GetDirectories(varFromDirectory);

            if (directories.Length > 0)
            {
                foreach (string d in directories)
                {
                    DeleteFolderFiles(d, varToDirectory + d.Substring(d.LastIndexOf("\\")));
                }
            }


            string[] files = Directory.GetFiles(varFromDirectory);

            if (files.Length > 0)
            {
                foreach (string s in files)
                {
                    File.Delete(varToDirectory + s.Substring(s.LastIndexOf("\\")));
                }
            }
        }
        #endregion

        #region 从文件的绝对路径中获取文件名( 包含扩展名 )
        /// <summary>
        /// 从文件的绝对路径中获取文件名( 包含扩展名 )
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public static string GetFileName(string filePath)
        {
            //获取文件的名称
            FileInfo fi = new FileInfo(filePath);
            return fi.Name;
        }
        #endregion

        #region 复制文件参考方法,页面中引用
        /// <summary>
        /// 复制文件参考方法,页面中引用
        /// </summary>
        /// <param name="cDir">新路径</param>
        /// <param name="TempId">模板引擎替换编号</param>
        public static void CopyFiles(string cDir, string TempId)
        {
            //if (Directory.Exists(Request.PhysicalApplicationPath + "\\Controls"))
            //{
            //    string TempStr = string.Empty;
            //    StreamWriter sw;
            //    if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\Default.aspx"))
            //    {
            //        TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\Default.aspx");
            //        TempStr = TempStr.Replace("{$ChannelId$}", TempId);

            //        sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\Default.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
            //        sw.Write(TempStr);
            //        sw.Close();
            //    }
            //    if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\Column.aspx"))
            //    {
            //        TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\Column.aspx");
            //        TempStr = TempStr.Replace("{$ChannelId$}", TempId);

            //        sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\List.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
            //        sw.Write(TempStr);
            //        sw.Close();
            //    }
            //    if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\Content.aspx"))
            //    {
            //        TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\Content.aspx");
            //        TempStr = TempStr.Replace("{$ChannelId$}", TempId);

            //        sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\View.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
            //        sw.Write(TempStr);
            //        sw.Close();
            //    }
            //    if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\MoreDiss.aspx"))
            //    {
            //        TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\MoreDiss.aspx");
            //        TempStr = TempStr.Replace("{$ChannelId$}", TempId);

            //        sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\DissList.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
            //        sw.Write(TempStr);
            //        sw.Close();
            //    }
            //    if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\ShowDiss.aspx"))
            //    {
            //        TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\ShowDiss.aspx");
            //        TempStr = TempStr.Replace("{$ChannelId$}", TempId);

            //        sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\Diss.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
            //        sw.Write(TempStr);
            //        sw.Close();
            //    }
            //    if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\Review.aspx"))
            //    {
            //        TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\Review.aspx");
            //        TempStr = TempStr.Replace("{$ChannelId$}", TempId);

            //        sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\Review.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
            //        sw.Write(TempStr);
            //        sw.Close();
            //    }
            //    if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\Search.aspx"))
            //    {
            //        TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\Search.aspx");
            //        TempStr = TempStr.Replace("{$ChannelId$}", TempId);

            //        sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\Search.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
            //        sw.Write(TempStr);
            //        sw.Close();
            //    }
            //}
        }
        #endregion

        #region 创建一个文件
        /// <summary>
        /// 创建一个文件。
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static void CreateFile(string filePath)
        {
            try
            {
                //如果文件不存在则创建该文件
                if (!IsExistFile(filePath))
                {
                    //创建一个FileInfo对象
                    FileInfo file = new FileInfo(filePath);

                    //创建文件
                    FileStream fs = file.Create();

                    //关闭文件流
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 创建一个文件,并将字节流写入文件。
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="buffer">二进制流数据</param>
        public static void CreateFile(string filePath, byte[] buffer)
        {
            try
            {
                //如果文件不存在则创建该文件
                if (!IsExistFile(filePath))
                {
                    //创建一个FileInfo对象
                    FileInfo file = new FileInfo(filePath);

                    //创建文件
                    FileStream fs = file.Create();

                    //写入二进制流
                    fs.Write(buffer, 0, buffer.Length);

                    //关闭文件流
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
                throw ex;
            }
        }
        #endregion

        #region 获取文本文件的行数
        /// <summary>
        /// 获取文本文件的行数
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public static int GetLineCount(string filePath)
        {
            //将文本文件的各行读到一个字符串数组中
            string[] rows = File.ReadAllLines(filePath);

            //返回行数
            return rows.Length;
        }
        #endregion

        #region 获取指定目录中的子目录列表
        /// <summary>
        /// 获取指定目录及子目录中所有子目录列表
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>
        /// <param name="isSearchChild">是否搜索子目录</param>
        public static string[] GetDirectories(string directoryPath, string searchPattern, bool isSearchChild)
        {
            try
            {
                if (isSearchChild)
                {
                    return Directory.GetDirectories(directoryPath, searchPattern, SearchOption.AllDirectories);
                }
                else
                {
                    return Directory.GetDirectories(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 向文本文件写入内容

        /// <summary>
        /// 向文本文件中写入内容
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="text">写入的内容</param>
        /// <param name="encoding">编码</param>
        public static void WriteText(string filePath, string text, Encoding encoding)
        {
            //向文件写入内容
            File.WriteAllText(filePath, text, encoding);
        }
        #endregion

        #region 向文本文件的尾部追加内容
        /// <summary>
        /// 向文本文件的尾部追加内容
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="content">写入的内容</param>
        public static void AppendText(string filePath, string content)
        {
            File.AppendAllText(filePath, content);
        }
        #endregion

        #region 将现有文件的内容复制到新文件中
        /// <summary>
        /// 将源文件的内容复制到目标文件中
        /// </summary>
        /// <param name="sourceFilePath">源文件的绝对路径</param>
        /// <param name="destFilePath">目标文件的绝对路径</param>
        public static void Copy(string sourceFilePath, string destFilePath)
        {
            File.Copy(sourceFilePath, destFilePath, true);
        }
        #endregion

        #region 将文件移动到指定目录
        /// <summary>
        /// 将文件移动到指定目录
        /// </summary>
        /// <param name="sourceFilePath">需要移动的源文件的绝对路径</param>
        /// <param name="descDirectoryPath">移动到的目录的绝对路径</param>
        public static void Move(string sourceFilePath, string descDirectoryPath)
        {
            //获取源文件的名称
            string sourceFileName = GetFileName(sourceFilePath);

            if (IsExistDirectory(descDirectoryPath))
            {
                //如果目标中存在同名文件,则删除
                if (IsExistFile(descDirectoryPath + "\\" + sourceFileName))
                {
                    DeleteFiles(descDirectoryPath + "\\" + sourceFileName);
                }
                //将文件移动到指定目录
                File.Move(sourceFilePath, descDirectoryPath + "\\" + sourceFileName);
            }
        }
        #endregion

        #region 从文件的绝对路径中获取文件名( 不包含扩展名 )
        /// <summary>
        /// 从文件的绝对路径中获取文件名( 不包含扩展名 )
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public static string GetFileNameNoExtension(string filePath)
        {
            //获取文件的名称
            FileInfo fi = new FileInfo(filePath);
            return fi.Name.Split('.')[0];
        }
        #endregion

        #region 从文件的绝对路径中获取扩展名
        /// <summary>
        /// 从文件的绝对路径中获取扩展名
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public static string GetExtension(string filePath)
        {
            //获取文件的名称
            FileInfo fi = new FileInfo(filePath);
            return fi.Extension;
        }
        #endregion

        #region 清空文件内容
        /// <summary>
        /// 清空文件内容
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static void ClearFile(string filePath)
        {
            //删除文件
            File.Delete(filePath);

            //重新创建该文件
            CreateFile(filePath);
        }
        #endregion




    }
}
