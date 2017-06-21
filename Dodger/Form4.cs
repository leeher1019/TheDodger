using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form4 : Form
    {
        public static bool restart,
                           backMenu;
        private bool runNOclose = false;
        public Form4()
        {
            InitializeComponent();
            restart = false;
            backMenu = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            runNOclose = true;
            restart = true;
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            runNOclose = true;
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
