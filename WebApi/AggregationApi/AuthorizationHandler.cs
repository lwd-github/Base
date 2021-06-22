using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AggregationApi
{
    /// <summary>
    /// 权限处理器
    /// </summary>
    public class AuthorizationHandler : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var allAttrs = GetAttributes(context.ActionDescriptor as ControllerActionDescriptor);
        }


        /// <summary>
        /// 获取Action所有特性，包括当前控制器
        /// </summary>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        private IEnumerable<object> GetAttributes(ControllerActionDescriptor descriptor)
        {
            var allAttrs = new List<object>();
            // action特性
            var actionAttrs = descriptor.MethodInfo.GetCustomAttributes(inherit: true);
            if (actionAttrs != null)
                allAttrs.AddRange(actionAttrs);

            // controller特性
            var controllerAttrs = descriptor.ControllerTypeInfo.GetCustomAttributes(inherit: true);
            if (controllerAttrs != null)
                allAttrs.AddRange(controllerAttrs);

            return allAttrs;
        }
    }
}
