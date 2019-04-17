using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web.Script.Serialization;
using System.Web;

namespace UtilsCore
{
    /// <summary>
    /// 网络工具类
    /// </summary>
    public sealed class WebHelper
    {
        private const string ContentType = "application/x-www-form-urlencoded";
        private const string Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/x-silverlight, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, application/x-silverlight-2-b1, */*";
        private const string UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
        private int _timeout = 100000;

        /// <summary>
        /// 请求与响应的超时时间
        /// </summary>
        public int Timeout
        {
            get { return _timeout; }
            set { _timeout = value; }
        }

        /// <summary>
        /// 执行HTTP POST请求
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="data">参数</param>
        /// <param name="tips">提示</param>
        /// <returns></returns>
        public T DoPost<T>(string url, object data,out string tips)
        {
            try
            {
                var request = WebRequest.Create(url);
                request.Timeout = 60 * 60 * 1000;
                request.Method = "POST";
                request.ContentType = "application/json;charset=UTF-8";
                var js = new JavaScriptSerializer();
                var @params = js.Serialize(data);
                var reqStreamWriter = new StreamWriter(request.GetRequestStream());
                reqStreamWriter.Write(@params);
                reqStreamWriter.Close();
                var response = request.GetResponse() as HttpWebResponse;
                var responseStream = new StreamReader(response.GetResponseStream());
                var result = responseStream.ReadToEnd();
                responseStream.Close();
                tips = "请求成功";
                return js.Deserialize<T>(result);
            }
            catch (Exception ex)
            {
                tips=$"请求失败：{ex.Message}";
                return default(T);
            }
        }

        /// <summary>
        /// 执行HTTP POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="rspEncoding">响应编码</param>
        /// <returns>HTTP响应</returns>
        public string DoPost(string url,Encoding rspEncoding = null)
        {
            return DoPost(url, null, rspEncoding);
        }

        /// <summary>
        /// 执行HTTP POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="rspEncoding">响应编码</param>
        /// <returns>HTTP响应</returns>
        public string DoPost(string url, IDictionary<string, string> parameters, Encoding rspEncoding = null)
        {
            HttpWebRequest req = GetWebRequest(url, "POST");
            req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";

            byte[] postData = Encoding.UTF8.GetBytes(BuildQuery(parameters));
            Stream reqStream = req.GetRequestStream();
            reqStream.Write(postData, 0, postData.Length);
            reqStream.Close();

            var rsp = (HttpWebResponse)req.GetResponse();
            if (rspEncoding != null) return GetResponseAsString(rsp, rspEncoding);
            if (rsp.CharacterSet != null) rspEncoding = Encoding.GetEncoding(rsp.CharacterSet);
            return GetResponseAsString(rsp, rspEncoding);
        }

        /// <summary>
        /// 执行带文件上传的HTTP POST请求。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="textParams">请求文本参数</param>
        /// <param name="fileParams">请求文件参数</param>
        /// <returns>HTTP响应</returns>
        public string DoPost(string url, IDictionary<string, string> textParams, IDictionary<string, FileItem> fileParams)
        {
            // 如果没有文件参数，则走普通POST请求
            if (fileParams == null || fileParams.Count == 0)
            {
                return DoPost(url, textParams);
            }

            string boundary = DateTime.Now.Ticks.ToString("X"); // 随机分隔线

            HttpWebRequest req = GetWebRequest(url, "POST");
            req.ContentType = "multipart/form-data;charset=utf-8;boundary=" + boundary;

            var reqStream = req.GetRequestStream();
            byte[] itemBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
            byte[] endBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");

            // 组装文本请求参数
            string textTemplate = "Content-Disposition:form-data;name=\"{0}\"\r\nContent-Type:text/plain\r\n\r\n{1}";
            IEnumerator<KeyValuePair<string, string>> textEnum = textParams.GetEnumerator();
            while (textEnum.MoveNext())
            {
                string textEntry = string.Format(textTemplate, textEnum.Current.Key, textEnum.Current.Value);
                byte[] itemBytes = Encoding.UTF8.GetBytes(textEntry);
                reqStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
                reqStream.Write(itemBytes, 0, itemBytes.Length);
            }

            // 组装文件请求参数
            string fileTemplate = "Content-Disposition:form-data;name=\"{0}\";filename=\"{1}\"\r\nContent-Type:{2}\r\n\r\n";
            IEnumerator<KeyValuePair<string, FileItem>> fileEnum = fileParams.GetEnumerator();
            while (fileEnum.MoveNext())
            {
                string key = fileEnum.Current.Key;
                FileItem fileItem = fileEnum.Current.Value;
                string fileEntry = string.Format(fileTemplate, key, fileItem.GetFileName(), fileItem.GetMimeType());
                byte[] itemBytes = Encoding.UTF8.GetBytes(fileEntry);
                reqStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
                reqStream.Write(itemBytes, 0, itemBytes.Length);

                byte[] fileBytes = fileItem.GetContent();
                reqStream.Write(fileBytes, 0, fileBytes.Length);
            }

            reqStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
            reqStream.Close();

            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
            Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);
            return GetResponseAsString(rsp, encoding);
        }

        /// <summary>
        /// 执行HTTP POST请求
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="postData">post 提交的字符串</param>
        /// <param name="isPost">是否是post</param>
        /// <param name="cookieContainer">CookieContainer</param>
        /// <param name="rspEncoding">响应编码</param>
        public string DoPost(string url, string postData, bool isPost, CookieContainer cookieContainer, Encoding rspEncoding = null)
        {
            if (rspEncoding == null)
            {
                rspEncoding = Encoding.GetEncoding("UTF-8");
            }
            if (string.IsNullOrEmpty(postData)) return DoGet(url, cookieContainer, rspEncoding);
            HttpWebRequest httpWebRequest = null;
            HttpWebResponse httpWebResponse = null;
            try
            {
                byte[] byteRequest = Encoding.Default.GetBytes(postData);
                httpWebRequest = GetWebRequest(url, isPost ? "POST" : "GET");
                httpWebRequest.CookieContainer = cookieContainer;
                httpWebRequest.ContentType = ContentType;
                httpWebRequest.Referer = url;
                httpWebRequest.Accept = Accept;
                httpWebRequest.UserAgent = UserAgent;
                httpWebRequest.ContentLength = byteRequest.Length;
                Stream stream = httpWebRequest.GetRequestStream();
                stream.Write(byteRequest, 0, byteRequest.Length);
                stream.Close();
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream responseStream = httpWebResponse.GetResponseStream();
                if (responseStream != null)
                {
                    StreamReader streamReader = new StreamReader(responseStream, rspEncoding);
                    string html = streamReader.ReadToEnd();
                    streamReader.Close();
                    responseStream.Close();
                    httpWebRequest.Abort();
                    httpWebResponse.Close();
                    return html;
                }
                return string.Empty;
            }
            catch (Exception)
            {
                httpWebRequest?.Abort();
                httpWebResponse?.Close();
                return string.Empty;
            }
        }

        /// <summary>
        /// 执行HTTP GET请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="rspEncoding">响应编码</param>
        /// <returns></returns>
        public string DoGet(string url,Encoding rspEncoding = null)
        {
            return DoGet(url, parameters: null, rspEncoding: rspEncoding);
        }

        /// <summary>
        /// 执行HTTP GET请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="rspEncoding">响应编码</param>
        /// <returns></returns>
        public string DoGet(string url, IDictionary<string, string> parameters, Encoding rspEncoding = null)
        {
            if (parameters != null && parameters.Count > 0)
            {
                if (url.Contains("?"))
                {
                    url = url + "&" + BuildQuery(parameters);
                }
                else
                {
                    url = url + "?" + BuildQuery(parameters);
                }
            }

            HttpWebRequest req = GetWebRequest(url, "GET");
            req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";

            var rsp = (HttpWebResponse)req.GetResponse();
            if (rspEncoding != null) return GetResponseAsString(rsp, rspEncoding);
            if (rsp.CharacterSet != null) rspEncoding = Encoding.GetEncoding(rsp.CharacterSet);
            return GetResponseAsString(rsp, rspEncoding);
        }

        /// <summary>
        /// 执行HTTP GET请求
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="cookieContainer">CookieContainer</param>
        /// <param name="rspEncoding">响应编码</param>
        public string DoGet(string url, CookieContainer cookieContainer, Encoding rspEncoding = null)
        {
            if (rspEncoding == null)
            {
                rspEncoding = Encoding.GetEncoding("UTF-8");
            }
            HttpWebRequest httpWebRequest = null;
            HttpWebResponse httpWebResponse = null;
            try
            {
                httpWebRequest = GetWebRequest(url, "GET");
                httpWebRequest.CookieContainer = cookieContainer;
                httpWebRequest.ContentType = ContentType;
                httpWebRequest.Referer = url;
                httpWebRequest.Accept = Accept;
                httpWebRequest.UserAgent = UserAgent;
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream responseStream = httpWebResponse.GetResponseStream();
                if (responseStream != null)
                {
                    StreamReader streamReader = new StreamReader(responseStream, rspEncoding);
                    string html = streamReader.ReadToEnd();
                    streamReader.Close();
                    responseStream.Close();
                    httpWebRequest.Abort();
                    httpWebResponse.Close();
                    return html;
                }
                return string.Empty;
            }
            catch (Exception)
            {
                httpWebRequest?.Abort();
                httpWebResponse?.Close();
                return string.Empty;
            }
        }

        /// <summary>
        /// CheckValidationResult
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="certificate">certificate</param>
        /// <param name="chain">chain</param>
        /// <param name="errors">errors</param>
        /// <returns></returns>
        public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        { //直接确认，否则打不开
            return true;
        }

        /// <summary>
        /// GetWebRequest
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="method">method</param>
        /// <returns></returns>
        public HttpWebRequest GetWebRequest(string url, string method)
        {
            HttpWebRequest req;
            if (url.Contains("https"))
            {
                ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
                req = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
            }
            else
            {
                req = (HttpWebRequest)WebRequest.Create(url);
            }
            req.ServicePoint.Expect100Continue = false;
            req.Method = method;
            req.KeepAlive = true;
            req.UserAgent = "Top4Net";
            req.Timeout = _timeout;
            return req;
        }

        /// <summary>
        /// 把响应流转换为文本。
        /// </summary>
        /// <param name="rsp">响应流对象</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>响应文本</returns>
        public string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            Stream stream = null;
            StreamReader reader = null;

            try
            {
                // 以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                reader = new StreamReader(stream, encoding);
                return reader.ReadToEnd();
            }
            finally
            {
                // 释放资源
                if (reader != null) reader.Close();
                if (stream != null) stream.Close();
                if (rsp != null) rsp.Close();
            }
        }

        /// <summary>
        /// 组装GET请求URL。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns>带参数的GET请求URL</returns>
        public string BuildGetUrl(string url, IDictionary<string, string> parameters)
        {
            if (parameters != null && parameters.Count > 0)
            {
                if (url.Contains("?"))
                {
                    url = url + "&" + BuildQuery(parameters);
                }
                else
                {
                    url = url + "?" + BuildQuery(parameters);
                }
            }
            return url;
        }

        /// <summary>
        /// 组装普通文本请求参数。
        /// </summary>
        /// <param name="parameters">Key-Value形式请求参数字典</param>
        /// <returns>URL编码后的请求数据</returns>
        public static string BuildQuery(IDictionary<string, string> parameters)
        {
            StringBuilder postData = new StringBuilder();
            bool hasParam = false;

            IEnumerator<KeyValuePair<string, string>> dem = parameters.GetEnumerator();
            while (dem.MoveNext())
            {
                string name = dem.Current.Key;
                string value = dem.Current.Value;
                // 忽略参数名或参数值为空的参数
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
                {
                    if (hasParam)
                    {
                        postData.Append("&");
                    }

                    postData.Append(name);
                    postData.Append("=");
                    postData.Append(HttpUtility.UrlEncode(value, Encoding.UTF8));
                    hasParam = true;
                }
            }

            return postData.ToString();
        }
    }

    /// <summary>
    /// 文件元数据。
    /// 可以使用以下几种构造方法：
    /// 本地路径：new FileItem("C:/temp.jpg");
    /// 本地文件：new FileItem(new FileInfo("C:/temp.jpg"));
    /// 字节流：new FileItem("abc.jpg", bytes);
    /// </summary>
    public class FileItem
    {
        private string _fileName;
        private string _mimeType;
        private byte[] _content;
        private readonly FileInfo _fileInfo;

        /// <summary>
        /// 基于本地文件的构造器。
        /// </summary>
        /// <param name="fileInfo">本地文件</param>
        public FileItem(FileInfo fileInfo)
        {
            if (fileInfo == null || !fileInfo.Exists)
            {
                throw new ArgumentException("fileInfo is null or not exists!");
            }
            _fileInfo = fileInfo;
        }

        /// <summary>
        /// 基于本地文件全路径的构造器。
        /// </summary>
        /// <param name="filePath">本地文件全路径</param>
        public FileItem(string filePath)
            : this(new FileInfo(filePath))
        { }

        /// <summary>
        /// 基于文件名和字节流的构造器。
        /// </summary>
        /// <param name="fileName">文件名称（服务端持久化字节流到磁盘时的文件名）</param>
        /// <param name="content">文件字节流</param>
        public FileItem(string fileName, byte[] content)
        {
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentNullException("fileName");
            if (content == null || content.Length == 0) throw new ArgumentNullException("content");

            _fileName = fileName;
            _content = content;
        }

        /// <summary>
        /// 基于文件名、字节流和媒体类型的构造器。
        /// </summary>
        /// <param name="fileName">文件名（服务端持久化字节流到磁盘时的文件名）</param>
        /// <param name="content">文件字节流</param>
        /// <param name="mimeType">媒体类型</param>
        public FileItem(string fileName, byte[] content, string mimeType)
            : this(fileName, content)
        {
            if (string.IsNullOrEmpty(mimeType)) throw new ArgumentNullException("mimeType");
            _mimeType = mimeType;
        }

        /// <summary>
        /// GetFileName
        /// </summary>
        /// <returns></returns>
        public string GetFileName()
        {
            if (_fileName == null && _fileInfo != null && _fileInfo.Exists)
            {
                _fileName = _fileInfo.FullName;
            }
            return _fileName;
        }

        /// <summary>
        /// GetMimeType
        /// </summary>
        /// <returns></returns>
        public string GetMimeType()
        {
            return _mimeType ?? (_mimeType = GetMimeType(GetContent()));
        }

        /// <summary>
        /// GetContent
        /// </summary>
        /// <returns></returns>
        public byte[] GetContent()
        {
            if (_content != null || _fileInfo == null || !_fileInfo.Exists) return _content;
            using (Stream fileStream = _fileInfo.OpenRead())
            {
                _content = new byte[fileStream.Length];
                fileStream.Read(_content, 0, _content.Length);
            }

            return _content;
        }

        /// <summary>
        /// 获取文件的真实媒体类型。目前只支持JPG, GIF, PNG, BMP四种图片文件。
        /// </summary>
        /// <param name="fileData">文件字节流</param>
        /// <returns>媒体类型</returns>
        public static string GetMimeType(byte[] fileData)
        {
            string suffix = GetFileSuffix(fileData);
            string mimeType;

            switch (suffix)
            {
                case "JPG": mimeType = "image/jpeg"; break;
                case "GIF": mimeType = "image/gif"; break;
                case "PNG": mimeType = "image/png"; break;
                case "BMP": mimeType = "image/bmp"; break;
                default: mimeType = "application/octet-stream"; break;
            }

            return mimeType;
        }

        /// <summary>
        /// 根据文件后缀名获取文件的媒体类型。
        /// </summary>
        /// <param name="fileName">带后缀的文件名或文件全名</param>
        /// <returns>媒体类型</returns>
        public static string GetMimeType(string fileName)
        {
            string mimeType;
            fileName = fileName.ToLower();

            if (fileName.EndsWith(".bmp", StringComparison.CurrentCulture))
            {
                mimeType = "image/bmp";
            }
            else if (fileName.EndsWith(".gif", StringComparison.CurrentCulture))
            {
                mimeType = "image/gif";
            }
            else if (fileName.EndsWith(".jpg", StringComparison.CurrentCulture) || fileName.EndsWith(".jpeg", StringComparison.CurrentCulture))
            {
                mimeType = "image/jpeg";
            }
            else if (fileName.EndsWith(".png", StringComparison.CurrentCulture))
            {
                mimeType = "image/png";
            }
            else
            {
                mimeType = "application/octet-stream";
            }

            return mimeType;
        }
        /// <summary>
        /// 获取文件的真实后缀名。目前只支持JPG, GIF, PNG, BMP四种图片文件。
        /// </summary>
        /// <param name="fileData">文件字节流</param>
        /// <returns>JPG, GIF, PNG or null</returns>
        public static string GetFileSuffix(byte[] fileData)
        {
            if (fileData == null || fileData.Length < 10)
            {
                return null;
            }
            if (fileData[0] == 'G' && fileData[1] == 'I' && fileData[2] == 'F')
            {
                return "GIF";
            }
            if (fileData[1] == 'P' && fileData[2] == 'N' && fileData[3] == 'G')
            {
                return "PNG";
            }
            if (fileData[6] == 'J' && fileData[7] == 'F' && fileData[8] == 'I' && fileData[9] == 'F')
            {
                return "JPG";
            }
            if (fileData[0] == 'B' && fileData[1] == 'M')
            {
                return "BMP";
            }
            return null;
        }
    }
}
