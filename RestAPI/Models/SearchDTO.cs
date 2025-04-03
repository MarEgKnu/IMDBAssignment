namespace RestAPI.Models
{
    public record SearchDTO(string search = "", int offset = 0, int rows = 50, bool ascending = true)
    {

    }
}
