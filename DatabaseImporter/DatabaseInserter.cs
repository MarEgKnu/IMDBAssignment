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
    public abstract class DatabaseInserter
    {
        public abstract void Start(string filePath, InsertMode insertMode);
        public void InsertMany(string[] lines, SqlConnection connection)
        {
            for(int i = 1; i < lines.Length; i++)
            {
                InsertOne(lines[i].Split('\t'), connection);
            }
        }
        public void InsertMany(StreamReader sr, SqlConnection connection)
        {
            while (!sr.EndOfStream)
            {
                InsertOne(sr.ReadLine().Split('\t'), connection);
            }

        }
        public abstract void InsertBulk(DataTable records, SqlConnection connection);

        public abstract void InsertOne(string[] line, SqlConnection connection);


    }
}
