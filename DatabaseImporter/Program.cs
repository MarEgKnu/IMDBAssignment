using DatabaseImporter;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net.Sockets;


DatabaseInserter titlesInserter = new TitlesInserter();
DatabaseInserter profInserter = new ProfessionInserter();
DatabaseInserter pplInserter = new PeopleInserterSingleThread();
DatabaseInserter catInserter = new CategoryInserter();
DBFileValidator principalsValidator = new PrincipalsTSVValidator();
using(SqlConnection connection = new SqlConnection(Secret.ConnectionString))
{
    connection.Open();
    Console.WriteLine("Enter path:");
    string path = Console.ReadLine();
    catInserter.Insert(path, connection);
    //principalsValidator.Validate(path);
    //pplInserter.Insert("C:\\Users\\Marius\\Downloads\\name.basics.tsv\\name.basics.tsv", connection);


    //titlesInserter.Insert("C:\\Users\\Marius\\Downloads\\title.basics.tsv\\title.basics.tsv", connection);
    //profInserter.Insert("C:\\Users\\Marius\\Downloads\\name.basics.tsv\\name.basics.tsv", connection);

}








