using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WebApiHelper
{
    /// <summary>
    /// 格式化JsonResult（日期,命名方式）
    /// </summary>
    public class JsonFormatConfig
    {
        /// <summary>
        /// 格式化JsonResult（日期,命名方式）配置
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            config.Services.Replace(typeof(IContentNegotiator), new JsonContentNegotiator());
        }

        /// <summary>
        /// 格式化JsonResult（日期,命名方式）配置
        /// </summary>
        /// <param name="config"></param>
        /// <param name="dateTimeFormat"></param>
        public static void Register(HttpConfiguration config, string dateTimeFormat)
        {
            config.Services.Replace(typeof(IContentNegotiator), new JsonContentNegotiator(dateTimeFormat));
        }

        /// <summary>
        /// 格式化JsonResult（日期,命名方式）配置
        /// </summary>
        /// <param name="config"></param>
        /// <param name="namingType"></param>
        /// <param name="dateTimeFormat"></param>
        /// <param name="formatting"></param>
        public static void Register(HttpConfiguration config, NamingType namingType, string dateTimeFormat = "yyyy-MM-dd HH:mm:ss", Formatting formatting = Formatting.None)
        {
            config.Services.Replace(typeof(IContentNegotiator), new JsonContentNegotiator(namingType, dateTimeFormat, formatting));
        }

    }

    /// <summary>
    /// 格式化JsonResult（日期,命名方式）
    /// </summary>
    public class JsonContentNegotiator : IContentNegotiator
    {  
        /// <summary>
        /// 命名方式
        /// </summary>
        public NamingType NamingType { get; set; }

        /// <summary>
        /// 日期格式
        /// </summary>
        public string DateTimeFormat { get; set; }

        /// <summary>
        /// 是否缩进
        /// </summary>
        public Formatting Formatting { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public JsonContentNegotiator()
        {
            NamingType = NamingType.CamelCase;
            DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            Formatting = Formatting.None;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dateTimeFormat">日期格式</param>
        public JsonContentNegotiator(string dateTimeFormat)
        {
            NamingType = NamingType.CamelCase;
            DateTimeFormat = dateTimeFormat;
            Formatting = Formatting.None;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="namingType">命名方式</param>
        /// <param name="dateTimeFormat">日期格式</param>
        /// <param name="formatting">是否缩进</param>
        public JsonContentNegotiator(NamingType namingType, string dateTimeFormat = "yyyy-MM-dd HH:mm:ss", Formatting formatting = Formatting.None)
        {
            NamingType = namingType;
            DateTimeFormat = dateTimeFormat;
            Formatting = formatting;
        }

        /// <summary>
        /// 通过从可以序列化给定类型对象的给定请求的传入格式化程序中选择最合适的格式来执行内容协商
        /// </summary>
        /// <param name="type">要序列化的类型</param>
        /// <param name="request">request message，其中包含用于执行协商的头值</param>
        /// <param name="formatters">objects from which to choose</param>
        /// <returns></returns>
        public ContentNegotiationResult Negotiate(Type type, HttpRequestMessage request, IEnumerable<MediaTypeFormatter> formatters)
        {
            var jsonFormatter = new JsonMediaTypeFormatter();
            JsonSerializerSettings setting = jsonFormatter.SerializerSettings;
            //不理Null值
            setting.NullValueHandling = NullValueHandling.Ignore;
            //格式化命名方式
            setting.ContractResolver = NamingType == NamingType.CamelCase ? new CamelCasePropertyNamesContractResolver() : new DefaultContractResolver();
            //是否缩进
            setting.Formatting = Formatting;
            //格式化时间
            if (!string.IsNullOrEmpty(DateTimeFormat))
            {
                setting.DateFormatString = DateTimeFormat;
            }
            var result = new ContentNegotiationResult(jsonFormatter, new MediaTypeHeaderValue("application/json"));
            return result;
        }
    }

    /// <summary>
    /// 命名方式
    /// </summary>
    public enum NamingType
    {
        /// <summary>
        /// 默认命名
        /// </summary>
        Default = 0,
        /// <summary>
        /// 驼峰命名
        /// </summary>
        CamelCase = 1

    }
}
