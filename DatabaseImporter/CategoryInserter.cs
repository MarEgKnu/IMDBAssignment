using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseImporter
{
    public class CategoryInserter : DatabaseInserter
    {
        HashSet<string> writtenProfessions = new HashSet<string>();
        public override void Insert(string filePath, SqlConnection connection)
        {
            IEnumerable<string> lines = File.ReadLines(filePath).Skip(1);
            foreach (string line in lines)
            {
                string[] fields = line.Split("\t");

                if (fields.Length != 6)
                {
                    throw new InvalidDataException($"line: {string.Join("\t", fields)} only has {fields.Length} values");
                }
                if (writtenProfessions.Contains(fields[3]))
                {
                    continue;
                }

                SqlCommand cmd = new SqlCommand("InsertCategory", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter names = new()
                {
                    ParameterName = "@Name",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 25,
                    Direction = ParameterDirection.Input,
                    Value = fields[3]
                };
                cmd.Parameters.Add(names);
                cmd.ExecuteNonQuery();
                writtenProfessions.Add(fields[3]);


            }
        }
    }
}
