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
        public abstract void Insert(string filePath, SqlConnection connection);


    }
}
