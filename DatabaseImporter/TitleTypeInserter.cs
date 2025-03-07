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
    public class TitleTypeInserter : DatabaseInserter
    {
        HashSet<string> writtenTitleTypes = new HashSet<string>();

        public override void InsertBulk(DataTable records, SqlConnection connection)
        {
            throw new NotImplementedException();
        }

        public override void InsertOne(string[] line, SqlConnection connection)
        {
            if (line.Length != 9)
            {
                throw new InvalidDataException($"line: {string.Join("\t", line)} only has {line.Length} values");
            }
            if(writtenTitleTypes.Contains(line[1]))
            {
                return;
            }
            SqlCommand cmd = new SqlCommand("InsertTitleType", connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter name = new()
            {
                ParameterName = "@Name",
                SqlDbType = SqlDbType.VarChar,
                Size = 50,
                Direction = ParameterDirection.Input,
                Value = line[1]
            };
            cmd.Parameters.Add(name);
            cmd.ExecuteNonQuery();
            writtenTitleTypes.Add(line[1]);
        }

        public override void Start(string filePath, InsertMode insertMode)
        {
            throw new NotImplementedException();
        }
    }
}
