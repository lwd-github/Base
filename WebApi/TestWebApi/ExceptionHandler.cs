using Framework.Common.Exceptions;
using Framework.Common.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TestWebApi
{
    /// <summary>
    /// 异常处理器
    /// </summary>
    public class ExceptionHandler : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (!context.ExceptionHandled) //异常有没有被处理过
            {
                string controlerName = (string)context.RouteData.Values["controller"];
                string actionName = (string)context.RouteData.Values["action"];
                string msgTemplate = "在执行Controller[{0}]的Action[{1}]时产生异常：{2}";

                bool isValidationException =  context.Exception is ValidationException; //是否为验证异常

                //短路器
                context.Result = new JsonResult(
                    new Result
                    {
                        Status = false,
                        Message = isValidationException ? context.Exception.Message : "系统发生了未知异常，请联系管理员"
                    }
                 );

                if (!isValidationException)
                {
                    //写日志
                    System.Console.WriteLine(string.Format(msgTemplate, controlerName, actionName, context.Exception));
                }

                context.ExceptionHandled = true; //标记异常已经被处理
            }
        }
    }
}
