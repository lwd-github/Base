using Framework.Common.Exception;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Validator
{
    public class Verify
    {
        /// <summary>
        /// 如果条件为true，抛出异常
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="message"></param>
        public static void If(bool condition, string message)
        {
            If(condition, new ValidationException(message));
        }


        /// <summary>
        /// 如果条件为true，抛出异常
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="exception"></param>
        public static void If<T>(bool condition, T exception) where T : Exception
        {
            if (condition)
            {
                throw exception;
            }   
        }


        /// <summary>
        /// 如果对象Null，抛出异常
        /// </summary>
        /// <param name="item"></param>
        /// <param name="message"></param>
        public static void IfNull(object item, string message)
        {
            IfNull(item, new ValidationException(message));
        }


        /// <summary>
        /// 如果对象Null，抛出异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="exception"></param>
        public static void IfNull<T>(object item, T exception) where T : Exception
        {
            if (item == null)
            {
                throw exception;
            }    
        }
    }
}
