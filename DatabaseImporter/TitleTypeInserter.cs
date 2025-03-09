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


        public override void Insert(string filePath, SqlConnection connection)
        {
            string[] lines = File.ReadAllLines(filePath).Skip(1).ToArray();
            foreach (var line in lines)
            {
                string[] fields = line.Split("\t");
                if (fields.Length != 9)
                {
                    throw new InvalidDataException($"line: {string.Join("\t", fields)} only has {fields.Length} values");
                }
                if (writtenTitleTypes.Contains(fields[1]))
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
                writtenTitleTypes.Add(fields[1]);
            }
        }

    }
}
