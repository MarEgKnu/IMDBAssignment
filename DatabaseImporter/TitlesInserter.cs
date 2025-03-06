using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fileTest
{
    internal class TitlesInserter : DatabaseInserter
    {
        public override void InsertBulk(DataTable records, SqlConnection connection)
        {
            SqlCommand cmd = new SqlCommand("InsertTitlesBulk", connection) { CommandType = CommandType.StoredProcedure, CommandTimeout = 300};
            SqlParameter param = new SqlParameter("@InData", SqlDbType.Structured) { TypeName = "RawTitleData"};
            cmd.Parameters.Add(param);
            cmd.ExecuteNonQuery();
        }

        public override void InsertOne(string[] line, SqlConnection connection)
        {
            throw new NotImplementedException();
        }
    }
}
