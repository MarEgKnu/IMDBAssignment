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
    public class GenreInserter : DatabaseInserter
    {
        HashSet<string> writtenGenres = new HashSet<string>();

        public override void InsertBulk(DataTable records, SqlConnection connection)
        {
            throw new NotImplementedException();
        }

        public override void InsertOne(string[] line, SqlConnection connection)
        {
            if(line.Length != 9)
            {
                throw new InvalidDataException($"line: {string.Join("\t", line)} only has {line.Length} values");
            }
            else if (line[8] == @"\N")
            {
                return;
            }
            string[] genres = line[8].Split(',');
            for( int i = 0; i < genres.Length; i++)
            {
                if(writtenGenres.Contains(genres[i]))
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

        public override void Start(string filePath, InsertMode insertMode)
        {
            throw new NotImplementedException();
        }
    }
}
