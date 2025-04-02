using Microsoft.Data.SqlClient;

namespace ReadFromDatabase
{
    public class ReadClass
    {
        private string _connectionstring = DatabaseImporter.Secret.ConnectionString;
        public List<PersonWithTitles> ReadPersonsWithTitles(string search = "", int offset = 0, int fetch = 50, bool? ascending = true)
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
                while (reader.Read()) {
                    PersonWithTitles pwt = ReadPersonWithTitles(reader);
                    persons.Add(pwt); 
                }


            }

            return persons; 
        }

        private PersonWithTitles ReadPersonWithTitles(SqlDataReader reader)
        {
            bool birthyearnull = reader.IsDBNull(3);
            bool deathyearnull = reader.IsDBNull(4);
            bool rolesnull = reader.IsDBNull(5);
            bool titlesnull = reader.IsDBNull(6); 

            int? birthyear = birthyearnull ? null : reader.GetInt16(3);
            int? deathyear = deathyearnull ? null : reader.GetInt16(4);
            string[]? roles = rolesnull ? null : reader.GetString(5).Split(",");
            string[]? titles = titlesnull ? null : reader.GetString(6).Split(",");
            return new PersonWithTitles(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), birthyear, deathyear, roles!, titles!); 
        }
    }
}
