using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadFromDatabase
{
    public class ProfessionRepositoryDB : IProfessionRepository
    {
        private string _connectionstring = DatabaseImporter.Secret.ConnectionString;
        public List<Profession> GetAll()
        {
            List<Profession> professions = new List<Profession>();

            using (SqlConnection connection = new SqlConnection(_connectionstring))
            {
                connection.Open();
                string query = "SELECT * FROM ProfessionView";
                SqlCommand cmd = new SqlCommand(query, connection);

                cmd.CommandType = CommandType.Text;
                cmd.Prepare();



                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Profession profession = CreateProfession(reader);
                    professions.Add(profession);
                }
            }

            return professions;
        }

        public Profession? GetByID(byte id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionstring))
            {
                connection.Open();
                string query = "SELECT * FROM GetProfessionByID(@ID)";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.TinyInt)
                {
                    Value = id
                });
                cmd.Prepare();



                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Profession profession = CreateProfession(reader);
                    return profession;
                }
            }

            return null;
        }

        private Profession CreateProfession(SqlDataReader reader)
        {
            return new Profession(reader.GetByte(0), reader.GetString(1));
        }
    }
}
