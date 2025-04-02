using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlClient.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseImporter
{
    public class PrincipalsInserter : DatabaseInserter
    {
        const int BATCHES = 10000;
        const int TCONST_MAX_SIZE = 12;
        const int NCONST_MAX_SIZE = 12;
        const int CATEGORY_MAX_SIZE = 20;
        const int JOB_MAX_SIZE = 290;
        const int CHARACTERS_MAX_SIZE = 465;
        private static SqlMetaData[] _principalsMetaData = new SqlMetaData[]
        {
            new SqlMetaData("tconst", System.Data.SqlDbType.VarChar, TCONST_MAX_SIZE),
            new SqlMetaData("ordering", System.Data.SqlDbType.TinyInt),
            new SqlMetaData("nconst", System.Data.SqlDbType.VarChar, NCONST_MAX_SIZE),
            new SqlMetaData("category", System.Data.SqlDbType.VarChar, CATEGORY_MAX_SIZE),
            new SqlMetaData("job", System.Data.SqlDbType.VarChar, JOB_MAX_SIZE),
            new SqlMetaData("characters", System.Data.SqlDbType.VarChar, CHARACTERS_MAX_SIZE),

        };
        public override void Insert(string filePath, SqlConnection connection)
        {
            int index = -1;
            IEnumerable<string> lines = File.ReadLines(filePath).Skip(1);
            List<SqlDataRecord> paramBuffer = new List<SqlDataRecord>(lines.Count() / BATCHES);
            //string[] lines = File.ReadAllLines(filePath).Skip(1).ToArray();
            foreach (string line in lines)
            {
                string[] fields = line.Split('\t');
                SqlDataRecord record = new SqlDataRecord(_principalsMetaData);
                index++;

                record.SetString(0, fields[0]);
                record.SetByte(1, byte.Parse(fields[1]));
                record.SetString(2, fields[2]);

                record.SetString(3, fields[3]);
                if (fields[4].IsNullString())
                {
                    record.SetDBNull(4);
                }
                else
                {
                    record.SetString(4, fields[4]);
                }
                if (fields[5].IsNullString())
                {
                    record.SetDBNull(5);
                }
                else
                {
                    record.SetString(5, fields[5]);
                }
                paramBuffer.Add(record);

                if ((index + 1) % paramBuffer.Count == 0)
                {
                    SqlCommand cmd = new SqlCommand("InsertPrincipalsBulk", connection) { CommandType = CommandType.StoredProcedure, CommandTimeout = 60 };
                    SqlParameter param = new SqlParameter("@InData", SqlDbType.Structured) { TypeName = "dbo.RawPrincipalData", Value = paramBuffer };
                    cmd.Parameters.Add(param);
                    cmd.ExecuteNonQuery();
                    paramBuffer.Clear();
                }
                
            }
            // the leftover rows
            SqlCommand cmd2 = new SqlCommand("InsertPrincipalsBulk", connection) { CommandType = CommandType.StoredProcedure, CommandTimeout = 60 };
            SqlParameter param2 = new SqlParameter("@InData", SqlDbType.Structured) { TypeName = "dbo.RawPrincipalData", Value = paramBuffer };
            cmd2.Parameters.Add(param2);
            cmd2.ExecuteNonQuery();
        }
    }
}
