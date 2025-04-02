using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseImporter
{
    public abstract class DBFileValidator
    {
        public abstract void Validate(string filePath, SqlConnection connection = null);
    }
}
