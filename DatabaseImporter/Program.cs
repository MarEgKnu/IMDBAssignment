using DatabaseImporter;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net.Sockets;


DatabaseInserter titlesInserter = new TitlesInserter();
DatabaseInserter profInserter = new ProfessionInserter();
DatabaseInserter pplInserter = new PeopleInserter();
using(SqlConnection connection = new SqlConnection(Secret.ConnectionString))
{
    connection.Open();
    pplInserter.Insert("C:\\Users\\Marius\\Downloads\\name.basics.tsv\\name.basics.tsv", connection);
    //titlesInserter.Insert("C:\\Users\\Marius\\Downloads\\title.basics.tsv\\title.basics.tsv", connection);
    //profInserter.Insert("C:\\Users\\Marius\\Downloads\\name.basics.tsv\\name.basics.tsv", connection);

}








