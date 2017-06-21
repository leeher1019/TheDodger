using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.SQLite;

namespace WindowsFormsApplication1
{
    class Db
    {
        public static SQLiteConnection getConnection()
        {
            if (!Directory.Exists("DataBase"))
            {
                Directory.CreateDirectory("DataBase");
            }
            if (!File.Exists("DataBase/dbDodger.db"))
            {
                SQLiteConnection.CreateFile("DataBase/dbDodger.db");
            }
            return new SQLiteConnection("Data Source = DataBase/dbDodger.db");
        }
    }
}
