using System;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MvcHelper
{
    /// <summary>
    /// 格式化JsonResult（日期,命名方式）
    /// </summary>
    public class JsonFormatResult: JsonResult
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
        public JsonFormatResult() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="behavior">请求方式</param>
        public JsonFormatResult(object data,JsonRequestBehavior behavior = JsonRequestBehavior.DenyGet)
        {
            Data = data;
            NamingType = NamingType.CamelCase;
            DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            Formatting = Formatting.None;
            JsonRequestBehavior = behavior;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="dateTimeFormat">日期格式</param>
        /// <param name="behavior">请求方式</param>
        public JsonFormatResult(object data, string dateTimeFormat,JsonRequestBehavior behavior = JsonRequestBehavior.DenyGet)
        {
            Data = data;
            NamingType = NamingType.CamelCase;
            DateTimeFormat = dateTimeFormat;
            Formatting = Formatting.None;
            JsonRequestBehavior = behavior;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="namingType">命名方式</param>
        /// <param name="dateTimeFormat">日期格式</param>
        /// <param name="formatting">是否缩进</param>
        /// <param name="behavior">请求方式</param>
        public JsonFormatResult(object data, NamingType namingType, string dateTimeFormat = "yyyy-MM-dd HH:mm:ss", Formatting formatting=Formatting.None, JsonRequestBehavior behavior = JsonRequestBehavior.DenyGet)
        {
            Data = data;
            NamingType = namingType;
            DateTimeFormat = dateTimeFormat;
            Formatting = formatting;
            JsonRequestBehavior = behavior;
        }

        /// <summary>
        /// 执行结果
        /// </summary>
        /// <param name="context">上下文</param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if ((this.JsonRequestBehavior == JsonRequestBehavior.DenyGet) && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("此请求已被阻止，因为当用在 GET 请求中时，会将敏感信息透漏给第三方网站。若要允许 GET 请求，请将 JsonRequestBehavior 设置为 AllowGet。");
            }
            HttpResponseBase base2 = context.HttpContext.Response;
            base2.ContentType = !string.IsNullOrEmpty(this.ContentType) ? this.ContentType : "application/json";
            if (this.ContentEncoding != null)
            {
                base2.ContentEncoding = this.ContentEncoding;
            }
            if (this.Data == null) return;
            JsonSerializerSettings setting = new JsonSerializerSettings();
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
            string jsonResult = JsonConvert.SerializeObject(this.Data, setting);
            base2.Write(jsonResult);
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
        Default=0,
        /// <summary>
        /// 驼峰命名
        /// </summary>
        CamelCase = 1

    }
}
