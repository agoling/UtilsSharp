using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilsCore.Result;

namespace Demo.ProjectService.Implementation
{
    /// <summary>
    /// 学校信息
    /// </summary>
    public class SchoolInfoService: ISchoolInfoService
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public BaseResult<SchoolInfo> Get(string schoolCode)
        {
            var result = new BaseResult<SchoolInfo>();
            try
            {
                //数据库交互处理…
                //模拟数据
                var schoolInfo = new SchoolInfo
                {
                    SchoolCode = "0591001",
                    SchoolName = "福州一中",
                    SchoolType = SchoolType.高中
                };
                result.Result = schoolInfo;
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
