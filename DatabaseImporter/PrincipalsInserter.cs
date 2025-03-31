using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlClient.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseImporter
{
    public class PrincipalsInserter : DatabaseInserter
    {
        const int BATCHES = 1000;
        const int TCONST_MAX_SIZE = 12;
        const int ORDERING_MAX_SIZE = 2;
        const int NCONST_MAX_SIZE = 12;
        const int CATEGORY_MAX_SIZE = 20;
        const int JOB_MAX_SIZE = 290;
        const int CHARACTERS_MAX_SIZE = 465;
        private static SqlMetaData[] _principalsMetaData = new SqlMetaData[]
        {
            new SqlMetaData("tconst", System.Data.SqlDbType.VarChar, TCONST_MAX_SIZE),
            new SqlMetaData("ordering", System.Data.SqlDbType.VarChar, ORDERING_MAX_SIZE),
            new SqlMetaData("nconst", System.Data.SqlDbType.VarChar, NCONST_MAX_SIZE),
            new SqlMetaData("category", System.Data.SqlDbType.VarChar, CATEGORY_MAX_SIZE),
            new SqlMetaData("job", System.Data.SqlDbType.VarChar, JOB_MAX_SIZE),
            new SqlMetaData("characters", System.Data.SqlDbType.VarChar, CHARACTERS_MAX_SIZE),

        };
        public override void Insert(string filePath, SqlConnection connection)
        {
            int index = -1;
            IEnumerable<string> lines = File.ReadLines(filePath).Skip(1);
            SqlDataRecord[] paramBuffer = new SqlDataRecord[lines.Count() / BATCHES];
            //string[] lines = File.ReadAllLines(filePath).Skip(1).ToArray();
            foreach (string line in lines)
            {
                string[] fields = line.Split('\t');
                index++;
                paramBuffer[index] = new SqlDataRecord(_principalsMetaData);
                paramBuffer[index].SetString(0, fields[0]);
                paramBuffer[index].SetString(1, fields[1]);
                paramBuffer[index].SetString(2, fields[2]);
                paramBuffer[index].SetString(3, fields[3]);
                paramBuffer[index].SetString(4, fields[4]);
                paramBuffer[index].SetString(5, fields[5]);

                if(index + 1 % BATCHES == 0)
                {
                    // call procedure to batch insert
                }
            }
        }
    }
}
