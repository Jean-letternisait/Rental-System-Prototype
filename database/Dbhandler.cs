//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using SQLite;
//using SQLitePCL;

//namespace cpsy200FinalProject.database
//{
//    public class Dbhandler
//    {
//        // Creates connection with database
//        public ISQLiteAsyncConnection CreateConnection() 
//        {
//            return new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory,"rentalVillage.db3"),
//                // allows prgoram to be able to read and write to database
//                SQLiteOpenFlags.ReadWrite | 
//                // Creates database if not previously done
//                SQLiteOpenFlags.Create |
//                // allows multi threaded database access
//                SQLiteOpenFlags.SharedCache);
//        }
//    }
//}
