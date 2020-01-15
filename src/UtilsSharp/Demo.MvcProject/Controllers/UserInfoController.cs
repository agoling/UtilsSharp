using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Demo.ProjectService;
using MvcHelper;

namespace Demo.MvcProject.Controllers
{
    [RoutePrefix("api/userinfo")]
    [Route("{action}")]
    public class UserInfoController : Controller
    {

        private readonly IUserInfoService _userInfoService;

        public UserInfoController(IUserInfoService userInfoService)
        {
            _userInfoService = userInfoService;
        }

        // GET: UserInfo
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <returns></returns>
        public JsonResult Save(UserInfo userInfo)
        {
            var result = _userInfoService.Save(userInfo);
            return new JsonFormatResult(result);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        public JsonResult Delete(int id)
        {
            var result = _userInfoService.Delete(id);
            return new JsonFormatResult(result);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public JsonResult Get(int id)
        {
            var result = _userInfoService.Get(id);
            return new JsonFormatResult(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="rule">搜索规则</param>
        /// <returns></returns>
        public JsonResult Search(SearchRule rule)
        {
            var result = _userInfoService.Search(rule);
            return new JsonFormatResult(result, JsonRequestBehavior.AllowGet);
        }
    }
}