namespace RestAPI.Models
{
    public record TitleBasicDTO(string tconst, string titleType, string? primaryTitle, string? originalTitle, bool isAdult, short? startYear, short? endYear, int? runTimeMinutes,
                               string[]? Genres);
}
