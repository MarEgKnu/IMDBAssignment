using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadFromDatabase
{
    public interface IProfessionRepository
    {
        List<Profession> GetAll();

        Profession? GetByID(byte id);
    }
}
