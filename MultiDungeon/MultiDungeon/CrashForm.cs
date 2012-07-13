using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MultiDungeon
{
    public partial class CrashForm : Form
    {
        public CrashForm()
        {
            InitializeComponent();
        }

        public void SetInfo(string s1, string s2)
        {
            e1.Text = s1;
            e2.Text = s2;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
