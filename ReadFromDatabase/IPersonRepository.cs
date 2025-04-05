using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadFromDatabase
{
    public interface IPersonRepository
    {
        public List<PersonWithTitles> ReadPersonsBasic(string search = "", int offset = 0, int fetch = 50, bool? ascending = true);
    }
}
