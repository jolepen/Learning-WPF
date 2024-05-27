using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfExampleForToolkit.Services
{
    public class SqlService : DatabaseService
    {
        public SqlService(string connectionString) : base(connectionString)
        {
            this.Connection = new SqlConnection(this.ConnectionString);

            this.Command = new SqlCommand();
        }
    }
}
