using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PLC_POSITION.VIEW
{
    public partial class WebChoose : Form
    {
        public WebChoose()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process.Start("http://192.168.133.7:9090/index.aspx");
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Process.Start("http://172.16.13.1:9090/index.aspx");
            this.Close();
        }
    }
}
