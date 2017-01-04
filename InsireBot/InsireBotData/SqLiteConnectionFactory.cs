using System.Data;
using System.Data.SQLite;
using System.IO;

namespace InsireBot.Data
{
    public static class SqLiteConnectionFactory
    {
        private const string DBFILENAME = "InsireBotDB.sqlite";

        public static IDbConnection Get()
        {
            var dir = new DirectoryInfo(".").FullName;
            var path = Path.Combine(dir, DBFILENAME);

            var connection =  new SQLiteConnection($"Data Source={path}; Version=3;");

            if (!File.Exists(path))
                SQLiteConnection.CreateFile(path);

            return connection;
        }

        //public static void Seed<T>()
        //{
        //    using (var sqLiteConnection = GetSQLite())
        //        sqLiteConnection.CreateTable<T>();
        //}

        /*
         * 
         * We deduce the following rules of thumb from the matrix above:
         * A database page size of 8192 or 16384 gives the best performance for large BLOB I/O.
         * For BLOBs smaller than 100KB, reads are faster when the BLOBs are stored directly in the database file. For BLOBs larger than 100KB, reads from a separate file are faster.
         * Of course, your mileage may vary depending on hardware, filesystem, and operating system. Double-check these figures on target hardware before committing to a particular design.
         * 
         */
    }
}
