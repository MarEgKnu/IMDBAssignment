using fileTest;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net.Sockets;



GC.TryStartNoGCRegion(8000000000);

using(SqlConnection conn = new SqlConnection(Secret.ConnectionString))
{
    TitlesInserter titlesInserter = new TitlesInserter();
    conn.Open();
    using (StreamReader sr = File.OpenText("C:\\Users\\Marius\\Downloads\\title.basics.tsv\\title.basics.tsv"))
    {
        sr.ReadLine();
        titlesInserter.InsertBulk(DataTableHelpers.CreateDataTableTitles(sr), conn);
        GC.EndNoGCRegion();
    }

}

Environment.Exit(0);















using (var sr = File.OpenText("C:\\Users\\Marius\\Downloads\\title.basics.tsv\\title.basics.tsv"))
using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
{
    sr.ReadLine();
    int line = 1;
    connection.Open();
    //SqlTransaction transaction = connection.BeginTransaction();
    while (!sr.EndOfStream)
    {
        line++;
        string[] fields = sr.ReadLine().Split("\t");
        if (fields.Length != 9)
        {
            throw new InvalidDataException($"Number of fields on line {line} was {fields.Length}, not 9");
        }
        SqlCommand command = new SqlCommand("AddTitleFromTSV", connection);
        command.CommandType = CommandType.StoredProcedure;
        SqlParameter tConst = new()
        {
            ParameterName = "@TConst",
            SqlDbType = SqlDbType.VarChar,
            Direction = ParameterDirection.Input,
            Value = fields[0]
        };
        SqlParameter titleType = new()
        {
            ParameterName = "@TitleType",
            SqlDbType = SqlDbType.VarChar,
            Direction = ParameterDirection.Input,
            Value = fields[1]
        };
        SqlParameter primaryTitle = new()
        {
            ParameterName = "@PrimaryTitle",
            SqlDbType = SqlDbType.VarChar,
            Direction = ParameterDirection.Input,
            Value = fields[2]
        };
        SqlParameter originalTitle = new()
        {
            ParameterName = "@OriginalTitle",
            SqlDbType = SqlDbType.VarChar,
            Direction = ParameterDirection.Input,
            Value = fields[3]
        };
        bool isAdultBool;
        if (fields[4] == "1")
        {
            isAdultBool = true;
        }
        else if( fields[4] == "0") {
            isAdultBool = false;
        }
        else
        {
            throw new InvalidDataException("IsAdult was not 0 or 1");
        }
        SqlParameter isAdult = new()
        {
            ParameterName = "@IsAdult",
            SqlDbType = SqlDbType.Bit,
            Direction = ParameterDirection.Input,
            Value = isAdultBool
        };
        ushort? startYearValue = fields[5].ToUshortOrNull();
        SqlParameter startYear = new()
        {
            ParameterName = "@StartYear",
            SqlDbType = SqlDbType.SmallInt,
            Direction = ParameterDirection.Input,
            Value = (startYearValue is null) ? DBNull.Value : startYearValue
        };
        ushort? endYearValue = fields[6].ToUshortOrNull();
        SqlParameter endYear = new()
        {
            ParameterName = "@EndYear",
            SqlDbType = SqlDbType.SmallInt,
            Direction = ParameterDirection.Input,
            Value = (endYearValue is null) ? DBNull.Value : endYearValue
        };
        int? runTimeMinValue = fields[7].ToIntOrNull();
        SqlParameter runTimeMin = new()
        {
            ParameterName = "@RuntimeMinutes",
            SqlDbType = SqlDbType.Int,
            Direction = ParameterDirection.Input,
            Value = (runTimeMinValue is null) ? DBNull.Value : runTimeMinValue
        };
        SqlParameter genres = new()
        {
            ParameterName = "@Genres",
            SqlDbType = SqlDbType.VarChar,
            Direction = ParameterDirection.Input,
            Value = (fields[8].IsNullString()) ? DBNull.Value : fields[8]
        };
        command.Parameters.Add(tConst);
        command.Parameters.Add(titleType);
        command.Parameters.Add(primaryTitle);
        command.Parameters.Add(originalTitle);
        command.Parameters.Add(isAdult);
        command.Parameters.Add(startYear);
        command.Parameters.Add(endYear);
        command.Parameters.Add(runTimeMin);
        command.Parameters.Add(genres);
        command.ExecuteNonQuery();
    }
    //transaction.Commit();
}

