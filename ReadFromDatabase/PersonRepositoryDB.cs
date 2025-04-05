using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadFromDatabase
{
    public class PersonRepositoryDB : IPersonRepository
    {
        private string _connectionstring = DatabaseImporter.Secret.ConnectionString;
        public List<PersonWithTitles> ReadPersonsBasic(string search = "", int offset = 0, int fetch = 50, bool? ascending = true)
        {
            List<PersonWithTitles> persons = new List<PersonWithTitles>();

            using (SqlConnection connection = new SqlConnection(_connectionstring))
            {
                connection.Open();
                string query = "GetPersons";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@searchString", System.Data.SqlDbType.VarChar, 50)
                {
                    Value = search
                });
                cmd.Parameters.Add(new SqlParameter("@offset", System.Data.SqlDbType.Int)
                {
                    Value = offset
                });
                cmd.Parameters.Add(new SqlParameter("@rows", System.Data.SqlDbType.Int)
                {
                    Value = fetch
                }); cmd.Parameters.Add(new SqlParameter("@ascending", System.Data.SqlDbType.Bit)
                {
                    Value = ascending
                });

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    PersonWithTitles pwt = ReadPersonWithTitles(reader);
                    persons.Add(pwt);
                }


            }

            return persons;
        }

        public PersonWithTitles AddPersonBasic(PersonWithTitles person)
        {
            using (SqlConnection connection = new SqlConnection(_connectionstring))
            {
                connection.Open();
                string query = "AddPerson";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@nconst", System.Data.SqlDbType.VarChar, 12)
                {
                    Value = person.NConst
                });
                cmd.Parameters.Add(new SqlParameter("@primaryName", System.Data.SqlDbType.VarChar, 120)
                {
                    Value = person.PrimaryName
                });
                cmd.Parameters.Add(new SqlParameter("@birthYear", System.Data.SqlDbType.SmallInt)
                {
                    Value = person.BirthYear
                }); cmd.Parameters.Add(new SqlParameter("@deathYear", System.Data.SqlDbType.SmallInt)
                {
                    Value = person.DeathYear
                });
                if (person.Roles != null)
                {
                    cmd.Parameters.Add(new SqlParameter("@roles", System.Data.SqlDbType.VarChar, 500)
                    {
                        Value = string.Join('\t', person.Roles)
                    });
                }
                if (person.Titles != null)
                {
                    cmd.Parameters.Add(new SqlParameter("@knownForTitles", System.Data.SqlDbType.VarChar, 500)
                    {
                        Value = string.Join('\t', person.Titles)
                    });
                }


                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    PersonWithTitles twg = ReadPersonWithTitles(reader);
                    return twg;
                }
            }

            return null;
        }
        private PersonWithTitles ReadPersonWithTitles(SqlDataReader reader)
        {
            bool birthyearnull = reader.IsDBNull(3);
            bool deathyearnull = reader.IsDBNull(4);
            bool rolesnull = reader.IsDBNull(5);
            bool titlesnull = reader.IsDBNull(6);

            int? birthyear = birthyearnull ? null : reader.GetInt16(3);
            int? deathyear = deathyearnull ? null : reader.GetInt16(4);
            string[]? roles = rolesnull ? null : reader.GetString(5).Split("\t");
            string[]? titles = titlesnull ? null : reader.GetString(6).Split("\t");
            return new PersonWithTitles(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), birthyear, deathyear, roles!, titles!);
        }
    }
}
