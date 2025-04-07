using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadFromDatabase
{
    public interface IGenreRepository
    {
        List<Genre> GetAll();

        Genre? GetByID(byte id);
    }
}
