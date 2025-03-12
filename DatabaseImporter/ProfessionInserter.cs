using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseImporter
{
    public class ProfessionInserter : DatabaseInserter
    {
        HashSet<string> writtenProfessions = new HashSet<string>();
        public override void Insert(string filePath, SqlConnection connection)
        {
            string[] lines = File.ReadAllLines(filePath).Skip(1).ToArray();
            foreach (var line in lines)
            {
                string[] fields = line.Split("\t");

                if (fields.Length != 6)
                {
                    throw new InvalidDataException($"line: {string.Join("\t", fields)} only has {fields.Length} values");
                }
                else if (fields[4] == @"\N")
                {
                    return;
                }
                string[] professions = fields[4].Split(',');
                for (int i = 0; i < professions.Length; i++)
                {
                    if (writtenProfessions.Contains(professions[i]))
                    {
                        continue;
                    }

                    SqlCommand cmd = new SqlCommand("InsertProfession", connection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlParameter names = new()
                    {
                        ParameterName = "@Name",
                        SqlDbType = SqlDbType.VarChar,
                        Size = 30,
                        Direction = ParameterDirection.Input,
                        Value = professions[i]
                    };
                    cmd.Parameters.Add(names);
                    cmd.ExecuteNonQuery();
                    writtenProfessions.Add(professions[i]);

                }
            }
        }
    }
}
