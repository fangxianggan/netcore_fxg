using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetCore.Core.Util
{
    /// <summary>
    /// 文件断点续传类
    /// </summary>
    public class FileUploadUtil
    {

        /// <summary>
        /// 删除文件夹及其内容
        /// <para>附带删除超过一个月的文件以及文件夹</para>
        /// </summary>
        /// <param name="strPath"></param>
        public static void DeleteFolder(string strPath)
        {
            if (Directory.Exists(strPath))
                Directory.Delete(strPath, true);

            #region 删除一个月以前的临时文件夹与文件
            var chunkTemp = Path.GetDirectoryName(strPath);
            DirectoryInfo dir = new DirectoryInfo(chunkTemp);
            DirectoryInfo[] dii = dir.GetDirectories();
            // 超过一个月的文件夹和文件
            var expireDate = DateTime.Now.AddMonths(-1);
            var deleteExpire = dii.Where(t => t.LastWriteTime < expireDate).ToList();
            if (deleteExpire.Any())
            {
                foreach (var item in deleteExpire)
                {
                    FileUtil.DeleteFolder(chunkTemp + "/" + item);
                    //Directory.Delete(chunkTemp + "/" + item, true);
                }
            }

            var deleteExpireFile = dir.GetFiles().Where(t => t.LastWriteTime < expireDate).ToList();
            if (deleteExpireFile.Any())
            {
                foreach (var item in deleteExpireFile)
                {
                    FileUtil.DeleteFiles(chunkTemp + "/" + item);
                }
            }
            #endregion
        }

        /// <summary>
        /// 文件上传后调用自有业务 
        /// </summary>
        /// <param name="targetPath"></param>
        /// <returns>返回文件对象</returns>
        public static string newFilePath(string webRootPath, string targetPath, string fileName)
        {

            var filePath = GetPath(webRootPath);
            if (!Directory.Exists(filePath))
            {
                DicCreate(filePath);
            }

            // 移动文件            
            var file = new FileInfo(targetPath);
            var newFilePath = filePath + fileName;
            if (System.IO.File.Exists(newFilePath))
            {
                System.IO.File.Delete(newFilePath);
            }
            file.MoveTo(newFilePath);

            return newFilePath;
        }

        /// <summary>
        /// C#获取文件MD5值方法
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetMD5HashFromFile(string filePath)
        {
            try
            {
                FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }

        /// <summary>
        /// 校验合并后的文件
        /// <para>1.是否没有漏掉块(chunk)</para>
        /// <para>2.检测文件大小是否跟客户端一样</para>
        /// <para>3.检查文件的MD5值是否一致</para>
        /// </summary>
        /// <param name="targetPath"></param>
        /// <returns></returns>


        /// <summary>
        /// 将磁盘上的切片源合并成一个文件
        /// <returns>返回所有切片文件的字节总和</returns>
        /// </summary>
        /// <param name="sourcePath">磁盘上的切片源</param>
        /// <param name="targetPath">目标文件路径</param>
        public static int MergeDiskFile(string sourcePath, string targetPath)
        {
            FileStream addFile = null;
            BinaryWriter addWriter = null;
            try
            {
                var streamTotalSize = 0;
                addFile = new FileStream(targetPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write);
                addWriter = new BinaryWriter(addFile);
                // 获取目录下所有的切片文件块
                FileInfo[] files = new DirectoryInfo(sourcePath).GetFiles();
                // 按照文件名(数字)进行排序
                var orderFileInfoList = files.OrderBy(f => int.Parse(f.Name));
                foreach (FileInfo diskFile in orderFileInfoList)
                {
                    //获得上传的分片数据流 
                    Stream stream = diskFile.Open(FileMode.Open,FileAccess.Read,FileShare.Read);
                    BinaryReader tempReader = new BinaryReader(stream);
                    var streamSize = (int)stream.Length;
                    //将上传的分片追加到临时文件末尾
                    addWriter.Write(tempReader.ReadBytes(streamSize));
                    streamTotalSize += streamSize;
                    //关闭BinaryReader文件阅读器
                    tempReader.Close();
                    stream.Close();

                    tempReader.Dispose();
                    stream.Dispose();
                }
                addWriter.Close();
                addFile.Close();
                addWriter.Dispose();
                addFile.Dispose();
                return streamTotalSize;
            }
            catch (Exception ex)
            {
                if (addFile != null)
                {
                    addFile.Close();
                    addFile.Dispose();
                }
                if (addWriter != null)
                {
                    addWriter.Close();
                    addWriter.Dispose();
                }
                throw ex;
            }
        }

        public static List<int> DelLastSourceFileRef(string sourcePath)
        {
            List<int> ids = new List<int>();
            FileInfo[] files = new DirectoryInfo(sourcePath).GetFiles();
            // 按照文件名(数字)进行排序
            var orderFileInfoList = files.OrderBy(f => int.Parse(f.Name));
            foreach (FileInfo diskFile in orderFileInfoList)
            {
                int index = Convert.ToInt32(diskFile.Name);
                ids.Add(index);
            }
            if (ids.Count > 0)
            {
                var max = ids.Max();
                ids.Remove(max);
            }
            return ids;
        }

        /// <summary>
        /// 获取已经上传的分片文件名称
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <returns></returns>
        public static List<int> GetSourcePathGetFilesNames(string sourcePath)
        {
            List<int> ids = new List<int>();
            FileInfo[] files = new DirectoryInfo(sourcePath).GetFiles();
            // 按照文件名(数字)进行排序
            var orderFileInfoList = files.OrderBy(f => int.Parse(f.Name));
            foreach (FileInfo diskFile in orderFileInfoList)
            {
                int index = Convert.ToInt32(diskFile.Name);
                ids.Add(index);
            }
            return ids;
        }
        /// <summary>
        /// 获得文件MD5文件夹
        /// </summary>
        /// <returns></returns>
        public static string GetFileMd5Folder(string webRootPath, string identifier)
        {
            if (string.IsNullOrEmpty(identifier))
            {
                throw new Exception("缺少文件MD5值");
            }

            string root = GetPath(webRootPath);
            return root + "ChunkTemp\\" + identifier;
        }

        /// <summary>
        /// 获得上传文件目录
        /// </summary>
        /// <returns></returns>
        public static string GetPath(string webRootPath)
        {
            return webRootPath + "\\Files\\";
        }

        /// <summary>
        /// 文件目录如果不存在，就创建一个新的目录
        /// </summary>
        /// <param name="path"></param>
        public static void DicCreate(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
