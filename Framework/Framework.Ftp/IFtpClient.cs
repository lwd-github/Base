using System;
using System.IO;

namespace Framework.Ftp
{
    /// <summary>
    /// Ftp客户端
    /// </summary>
    public interface IFtpClient
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="localPath">本地文件路径</param>
        /// <param name="remotePath">远程文件路径</param>
        /// <returns></returns>
        bool Upload(string localPath, string remotePath);

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileStream">文件流</param>
        /// <param name="remotePath">远程文件路径</param>
        /// <returns></returns>
        bool Upload(Stream fileStream, string remotePath);
    }
}
