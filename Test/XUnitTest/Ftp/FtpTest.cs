using Framework.Ftp;
using Framework.IOC;
using System;
using System.Collections.Generic;
using System.IO;
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
            var path = @"C:\Users\future\Desktop\test.jpg";
            var result = _ftpClient.Upload(path, $"{DateTime.Now.ToString("yyyyMMdd")}/test.jpg");

            var fileStream = File.OpenRead(path);
            result = _ftpClient.Upload(fileStream, $"{DateTime.Now.ToString("yyyyMMdd")}/test.jpg");
            Assert.True(result);
        }
    }
}
