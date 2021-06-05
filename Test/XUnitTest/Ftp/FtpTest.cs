using Framework.Ftp;
using Framework.IOC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTest.Ftp
{
    public class FtpTest : BaseTest
    {
        readonly IFtpClient _ftpClient;

        public FtpTest()
        {
            _ftpClient = IocManager.Resolve<IFtpClient>();
        }


        /// <summary>
        /// Ftp客户端测试
        /// </summary>
        [Fact]
        public void Test()
        {
            var result = _ftpClient.Upload(@"C:\Users\future\Desktop\RcPI5BqGtPQjcu.jpg", $"{DateTime.Now.ToString("yyyyMMdd")}/test.jpg");
            Assert.True(result);
        }
    }
}
