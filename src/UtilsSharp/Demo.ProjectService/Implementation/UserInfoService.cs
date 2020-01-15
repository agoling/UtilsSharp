using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilsCore.Result;

namespace Demo.ProjectService.Implementation
{
    /// <summary>
    /// 用户信息服务
    /// </summary>
    public class UserInfoService:IUserInfoService
    {
        private readonly ISchoolInfoService _schoolInfoService;

        public UserInfoService(ISchoolInfoService schoolInfoService)
        {
            _schoolInfoService = schoolInfoService;
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <returns></returns>
        public BaseResult Save(UserInfo userInfo)
        {
            var result=new BaseResult();
            try
            {
                //数据库交互处理…
                result.Msg = "保存成功！";
                return result;
            }
            catch (Exception ex)
            {
               result.SetError(ex.Message,5000);
               return result;
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        public BaseResult Delete(int id)
        {
            var result = new BaseResult();
            try
            {
                //数据库交互处理…
                result.Msg = "删除成功！";
                return result;
            }
            catch (Exception ex)
            {
                result.SetError(ex.Message, 5000);
                return result;
            }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public BaseResult<UserInfo> Get(int id)
        {
            var result = new BaseResult<UserInfo>();
            try
            {
                //数据库交互处理…
                //模拟数据
                var r= _schoolInfoService.Get("0591001");
                if (r.Code != 200)
                {
                   result.SetError(r.Msg,r.Code);
                   return result;
                }
                var userInfo = new UserInfo
                {
                    UserName = "jim", Tel = "13556598568", Address = "福建福州", Email = "13556598568.qq.com",SchoolName = r.Result.SchoolName
                };
                result.Result = userInfo;
                result.Msg = "获取成功！";
                return result;
            }
            catch (Exception ex)
            {
                result.SetError(ex.Message, 5000);
                return result;
            }
        }

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="rule">搜索规则</param>
        /// <returns></returns>
        public BasePagedResult<UserInfo> Search(SearchRule rule)
        {
            var result = new BasePagedResult<UserInfo>();
            try
            {
                //数据库交互处理…
                //模拟数据
                var list = new List<UserInfo>
                {
                    new UserInfo
                    {
                        UserName = "jim", Tel = "13556598568", Address = "福建福州", Email = "13556598568.qq.com"
                    },
                    new UserInfo
                    {
                        UserName = "jim", Tel = "13556598568", Address = "福建福州", Email = "13556598568.qq.com"
                    },
                    new UserInfo
                    {
                        UserName = "jim", Tel = "13556598568", Address = "福建福州", Email = "13556598568.qq.com"
                    }
                };
                var paged=new PagedList<UserInfo>(list, rule.PageIndex,rule.PageSize, list.Count);
                result.Result.List = paged;
                result.Result.TotalCount = paged.TotalCount;
                result.Result.PageSize = paged.PageSize;
                result.Result.PageIndex = paged.PageIndex;
                result.Result.Params = rule;
                result.Msg = "获取成功！";
                return result;
            }
            catch (Exception ex)
            {
                result.SetError(ex.Message, 5000);
                return result;
            }
        }

    }
}
