using DatabaseImporter;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net.Sockets;




DatabaseInserter titlesInserter = new TitlesInserter();
DatabaseInserter profInserter = new ProfessionInserter();
DatabaseInserter pplInserter = new PeopleInserterSingleThread();
DatabaseInserter catInserter = new CategoryInserter();
DatabaseInserter principalsInserter = new PrincipalsInserter();
DBFileValidator principalsValidator = new PrincipalsTSVValidator();
DBFileValidator crewExistsVal = new CrewExistsValidator();

Dictionary<string, Action<string, SqlConnection>> cmdOptions = new Dictionary<string, Action<string, SqlConnection>>()
{

    {"insert titles", titlesInserter.Insert },
    {"insert professions", profInserter.Insert },
    {"insert people", pplInserter.Insert },
    {"insert categories", catInserter.Insert },
    {"insert principals", principalsInserter.Insert },
    {"validate principals", (string path, SqlConnection conn) => principalsValidator.Validate(path)},
    {"validate crew exists", crewExistsVal.Validate},
};



using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
{
    bool finished = false;
    connection.Open();
    while(!finished)
    {
        Console.WriteLine("Enter command option: ");
        string option = Console.ReadLine();
        if (cmdOptions.TryGetValue(option.ToLower(), out var cmd))
        {
            while(!finished)
            {
                Console.WriteLine("Command accepted, enter path:");
                string path = Console.ReadLine();
                if (File.Exists(path))
                {
                    cmd(path, connection);
                    finished = true;
                }
                else
                {
                    Console.WriteLine("Invalid path, try again");
                }
            }
        
        }
        else
        {
            Console.WriteLine("Invalid command, try again");
        }
    }

}








