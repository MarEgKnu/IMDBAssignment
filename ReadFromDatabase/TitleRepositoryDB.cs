using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ReadFromDatabase
{
    public class TitleRepositoryDB : ITitleRepository
    {
        private string _connectionstring = DatabaseImporter.Secret.ConnectionString;

        public TitleWithGenres AddTitleBasic(TitleWithGenres title)
        {
            using (SqlConnection connection = new SqlConnection(_connectionstring))
            {
                connection.Open();
                string query = "AddTitle";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@tconst", System.Data.SqlDbType.VarChar, 12)
                {
                    Value = title.TConst
                });
                cmd.Parameters.Add(new SqlParameter("@titleType", System.Data.SqlDbType.VarChar, 40)
                {
                    Value = title.TitleType
                });
                cmd.Parameters.Add(new SqlParameter("@primaryTitle", System.Data.SqlDbType.VarChar, 500)
                {
                    Value = title.PrimaryTitle
                }); cmd.Parameters.Add(new SqlParameter("@originalTitle", System.Data.SqlDbType.VarChar, 500)
                {
                    Value = title.OriginalTitle
                });
                cmd.Parameters.Add(new SqlParameter("@isAdult", System.Data.SqlDbType.Bit)
                {
                    Value = title.IsAdult
                });
                cmd.Parameters.Add(new SqlParameter("@startYear", System.Data.SqlDbType.SmallInt)
                {
                    Value = title.StartYear
                });
                cmd.Parameters.Add(new SqlParameter("@endYear", System.Data.SqlDbType.SmallInt)
                {
                    Value = title.EndYear
                });
                cmd.Parameters.Add(new SqlParameter("@runTimeMinutes", System.Data.SqlDbType.Int)
                {
                    Value = title.RunTimeMinutes
                });
                if(title.AggregatedGenres != null)
                {
                    cmd.Parameters.Add(new SqlParameter("@genres", System.Data.SqlDbType.VarChar, 500)
                    {
                        Value = string.Join('\t', title.AggregatedGenres)
                    });
                }
               

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    TitleWithGenres twg = ReadTitleWithGenres(reader);
                    return twg;
                }
            }

            return null;
        }

        public List<TitleWithGenres> GetTitlesBasic(string search = "", int offset = 0, int fetch = 50, bool ascending = true)
        {
            List<TitleWithGenres> titles = new List<TitleWithGenres>();

            using (SqlConnection connection = new SqlConnection(_connectionstring))
            {
                connection.Open();
                string query = "GetTitles";
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
                    TitleWithGenres twg = ReadTitleWithGenres(reader);
                    titles.Add(twg);
                }


            }

            return titles;
        }

        public bool DeleteTitle(int id)
        {

            using (SqlConnection connection = new SqlConnection(_connectionstring))
            {
                connection.Open();
                string query = "DeleteTitle";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@ID", System.Data.SqlDbType.Int)
                {
                    Value = id
                });

                int rowDeleted = cmd.ExecuteNonQuery();
                if (rowDeleted > 0)
                {
                    return true;
                }

            }

            return false;
        }

        public bool UpdateTitle(int id, TitleWithGenres title)
        {
            using (SqlConnection connection = new SqlConnection(_connectionstring))
            {
                connection.Open();
                string query = "UpdateTitle";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@ID", System.Data.SqlDbType.Int)
                {
                    Value = id
                });
                cmd.Parameters.Add(new SqlParameter("@titleType", System.Data.SqlDbType.VarChar, 40)
                {
                    Value = title.TitleType
                });
                cmd.Parameters.Add(new SqlParameter("@primaryTitle", System.Data.SqlDbType.VarChar, 500)
                {
                    Value = title.PrimaryTitle
                }); cmd.Parameters.Add(new SqlParameter("@originalTitle", System.Data.SqlDbType.VarChar, 500)
                {
                    Value = title.OriginalTitle
                });
                cmd.Parameters.Add(new SqlParameter("@isAdult", System.Data.SqlDbType.Bit)
                {
                    Value = title.IsAdult
                });
                cmd.Parameters.Add(new SqlParameter("@startYear", System.Data.SqlDbType.SmallInt)
                {
                    Value = title.StartYear
                });
                cmd.Parameters.Add(new SqlParameter("@endYear", System.Data.SqlDbType.SmallInt)
                {
                    Value = title.EndYear
                });
                cmd.Parameters.Add(new SqlParameter("@runTimeMinutes", System.Data.SqlDbType.Int)
                {
                    Value = title.RunTimeMinutes
                });
                if (title.AggregatedGenres != null)
                {
                    cmd.Parameters.Add(new SqlParameter("@genres", System.Data.SqlDbType.VarChar, 500)
                    {
                        Value = string.Join('\t', title.AggregatedGenres)
                    });
                }


                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return true;
                }
            }

            return false;
        }
        private TitleWithGenres ReadTitleWithGenres(SqlDataReader reader)
        {
            bool primarytitlenull = reader.IsDBNull(3);
            bool originaltitlenull = reader.IsDBNull(4);
            bool startyearnull = reader.IsDBNull(6);
            bool endyearnull = reader.IsDBNull(7);
            bool runtimeminutesnull = reader.IsDBNull(8);
            bool aggregatedgenresnull = reader.IsDBNull(9);

            return new TitleWithGenres(reader.GetInt32(0), reader.GetString(1), reader.GetString(2),
                primarytitlenull ? null : reader.GetString(3),
                originaltitlenull ? null : reader.GetString(4),
                reader.GetBoolean(5),
                startyearnull ? null : reader.GetInt16(6),
                endyearnull ? null : reader.GetInt16(7),
                runtimeminutesnull ? null : reader.GetInt32(8),
                aggregatedgenresnull ? null : reader.GetString(9).Split("\t")
                );
        }
    }
}
