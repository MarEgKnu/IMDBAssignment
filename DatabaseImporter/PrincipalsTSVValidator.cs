using Microsoft.Data.SqlClient;

namespace DatabaseImporter
{
    [Flags]
    enum Types
    {
        String = 0x1,
        Number = 0x2,
        Nullable = 0x4,
    }
    public class PrincipalsTSVValidator : DBFileValidator
    {
        const int NUM_OF_FIELDS = 6;
        private string[] fieldNames = new string[NUM_OF_FIELDS] { "tconst", "ordering", "nconst", "category", "job", "characters" };
        private Types[] fieldTypes = new Types[NUM_OF_FIELDS] { Types.String, Types.Number, Types.String, Types.String, Types.String | Types.Nullable, Types.String | Types.Nullable };
        private bool[] fieldNullabillity = new bool[NUM_OF_FIELDS];
        private object[] largestFieldValues = new object[NUM_OF_FIELDS];

        public PrincipalsTSVValidator()
        {
            for (int i = 0; i < largestFieldValues.Length; i++)
            {
                if (fieldTypes[i].HasFlag(Types.String))
                {
                    largestFieldValues[i] = string.Empty;
                }
                else
                {
                    largestFieldValues[i] = 0;
                }
            }
        }
        public override void Validate(string filePath, SqlConnection connection = null)
        {
            int lineNum = 1;
            IEnumerable<string> lines = File.ReadLines(filePath).Skip(1);
            //string[] lines = File.ReadAllLines(filePath).Skip(1).ToArray();
            foreach (string line in lines)
            {
                lineNum++;
                string[] fields = line.Split("\t");

                if (fields.Length != 6)
                {
                    throw new InvalidDataException($"line: {string.Join("\t", fields)} only has {fields.Length} values");
                }
                for (int fieldNum = 0; fieldNum < fields.Length; fieldNum++)
                {
                    // strings
                    if (fieldTypes[fieldNum].HasFlag(Types.String))
                    {
                        // check if its the longest
                        if (fields[fieldNum].Length > largestFieldValues[fieldNum].ToString().Length)
                        {
                            largestFieldValues[fieldNum] = fields[fieldNum];
                        }
                        //check if null
                        if (!fieldNullabillity[fieldNum] && fields[fieldNum] == @"\N")
                        {
                            fieldNullabillity[fieldNum] = true;
                            Console.WriteLine($"data for {fieldNames[fieldNum]} was able to be null on line {lineNum}, data: {line}");
                        }
                    }
                    // numbers
                    else
                    {
                        //check if null
                        if (!fieldNullabillity[fieldNum] && fields[fieldNum] == @"\N")
                        {
                            fieldNullabillity[fieldNum] = true;
                            Console.WriteLine($"data for {fieldNames[fieldNum]} was able to be null on line {lineNum}, data: {line}");
                        }
                        // check if its the longest, and if it is even parseable to an int
                        else if (int.TryParse(fields[fieldNum], out int result))
                        {
                            if (result > (int)largestFieldValues[fieldNum])
                            {
                                largestFieldValues[fieldNum] = result;
                            }
                        }
                        else
                        {
                            Console.WriteLine($"{fieldNames[fieldNum]} data on line {lineNum} could not be parsed to int, data: {line}");
                        }
                    }
                }

            }
            for (int fieldNum = 0; fieldNum < NUM_OF_FIELDS; fieldNum++)
            {
                if (fieldTypes[fieldNum].HasFlag(Types.String))
                {
                    Console.WriteLine($"Field {fieldNames[fieldNum]} longest word is {largestFieldValues[fieldNum]} with {largestFieldValues[fieldNum].ToString().Length} characters, and Nullable = {fieldNullabillity[fieldNum]}\n");
                }
                else
                {
                    Console.WriteLine($"Field {fieldNames[fieldNum]} largest number is {largestFieldValues[fieldNum]}, and Nullable = {fieldNullabillity[fieldNum]}\n");
                }
            }
        }
    }
}
