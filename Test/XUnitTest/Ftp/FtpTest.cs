using DTO.Constant;
using Framework.FTP;
using Framework.IdGenerator;
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
            var now = DateTime.Now;

            //本地文件
            //var path = @"C:\Users\future\Desktop\test.jpg";
            var path = @"C:\Users\duan\Desktop\timg.jpg";
            var extension = Path.GetExtension(path);

            var fileName = Guid.NewGuid().ToString().Replace("-", string.Empty);
            var result = await _ftpClient.UploadFileAsync(path, $"{SystemConstant.SystemName}/{Enumeration.System.EModule.Product}/{now.Year}/{now.Month.ToString().PadLeft(2, '0')}/{now.Day.ToString().PadLeft(2, '0')}/{fileName}{extension}");

            fileName = new IdWorker(1, 1).NextId().ToString();
            var fileStream = File.OpenRead(path);
            result = await _ftpClient.UploadAsync(fileStream, $"{SystemConstant.SystemName}/{Enumeration.System.EModule.Product}/{now.Year}/{now.Month.ToString().PadLeft(2, '0')}/{now.Day.ToString().PadLeft(2, '0')}/{fileName}{extension}");
            Assert.True(result);
        }
    }
}
