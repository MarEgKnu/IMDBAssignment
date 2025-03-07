using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseImporter
{
    public static class StringExtensions
    {
        public static ushort? ToUshortOrNull(this string str)
        {
            if(str is null) throw new ArgumentNullException(nameof(str));
            if(str.IsNullString())
            {
                return null;
            }
            else
            {
                return ushort.Parse(str);
            }
        }
        public static bool IsNullString(this string str)
        {
            if (str is null) throw new ArgumentNullException(nameof(str));
            if (str == @"\N")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static int? ToIntOrNull(this string str)
        {
            if (str is null) throw new ArgumentNullException(nameof(str));
            if (str.IsNullString())
            {
                return null;
            }
            else
            {
                return int.Parse(str);
            }
        }
    }
}
