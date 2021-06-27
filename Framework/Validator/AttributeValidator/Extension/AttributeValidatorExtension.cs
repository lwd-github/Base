using Framework.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Validator.AttributeValidator.Extension
{
    public static class AttributeValidatorExtension
    {
        /// <summary>
        /// 验证
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns>Result.Code：0=验证失败；1=验证成功</returns>
        public static Result Validate<T>(this T obj)
        {
            List<string> ErrorMessageList = new List<string>();
            Type type = obj.GetType();

            foreach (var property in type.GetProperties())
            {
                if (property.IsDefined(typeof(BaseValidator), true))
                {
                    var value = property.GetValue(obj);

                    foreach (BaseValidator attr in property.GetCustomAttributes(typeof(BaseValidator), true))
                    {
                        if (!attr.Valitate(value))
                        {
                            ErrorMessageList.Add(attr.ErrorMessage);
                        }
                    }
                }
            }

            return new Result
            {
                Code = ErrorMessageList.Any() ? 1 : 0,
                Message = string.Join(";", ErrorMessageList)
            };
        }
    }
}
