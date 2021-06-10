using Framework.FTP;
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
        readonly IFTPClient _ftpClient;

        public FtpTest()
        {
            _ftpClient = IocManager.Resolve<IFTPClient>();
        }


        /// <summary>
        /// Ftp客户端测试
        /// </summary>
        [Fact]
        public async void Test()
        {
            var path = @"C:\Users\future\Desktop\test.jpg";
            var result = await _ftpClient.UploadFileAsync(path, $"{DateTime.Now.ToString("yyyyMMdd")}/test1.jpg");

            var fileStream = File.OpenRead(path);
            result = await _ftpClient.UploadAsync(fileStream, $"{DateTime.Now.ToString("yyyyMMdd")}/test2.jpg");
            Assert.True(result);
        }
    }
}
