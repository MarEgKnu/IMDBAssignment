using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlClient.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseImporter
{
    public class CrewExistsValidator : DBFileValidator
    {
        const int BATCHES = 1000;
        const int TCONST_MAX_SIZE = 12;
        const int NCONST_MAX_SIZE = 12;
        const int CATEGORY_MAX_SIZE = 20;
        const int JOB_MAX_SIZE = 290;
        const int CHARACTERS_MAX_SIZE = 465;
        private static SqlMetaData[] _crewMetaData = new SqlMetaData[]
        {
            new SqlMetaData("tconst", System.Data.SqlDbType.VarChar, TCONST_MAX_SIZE),
            new SqlMetaData("directors", System.Data.SqlDbType.VarChar, NCONST_MAX_SIZE),
            new SqlMetaData("writers", System.Data.SqlDbType.VarChar, NCONST_MAX_SIZE),


        };
        public override void Validate(string filePath, SqlConnection connection = null)
        {
            IEnumerable<string> lines = File.ReadLines(filePath).Skip(1);
            List<SqlDataRecord> paramBuffer = new List<SqlDataRecord>(lines.Count() * 3);
            //string[] lines = File.ReadAllLines(filePath).Skip(1).ToArray();
            foreach (string line in lines)
            {
                string[] fields = line.Split('\t');
                if (fields[1].IsNullString() && fields[2].IsNullString())
                {
                    continue;
                }
                string[] directors = fields[1].Split(",");
                string[] writers = fields[2].Split(",");
                if (fields[1].IsNullString())
                {
                    for(int i = 0; i < writers.Length; i++)
                    {
                        paramBuffer.Add(CreateCrewRecord(fields[0], null, writers[i]));
                    }
                }
                else if (fields[2].IsNullString())
                {
                    for (int i = 0; i < directors.Length; i++)
                    {
                        paramBuffer.Add(CreateCrewRecord(fields[0], directors[i], null));
                    }
                }
                else
                {
                    for (int i = 0; i < writers.Length; i++)
                    {
                        for (int j = 0; j < directors.Length; j++)
                        {
                            paramBuffer.Add(CreateCrewRecord(fields[0], directors[j], writers[i]));
                        }
                    }
                }
                
            }
            int x = 5;
        }
        private SqlDataRecord CreateCrewRecord(string tconst, string? director, string? writer)
        {
            SqlDataRecord record = new SqlDataRecord(_crewMetaData);
            record.SetString(0, tconst);
            if(director is not null)
            {
                record.SetString(0, director);
            }
            if (writer is not null)
            {
                record.SetString(0, writer);
            }
            return record;
        }
    }
}
