using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dependency;
using UtilsCore.Abstract;
using UtilsCore.Result;

namespace Demo.ProjectService
{
    /// <summary>
    /// 用户信息服务接口
    /// </summary>
    public interface IUserInfoService:IUnitOfWorkDependency
    {
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <returns></returns>
        BaseResult Save(UserInfo userInfo);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        BaseResult Delete(int id);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        BaseResult<UserInfo> Get(int id);

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="rule">搜索规则</param>
        /// <returns></returns>
        BasePagedResult<UserInfo> Search(SearchRule rule);
    }

    /// <summary>
    /// 搜索规则
    /// </summary>
    public class SearchRule : BasePage
    {
        /// <summary>
        /// 按电话号码搜索
        /// </summary>
        public string Tel { set; get; }
    }

    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { set; get; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Tel { set; get; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { set; get; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { set; get; }
        /// <summary>
        /// 最高学历学校名称
        /// </summary>
        public string SchoolName { set; get; }
    }
}
