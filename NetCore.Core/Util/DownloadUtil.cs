using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.Core.Util
{
    public class DownloadUtil
    {
        public HttpRequest request = null;
        public HttpResponse response = null;
        private int HttpRangeSize = 1024 * 1024; //最大块长度 1M
        public DownloadUtil(HttpContext ctx)
        {
            request = ctx.Request;
            response = ctx.Response;
        }

        public async Task WriteFile(string file)
        {
            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                string range = request.Headers["Range"];
                range = range ?? "";
                range = range.Trim().ToLower();
                if (fs.CanSeek)
                {
                    if (range.StartsWith("bytes=") && range.Contains("-"))
                    {
                        //分段输出文件
                        int start = -1, end = -1;
                        var rgs = range.Substring(6).Split('-');
                        int.TryParse(rgs[0], out start);
                        int.TryParse(rgs[1], out end);
                        if (rgs[0] == "")
                        {
                            start = (int)fs.Length - end;
                            end = (int)fs.Length - 1;
                        }
                        if (rgs[1] == "")
                        {
                            end = (int)fs.Length - 1;
                        }


                        int rangLen = end - start + 1;
                        if (rangLen > 0)
                        {
                            if (rangLen > HttpRangeSize)
                            {
                                rangLen = HttpRangeSize;
                                end = start + HttpRangeSize - 1;
                            }
                        }
                        else
                        {
                            throw new Exception("Range error");
                        }

                        long size = fs.Length;
                        //如果是整个文件返回200，否则返回206
                        if (start == 0 && end + 1 >= size)
                        {
                            response.StatusCode = 200;
                        }
                        else
                        {
                            response.StatusCode = 206;
                        }
                        // response.Headers.Add("Accept-Ranges", "bytes");
                        response.Headers.Add("Content-Range", $"bytes {start}-{end}/{size}");
                        response.Headers.Add("Content-Length", rangLen.ToString());

                        int readLen, total = 0;
                        byte[] buffer = new byte[40960];
                        //流定位到指定位置
                        try
                        {
                            fs.Seek(start, SeekOrigin.Begin);
                            while (total < rangLen && (readLen = fs.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                total += readLen;
                                if (total > rangLen)
                                {
                                    readLen -= (total - rangLen);
                                    total = rangLen;
                                }
                                await response.Body.WriteAsync(buffer, 0, readLen);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }

                    }

                }

            }

        }
    }

    public class DownloadRange
    {

        public HttpContext context = null;
        public HttpRequest request = null;
        public HttpResponse response = null;
        public DownloadRange(HttpContext ctx)
        {
            this.context = ctx;
            this.request = ctx.Request;
            this.response = ctx.Response;
        }
        private int HttpRangeSize = 1024 * 1024; //最大块长度 1M
        public void WriteFile(string file)
        {
            using (var fs = File.OpenRead(file))
            {
                WriteStream(fs);
            }
        }
        private void WriteStream(Stream fs)
        {
            string range = request.Headers["Range"];
            range = range ?? "";
            range = range.Trim().ToLower();
            if (fs.CanSeek)
            {
                if (range.StartsWith("bytes=") && range.Contains("-"))
                {
                    //分段输出文件
                    int start = -1, end = -1;
                    var rgs = range.Substring(6).Split('-');
                    int.TryParse(rgs[0], out start);
                    int.TryParse(rgs[1], out end);
                    if (rgs[0] == "")
                    {
                        start = (int)fs.Length - end;
                        end = (int)fs.Length - 1;
                    }
                    if (rgs[1] == "")
                    {
                        end = (int)fs.Length - 1;
                    }
                    WriteRangeStream(fs, start, end);
                }
                else
                {
                    //输出整个文件
                    int l;
                    byte[] buffer = new byte[40960];
                    while ((l = fs.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        response.Body.Write(buffer, 0, l);
                    }
                }
            }
        }
        private void WriteRangeStream(Stream fs, int start, int end)
        {
            using (fs)
            {
                int rangLen = end - start + 1;
                if (rangLen > 0)
                {
                    if (rangLen > HttpRangeSize)
                    {
                        rangLen = HttpRangeSize;
                        end = start + HttpRangeSize - 1;
                    }
                }
                else
                {
                    throw new Exception("Range error");
                }

                long size = fs.Length;
                //如果是整个文件返回200，否则返回206
                if (start == 0 && end + 1 >= size)
                {
                    response.StatusCode = 200;
                }
                else
                {
                    response.StatusCode = 206;
                }
                // response.Headers.Add("Accept-Ranges", "bytes");
                response.Headers.Add("Content-Range", $"bytes {start}-{end}/{size}");
                response.Headers.Add("Content-Length", rangLen.ToString());

                int readLen, total = 0;
                byte[] buffer = new byte[40960];
                //流定位到指定位置
                try
                {
                    fs.Seek(start, SeekOrigin.Begin);
                    while (total < rangLen && (readLen = fs.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        total += readLen;
                        if (total > rangLen)
                        {
                            readLen -= (total - rangLen);
                            total = rangLen;
                        }
                        response.Body.Write(buffer, 0, readLen);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }

    public class PartialContentFileStream : Stream
    {
        private readonly long _start;
        private readonly long _end;
        private long _position;
        private FileStream _fileStream;
        public PartialContentFileStream(FileStream fileStream, long start, long end)
        {
            _start = start;
            _position = start;
            _end = end;
            _fileStream = fileStream;

            if (start > 0)
            {
                _fileStream.Seek(start, SeekOrigin.Begin);
            }
        }

        /// <summary>
        /// 将缓冲区数据写到文件
        /// </summary>
        public override void Flush()
        {
            _fileStream.Flush();
        }

        /// <summary>
        /// 设置当前下载位置
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            if (origin == SeekOrigin.Begin)
            {
                _position = _start + offset;
                return _fileStream.Seek(_start + offset, origin);
            }
            else if (origin == SeekOrigin.Current)
            {
                _position += offset;
                return _fileStream.Seek(_position + offset, origin);
            }
            else
            {
                throw new NotImplementedException("SeekOrigin.End未实现");
            }
        }

        /// <summary>
        /// 依据偏离位置读取
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            int byteCountToRead = count;
            if (_position + count > _end)
            {
                byteCountToRead = (int)(_end - _position) + 1;
            }
            var result = _fileStream.Read(buffer, offset, byteCountToRead);
            _position += byteCountToRead;
            return result;
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            int byteCountToRead = count;
            if (_position + count > _end)
            {
                byteCountToRead = (int)(_end - _position);
            }
            var result = _fileStream.BeginRead(buffer, offset,
                                               count, (s) =>
                                               {
                                                   _position += byteCountToRead;
                                                   callback(s);
                                               }, state);
            return result;
        }

        public override void SetLength(long value)
        {
            throw new System.NotImplementedException();
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            return _fileStream.EndRead(asyncResult);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new System.NotImplementedException();
        }

        public override int ReadByte()
        {
            int result = _fileStream.ReadByte();
            _position++;
            return result;
        }

        public override void WriteByte(byte value)
        {
            throw new NotImplementedException();
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override long Length
        {
            get { return _end - _start; }
        }

        public override long Position
        {
            get { return _position; }
            set
            {
                _position = value;
                _fileStream.Seek(_position, SeekOrigin.Begin);
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _fileStream.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
