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
                ValidateTitleFields(fields);
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
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                DataRow dr = dt.NewRow();
                string[] fields = line.Split('\t');
                ValidateTitleFields(fields);
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
        private static void ValidateTitleFields(string[] fields)
        {
            if (fields.Length != 9)
            {
                throw new InvalidDataException($"{fields.Length} fields were detected, not 9");
            }
            else if (fields[0].Length > 12)
            {
                throw new InvalidDataException($"tconst field {fields[0]} was longer than 12 characters long");
            }
            else if (fields[1].Length > 12)
            {
                throw new InvalidDataException($"titleType field {fields[1]} was longer than 12 characters long");
            }
            else if (fields[2].Length > 500)
            {
                throw new InvalidDataException($"primaryTitle field {fields[2]} was longer than 500 characters long");
            }
            else if (fields[3].Length > 500)
            {
                throw new InvalidDataException($"originalTitle field {fields[3]} was longer than 500 characters long");
            }
            else if (fields[4].Length > 2)
            {
                throw new InvalidDataException($"isAdult field {fields[4]} was longer than 2 characters long");
            }
            else if (fields[5].Length > 4)
            {
                throw new InvalidDataException($"startYear field {fields[5]} was longer than 4 characters long");
            }
            else if (fields[6].Length > 4)
            {
                throw new InvalidDataException($"endYear field {fields[6]} was longer than 4 characters long");
            }
            else if (fields[7].Length > 8)
            {
                throw new InvalidDataException($"runTimeMinutes field {fields[7]} was longer than 8 characters long");
            }
            else if (fields[8].Length > 100)
            {
                throw new InvalidDataException($"Genres field {fields[8]} was longer than 100 characters long");
            }
        }
    }
}
