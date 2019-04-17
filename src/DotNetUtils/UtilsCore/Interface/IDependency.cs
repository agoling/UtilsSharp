using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilsCore.Interface
{
    /// <summary>
    /// 依赖接口标识
    /// </summary>
    public interface IDependency
    {

    }
    /// <summary>
    /// 表示实现者是一个单列依赖。
    /// </summary>
    public interface ISingletonDependency : IDependency
    {

    }

    /// <summary>
    /// 表示实现者是一个瞬态依赖。
    /// </summary>
    public interface ITransientDependency : IDependency
    {

    }

    /// <summary>
    /// 表示实现者是一个工作单元依赖。
    /// </summary>
    public interface IUnitOfWorkDependency : IDependency
    {

    }
}
