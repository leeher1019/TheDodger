using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace WindowsFormsApplication1
{
    public class Table
    {
        //利用資料庫查詢把內容查出後存到List的泛型裡
        public List<getList> getAllList()
        {
            List<getList> list = new List<getList>();
            SQLiteConnection conn = Db.getConnection();

            conn.Open();
            try
            {
                String sql = "SELECT * FROM tblDodger order by Score DESC";
                SQLiteCommand com = conn.CreateCommand();
                com.CommandText = sql;
                SQLiteDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                    getList idc = new getList();

                    idc.PlayerName = reader.GetString(0);
                    idc.Score = reader.GetString(1);

                    list.Add(idc);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                conn.Close();
            }
            return list;
        }        
    }

    public class getList
    {        
        public String PlayerName
        {
            set;
            get;
        }
        public String Score
        {
            set;
            get;
        }
    }
}
