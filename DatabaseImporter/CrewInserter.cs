﻿using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlClient.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseImporter
{
    public class CrewInserter : DatabaseInserter
    {
        const int BATCHES = 1000;
        const int TCONST_MAX_SIZE = 12;
        const int WRITERS_MAX_SIZE = -1;
        const int DIRECTORS_MAX_SIZE = -1;
        private static SqlMetaData[] _crewMetaData = new SqlMetaData[]
        {
            new SqlMetaData("tconst", System.Data.SqlDbType.VarChar, TCONST_MAX_SIZE),
            new SqlMetaData("directors", System.Data.SqlDbType.VarChar, DIRECTORS_MAX_SIZE),
            new SqlMetaData("writers", System.Data.SqlDbType.VarChar, WRITERS_MAX_SIZE),


        };
        public override void Insert(string filePath, SqlConnection connection)
        {
            int index = -1;
            IEnumerable<string> lines = File.ReadLines(filePath).Skip(1);
            List<SqlDataRecord> paramBuffer = new List<SqlDataRecord>(lines.Count() / BATCHES);
            //string[] lines = File.ReadAllLines(filePath).Skip(1).ToArray();
            foreach (string line in lines)
            {
                string[] fields = line.Split('\t');
                SqlDataRecord record = new SqlDataRecord(_crewMetaData);

                if (fields[1].IsNullString() && fields[2].IsNullString())
                {
                    continue;
                }
                index++;

                record.SetString(0, fields[0]);
                if (fields[1].IsNullString())
                {
                    record.SetDBNull(1);
                }
                else
                {
                    record.SetString(1, fields[1]);
                }
                if (fields[2].IsNullString())
                {
                    record.SetDBNull(2);
                }
                else
                {
                    record.SetString(2, fields[2]);
                }

                paramBuffer.Add(record);

                if ((index + 1) % paramBuffer.Capacity == 0)
                {
                    SqlCommand cmd = new SqlCommand("InsertCrewLightBulk", connection) { CommandType = CommandType.StoredProcedure, CommandTimeout = 60 };
                    SqlParameter param = new SqlParameter("@InData", SqlDbType.Structured) { TypeName = "dbo.RawCrewData", Value = paramBuffer };
                    cmd.Parameters.Add(param);
                    cmd.ExecuteNonQuery();
                    paramBuffer.Clear();
                }
            }
            SqlCommand lastCmd = new SqlCommand("InsertCrewLightBulk", connection) { CommandType = CommandType.StoredProcedure, CommandTimeout = 60 };
            SqlParameter lastParam = new SqlParameter("@InData", SqlDbType.Structured) { TypeName = "dbo.RawCrewData", Value = paramBuffer };
            lastCmd.Parameters.Add(lastParam);
            lastCmd.ExecuteNonQuery();
            paramBuffer.Clear();
        }
    }
}
