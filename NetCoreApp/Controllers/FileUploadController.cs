using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Xml;
using NetCore.Core.EntityModel.ReponseModels;
using NetCore.Core.Util;
using NetCore.DTO.ReponseViewModel;
using NetCore.DTO.ReponseViewModel.FileUpload;
using NetCore.DTO.RequestViewModel;
using NetCore.DTO.RequestViewModel.FileUpload;
using NetCore.DTO.TestModel;
using NetCore.Services.IServices;
using NetCore.Services.IServices.I_StoreFiles;

namespace NetCoreApp.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("dev-api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private static IHostingEnvironment _hostingEnvironment;
        private readonly IFileUploadServices _fileUploadServices;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostingEnvironment"></param>
        public FileUploadController(IHostingEnvironment hostingEnvironment, IFileUploadServices fileUploadServices)
        {
            _hostingEnvironment = hostingEnvironment;
            _fileUploadServices = fileUploadServices;
        }
        /// <summary>
        /// Form表单之单文件上传
        /// </summary>
        /// <param name="formFile">form表单文件流信息</param>
        /// <returns></returns>
        /// 
        [HttpPost, Route("FormSingleFileUpload")]
        public JsonResult FormSingleFileUpload(IFormFile formFile)
        {
            var currentDate = DateTime.Now;
            var webRootPath = _hostingEnvironment.WebRootPath;//>>>相当于HttpContext.Current.Server.MapPath("") 
            try
            {
                var filePath = $"/UploadFile/{currentDate:yyyyMMdd}/";

                //创建每日存储文件夹
                if (!Directory.Exists(webRootPath + filePath))
                {
                    Directory.CreateDirectory(webRootPath + filePath);
                }
                if (formFile != null)
                {
                    //文件后缀
                    var fileExtension = Path.GetExtension(formFile.FileName);//获取文件格式，拓展名

                    //判断文件大小
                    var fileSize = formFile.Length;

                    if (fileSize > 1024 * 1024 * 10) //10M TODO:(1mb=1024X1024b)
                    {
                        return new JsonResult(new { isSuccess = false, resultMsg = "上传的文件不能大于10M" });
                    }

                    //保存的文件名称(以名称和保存时间命名)
                    var saveName = formFile.FileName.Substring(0, formFile.FileName.LastIndexOf('.')) + "_" + currentDate.ToString("HHmmss") + fileExtension;

                    //文件保存
                    using (var fs = System.IO.File.Create(webRootPath + filePath + saveName))
                    {
                        formFile.CopyTo(fs);
                        fs.Flush();
                    }

                    //完整的文件路径
                    var completeFilePath = Path.Combine(filePath, saveName);

                    return new JsonResult(new { isSuccess = true, returnMsg = "上传成功", completeFilePath = completeFilePath });
                }
                else
                {
                    return new JsonResult(new { isSuccess = false, resultMsg = "上传失败，未检测上传的文件信息~" });
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new { isSuccess = false, resultMsg = "文件保存失败，异常信息为：" + ex.Message });
            }

        }



        #region 文件分片上传


        /// <summary>
        /// 获取指定文件的已上传的最大文件块
        /// </summary>
        /// <param name="md5">文件唯一值</param>
        /// <param name="ext">文件后缀</param>
        /// <returns></returns>
        [HttpGet, Route("GetMaxChunk")]
        public async Task<HttpReponseViewModel<int>> GetMaxChunk(string md5, string ext)
        {
            FileUploadCheckChunkViewModel model = new FileUploadCheckChunkViewModel();
            model.Ext = ext;
            model.Md5 = md5;
            model.RootPath = _hostingEnvironment.WebRootPath;
            return await Task.Run(() =>
            {
                return _fileUploadServices.GetMaxChunk(model);
            });
        }

        /// <summary>
        /// 检查 断点续传 和 秒传
        /// </summary>
        /// <param name="chunkNumber"></param>
        /// <param name="chunkSize"></param>
        /// <param name="currentChunkSize"></param>
        /// <param name="totalSize"></param>
        /// <param name="identifier"></param>
        /// <param name="filename"></param>
        /// <param name="relativePath"></param>
        /// <param name="totalChunks"></param>
        /// <returns></returns>
        [HttpGet, Route("ChunkUpload")]
        public async Task<HttpReponseViewModel<FileUploadResViewModel>> ChunkUpload(int chunkNumber, int chunkSize, int currentChunkSize, int totalSize, string identifier, string filename, string relativePath, int totalChunks)
        {
            FileUploadReqViewModel fileUpload = new FileUploadReqViewModel();
            fileUpload.Identifier = identifier;
            fileUpload.FileName = filename;
            fileUpload.ChunkNumber = chunkNumber;
            fileUpload.ChunkNumber = chunkSize;
            fileUpload.CurrentChunkSize = currentChunkSize;
            fileUpload.RelativePath = relativePath;
            fileUpload.TotalChunks = totalChunks;
            fileUpload.TotalSize = totalSize;
            return await Task.Run(() =>
            {
                return _fileUploadServices.CheckFileState(fileUpload);
            });
        }

        /// <summary>
        /// 文件分块上传
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost, Route("ChunkUpload")]
        public async Task<HttpReponseViewModel<FileUploadResViewModel>> ChunkUpload(IFormFile file)
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

            try
            {
                foreach (var item in Request.Form.Keys)
                {
                    if (item == "chunkNumber")
                    {
                        chunkNumber = Convert.ToInt32(Request.Form["chunkNumber"].ToString());
                    }
                    if (item == "identifier")
                    {
                        identifier = Request.Form["identifier"].ToString();
                    }
                    if (item == "totalChunks")
                    {
                        totalChunks = Convert.ToInt32(Request.Form["totalChunks"].ToString());
                    }
                    if (item == "filename")
                    {
                        fileName = Request.Form["filename"].ToString();
                    }
                    if (item == "chunkSize")
                    {
                        chunkSize = Convert.ToInt32(Request.Form["chunkSize"].ToString());
                    }
                    if (item == "currentChunkSize")
                    {
                        currentChunkSize = Convert.ToInt32(Request.Form["currentChunkSize"].ToString());
                    }
                    if (item == "totalSize")
                    {
                        totalSize = Convert.ToInt32(Request.Form["totalSize"].ToString());
                    }
                    if (item == "relativePath")
                    {
                        relativePath = Request.Form["relativePath"].ToString();
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

                return await Task.Run(() =>
                {
                    return _fileUploadServices.ChunkUpload(fileUpload);
                });
            }
            catch (Exception ex)
            {
                res.Flag = false;
                res.Code = 500;
                res.Data = null;
                return res;
            }
        }



        /// <summary>
        ///  合并文件
        /// </summary>
        /// <param name="fileUpload"></param>
        /// <returns></returns>
        [HttpPost, Route("MergeFiles")]
        public async Task<HttpReponseViewModel<FileUploadResViewModel>> MergeFiles(FileUploadReqViewModel fileUpload)
        {
            return await Task.Run(()=> {
                return _fileUploadServices.MergeFiles(fileUpload);
            });

        }
        #endregion





    }

}