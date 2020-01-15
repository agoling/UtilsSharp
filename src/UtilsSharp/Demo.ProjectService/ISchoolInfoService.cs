using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dependency;
using UtilsCore.Result;

namespace Demo.ProjectService
{
    /// <summary>
    /// 学校信息接口
    /// </summary>
    public interface ISchoolInfoService:IUnitOfWorkDependency
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        BaseResult<SchoolInfo> Get(string schoolCode);
    }

    /// <summary>
    /// 学校信息
    /// </summary>
    public class SchoolInfo
    {
        /// <summary>
        /// 学校代码
        /// </summary>
        public string SchoolCode { set; get; }

        /// <summary>
        /// 学校名称
        /// </summary>
        public string SchoolName { set; get; }

        /// <summary>
        /// 学校类型
        /// </summary>
        public SchoolType SchoolType { set; get; }
    }

    /// <summary>
    /// 学校类型
    /// </summary>
    public enum SchoolType
    {
        小学=0,
        初中=1,
        高中=2,
        大学=3
    }
}
