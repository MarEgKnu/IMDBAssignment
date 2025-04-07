using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadFromDatabase
{
    public class Genre
    {
        public Genre(byte id, string name)
        {
            Name = name;
            ID = id;
        }
        public byte ID { get; set; }

        public string Name { get; set; }
    }
}
