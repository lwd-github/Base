using Framework.FTP.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Framework.FTP
{
    /// <summary>
    /// Ftp客户端
    /// </summary>
    public class FTPClient: IFTPClient
    {
        FluentFTP.FtpClient _ftpClient;

        public FTPClient(FTPConfig config)
        {
            _ftpClient = new FluentFTP.FtpClient(config.Host, config.Port, config.User, config.Password);
        }


        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="localPath">本地文件路径</param>
        /// <param name="remotePath">远程文件路径</param>
        /// <returns></returns>
        public async Task<bool> UploadFileAsync(string localPath, string remotePath)
        {
            var ftpStatus = await _ftpClient.UploadFileAsync(localPath, remotePath, FluentFTP.FtpRemoteExists.Overwrite, true);
            return ftpStatus == FluentFTP.FtpStatus.Success;
        }


        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileStream">文件流</param>
        /// <param name="remotePath">远程文件路径</param>
        /// <returns></returns>
        public async Task<bool> UploadAsync(Stream fileStream, string remotePath)
        {
            var ftpStatus = await _ftpClient.UploadAsync(fileStream, remotePath, FluentFTP.FtpRemoteExists.Overwrite, true);
            return ftpStatus == FluentFTP.FtpStatus.Success;
        }
    }
}
