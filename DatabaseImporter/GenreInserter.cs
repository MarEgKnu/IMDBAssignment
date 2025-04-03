using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseImporter
{
    public class GenreInserter : DatabaseInserter
    {
        HashSet<string> writtenGenres = new HashSet<string>();



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
                else if (fields[8] == @"\N")
                {
                    return;
                }
                string[] genres = fields[8].Split(',');
                for (int i = 0; i < genres.Length; i++)
                {
                    if (writtenGenres.Contains(genres[i]))
                    {
                        continue;
                    }

                    SqlCommand cmd = new SqlCommand("InsertGenre", connection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlParameter names = new()
                    {
                        ParameterName = "@Name",
                        SqlDbType = SqlDbType.VarChar,
                        Size = 50,
                        Direction = ParameterDirection.Input,
                        Value = genres[i]
                    };
                    cmd.Parameters.Add(names);
                    cmd.ExecuteNonQuery();
                    writtenGenres.Add(genres[i]);

                }
            }
        }
    }
}
