using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace WindowsFormsApplication1
{
    public partial class Form3 : Form
    {
        public static bool restart,
                           backMenu;
        private bool runNOclose = false;
        public Form3()
        {
            InitializeComponent();
            restart = false;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            runNOclose = true;
            using (SQLiteConnection con = new SQLiteConnection("Data Source = DataBase/dbDodger.db"))//建立與資料庫的連線
            {
                try
                {
                    con.Open();
                    SQLiteCommand cmd = new SQLiteCommand();
                    cmd.Connection = con;
                    cmd.CommandText = @"insert into tblDodger (PlayerName, Score) values (@PlayerName, @Score)";//輸入數據至資料庫
                    if (textBox1YourName.Text != "")//輸入名稱空白則不輸入數據
                    {
                        cmd.Parameters.Add(new SQLiteParameter("@PlayerName", textBox1YourName.Text));
                        cmd.Parameters.Add(new SQLiteParameter("@Score", label3YourScore.Text));
                    }
                    cmd.ExecuteNonQuery();
                    
                    con.Close();
                }
                catch (Exception ex)
                {                    
                }
            }
            restart = true;
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            runNOclose = true;
            using (SQLiteConnection con = new SQLiteConnection("Data Source = DataBase/dbDodger.db"))//同上
            {
                try
                {
                    con.Open();
                    SQLiteCommand cmd = new SQLiteCommand();
                    cmd.Connection = con;
                    cmd.CommandText = @"insert into tblDodger (PlayerName, Score) values (@PlayerName, @Score)";
                    if (textBox1YourName.Text != "")
                    {
                        cmd.Parameters.Add(new SQLiteParameter("@PlayerName", textBox1YourName.Text));
                        cmd.Parameters.Add(new SQLiteParameter("@Score", label3YourScore.Text));
                    }
                    cmd.ExecuteNonQuery();
                    
                    con.Close();
                }
                catch (Exception ex)
                {
                }
            }
            backMenu = true;
            this.Dispose();
        }

        private void onFormClosed(object sender, FormClosedEventArgs e)
        {
            if (runNOclose == false)//按叉叉離開程式
                Application.Exit();
        }
    }
}
