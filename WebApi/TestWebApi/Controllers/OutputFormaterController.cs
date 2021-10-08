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
        public class Formater
        { 
            public CNY S1 { get; set; }

            public DateTime S2 { get; set; }

            public float S3 { get; set; }

            public double S4 { get; set; }

            public Double S5 { get; set; }

            public decimal S6 { get; set; }
        }

        [HttpGet("/outputformater")]
        public Result<Formater> Get()
        {
            var f1 = new Formater { S1 = 12.126M, S2 = DateTime.Now};
            var f2 = new Formater { S1 = 100M, S2 = DateTime.Now };

            return new Result<Formater>
            {
                Data = new Formater { S1 = f1.S1 + f2.S1, S2 = DateTime.Now, S3 = 12323.123F, S4 = 567.126, S5 = 89.233, S6 = 9998.016M }
            };
        }
    }
}
