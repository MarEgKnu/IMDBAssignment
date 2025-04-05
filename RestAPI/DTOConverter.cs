using ReadFromDatabase;
using RestAPI.Models;
using System.Reflection.Metadata.Ecma335;

namespace RestAPI
{
    public static class DTOConverter
    {
        public static TitleWithGenres ConvertTitlesBasicDTO(TitleBasicDTO dto)
        {
            return new TitleWithGenres(0, dto.tconst, dto.titleType, dto.primaryTitle, dto.originalTitle, dto.isAdult, dto.startYear, dto.endYear, dto.runTimeMinutes, dto.Genres);
        }
    }
}
