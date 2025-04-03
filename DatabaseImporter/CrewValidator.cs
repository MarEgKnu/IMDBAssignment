using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseImporter
{

    public class CrewValidator : DBFileValidator
    {
        const int NUM_OF_FIELDS = 3;
        private string[] fieldNames = new string[NUM_OF_FIELDS] { "tconst", "directors", "writers" };   
        private string[] largestFieldValues = new string[NUM_OF_FIELDS];

        public CrewValidator()
        {
            for(int i = 0; i < largestFieldValues.Length; i++)
            {
                largestFieldValues[i] = string.Empty;
                
            }
        }
        public override void Validate(string filePath, SqlConnection connection = null)
        {
            int lineNum = 1;
            IEnumerable<string> lines = File.ReadLines(filePath).Skip(1);
            //string[] lines = File.ReadAllLines(filePath).Skip(1).ToArray();
            foreach(string line in lines)
            {
                lineNum++;
                string[] fields = line.Split("\t");

                if (fields.Length != NUM_OF_FIELDS)
                {
                    throw new InvalidDataException($"line: {string.Join("\t", fields)} only has {fields.Length} values");
                }
                for (int fieldNum = 0; fieldNum < fields.Length; fieldNum++)
                {
                    // check if its the longest
                    if (fields[fieldNum].Length > largestFieldValues[fieldNum].Length)
                    {
                        largestFieldValues[fieldNum] = fields[fieldNum];
                    }                
                }
      
            }
            for(int fieldNum = 0; fieldNum < NUM_OF_FIELDS; fieldNum++)
            {
                Console.WriteLine($"Field {fieldNames[fieldNum]} longest word is {largestFieldValues[fieldNum]} with {largestFieldValues[fieldNum].ToString().Length} characters\n");
                                
            }
        }
    }
}
