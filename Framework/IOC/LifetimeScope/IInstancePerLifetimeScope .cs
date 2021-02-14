using System;
using System.Collections.Generic;
using System.Text;

namespace IOC.LifetimeScope
{
    /// <summary>
    /// 同一个生命周期域中，每次调用，都会使用同一个实例化的对象；每次都用同一个对象；且每个不同的生命周期域中的实例是唯一的，不共享的
    /// </summary>
    public interface IInstancePerLifetimeScope
    {
    }
}
