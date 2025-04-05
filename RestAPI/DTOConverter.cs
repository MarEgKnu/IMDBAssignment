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
        public static PersonWithTitles ConvertPersonBasicDTO(PersonBasicDTO dto)
        {
            string[]? titleIDsString;
            if (dto.titles is null)
            {
                titleIDsString = null;
            }
            else
            {
                titleIDsString = new string[dto.titles.Length];
                for (int i = 0; i < titleIDsString.Length; i++)
                {
                    titleIDsString[i] = dto.titles[i].ToString();
                }
            }

            return new PersonWithTitles(0, dto.nConst, dto.primaryName, dto.birthYear, dto.deathYear, dto.roles, titleIDsString);
        }
    }
}
