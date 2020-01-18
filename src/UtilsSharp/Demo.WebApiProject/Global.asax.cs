using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
//using WebApiHelper;

namespace Demo.WebApiProject
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //依赖注入注册
            //AutofacConfig.Register();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
