using Framework.Common.Currency;
using Framework.Common.Results;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebApi.Controllers
{
    [ApiController]
    public class OutputFormaterController : ControllerBase
    {
        public class OutputFormater
        {
            public CNY S1 { get; set; }

            public DateTime S2 { get; set; }

            public float S3 { get; set; }

            public double S4 { get; set; }

            public Double S5 { get; set; }

            public decimal S6 { get; set; }
        }


        public class InputFormater
        {
            public int I1 { get; set; }

            public int? I2 { get; set; }

            public EStatus I3 { get; set; }

            public EStatus? I4 { get; set; }

            public DateTime I5 { get; set; }

            public DateTime? I6 { get; set; }
        }

        public enum EStatus
        {
            A = 0,
            B = 1
        }

        [HttpGet("/outputformater")]
        public Result<OutputFormater> Get()
        {
            var f1 = new OutputFormater { S1 = 12.126M, S2 = DateTime.Now };
            var f2 = new OutputFormater { S1 = 100M, S2 = DateTime.Now };

            return new Result<OutputFormater>
            {
                Data = new OutputFormater { S1 = f1.S1 + f2.S1, S2 = DateTime.Now, S3 = 12323.123F, S4 = 567.126, S5 = 89.233, S6 = 9998.016M }
            };
        }


        [HttpPost("/inputformater")]
        public void Post(InputFormater input)
        {

        }
    }
}
