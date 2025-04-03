using Microsoft.Data.SqlClient;

namespace DatabaseImporter
{
    public abstract class DatabaseInserter
    {
        public abstract void Insert(string filePath, SqlConnection connection);


    }
}
