using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseImporter
{
    public static class DataTableHelpers
    {
        public static DataTable CreateDataTableTitles(IEnumerable<string> lines)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("tconst",  typeof(string));
            dt.Columns.Add("titleType", typeof(string));
            dt.Columns.Add("primaryTitle", typeof(string));
            dt.Columns.Add("originalTitle", typeof(string));
            dt.Columns.Add("isAdult", typeof(string));
            dt.Columns.Add("startYear", typeof(string));
            dt.Columns.Add("endYear", typeof(string));
            dt.Columns.Add("runTimeMinutes", typeof(string));
            dt.Columns.Add("genres", typeof(string));
            foreach (string line in lines)
            {
                DataRow dr = dt.NewRow();
                string[] fields = line.Split('\t');
                if(fields.Length != 9)
                {
                    throw new InvalidDataException($"{fields.Length} fields were detected, not 9");
                }
                else if (fields[0].Length > 12)
                {
                    throw new InvalidDataException("");
                }
                else if (fields[1].Length > 12)
                {
                    throw new InvalidDataException("");
                }
                else if (fields[2].Length > 255)
                {
                    throw new InvalidDataException("");
                }
                else if (fields[3].Length > 255)
                {
                    throw new InvalidDataException("");
                }
                else if (fields[4].Length > 2)
                {
                    throw new InvalidDataException("");
                }
                else if (fields[5].Length > 4)
                {
                    throw new InvalidDataException("");
                }
                else if (fields[6].Length > 4)
                {
                    throw new InvalidDataException("");
                }
                else if (fields[7].Length > 6)
                {
                    throw new InvalidDataException("");
                }
                else if (fields[8].Length > 100)
                {
                    throw new InvalidDataException("");
                }
                dr["tconst"] = fields[0];
                dr["titleType"] = fields[1];
                dr["primaryTitle"] = fields[2];
                dr["originalTitle"] = fields[3];
                dr["isAdult"] = fields[4];
                dr["startYear"] = fields[5];
                dr["endYear"] = fields[6];
                dr["runTimeMinutes"] = fields[7];
                dr["genres"] = fields[8];
                dt.Rows.Add(dr);

            }
            return dt;
        }
        public static DataTable CreateDataTableTitles(StreamReader sr)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("tconst", typeof(string));
            dt.Columns.Add("titleType", typeof(string));
            dt.Columns.Add("primaryTitle", typeof(string));
            dt.Columns.Add("originalTitle", typeof(string));
            dt.Columns.Add("isAdult", typeof(string));
            dt.Columns.Add("startYear", typeof(string));
            dt.Columns.Add("endYear", typeof(string));
            dt.Columns.Add("runTimeMinutes", typeof(string));
            dt.Columns.Add("genres", typeof(string));
            while(!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                DataRow dr = dt.NewRow();
                string[] fields = line.Split('\t');
                if (fields.Length != 9)
                {
                    throw new InvalidDataException($"{fields.Length} fields were detected, not 9");
                }
                dr["tconst"] = fields[0];
                dr["titleType"] = fields[1];
                dr["primaryTitle"] = fields[2];
                dr["originalTitle"] = fields[3];
                dr["isAdult"] = fields[4];
                dr["startYear"] = fields[5];
                dr["endYear"] = fields[6];
                dr["runTimeMinutes"] = fields[7];
                dr["genres"] = fields[8];
                dt.Rows.Add(dr);

            }
            return dt;
        }
    }
}
