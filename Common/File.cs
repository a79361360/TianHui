﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Net;
using ZXing;
using ZXing.QrCode;
using System.Drawing;
using System.Drawing.Imaging;

namespace Fgly.Common
{
    /// <summary>
    /// File 操作类
    /// </summary>
    public sealed class File
    {
        internal File() { }

        /// <summary>
        /// 获取物理路径
        /// </summary>
        /// <param name="virtualPath">虚拟路径</param>
        /// <returns></returns>
        public string GetPhysicalPath(string virtualPath)
        {
            if (string.IsNullOrEmpty(virtualPath))
                throw new Exception("虚拟路径为空");

            try
            {
                return HttpContext.Current.Server.MapPath(virtualPath);
            }
            catch
            {
                throw new Exception("错误的虚拟路径");
            }
        }

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public bool ExistFile(string virtualPath)
        {
            if (string.IsNullOrEmpty(virtualPath))
                throw new Exception("路径为空");

            return System.IO.File.Exists(GetPhysicalPath(virtualPath));
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public bool DeleteFile(string virtualPath)
        {
            if (string.IsNullOrEmpty(virtualPath))
                throw new Exception("路径为空");

            try
            {
                System.IO.File.Delete(GetPhysicalPath(virtualPath));
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取文件大小(btyes)
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public long GetFileSize(string virtualPath)
        {
            if (ExistFile(virtualPath))
                return new FileInfo(GetPhysicalPath(virtualPath)).Length;
            return 0;
        }

        /// <summary>
        /// 上传图片文件
        /// </summary>
        /// <param name="url">提交的地址</param>
        /// <param name="poststr">发送的文本串   比如：user=eking&pass=123456  </param>
        /// <param name="fileformname">文本域的名称  比如：name="file"，那么fileformname=file  </param>
        /// <param name="filepath">上传的文件路径  比如： c:\12.jpg </param>
        /// <param name="cookie">cookie数据</param>
        /// <param name="refre">头部的跳转地址</param>
        /// <returns></returns>
        public string HttpUploadFile(string url, string poststr, string fileformname, string filepath, string cookie, string refre)
        {

            // 这个可以是改变的，也可以是下面这个固定的字符串
            string boundary = "—————————7d930d1a850658";

            // 创建request对象
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
            webrequest.ContentType = "multipart/form-data; boundary=" + boundary;
            webrequest.Method = "POST";
            webrequest.Headers.Add("Cookie: " + cookie);
            webrequest.Referer = refre;

            // 构造发送数据
            StringBuilder sb = new StringBuilder();

            // 文本域的数据，将user=eking&pass=123456  格式的文本域拆分 ，然后构造
            foreach (string c in poststr.Split('&'))
            {
                string[] item = c.Split('=');
                if (item.Length != 2)
                {
                    break;
                }
                string name = item[0];
                string value = item[1];
                sb.Append("–" + boundary);
                sb.Append("\r\n");
                sb.Append("Content-Disposition: form-data; name=\"" + name + "\"");
                sb.Append("\r\n\r\n");
                sb.Append(value);
                sb.Append("\r\n");
            }

            // 文件域的数据
            sb.Append("–" + boundary);
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=\"icon\";filename=\"" + filepath + "\"");
            sb.Append("\r\n");

            sb.Append("Content-Type: ");
            sb.Append("image/pjpeg");
            sb.Append("\r\n\r\n");

            string postHeader = sb.ToString();
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(postHeader);

            //构造尾部数据
            byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n–" + boundary + "–\r\n");

            FileStream fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            long length = postHeaderBytes.Length + fileStream.Length + boundaryBytes.Length;
            webrequest.ContentLength = length;

            Stream requestStream = webrequest.GetRequestStream();

            // 输入头部数据
            requestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);

            // 输入文件流数据
            byte[] buffer = new Byte[checked((uint)Math.Min(4096, (int)fileStream.Length))];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                requestStream.Write(buffer, 0, bytesRead);

            // 输入尾部数据
            requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
            WebResponse responce = webrequest.GetResponse();
            Stream s = responce.GetResponseStream();
            StreamReader sr = new StreamReader(s);

            // 返回数据流(源码)
            return sr.ReadToEnd();
        }

        /// <summary>
        /// 生成二维码,保存成图片
        /// </summary>
        /// <param name="text">生成二维码的字符串</param>
        /// <param name="url">保存图片的绝对地址</param>
        public bool GenerateImgUrl(string text,string url)
        {
            try
            {
                BarcodeWriter writer = new BarcodeWriter();
                writer.Format = BarcodeFormat.QR_CODE;
                QrCodeEncodingOptions options = new QrCodeEncodingOptions();
                options.DisableECI = true;
                //设置内容编码
                options.CharacterSet = "UTF-8";
                //设置二维码的宽度和高度
                options.Width = 270;
                options.Height = 270;
                //设置二维码的边距,单位不是固定像素
                options.Margin = 0;
                writer.Options = options;

                Bitmap map = writer.Write(text);
                string filename = @"" + url;
                map.Save(filename, ImageFormat.Png);
                map.Dispose();
                return true;
            }
            catch {
                return false;
            }
        }
        /// <summary>
        /// 单个文件上传的时候使用这个方法
        /// </summary>
        /// <param name="virtualpath"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public string HttpUploadFile(string virtualpath, string filename)
        {
            string path = "", suffix = "";
            if (HttpContext.Current.Request.Files.AllKeys.Length > 0)
            {
                try
                {
                    string filePath = HttpContext.Current.Server.MapPath(virtualpath);
                    if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);
                    string fname = HttpContext.Current.Request.Files[0].FileName;
                    suffix = fname.Substring(fname.LastIndexOf(".") + 1, fname.Length - (fname.LastIndexOf(".") + 1));
                    if (string.IsNullOrEmpty(filename))
                    {
                        filename = HttpContext.Current.Request.Files[0].FileName;
                    }
                    //这里我直接用索引来获取第一个文件，如果上传了多个文件，可以通过遍历HttpContext.Current.Request.Files.AllKeys取“key值”，再通过HttpContext.Current.Request.Files[“key值”]获取文件
                    path = Path.Combine(filePath, filename);
                    HttpContext.Current.Request.Files[0].SaveAs(path);
                }
                catch
                {

                }
            }
            return path;
        }
    }
}
