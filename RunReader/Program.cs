// See https://aka.ms/new-console-template for more information
using ReadFromDatabase;

ReadClass rc = new();

List<PersonWithTitles> list = rc.ReadPersonsWithTitles("sej", 0, 50, true); 

foreach (PersonWithTitles person in list)
{
    Console.WriteLine(person);
}