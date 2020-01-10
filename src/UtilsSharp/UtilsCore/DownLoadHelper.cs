using System;
using System.IO;
using System.Web;
using System.Threading;

namespace UtilsCore
{
    /// <summary>
    /// 文件下载帮助类
    /// </summary>
    public class DownLoadHelper
    {
        #region ResponseFile 输出硬盘文件，提供下载 支持大文件、续传、速度限制、资源占用小

        /// <summary>
        /// 网页文件字节流下载
        /// </summary>
        /// <param name="response">Page.Request对象</param>
        /// <param name="bytes">字节流</param>
        /// <param name="fileName">文件名 如:pic.jpg file.mp4 file.csv</param>
        /// <returns>返回是否成功</returns>
        public static bool ResponseFile(HttpResponseBase response, byte[] bytes, string fileName)
        {
            try
            {
                response.AddHeader("Connection", "Keep-Alive");
                response.ContentType = "application/octet-stream";
                //通知浏览器下载文件而不是打开  
                response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
                response.BinaryWrite(bytes);
                response.Flush();
                response.End();
                response.Close();
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        ///  输出硬盘文件，提供下载 支持大文件、续传、速度限制、资源占用小
        /// </summary>
        /// <param name="request">Page.Request对象</param>
        /// <param name="response">Page.Response对象</param>
        /// <param name="fileName">下载文件名</param>
        /// <param name="fullPath">带文件名下载路径</param>
        /// <param name="speed">每秒允许下载的字节数</param>
        /// <returns>返回是否成功</returns>
        public static bool ResponseFile(HttpRequest request, HttpResponse response, string fileName, string fullPath, long speed=10240* 1000)
        {
            try
            {
                FileStream myFile = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(myFile);
                try
                {
                    response.AddHeader("Accept-Ranges", "bytes");
                    response.Buffer = false;
                    long fileLength = myFile.Length;
                    long startBytes = 0;

                    int pack = 10240; //10K bytes
                    //int sleep = 200;   //每秒5次   即5*10K bytes每秒
                    int sleep = (int)Math.Floor((double)(1000 * pack / speed)) + 1;
                    if (request.Headers["Range"] != null)
                    {
                        response.StatusCode = 206;
                        string[] range = request.Headers["Range"].Split(new char[] { '=', '-' });
                        startBytes = Convert.ToInt64(range[1]);
                    }
                    response.AddHeader("Content-Length", (fileLength - startBytes).ToString());
                    if (startBytes != 0)
                    {
                        response.AddHeader("Content-Range", string.Format(" bytes {0}-{1}/{2}", startBytes, fileLength - 1, fileLength));
                    }
                    response.AddHeader("Connection", "Keep-Alive");
                    response.ContentType = "application/octet-stream";
                    response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));

                    br.BaseStream.Seek(startBytes, SeekOrigin.Begin);
                    int maxCount = (int)Math.Floor((double)((fileLength - startBytes) / pack)) + 1;

                    for (int i = 0; i < maxCount; i++)
                    {
                        if (response.IsClientConnected)
                        {
                            response.BinaryWrite(br.ReadBytes(pack));
                            Thread.Sleep(sleep);
                        }
                        else
                        {
                            i = maxCount;
                        }
                    }
                }
                catch
                {
                    return false;
                }
                finally
                {
                    br.Close();
                    myFile.Close();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        
        #endregion
    }
}
