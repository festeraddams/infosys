using System;
using System.Threading;
using System.Threading.Tasks;

namespace Renci.SshNet
{
    /// <summary>
    /// 
    /// </summary>
    public partial class SftpClient
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        partial void ExecuteThread(Action action)
        {
            ThreadPool.QueueUserWorkItem((o) => { action(); });
        }
    }
}