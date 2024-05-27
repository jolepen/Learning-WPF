using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfExampleForToolkit.Services
{
    public abstract class DatabaseService : IDatabaseService
    {
        private string _connectionString;
        public string ConnectionString => this._connectionString;

        protected DbConnection Connection { get; set; }

        protected DbCommand Command { get; set; }

        public DatabaseService(string connectionString)
        {
            this._connectionString = connectionString;
        }

        /// <summary>
        /// Task를 사용하는 메서드의 비동기 방식은 하나의 스레드가 비동기 작업을 하는것이 아닌, 여러 스레드를 활용하여 비동기 작업을 진행한다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual async Task<IList<T>> GetDatasAsync<T>(string query) where T : class
        {
            if (Connection == null || Command == null || string.IsNullOrEmpty(query))
            {
                return null;
            }

            //커넥션 열기
            await Connection.OpenAsync();

            //쿼리 입력
            Command.CommandText = query;
            Command.Connection = Connection;
            var returnDatas = new List<T>();

            //커맨드를 실행하고 리더를 반환한다.
            using var reader = await Command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var row = (IDataRecord)reader;

                var model = Activator.CreateInstance(typeof(T));

                returnDatas.Add(model as T);

                var properties = model.GetType().GetProperties();

                //프로퍼티 중 HasErrors라는 이름의 프로퍼티 빼고 나머지 데이터 입력
                foreach (var prop in properties.Where(p => p.Name != "HasErrors"))
                {
                    try
                    {
                        var value = row[prop.Name];
                        if (value is DBNull == false)
                        {
                            prop.SetValue(model, value);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
            }
            //컨넥션 닫기
            await Connection.CloseAsync();
            //결과 반환
            return returnDatas;
        }
    }
}
