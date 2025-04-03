// See https://aka.ms/new-console-template for more information
using ReadFromDatabase;

ReadClass rc = new();

List<TitleWithGenres> list = rc.GetTitles("sej");

foreach (TitleWithGenres title in list)
{
    Console.WriteLine(title);
}