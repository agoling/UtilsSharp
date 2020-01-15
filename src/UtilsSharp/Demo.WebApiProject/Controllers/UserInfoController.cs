using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UtilsCore.Result;
using Demo.ProjectService;

namespace Demo.WebApiProject.Controllers
{
    [RoutePrefix("api/userinfo")]
    [Route("{action}")]
    public class UserInfoController : ApiController
    {
        private readonly IUserInfoService _userInfoService;

        public UserInfoController(IUserInfoService userInfoService)
        {
            _userInfoService = userInfoService;
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <returns></returns>
        public BaseResult Save(UserInfo userInfo)
        {
            var result = _userInfoService.Save(userInfo);
            return result;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        public BaseResult Delete(int id)
        {
            var result = _userInfoService.Delete(id);
            return result;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public BaseResult<UserInfo> Get(int id)
        {
            var result = _userInfoService.Get(id);
            return result;
        }

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="rule">搜索规则</param>
        /// <returns></returns>
        public BasePagedResult<UserInfo> Search(SearchRule rule)
        {
            var result = _userInfoService.Search(rule);
            return result;
        }
    }
}
