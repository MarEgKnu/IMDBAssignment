using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseImporter
{
    internal class TitlesInserter : DatabaseInserter
    {
        const int DIVISOR = 1000;
        public override void InsertBulk(DataTable records, SqlConnection connection)
        {
            SqlCommand cmd = new SqlCommand("InsertTitlesBulk", connection) { CommandType = CommandType.StoredProcedure, CommandTimeout = 300 };
            SqlParameter param = new SqlParameter("@InData", SqlDbType.Structured) { TypeName = "dbo.RawTitleData", Value = records, SqlValue = records};
            cmd.Parameters.Add(param);
            cmd.ExecuteNonQuery();
        }

        public override void InsertOne(string[] line, SqlConnection connection)
        {
            throw new NotImplementedException();
        }

        public override void Start(string filePath, InsertMode insertMode)
        {
            if (insertMode == InsertMode.InsertBulk)
            {
                using (SqlConnection conn = new SqlConnection(Secret.ConnectionString))
                {
                    conn.Open();
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
                        InsertBulk(DataTableHelpers.CreateDataTableTitles(segment), conn);
                    }
                    //GC.EndNoGCRegion();


                }
            }
        }
    }
}
