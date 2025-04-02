using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseImporter
{
    public static class StringExtensions
    {
        public static short? ToShortOrNull(this string str)
        {
            if(str.IsNullString())
            {
                return null;
            }
            else
            {
                return short.Parse(str);
            }
        }
        public static object ToShortOrDBNull(this string str)
        {
            if (str.IsNullString())
            {
                return DBNull.Value;
            }
            else
            {
                return short.Parse(str);
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
            if (str.IsNullString())
            {
                return null;
            }
            else
            {
                return int.Parse(str);
            }
        }
        public static object ToIntOrDBNull(this string str)
        {
            if (str.IsNullString())
            {
                return DBNull.Value;
            }
            else
            {
                return int.Parse(str);
            }
        }
        public static object ToByteOrDBNull(this string str)
        {
            if (str.IsNullString())
            {
                return DBNull.Value;
            }
            else
            {
                return byte.Parse(str);
            }
        }
        public static object ToBoolOrDBNull(this string str)
        {
            if (str is null) throw new ArgumentNullException(nameof(str));
            if (str == @"\N")
            {
                return DBNull.Value;
            }
            else
            {
                if(str == "1")
                {
                    return true;
                }
                else if(str == "0")
                {
                    return false;
                }
            }
            throw new InvalidDataException($"The string {str} could not be parsed to a bool");
        }
        public static string Trim(this string str, int count, params char[] chars)
        {
            if(str.IsNullOrEmpty())
            {
                return str;
            }
            else if (count > str.Length)
            {
                throw new ArgumentException($"{nameof(count)} was larger than the length of the input string");
            }
            StringBuilder sb = new StringBuilder(str);
            for(int i = 0; i < count; i++)
            {
                if (chars.Contains( sb[i]))
                {
                    sb = sb.Remove(i, 1);
                }
            }
            for (int i = sb.Length - 1; i >= 0; i++)
            {
                if (chars.Contains(sb[i]))
                {
                    sb = sb.Remove(i, 1);
                }
            }
            return sb.ToString();
        }
    }
}
