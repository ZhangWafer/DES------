using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PLC_POSITION.VIEW
{
    public partial class WaitingForm : Form
    {
        public WaitingForm()
        {
            InitializeComponent();
        }

        int count_3s = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (count_3s >= 3)
            {
                label1.Text = "正在导出数据";
                count_3s = 0;
            }
            label1.Text += ".";
            count_3s++;
            if (Chart_Show.waitingBool==false)
            {
                timer1.Stop();
                this.Hide();
               
            }
        }

        private void WaitingForm_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Start();
        }
        
        
    } 
}
