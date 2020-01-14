using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilsCore.Abstract
{
    /// <summary>
    /// 基础分页参数
    /// </summary>
    public class BasePage
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { set; get; }

        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { set; get; }

        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string Keyword { set; get; }
    }
}
