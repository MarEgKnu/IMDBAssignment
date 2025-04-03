using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseImporter
{
    public class ProfessionInserter : DatabaseInserter
    {
        HashSet<string> writtenProfessions = new HashSet<string>();
        public override void Insert(string filePath, SqlConnection connection)
        {
            string[] lines = File.ReadAllLines(filePath).Skip(1).ToArray();
            for (int lineNum = 0; lineNum < lines.Length; lineNum++)
            {
                string[] fields = lines[lineNum].Split("\t");

                if (fields.Length != 6)
                {
                    throw new InvalidDataException($"line: {string.Join("\t", fields)} only has {fields.Length} values");
                }
                else if (fields[4] == @"\N")
                {
                    continue;
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
