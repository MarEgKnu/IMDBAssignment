using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadFromDatabase
{
    public class PersonWithTitlesIN
    {



        int id; string nconst; string? primaryname; int? birthyear; int? deathyear; string[]? roles; int[]? titles;

        public PersonWithTitlesIN(int id, string nConst, string? primaryName, int? birthYear, int? deathYear, string[]? roles, int[]? titles)
        {
            Id = id;
            NConst = nConst;
            PrimaryName = primaryName;
            BirthYear = birthYear;
            DeathYear = deathYear;
            Roles = roles;
            Titles = titles;
        }

        public int Id { get => id; set => id = value; }
        public string NConst { get => nconst; set => nconst = value; }
        public string? PrimaryName { get => primaryname; set => primaryname = value; }
        public int? BirthYear { get => birthyear; set => birthyear = value; }
        public int? DeathYear { get => deathyear; set => deathyear = value; }
        public string[]? Roles { get => roles; set => roles = value; }
        public int[]? Titles { get => titles; set => titles = value; }

        public override string ToString()
        {
            return $"{nameof(Id)}={Id.ToString()}, {nameof(NConst)}={NConst}, {nameof(PrimaryName)}={PrimaryName}, {nameof(BirthYear)}={BirthYear.ToString()}, {nameof(DeathYear)}={DeathYear.ToString()}, {nameof(Roles)}={string.Join(",", Roles ?? ["none"])}, {nameof(Titles)}={(Titles is null ? "none" : string.Join(',', Titles))}";
        }
    }
}
