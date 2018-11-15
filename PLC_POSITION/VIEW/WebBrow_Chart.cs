using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Microsoft.Office.Interop.Excel;
using PLC_POSITION.Class;
using PLC_POSITION.VIEW;
using Application = System.Windows.Forms.Application;
using DataTable = System.Data.DataTable;
using Series = System.Windows.Forms.DataVisualization.Charting.Series;
using System.Runtime.InteropServices;

namespace PLC_POSITION
{
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class WebBrow_Chart : Form
    {

        public WebBrow_Chart()
        {
            InitializeComponent();
        }

        private DataTable referDataTable;
        //窗口载入方法
        private void WebBrow_Chart_Load(object sender, EventArgs e) 
        {
            string path = "C:\\Users\\zhangyufei\\Desktop\\2018.8.17桌面整理\\PLC_POSITION\\PLC_POSITION\\Html\\line-marker.html";
           // string path = Application.StartupPath+"\\line-marker2.html";
            this.webBrowser1.Url = new System.Uri(path, System.UriKind.Absolute);
            referDataTable = SqlHelper.ExecuteDataTable("select * from Refer_Table");
        }

        //确定按钮方法
        private DataTable ResultTable;
        private void button1_Click(object sender, EventArgs e)
        {
            //填入必填项
            if (comboBox1.Text == "" || comboBox2.Text == "" || comboBox3.Text == "")
            {
                MessageBox.Show("必选项不得为空", "提示");
                return;
            }

            //查询Table语句
            string TableSelect = string.Format("Name='{0}' and  LeftRight='{1}' and UpDown='{2}'", comboBox2.Text,
                comboBox3.Text, comboBox4.Text);
            if (comboBox4.Text == "")
            {
                TableSelect = string.Format("Name='{0}' and  LeftRight='{1}'", comboBox2.Text,
                    comboBox3.Text);
            }

            //把筛选行的数组挑出来
            DataRow[] registerNum = null;
            try
            {
                registerNum = referDataTable.Select(TableSelect);
            }
            catch (Exception)
            {

                MessageBox.Show("输入信息错误");
            }
            
            //显示查出来的对应寄存器
            //MessageBox.Show(registerNum[0][4].ToString());
            //用对应寄存器的编号去找对应的记录表，到处datatable
            ResultTable = QueryRecordFromSql(Convert.ToInt16(registerNum[0][0]));

            string allResultString_value_X =null;
            string allResultString_value_Y = null;


            for (int i = 0; i < ResultTable.Rows.Count; i++)
            {
                allResultString_value_X += ResultTable.Rows[i][2] + "*";
                allResultString_value_Y += ResultTable.Rows[i][1] + "*";
            }

            string[] arrStrings=new string[2];
            arrStrings[0] = allResultString_value_X;
            arrStrings[1] = allResultString_value_Y;

            webBrowser1.Document.InvokeScript("ChartProduce",arrStrings);
            Console.WriteLine("Finish!");

        }

        //Sql查询历史记录方法
        private DataTable QueryRecordFromSql(int typeid)
        {
            string queryString =
                string.Format(
                    "select * from MAT_Chemical_Line.dbo.Simple_RecordTable where TypeId={0} and RecordTime >= '{1}' and RecordTime<='{2}'",
                    typeid, dateTimePicker1.Value.ToString("yyyy-MM-dd"),
                    dateTimePicker2.Value.AddDays(1).ToString("yyyy-MM-dd"));
            return SqlHelper.ExecuteDataTable(queryString);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var LLJK = new[]
            {
                "放流1", "显影清洗", "放流2-1", "放流2-5", "比重水", "放流3-1", "酸洗1", "水洗3-4", "去膜定量", "放流4-1", "汤洗4-3", "酸洗2",
                "水洗4-5", "防锈", "放流5-4"
            };
            var FDJK = new[] { "风刀1", "风刀2", "风刀3" };
            var PLYL = new[] { "显影", "蚀刻前", "蚀刻后", "去膜", "去膜清洗" };
            var PLXY = new[] { "显影1", "显影2", "显影3", "显影4", "显影5", "显影6", "显影7", "显影8" };
            var PLSKQ = new[] { "蚀刻前1", "蚀刻前2", "蚀刻前3", "蚀刻前4", "蚀刻前5", "蚀刻前6", "蚀刻前7", "蚀刻前8" };
            var PLSKH = new[] { "蚀刻后1", "蚀刻后2", "蚀刻后3", "蚀刻后4", "蚀刻后5", "蚀刻后6", "蚀刻后7", "蚀刻后8" };
            var QMQX = new[] { "去膜清洗1", "去膜清洗2", "去膜清洗3", "去膜清洗4" };
            var QM = new[] { "去膜1", "去膜2", "去膜3", "去膜4", "去膜5", "去膜6", "去膜7", "去膜8", "去膜9", "去膜10", "去膜11", "去膜12" };
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    comboBox2.DataSource = LLJK;
                    comboBox4.Enabled = false;
                    comboBox4.Text = null;

                    break;
                case 1:
                    comboBox2.DataSource = FDJK;
                    comboBox4.Enabled = true;
 
                    break;
                case 2:
                    comboBox2.DataSource = PLYL;
                    comboBox4.Enabled = false;
                    comboBox4.Text = null;

                    break;
                case 3:
                    comboBox2.DataSource = PLXY;
                    comboBox4.Enabled = true;
  
                    break;
                case 4:
                    comboBox2.DataSource = PLSKQ;
                    comboBox4.Enabled = true;
        
                    break;
                case 5:
                    comboBox2.DataSource = PLSKH;
                    comboBox4.Enabled = true;
                  
                    break;
                case 6:
                    comboBox2.DataSource = QMQX;
                    comboBox4.Enabled = true;
       
                    break;
                case 7:
                    comboBox2.DataSource = QM;
                    comboBox4.Enabled = true;
  
                    break;
            }
        }




    }
}
