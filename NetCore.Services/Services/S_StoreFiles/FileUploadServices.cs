using Microsoft.AspNetCore.Http;
using NetCore.Core.EntityModel.ReponseModels;
using NetCore.Core.Enum;
using NetCore.Core.Extensions;
using NetCore.Core.Util;
using NetCore.Domain.Interface;
using NetCore.DTO.ReponseViewModel.FileUpload;
using NetCore.DTO.ReponseViewModel.StoreFiles;
using NetCore.DTO.RequestViewModel.FileUpload;
using NetCore.EntityFrameworkCore.Models;
using NetCore.Services.IServices.I_StoreFiles;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Services.Services.S_StoreFiles
{
    public class FileUploadServices : IFileUploadServices
    {
        private readonly IBaseDomain<StoreFiles> _baseDomain;

        public FileUploadServices(IBaseDomain<StoreFiles> baseDomain)
        {
            _baseDomain = baseDomain;
        }

        public HttpReponseViewModel<FileUploadResViewModel> CheckFileState(HttpRequest request)
        {
            FileUploadReqViewModel fileUpload = new FileUploadReqViewModel();
            if (request.Query.Count() > 0)
            {
                foreach (var item in request.Query.ToList())
                {
                    if (item.Key == "chunkNumber")
                    {
                        fileUpload.ChunkNumber = Convert.ToInt32(item.Value);
                    }
                    if (item.Key == "chunkSize")
                    {
                        fileUpload.ChunkSize = Convert.ToInt32(item.Value);
                    }
                    if (item.Key == "currentChunkSize")
                    {
                        fileUpload.CurrentChunkSize = Convert.ToInt32(item.Value);
                    }
                    if (item.Key == "totalSize")
                    {
                        fileUpload.TotalSize = Convert.ToInt32(item.Value);
                    }
                    if (item.Key == "identifier")
                    {
                        fileUpload.Identifier = item.Value;
                    }
                    if (item.Key == "filename")
                    {
                        fileUpload.FileName = item.Value;
                    }
                    if (item.Key == "relativePath")
                    {
                        fileUpload.RelativePath = item.Value;
                    }
                    if (item.Key == "totalChunks")
                    {
                        fileUpload.TotalChunks = Convert.ToInt32(item.Value);
                    }
                    if (item.Key == "fileExt")
                    {
                        fileUpload.FileExt = item.Value;
                    }
                    if (item.Key == "fileType")
                    {
                        fileUpload.FileType = item.Value;
                    }
                    if (item.Key == "fileCategory")
                    {
                        fileUpload.FileCategory = item.Value;
                    }
                }
            }

            var webRoot = AppConfigUtil.FilePath;

            FileUploadResViewModel fileUploadRes = new FileUploadResViewModel();
            fileUploadRes.FState = 0;//新文件

            var md5Folder = FileUploadUtil.GetFileMd5Folder(webRoot, fileUpload.Identifier);
            //判断这个md5临时文件夹存不存在  存在证明可以用断点续传 返回已经上传分片文件
            if (Directory.Exists(md5Folder))
            {
                fileUploadRes.FState = 1;//可以断点续传
                fileUploadRes.Uploaded = FileUploadUtil.GetSourcePathGetFilesNames(md5Folder);
            }
            else
            {
                //判断改文件存不存在
                var path = FileUploadUtil.GetPath(webRoot) + fileUpload.FileName;
                if (System.IO.File.Exists(path))
                {
                    //判断该文件是不是已经上传过了  上传过了就秒传  直接拿已经存在的地址url
                    var fileMd5 = FileUploadUtil.GetMD5HashFromFile(path);
                    if (fileMd5.Equals(fileUpload.Identifier))
                    {
                        fileUploadRes.FState = 2;//已经存在该文件
                        fileUploadRes.SecondTransmission = true;
                        fileUploadRes.FilePathUrl = path;
                    }
                }
            }
            return new HttpReponseViewModel<FileUploadResViewModel>
            {
                Code = 20000,
                Data = fileUploadRes,
                Flag = true
            };
        }



        public async Task<HttpReponseViewModel<FileUploadResViewModel>> ChunkUpload(FileUploadReqViewModel fileUpload)
        {
            HttpReponseViewModel<FileUploadResViewModel> res = new HttpReponseViewModel<FileUploadResViewModel>();
            FileUploadResViewModel viewModel = new FileUploadResViewModel()
            {
                NeedMerge = false,
                FileName = fileUpload.FileName,
                Identifier = fileUpload.Identifier
            };
            var md5Folder = FileUploadUtil.GetFileMd5Folder("", fileUpload.Identifier);
            var filePath = "";  // 要保存的文件路径// 存在分片参数,并且，最大的片数大于1片时     
            if (fileUpload.TotalChunks > 1)
            {
                var uploadNumsOfLoop = 10;
                // 是10的倍数就休眠几秒（数据库设置的秒数）
                if (fileUpload.ChunkNumber % uploadNumsOfLoop == 0)
                {
                    var timesOfLoop = 10;   //休眠毫秒,可从数据库取值
                    Thread.Sleep(timesOfLoop);
                }
                //建立临时传输文件夹
                if (!Directory.Exists(md5Folder))
                {
                    Directory.CreateDirectory(md5Folder);
                }
                filePath = md5Folder + "/" + fileUpload.ChunkNumber;

                if (fileUpload.TotalChunks == fileUpload.ChunkNumber)
                {
                    viewModel.NeedMerge = true;
                }
                else
                {
                    viewModel.NeedMerge = false;
                }
            }
            else
            {
                var qfileName = fileUpload?.FileName;
                //没有分片直接保存
                filePath = md5Folder + Path.GetExtension(qfileName);
                viewModel.NeedMerge = true;
            }

            // 写入文件
            using (var addFile = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                if (fileUpload.File != null)
                {
                    await fileUpload.File.CopyToAsync(addFile);
                }
            }
            res.Data = viewModel;
            return res;
        }

        public async Task<HttpReponseViewModel<FileUploadResViewModel>> ChunkUpload(IFormFile file, HttpRequest request)
        {
            HttpReponseViewModel<FileUploadResViewModel> res = new HttpReponseViewModel<FileUploadResViewModel>();
            FileUploadReqViewModel fileUpload = new FileUploadReqViewModel();
            foreach (var item in request.Form.Keys)
            {
                if (item == "chunkNumber")
                {
                    fileUpload.ChunkNumber = Convert.ToInt32(request.Form["chunkNumber"].ToString());
                }
                if (item == "identifier")
                {
                    fileUpload.Identifier = request.Form["identifier"].ToString();
                }
                if (item == "totalChunks")
                {
                    fileUpload.TotalChunks = Convert.ToInt32(request.Form["totalChunks"].ToString());
                }
                if (item == "filename")
                {
                    fileUpload.FileName = request.Form["filename"].ToString();
                }
                if (item == "chunkSize")
                {
                    fileUpload.ChunkSize = Convert.ToInt32(request.Form["chunkSize"].ToString());
                }
                if (item == "currentChunkSize")
                {
                    fileUpload.CurrentChunkSize = Convert.ToInt32(request.Form["currentChunkSize"].ToString());
                }
                if (item == "totalSize")
                {
                    fileUpload.TotalSize = Convert.ToInt32(request.Form["totalSize"].ToString());
                }
                if (item == "relativePath")
                {
                    fileUpload.RelativePath = request.Form["relativePath"].ToString();
                }
                if (item == "fileExt")
                {
                    fileUpload.FileExt = request.Form["fileExt"].ToString();
                }
                if (item == "fileType")
                {
                    fileUpload.FileType  = request.Form["fileType"].ToString();
                }
                if (item == "fileCategory")
                {
                    fileUpload.FileCategory = request.Form["fileCategory"].ToString();
                }
            }
            fileUpload.File = file;

            FileUploadResViewModel viewModel = new FileUploadResViewModel()
            {
                NeedMerge = false,
                FileName = fileUpload.FileName,
                Identifier = fileUpload.Identifier
            };
            var webRoot = AppConfigUtil.FilePath;
            var md5Folder = FileUploadUtil.GetFileMd5Folder(webRoot, fileUpload.Identifier);

            //建立临时传输文件夹
            if (!Directory.Exists(md5Folder))
            {
                Directory.CreateDirectory(md5Folder);
            }
            var filePath = "";  // 要保存的文件路径// 存在分片参数,并且，最大的片数大于1片时     
            if (fileUpload.TotalChunks > 1)
            {
                var uploadNumsOfLoop = 10;
                // 是10的倍数就休眠几秒（数据库设置的秒数）
                if (fileUpload.ChunkNumber % uploadNumsOfLoop == 0)
                {
                    var timesOfLoop = 10;   //休眠毫秒,可从数据库取值
                    Thread.Sleep(timesOfLoop);
                }

                filePath = md5Folder + "/" + fileUpload.ChunkNumber;
                if (fileUpload.TotalChunks == fileUpload.ChunkNumber)
                {
                    viewModel.NeedMerge = true;
                }
                else
                {
                    viewModel.NeedMerge = false;
                }
            }
            else
            {
                var qfileName = fileUpload?.FileName;
                //没有分片直接保存
                filePath = md5Folder + Path.GetExtension(qfileName);
                viewModel.NeedMerge = true;
            }

            // 写入文件
            using (var addFile = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                if (fileUpload.File != null)
                {
                    await fileUpload.File.CopyToAsync(addFile);
                }
            }
            res.Data = viewModel;
            res.Code = 20000;
            return res;
        }

        public HttpReponseViewModel<int> GetMaxChunk(FileUploadCheckChunkViewModel model)
        {
            HttpReponseViewModel<int> res = new HttpReponseViewModel<int>();
            try
            {
                var webRootPath = model.RootPath;
                // 检测文件夹是否存在，不存在则创建
                var userPath = FileUploadUtil.GetPath(webRootPath);
                if (!Directory.Exists(userPath))
                {
                    FileUploadUtil.DicCreate(userPath);
                }

                var md5Folder = FileUploadUtil.GetFileMd5Folder(webRootPath, model.Md5);
                if (!Directory.Exists(md5Folder))
                {
                    FileUploadUtil.DicCreate(md5Folder);
                }
                var fileName = model.Md5 + "." + model.Ext;
                string targetPath = Path.Combine(md5Folder, fileName);
                // 文件已经存在，则可能存在问题，直接删除，重新上传
                if (File.Exists(targetPath))
                {
                    File.Delete(targetPath);
                }
                var dicInfo = new DirectoryInfo(md5Folder);
                var files = dicInfo.GetFiles();
                var chunk = files.Count();
                if (chunk > 1)
                {
                    //当文件上传中时，页面刷新，上传中断，这时最后一个保存的块的大小可能会有异常，所以这里直接删除最后一个块文件                  
                    res.Data = (chunk - 1);
                }
                res.Flag = true;
            }
            catch (Exception ex)
            {
                var errMsg = ex.Message;
                res.Flag = false;
            }
            res.Code = 20000;
            return res;
        }



        public async Task<HttpReponseViewModel<StoreFilesViewModel>> MergeFiles(FileUploadReqViewModel fileUpload)
        {
            HttpReponseViewModel<StoreFilesViewModel> res = new HttpReponseViewModel<StoreFilesViewModel>();
            try
            {
                var identifier = fileUpload.Identifier;
                var fileName = fileUpload.FileName;
                var webRoot = AppConfigUtil.FilePath;
                //源数据文件夹
                string sourcePath = FileUploadUtil.GetFileMd5Folder(webRoot, identifier);
                //合并后的文件路径
                string targetFilePath = sourcePath + Path.GetExtension(fileName);

                // 目标文件不存在，则需要合并
                if (!File.Exists(targetFilePath))
                {
                    if (!Directory.Exists(sourcePath))
                    {
                        res.Message = "为找到文件";
                        res.ResultSign = ResultSign.Error;
                        res.Code = 20000;
                        return res;
                    }
                    FileUploadUtil.MergeDiskFile(sourcePath, targetFilePath);
                }
                fileUpload.RelativePath = targetFilePath;
                var valid = VaildMergeFile(fileUpload);
                FileUploadUtil.DeleteFolder(sourcePath);

                //返回文件新的路径
                var newFilePath = FileUploadUtil.newFilePath(webRoot, targetFilePath, fileName);
                //存储成功
                if (newFilePath != "")
                {
                    StoreFiles storeFiles = fileUpload.MapTo<StoreFiles>();
                    storeFiles.ID = Guid.NewGuid();
                    storeFiles.RelationFilePath = newFilePath;
                    storeFiles.CreateBy = "";
                    storeFiles.FileName = fileName.Replace("." + storeFiles.FileExt, "");
                    res.Flag = await _baseDomain.AddDomain(storeFiles);
                    if (res.Flag)
                    {
                        res.ResultSign = ResultSign.Successful;
                        res.Data = storeFiles.MapTo<StoreFilesViewModel>();
                    }
                    else
                    {
                        res.ResultSign = ResultSign.Error;
                    }
                }
                res.Code = 20000;

                return res;
            }
            catch (Exception ex)
            {
                return res;
            }


        }



        public bool VaildMergeFile(FileUploadReqViewModel chunkFile)
        {
            var clientFileName = chunkFile.FileName;
            // 文件字节总数
            var fileTotalSize = chunkFile.TotalSize;
            var targetFile = new FileInfo(chunkFile.RelativePath);
            var streamTotalSize = targetFile.Length;
            try
            {
                if (streamTotalSize != fileTotalSize)
                {
                    throw new Exception("[" + clientFileName + "]文件上传时发生损坏，请重新上传");
                }

                // 对文件进行 MD5 唯一验证
                var identifier = chunkFile.Identifier;
                var fileMd5 = FileUploadUtil.GetMD5HashFromFile(chunkFile.RelativePath);
                if (!fileMd5.Equals(identifier))
                {
                    throw new Exception("[" + clientFileName + "],文件MD5值不对等");
                }
                return true;
            }
            catch (Exception ex)
            {
                // 删除本地错误文件
                System.IO.File.Delete(chunkFile.RelativePath);
                return false;
            }
        }


    }
}
