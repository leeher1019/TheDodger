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
    public partial class Form5 : Form
    {
        public static bool backMenu = false;
        private bool backNoClose = false;      
        public Form5()
        {
            InitializeComponent();
            List<getList> list = (new Table()).getAllList();//顯示資料庫的資料
            bindingSource1.DataSource = list;
        }

        private void btnBackMenu_Click(object sender, EventArgs e)
        {
            backNoClose = true;            
            Form2 form2 = new Form2();
            this.Dispose();
            form2.ShowDialog();
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void onFormClosed(object sender, FormClosedEventArgs e)
        {   
            if (backNoClose == false)//按叉叉離開程式
                Application.Exit();
        }
    }
}
