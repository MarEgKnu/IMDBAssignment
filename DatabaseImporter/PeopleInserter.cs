﻿using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlClient.Server;
using System.Data;

namespace DatabaseImporter
{
    public class PeopleInserter : DatabaseInserter
    {
        private object _isCmdReadyForSegmentLock = new object();
        private object _isSegmentReadyForCmdLock = new object();
        private bool _isSegmentReadyForCmd = false;
        private bool IsSegmentReadyForCmd
        {
            get
            {
                lock (_isSegmentReadyForCmdLock)
                {
                    return _isSegmentReadyForCmd;
                }
            }
            set
            {
                lock (_isSegmentReadyForCmdLock)
                {
                    _isSegmentReadyForCmd = value;
                }
            }

        }
        private bool _isCmdReadyForSegment = false;
        private bool IsCmdReadyForSegment
        {
            get
            {
                lock (_isCmdReadyForSegmentLock)
                {
                    return _isCmdReadyForSegment;
                }
            }
            set
            {
                lock (_isCmdReadyForSegmentLock)
                {
                    _isCmdReadyForSegment = value;
                }
            }

        }
        const int INITIAL_CAPACITY = 14211906; //How much list capacity to allocate to start with
        const int NCONST_MAX_SIZE = 12;
        const int PRIMARY_NAME_MAX_SIZE = 70;
        const int YEAR_MAX_SIZE = 4;
        const int PRIMARY_PROF_MAX_SIZE = 75;
        const int KNOWN_FOR_TITLES_MAX_SIZE = 48;
        const int DIVISOR = 1000;
        private IEnumerable<SqlDataRecord> _records = null;
        private static SqlMetaData[] _peopleMetaData = new SqlMetaData[]
        {
            new SqlMetaData("nconst", System.Data.SqlDbType.VarChar, NCONST_MAX_SIZE),
            new SqlMetaData("primaryName", System.Data.SqlDbType.VarChar, PRIMARY_NAME_MAX_SIZE),
            new SqlMetaData("birthYear", System.Data.SqlDbType.VarChar, YEAR_MAX_SIZE),
            new SqlMetaData("deathYear", System.Data.SqlDbType.VarChar, YEAR_MAX_SIZE),
            new SqlMetaData("primaryProfession", System.Data.SqlDbType.VarChar, PRIMARY_PROF_MAX_SIZE),
            new SqlMetaData("knownForTitles", System.Data.SqlDbType.VarChar, KNOWN_FOR_TITLES_MAX_SIZE),

        };
        public override void Insert(string filePath, SqlConnection connection)
        {
            //SqlDataRecord[] lines = CreateDataRecordsPeople(File.ReadAllLines(filePath)).ToArray();
            string[] lines = File.ReadAllLines(filePath).Skip(1).ToArray();
            int itemsPerSegment = lines.Length / DIVISOR;
            int extraItems = lines.Length % DIVISOR;
            Task creatingSegments = Task.Run(() =>
            {
                //while(IsSegmentReadyForCmd)
                //{
                for (int i = 0; i < DIVISOR; i++)
                {
                    ArraySegment<string> segment;
                    if (i == 0)
                    {
                        segment = new ArraySegment<string>(lines, 0, itemsPerSegment);
                    }
                    else if (i == DIVISOR - 1)
                    {
                        segment = new ArraySegment<string>(lines, i * itemsPerSegment, itemsPerSegment + extraItems);
                    }
                    else
                    {
                        segment = new ArraySegment<string>(lines, i * itemsPerSegment, itemsPerSegment);
                    }
                    IEnumerable<SqlDataRecord> tempRecords = CreateDataRecordsPeople(segment);
                    while (!IsCmdReadyForSegment)
                    {
                        Thread.Sleep(5); // sleep short amount of time    
                    }
                    lock (this)
                    {
                        // once it is ready, stop the waiting and pass the records to the main variable
                        _records = tempRecords;
                        IsSegmentReadyForCmd = true;
                    }

                }
                //}

            });
            Task runCommand = Task.Run(() =>
            {
                IsCmdReadyForSegment = true;
                while (!creatingSegments.IsCompleted || _records != null)
                {
                    lock (this)
                    {
                        if (IsSegmentReadyForCmd)
                        {
                            IsCmdReadyForSegment = false;
                            SqlCommand cmd = new SqlCommand("InsertPeopleBulk", connection) { CommandType = CommandType.StoredProcedure, CommandTimeout = 300 };
                            SqlParameter param = new SqlParameter("@InData", SqlDbType.Structured) { TypeName = "dbo.RawPeopleData", Value = _records };
                            cmd.Parameters.Add(param);
                            cmd.ExecuteNonQuery();
                            _records = null;
                            IsSegmentReadyForCmd = false;
                            IsCmdReadyForSegment = true;
                        }
                    }


                }

            });
            Task.WaitAll(creatingSegments, runCommand);
        }



        private static IEnumerable<SqlDataRecord> CreateDataRecordsPeople(IEnumerable<string> lines)
        {
            List<SqlDataRecord> records = new List<SqlDataRecord>();
            foreach (string line in lines)
            {

                string[] fields = line.Split('\t');
                ValidatePeopleFields(fields);
                SqlDataRecord record = new SqlDataRecord(_peopleMetaData);
                record.SetString(0, fields[0]);
                record.SetString(1, fields[1]);
                record.SetString(2, fields[2]);
                record.SetString(3, fields[3]);
                record.SetString(4, fields[4]);
                record.SetString(5, fields[5]);
                records.Add(record);
            }
            return records;
        }




        private static void ValidatePeopleFields(string[] fields)
        {
            if (fields.Length != 6)
            {
                throw new InvalidDataException($"{fields.Length} fields were detected, not 6");
            }
            else if (fields[0].Length > NCONST_MAX_SIZE)
            {
                throw new InvalidDataException($"nconst field {fields[0]} was longer than {NCONST_MAX_SIZE} characters long");
            }
            else if (fields[1].Length > PRIMARY_NAME_MAX_SIZE)
            {
                throw new InvalidDataException($"primaryName field {fields[1]} was longer than {PRIMARY_NAME_MAX_SIZE} characters long");
            }
            else if (fields[2].Length > YEAR_MAX_SIZE)
            {
                throw new InvalidDataException($"birthYear field {fields[2]} was longer than {YEAR_MAX_SIZE} characters long");
            }
            else if (fields[3].Length > YEAR_MAX_SIZE)
            {
                throw new InvalidDataException($"deathYear field {fields[3]} was longer than {YEAR_MAX_SIZE} characters long");
            }
            else if (fields[4].Length > PRIMARY_PROF_MAX_SIZE)
            {
                throw new InvalidDataException($"primaryProfession field {fields[4]} was longer than {PRIMARY_PROF_MAX_SIZE} characters long");
            }
            else if (fields[5].Length > KNOWN_FOR_TITLES_MAX_SIZE)
            {
                throw new InvalidDataException($"knownForTitles field {fields[5]} was longer than {KNOWN_FOR_TITLES_MAX_SIZE} characters long");
            }

        }
    }
}
