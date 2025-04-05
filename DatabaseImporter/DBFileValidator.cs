using Microsoft.Data.SqlClient;

namespace DatabaseImporter
{
    public abstract class DBFileValidator
    {
        public abstract void Validate(string filePath, SqlConnection connection = null);
    }
}
