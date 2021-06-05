using Framework.Ftp.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Framework.Ftp
{
    /// <summary>
    /// Ftp客户端
    /// </summary>
    public class FtpClient: IFtpClient
    {
        FluentFTP.FtpClient _ftpClient;

        public FtpClient(FtpConfig config)
        {
            _ftpClient = new FluentFTP.FtpClient(config.Host, config.Port, config.User, config.Password);
        }


        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="localPath">本地文件路径</param>
        /// <param name="remotePath">远程文件路径</param>
        /// <returns></returns>
        public bool Upload(string localPath, string remotePath)
        {
            var ftpStatus = _ftpClient.UploadFile(localPath, remotePath, FluentFTP.FtpRemoteExists.Overwrite, true);
            return ftpStatus == FluentFTP.FtpStatus.Success ? true : false;
        }


        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileStream">文件流</param>
        /// <param name="remotePath">远程文件路径</param>
        /// <returns></returns>
        public bool Upload(Stream fileStream, string remotePath)
        {
            var ftpStatus = _ftpClient.Upload(fileStream, remotePath, FluentFTP.FtpRemoteExists.Overwrite, true);
            return ftpStatus == FluentFTP.FtpStatus.Success ? true : false;
        }
    }
}
