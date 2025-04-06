using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadFromDatabase
{
    public interface ITitleRepository
    {
        public List<TitleWithGenres> GetTitlesBasic(string search = "", int offset = 0, int fetch = 50, bool ascending = true);

        public TitleWithGenres AddTitleBasic(TitleWithGenres title);

        public bool DeleteTitle(int id);

        public bool UpdateTitle(int id, TitleWithGenres title);



    }
}
