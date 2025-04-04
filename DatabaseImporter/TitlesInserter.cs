﻿using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseImporter
{
    internal class TitlesInserter : DatabaseInserter
    {
        const int DIVISOR = 1000;

        public override void Insert(string filePath, SqlConnection connection)
        {
            string[] lines = File.ReadAllLines(filePath).Skip(1).ToArray();
            int itemsPerSegment = lines.Length / DIVISOR;
            int extraItems = lines.Length % DIVISOR;
            for (int i = 0; i < DIVISOR; i++)
            {
                ArraySegment<string> segment;
                if (i == 0)
                {
                    segment = new ArraySegment<string>(lines, 0, itemsPerSegment);
                }
                else if (i == DIVISOR - 1)
                {
                    segment = new ArraySegment<string>(lines, i * itemsPerSegment, itemsPerSegment + extraItems);
                }
                else
                {
                    segment = new ArraySegment<string>(lines, i * itemsPerSegment, itemsPerSegment);
                }
                SqlCommand cmd = new SqlCommand("InsertTitlesBulk", connection) { CommandType = CommandType.StoredProcedure, CommandTimeout = 300 };
                SqlParameter param = new SqlParameter("@InData", SqlDbType.Structured) { TypeName = "dbo.RawTitleData", Value = DataTableHelpers.CreateDataTableTitles(segment) };
                cmd.Parameters.Add(param);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
