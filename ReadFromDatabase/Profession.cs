using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadFromDatabase
{
    public class Profession
    {
        public Profession(byte id, string name)
        {
            Name = name;
            ID = id;
        }
        public byte ID { get; set; }

        public string Name { get; set; }
    }
}
