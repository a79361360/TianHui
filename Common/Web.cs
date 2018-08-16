using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;
using System.Security.Cryptography.X509Certificates;

namespace Fgly.Common
{
    /// <summary>
    /// Web 操作类
    /// </summary>
    public sealed class Web
    {
        internal Web() { }

        /// <summary>
        /// 获取url字符串参数
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="defaultValue">默认键值</param>
        /// <returns></returns>
        public string Request(string key, string defaultValue)
        {
            return Request(key,defaultValue,false);
        }
        /// <summary>
        /// 获取url字符串参数
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="defaultValue">默认键值</param>
        /// <param name="isSaveSymbol">是否保留符号</param>
        /// <returns></returns>
        public string Request(string key, string defaultValue, bool isSaveSymbol)
        {
            string value = System.Web.HttpContext.Current.Request[key];
            if (string.IsNullOrEmpty(value))
                return defaultValue;
            if (!isSaveSymbol)
             return value.Replace("'", "''").Replace(";", "");
            return value;
        }
        /// <summary>
        /// 获取url字符串参数（解码）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public string RequestUrlDecode(string key, string defaultValue)
        {
            string value = System.Web.HttpContext.Current.Request[key];
            if (string.IsNullOrEmpty(value))
                return defaultValue;
            return HttpUtility.UrlDecode(value).Replace("'", "''").Replace(";", "").Replace("--", "");
        }

        /// <summary>
        /// 获取表单内数据
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="defaultValue">默认键值</param>
        /// <returns></returns>
        public string RequestForm(string key, string defaultValue)
        {
            return RequestForm(key, defaultValue,false);
        }

        /// <summary>
        /// 获取表单内数据
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="defaultValue">默认键值</param>
        /// <param name="isSaveSymbol">是否保留符号</param>
        /// <returns></returns>
        public string RequestForm(string key, string defaultValue, bool isSaveSymbol)
        {
            string value = System.Web.HttpContext.Current.Request.Form[key];
            if (string.IsNullOrEmpty(value))
                return defaultValue;
            if (!isSaveSymbol)
                return value.Replace("'", "''").Replace(";", "").Replace("--", "");
            return value;
        }

        /// <summary>
        /// 获取当前访问机IP地址
        /// </summary>
        /// <returns></returns>
        public string GetIP()
        {
            return (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == null || System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == "") ? System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] : System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        }

        /// <summary>
        /// 判断请求是否来自本站
        /// </summary>
        /// <returns></returns>
        public bool RequestIsHost()
        {
            string server_referrer = string.Empty, server_host = string.Empty;

            if (System.Web.HttpContext.Current.Request.UrlReferrer == null)
                return false;
            else
                server_referrer = System.Web.HttpContext.Current.Request.UrlReferrer.Host.ToLower();

            server_host = System.Web.HttpContext.Current.Request.Url.Host.ToLower();
            if (server_referrer.Equals(server_host))
                return true;
            return false;
        }

        /// <summary>
        /// 验证请求
        /// </summary>
        public void VerificationRequest()
        {
            if (!CommonManager.WebObj.RequestIsHost())
                System.Web.HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 验证表单
        /// </summary>
        public void VerificationFrom()
        {
            if (!CommonManager.WebObj.RequestIsHost() || System.Web.HttpContext.Current.Request.Form.Count == 0)
                System.Web.HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 发送Get请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <returns></returns>
        public string Get(string url)
        {
            //System.Net.ServicePointManager.DefaultConnectionLimit = 512;
            //int i = System.Net.ServicePointManager.DefaultPersistentConnectionLimit;
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 20000;
            request.AllowAutoRedirect = false;
            request.ServicePoint.Expect100Continue = false;

            try
            {
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        return reader.ReadToEnd();
                    }
                }
                else
                    return response.StatusDescription;
            }
            catch(Exception e)
            {
                return e.Message;
            }
            finally
            {
                request = null;
            }
        }

        /// <summary>
        /// 发送Post请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="data">发送的数据</param>
        /// <returns></returns>
        public string Post(string url, string data)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 20000;
            request.AllowAutoRedirect = false;
            request.ServicePoint.Expect100Continue = false;

            try
            {
                using (System.IO.StreamWriter write = new System.IO.StreamWriter(request.GetRequestStream()))
                {
                    write.Write(data);
                }

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        return reader.ReadToEnd();
                    }
                }
                else
                    return response.StatusDescription;
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                request = null;
            }
        }
        /// <summary>
        /// 发送Post请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="data">发送的数据</param>
        /// <param name="data">证书</param>
        /// <returns></returns>
        public string Post(string url, string data, X509Certificate cert)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 20000;
            request.AllowAutoRedirect = false;
            request.ServicePoint.Expect100Continue = false;
            request.ClientCertificates.Add(cert);       //添加证书
            try
            {
                using (System.IO.StreamWriter write = new System.IO.StreamWriter(request.GetRequestStream()))
                {
                    write.Write(data);
                }

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        return reader.ReadToEnd();
                    }
                }
                else
                    return response.StatusDescription;
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                request = null;
            }
        }

        /// <summary>
        /// 注册Js脚本到页面
        /// </summary>
        /// <param name="page"></param>
        /// <param name="js"></param>
        /// <param name="bottom"></param>
        public void RegJs(System.Web.UI.Page page, string js, bool bottom)
        {
            if (bottom)
                page.ClientScript.RegisterStartupScript(page.GetType(), new Random().Next(1000, 9999).ToString(), "<script type=\"text/javascript\" language=\"javascript\">" + js + "</script>");
            else
                page.ClientScript.RegisterClientScriptBlock(page.GetType(), new Random().Next(1000, 9999).ToString(), "<script type=\"text/javascript\" language=\"javascript\">" + js + "</script>");
        }
        /// <summary>
        /// 打印参数到日志,看一下参数的值
        /// </summary>
        /// <param name="parama"></param>
        public void WriteToLogByParama(string parama) { 
            //if(parama.IndexOf("sys."))
        }
        /// <summary>
        /// 取得当前域名,将Http的协议头加上例: http://www.qingqiu.com
        /// </summary>
        /// <returns></returns>
        public string GetCurHttpHost() {
            return "http://" + HttpContext.Current.Request.Url.Host;
        }
        public string GetCurHttpsHost()
        {
            return "https://" + HttpContext.Current.Request.Url.Host;
        }
        /// <summary>
        /// 返回当前的协议头
        /// </summary>
        /// <returns></returns>
        public string GetCurHttp()
        {
            string httpstr = HttpContext.Current.Request.Url.ToString().Split(':')[0];
            return httpstr;
        }
        /// <summary>
        /// 取得当前域名
        /// </summary>
        /// <returns></returns>
        public string GetCurHost()
        {
            return HttpContext.Current.Request.Url.Host;
        }
    }
}
