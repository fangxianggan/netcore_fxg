using Microsoft.AspNetCore.Http;
using NetCore.Core.EntityModel.ReponseModels;
using NetCore.Core.Extensions;
using NetCore.Core.Util;
using NetCore.Domain.Interface;
using NetCore.DTO.ReponseViewModel.FileUpload;
using NetCore.DTO.RequestViewModel.FileUpload;
using NetCore.EntityFrameworkCore.Models;
using NetCore.Services.IServices;
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

        public HttpReponseViewModel<FileUploadResViewModel> CheckFileState(FileUploadReqViewModel fileUpload)
        {
            FileUploadResViewModel fileUploadRes = new FileUploadResViewModel();
            fileUploadRes.FState = 0;//新文件
            var md5Folder = FileUploadUtil.GetFileMd5Folder("", fileUpload.Identifier);
            //判断这个md5临时文件夹存不存在  存在证明可以用断点续传 返回已经上传分片文件
            if (Directory.Exists(md5Folder))
            {
                fileUploadRes.FState = 1;//可以断点续传
                fileUploadRes.Uploaded = FileUploadUtil.GetSourcePathGetFilesNames(md5Folder);
            }
            else
            {
                //判断改文件存不存在
                var path = FileUploadUtil.GetPath("") + fileUpload.FileName;
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
            string identifier = "";
            int chunkNumber = 0;
            int totalChunks = 0;
            string fileName = "";
            int currentChunkSize = 0;
            int chunkSize = 0;
            int totalSize = 0;
            string relativePath = "";
            foreach (var item in request.Form.Keys)
            {
                if (item == "chunkNumber")
                {
                    chunkNumber = Convert.ToInt32(request.Form["chunkNumber"].ToString());
                }
                if (item == "identifier")
                {
                    identifier = request.Form["identifier"].ToString();
                }
                if (item == "totalChunks")
                {
                    totalChunks = Convert.ToInt32(request.Form["totalChunks"].ToString());
                }
                if (item == "filename")
                {
                    fileName = request.Form["filename"].ToString();
                }
                if (item == "chunkSize")
                {
                    chunkSize = Convert.ToInt32(request.Form["chunkSize"].ToString());
                }
                if (item == "currentChunkSize")
                {
                    currentChunkSize = Convert.ToInt32(request.Form["currentChunkSize"].ToString());
                }
                if (item == "totalSize")
                {
                    totalSize = Convert.ToInt32(request.Form["totalSize"].ToString());
                }
                if (item == "relativePath")
                {
                    relativePath = request.Form["relativePath"].ToString();
                }
            }

            FileUploadReqViewModel fileUpload = new FileUploadReqViewModel();
            fileUpload.Identifier = identifier;
            fileUpload.FileName = fileName;
            fileUpload.ChunkNumber = chunkNumber;
            fileUpload.ChunkSize = chunkSize;
            fileUpload.CurrentChunkSize = currentChunkSize;
            fileUpload.RelativePath = relativePath;
            fileUpload.TotalChunks = totalChunks;
            fileUpload.TotalSize = totalSize;
            fileUpload.File = file;

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
            using (var addFile = new FileStream(filePath, FileMode.OpenOrCreate,FileAccess.ReadWrite,FileShare.ReadWrite))
            {
                if (fileUpload.File != null)
                {
                    await fileUpload.File.CopyToAsync(addFile);
                }
            }
            res.Data = viewModel;
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
                if (System.IO.File.Exists(targetPath))
                {
                    System.IO.File.Delete(targetPath);

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



        public HttpReponseViewModel<FileUploadResViewModel> MergeFiles(FileUploadReqViewModel fileUpload)
        {
            HttpReponseViewModel<FileUploadResViewModel> res = new HttpReponseViewModel<FileUploadResViewModel>();
            try
            {
                var identifier = fileUpload.Identifier;
                var fileName = fileUpload.FileName;

                //源数据文件夹
                string sourcePath = FileUploadUtil.GetFileMd5Folder("", identifier);
                //合并后的文件路径
                string targetFilePath = sourcePath + Path.GetExtension(fileName);

                // 目标文件不存在，则需要合并
                if (!System.IO.File.Exists(targetFilePath))
                {
                    if (!Directory.Exists(sourcePath))
                    {
                        res.Message = "为找到文件";
                        res.Code = 20000;
                        return res;
                    }
                    FileUploadUtil.MergeDiskFile(sourcePath, targetFilePath);
                }
                fileUpload.RelativePath = targetFilePath;
                var valid = this.VaildMergeFile(fileUpload);
                FileUploadUtil.DeleteFolder(sourcePath);

                //返回文件新的路径
                var newFilePath = FileUploadUtil.newFilePath("", targetFilePath, fileName);
                //存储成功
                if (newFilePath != "")
                {
                    StoreFiles storeFiles = fileUpload.MapTo<StoreFiles>();
                    storeFiles.ID = Guid.NewGuid();
                    storeFiles.RelationFilePath = newFilePath;
                    storeFiles.CreateBy = "";
                    _baseDomain.AddDomain(storeFiles);
                }

                res.Code = 20000;
                res.Data = new FileUploadResViewModel()
                {
                    FilePathUrl = targetFilePath
                };
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
