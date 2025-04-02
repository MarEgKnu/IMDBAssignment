using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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
            int removeStart = 0;
            int removeEnd = 0;
            if(str.IsNullOrEmpty())
            {
                return str;
            }
            else if (count > str.Length)
            {
                // if the amount of chars to be trimmed is larger than the string itself, use the string length instead as count
                count = str.Length;
            }
            StringBuilder sb = new StringBuilder(str);
            for(int i = 0; i < count; i++)
            {
                if (chars.Contains( sb[i]))
                {
                    removeStart++;
                }
                else
                {
                    break;
                }
            }
            for (int i = str.Length - 1; i >= str.Length - count; i--)
            {
                if (chars.Contains(sb[i]))
                {
                    removeEnd++;
                }
                else
                {
                    break;
                }
            }
            if(removeStart + removeStart > str.Length)
            {
                throw new ArgumentException("Amount of characters to be trimmed is higher than the string length");
            }
            if(removeEnd > 0)
            {
                sb.Remove(sb.Length - removeEnd, removeEnd);
            }          
            sb.Remove(0, removeStart);
            return sb.ToString();
        }
    }
}
