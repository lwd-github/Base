using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Common.Results
{
    public class Result<T>
    {
        public Result()
        {

        }

        public Result(int code)
        {
            Code = code;
        }

        ///// <summary>
        ///// true:成功；false:失败
        ///// </summary>
        //public bool Status { get; set; }

        /// <summary>
        /// 业务编码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 返回的数据信息
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message {get;set;}
    }


    public class Result: Result<object>
    { 
    }
}
