using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Results
{
    public class ResultSuccess<T> : Framework.Common.Results.Result<T>
    {
        public ResultSuccess()
        {
            Code = (int)Enumeration.System.EResultCode.OK;
        }
    }


    public class ResultSuccess: Framework.Common.Results.Result
    {
        public ResultSuccess()
        {
            Code = (int)Enumeration.System.EResultCode.OK;
        }
    }
}
