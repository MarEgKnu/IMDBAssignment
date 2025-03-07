using DatabaseImporter;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net.Sockets;


DatabaseInserter titlesInserter = new TitlesInserter();

titlesInserter.Start("C:\\Users\\Marius\\Downloads\\title.basics.tsv\\title.basics.tsv", InsertMode.InsertBulk);







