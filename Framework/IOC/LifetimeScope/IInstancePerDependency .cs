using System;
using System.Collections.Generic;
using System.Text;

namespace IOC.LifetimeScope
{
    /// <summary>
    /// 每次调用，都会重新实例化对象；每次请求都创建一个新的对象
    /// </summary>
    public interface IInstancePerDependency
    {
    }
}
