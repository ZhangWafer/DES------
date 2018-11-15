using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PLC_POSITION.Class;
using PLC_POSITION.VIEW;


namespace PLC_POSITION
{
    public partial class Monitor : Form
    {
        public Monitor()
        {
            InitializeComponent();
        }

        TextBox[] textBoxsList = new TextBox[300];

        private void Form1_Load(object sender, EventArgs e)
        {
            #region 将textbox放入数组

            //将所有的Textbox都集合起来放入数组
            foreach (var item in this.tabPage1.Controls)
            {
                if (item is TextBox)
                {
                    int index = Convert.ToInt16(((TextBox)item).Name.ToString().Replace("textBox", ""));
                    textBoxsList[index] = (TextBox)item;
                }
            }
            foreach (var item in this.tabPage2.Controls)
            {
                if (item is TextBox)
                {
                    int index = Convert.ToInt16(((TextBox)item).Name.ToString().Replace("textBox", ""));
                    textBoxsList[index] = (TextBox)item;
                }
            }
            foreach (var item in this.tabPage3.Controls)
            {
                if (item is TextBox)
                {
                    int index = Convert.ToInt16(((TextBox)item).Name.ToString().Replace("textBox", ""));
                    textBoxsList[index] = (TextBox)item;
                }
            }
            foreach (var item in this.tabPage4.Controls)
            {
                if (item is TextBox)
                {
                    int index = Convert.ToInt16(((TextBox)item).Name.ToString().Replace("textBox", ""));
                    textBoxsList[index] = (TextBox)item;
                }
            }
            foreach (var item in this.tabPage5.Controls)
            {
                if (item is TextBox)
                {
                    int index = Convert.ToInt16(((TextBox)item).Name.ToString().Replace("textBox", ""));
                    textBoxsList[index] = (TextBox)item;
                }
            }
            foreach (var item in this.tabPage6.Controls)
            {
                if (item is TextBox)
                {
                    int index = Convert.ToInt16(((TextBox)item).Name.ToString().Replace("textBox", ""));
                    textBoxsList[index] = (TextBox)item;
                }
            }
            foreach (var item in this.tabPage7.Controls)
            {
                if (item is TextBox)
                {
                    int index = Convert.ToInt16(((TextBox)item).Name.ToString().Replace("textBox", ""));
                    textBoxsList[index] = (TextBox)item;
                }
            }
            foreach (var item in this.tabPage8.Controls)
            {
                if (item is TextBox)
                {
                    int index = Convert.ToInt16(((TextBox)item).Name.ToString().Replace("textBox", ""));
                    textBoxsList[index] = (TextBox)item;
                }
            }

            #endregion
          
        }


        //打开数据库界面
        private void button3_Click(object sender, EventArgs e)
        {
            Sql_query Sql_Form = new Sql_query();
            this.Visible = false;
            Sql_Form.ShowDialog(this);
        }

        //定时执行方法
        private void timer1_Tick(object sender, EventArgs e)
        {
            GetDataFromSql();
        }

        //从Sqlserver取数据
        private void GetDataFromSql()
        {
            string GetDataString = "select value from Temp_Table";
            SqlDataReader myDataReader = SqlHelper.ExecuteReader(GetDataString);
            myDataReader.Read();
            string  tempStirng = myDataReader.GetString(myDataReader.GetOrdinal("value"));
            string[] float_temp=new string[250];
            float_temp=tempStirng.Split('*');
            for (int i = 1; i < 213; i++)
            {
                textBoxsList[i].Text = float_temp[i].ToString();
            }
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            //启动定时器
            timer1.Enabled = true;
            timer1.Start();
            button1.Enabled = false;
            button2.Enabled = true;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            timer1.Stop();
            button1.Enabled = true;
            button2.Enabled = false;
        }


        //打开图表界面
        private void button4_Click_1(object sender, EventArgs e)
        {
            Process.Start("http://192.168.133.7:9090/index.aspx");
            
            //Chart_Show Show_Form = new Chart_Show();
            //this.Visible = false;
            //Show_Form.ShowDialog(this);
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            RealTimeChart realTimeChart = new RealTimeChart();
            this.Visible = false;
            realTimeChart.ShowDialog(this);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Chart_Show SHOWChart = new Chart_Show();
            this.Visible = false;
            SHOWChart.ShowDialog(this);
        }
    }
}
