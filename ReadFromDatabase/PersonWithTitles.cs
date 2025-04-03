namespace ReadFromDatabase
{
    public class PersonWithTitles
    {



        int id; string nconst; string primaryname; int? birthyear; int? deathyear; string[] roles; string[] titles;

        public PersonWithTitles(int id, string nConst, string primaryName, int? birthYear, int? deathYear, string[] roles, string[] titles)
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
        public string PrimaryName { get => primaryname; set => primaryname = value; }
        public int? BirthYear { get => birthyear; set => birthyear = value; }
        public int? DeathYear { get => deathyear; set => deathyear = value; }
        public string[] Roles { get => roles; set => roles = value; }
        public string[] Titles { get => titles; set => titles = value; }

        public override string ToString()
        {
            return $"{{{nameof(Id)}={Id.ToString()}, {nameof(NConst)}={NConst}, {nameof(PrimaryName)}={PrimaryName}, {nameof(BirthYear)}={BirthYear.ToString()}, {nameof(DeathYear)}={DeathYear.ToString()}, {nameof(Roles)}={string.Join(",", Roles ?? ["none"])}, {nameof(Titles)}={string.Join(",", Titles ?? ["none"])}}}";
        }
    }
}

