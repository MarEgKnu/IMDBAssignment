using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ReadFromDatabase
{
    public class GenreRepositoryDB : IGenreRepository
    {
        private string _connectionstring = DatabaseImporter.Secret.ConnectionString;
        public List<Genre> GetAll()
        {
            List<Genre> genres = new List<Genre>();

            using (SqlConnection connection = new SqlConnection(_connectionstring))
            {
                connection.Open();
                string query = "SELECT * FROM GetGenres()";
                SqlCommand cmd = new SqlCommand(query, connection);
                
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Prepare();



                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Genre genre = CreateGenre(reader);
                    genres.Add(genre);
                }
            }

            return genres;
        }

        public Genre? GetByID(byte id)
        {

            using (SqlConnection connection = new SqlConnection(_connectionstring))
            {
                connection.Open();
                string query = "SELECT * FROM GetGenre(@ID)";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@ID", id));
                cmd.Prepare();



                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Genre genre = CreateGenre(reader);
                    return genre;
                }
            }

            return null;
        }
        
        private Genre CreateGenre(SqlDataReader reader)
        {
            return new Genre(reader.GetByte(0), reader.GetString(1));
        }
    }
}
