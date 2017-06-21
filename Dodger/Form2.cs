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
    public partial class Form2 : Form
    {
        public static bool bPlay = false,
                           bLeaderBoard = false,
                           bAI_show = false,
                           bPlay_backMenu_choose = false;
        private bool runNOclose = false;
        public Form5 form5 = new Form5();               
        
        public Form2()
        {
            InitializeComponent();
        }

        private void button1Play_Click(object sender, EventArgs e)
        {            
            runNOclose = true;
            bPlay = true;
            bAI_show = !bPlay;
            this.Hide();
            if (Form3.backMenu == true)
                bPlay_backMenu_choose = true;
            else if (Form4.backMenu == true)
                bPlay_backMenu_choose = true;
            this.Dispose();
        }

        private void button3AI_Show_Click(object sender, EventArgs e)
        {            
            runNOclose = true;
            bAI_show = true;
            bPlay = !bAI_show;
            this.Hide();
            if (Form4.backMenu == true)
                bPlay_backMenu_choose = true;
            else if (Form3.backMenu == true)
                bPlay_backMenu_choose = true;
            this.Dispose();
        }

        private void button2LeaderBoard_Click(object sender, EventArgs e)
        {
            runNOclose = true;
            bLeaderBoard = true;
            this.Dispose();
            form5.ShowDialog();
        }

        private void onFormClosed(object sender, FormClosedEventArgs e)
        {
            if (runNOclose == false)//按叉叉離開程式
                Application.Exit();           
        }

        
    }
}
