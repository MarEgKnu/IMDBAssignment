using DatabaseImporter;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net.Sockets;


DatabaseInserter titlesInserter = new TitlesInserter();
using(SqlConnection connection = new SqlConnection(Secret.ConnectionString))
{
    connection.Open();
    titlesInserter.Insert("C:\\Users\\Marius\\Downloads\\title.basics.tsv\\title.basics.tsv", connection);
}








