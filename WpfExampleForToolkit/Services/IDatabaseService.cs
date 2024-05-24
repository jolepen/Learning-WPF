using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfExampleForToolkit.Services
{
    internal interface IDatabaseService
    {
        string ConnectionString { get; }
        /// <summary>
        /// GetDatasAsync
        /// </summary>
        /// <remarks>
        /// query를 실행해서 IList&lt;<typeparamref name="T"/>&gt;를 반환한다.
        /// </remarks>
        Task<IList<T>> GetDatasAsync<T>(string query) where T : class;
    }
}
