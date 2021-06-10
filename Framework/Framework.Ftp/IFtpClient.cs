using System;
using System.IO;
using System.Threading.Tasks;

namespace Framework.FTP
{
    /// <summary>
    /// FTP客户端
    /// </summary>
    public interface IFTPClient
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="localPath">本地文件路径</param>
        /// <param name="remotePath">远程文件路径</param>
        /// <returns></returns>
        Task<bool> UploadFileAsync(string localPath, string remotePath);

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileStream">文件流</param>
        /// <param name="remotePath">远程文件路径</param>
        /// <returns></returns>
        Task<bool> UploadAsync(Stream fileStream, string remotePath);
    }
}
