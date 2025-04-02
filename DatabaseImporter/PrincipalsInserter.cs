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
        const int BATCHES = 1000;
        const int TCONST_MAX_SIZE = 12;
        const int ORDERING_MAX_SIZE = 2;
        const int NCONST_MAX_SIZE = 12;
        const int CATEGORY_MAX_SIZE = 20;
        const int JOB_MAX_SIZE = 290;
        const int CHARACTERS_MAX_SIZE = 465;
        private static SqlMetaData[] _principalsMetaData = new SqlMetaData[]
        {
            new SqlMetaData("tconst", System.Data.SqlDbType.VarChar, TCONST_MAX_SIZE),
            new SqlMetaData("ordering", System.Data.SqlDbType.TinyInt),
            new SqlMetaData("nconst", System.Data.SqlDbType.VarChar, NCONST_MAX_SIZE),
            new SqlMetaData("category", System.Data.SqlDbType.VarChar, ORDERING_MAX_SIZE),
            new SqlMetaData("job", System.Data.SqlDbType.VarChar, JOB_MAX_SIZE),
            new SqlMetaData("characters", System.Data.SqlDbType.VarChar, CHARACTERS_MAX_SIZE),

        };
        public override void Insert(string filePath, SqlConnection connection)
        {
            int index = -1;
            IEnumerable<string> lines = File.ReadLines(filePath).Skip(1);
            SqlDataRecord[] paramBuffer = new SqlDataRecord[lines.Count() / BATCHES];
            //string[] lines = File.ReadAllLines(filePath).Skip(1).ToArray();
            foreach (string line in lines)
            {
                string[] fields = line.Split('\t');
                index++;
                paramBuffer[index] = new SqlDataRecord(_principalsMetaData);

                paramBuffer[index].SetString(0, fields[0]);              
                paramBuffer[index].SetByte(1, byte.Parse(fields[1]));
                paramBuffer[index].SetString(2, fields[2]);

                paramBuffer[index].SetString(3, fields[3]);
                if (fields[4].IsNullString())
                {
                    paramBuffer[index].SetDBNull(4);
                }
                else
                {
                    paramBuffer[index].SetString(4, fields[4]);
                }
                if (fields[5].IsNullString())
                {
                    paramBuffer[index].SetDBNull(5);
                }
                else
                {
                    paramBuffer[index].SetString(5, fields[5]);
                }

                if ((index + 1) % paramBuffer.Length == 0)
                {
                    SqlCommand cmd = new SqlCommand("InsertPrincipalsBulk", connection) { CommandType = CommandType.StoredProcedure, CommandTimeout = 300 };
                    SqlParameter param = new SqlParameter("@InData", SqlDbType.Structured) { TypeName = "dbo.RawPrincipalData", Value = paramBuffer };
                    cmd.Parameters.Add(param);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
