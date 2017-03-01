//using System.Data;
//using System.Data.SQLite;
//using System.IO;

//namespace Maple.Data
//{
//    public static class SqLiteConnectionFactory
//    {
//        private const string DBFILENAME = "Maple.sqlite";

//        public static IDbConnection Get(string path)
//        {
//            Directory.CreateDirectory(path);

//            var fullPath = Path.Combine(path, DBFILENAME);

//            var connection =  new SQLiteConnection($"Data Source={DBFILENAME}; Version=3;Pooling=True;Max Pool Size=100;");

//            if (!File.Exists(fullPath))
//                SQLiteConnection.CreateFile(fullPath);

//            return connection;
//        }

//        public static void DropDatabase(string path)
//        {
//            var fullPath = Path.Combine(path, DBFILENAME);

//            if (File.Exists(fullPath))
//                File.Delete(fullPath);
//        }

//        /*
//         * 
//         * We deduce the following rules of thumb from the matrix above:
//         * A database page size of 8192 or 16384 gives the best performance for large BLOB I/O.
//         * For BLOBs smaller than 100KB, reads are faster when the BLOBs are stored directly in the database file. For BLOBs larger than 100KB, reads from a separate file are faster.
//         * Of course, your mileage may vary depending on hardware, filesystem, and operating system. Double-check these figures on target hardware before committing to a particular design.
//         * 
//         */
//    }
//}
