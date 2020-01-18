using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebApiHelper;

namespace Demo.WebApiProject
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务

            // Web API 路由
            config.MapHttpAttributeRoutes();
            //注册返回参为小驼峰法命名
#if DEBUG
            JsonFormatConfig.Register(config, NamingType.Default);
#else
            JsonFormatConfig.Register(config);
#endif
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
