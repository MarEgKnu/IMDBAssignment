namespace RestAPI.Models
{
    public record PersonBasicDTO(string nConst, string? primaryName, int? birthYear, int? deathYear, string[]? roles, int[]? titles);
}
