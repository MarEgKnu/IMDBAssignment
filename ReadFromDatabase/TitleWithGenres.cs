namespace ReadFromDatabase
{
    public class TitleWithGenres
    {
        public TitleWithGenres(int iD, string tConst, string titleType, string? primaryTitle, string? originalTitle, bool isAdult, int? startYear, int? endYear, int? runTimeMinutes, string[]? aggregatedGenres)
        {
            ID = iD;
            TConst = tConst;
            TitleType = titleType;
            PrimaryTitle = primaryTitle;
            OriginalTitle = originalTitle;
            IsAdult = isAdult;
            StartYear = startYear;
            EndYear = endYear;
            RunTimeMinutes = runTimeMinutes;
            AggregatedGenres = aggregatedGenres;
        }

        public int ID { get; set; }
        public string TConst { get; set; }
        public string TitleType { get; set; }
        public string? PrimaryTitle { get; set; }
        public string? OriginalTitle { get; set; }
        public bool IsAdult { get; set; }
        public int? StartYear { get; set; }
        public int? EndYear { get; set; }
        public int? RunTimeMinutes { get; set; }
        public string[]? AggregatedGenres { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(ID)}={ID.ToString()}, {nameof(TConst)}={TConst}, {nameof(TitleType)}={TitleType}, {nameof(PrimaryTitle)}={PrimaryTitle}, {nameof(OriginalTitle)}={OriginalTitle}, {nameof(IsAdult)}={IsAdult.ToString()}, {nameof(StartYear)}={StartYear.ToString()}, {nameof(EndYear)}={EndYear.ToString()}, {nameof(RunTimeMinutes)}={RunTimeMinutes.ToString()}, {nameof(AggregatedGenres)}={(AggregatedGenres != null ? string.Join(",", AggregatedGenres) : "non")}}}";
        }
    }
}
