using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public partial class Chart_Show : Form
    {
        public Chart_Show()
        {
            InitializeComponent();
        }

        private DataTable referDataTable;

        private void Chart_Show_Load(object sender, EventArgs e)
        {
            referDataTable = SqlHelper.ExecuteDataTable("select * from Refer_Table");
            //var series = chart1.Series[0];
            //chart1.Series[0].BorderDashStyle = ChartDashStyle.Dash;
            
            //for (int i = 0; i < 9; i++)
            //{
            //    series.Points.AddXY(i, 9);
            //}
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var LLJK = new[]
            {
                "放流1", "显影清洗", "放流2-1", "放流2-5", "比重水", "放流3-1", "酸洗1", "水洗3-4", "去膜定量", "放流4-1", "汤洗4-3", "酸洗2",
                "水洗4-5", "防锈", "放流5-4"
            };
            var FDJK = new[] {"风刀1", "风刀2", "风刀3"};
            var PLYL = new[] {"显影", "蚀刻前", "蚀刻后", "去膜", "去膜清洗"};
            var PLXY = new[] {"显影1", "显影2", "显影3", "显影4", "显影5", "显影6", "显影7", "显影8"};
            var PLSKQ = new[] {"蚀刻前1", "蚀刻前2", "蚀刻前3", "蚀刻前4", "蚀刻前5", "蚀刻前6", "蚀刻前7", "蚀刻前8"};
            var PLSKH = new[] {"蚀刻后1", "蚀刻后2", "蚀刻后3", "蚀刻后4", "蚀刻后5", "蚀刻后6", "蚀刻后7", "蚀刻后8"};
            var QMQX = new[] {"去膜清洗1", "去膜清洗2", "去膜清洗3", "去膜清洗4"};
            var QM = new[] {"去膜1", "去膜2", "去膜3", "去膜4", "去膜5", "去膜6", "去膜7", "去膜8", "去膜9", "去膜10", "去膜11", "去膜12"};
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


        //确定按钮执行方法
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
           

            
            //timer1.Enabled = true;
            //timer1.Start();
        }


 



        private void button2_Click(object sender, EventArgs e)
        {
            this.Owner.Visible = true;
            this.Visible = false;
        }



        private void Chart_Show_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

 

        //导出excle数据方法
        public static bool waitingBool = false;

        public void DataTableToExcel(System.Data.DataTable tmDataTable, string strFileName)
        {
            WaitingForm wtForm = new WaitingForm();
            waitingBool = true;
            wtForm.Show();
            if (strFileName == null)
            {
                return;
            }
            int RowNum = tmDataTable.Rows.Count;
            int ColumnNum = tmDataTable.Columns.Count;
            int RowIndex = 2;
            int ColumnIndex = 0;
            Microsoft.Office.Interop.Excel.Application xlapp = new Microsoft.Office.Interop.Excel.Application();

            //打开Excel应用
            xlapp.DefaultFilePath = "";
            xlapp.DisplayAlerts = true;
            Microsoft.Office.Interop.Excel.Workbooks workbooks = xlapp.Workbooks;
            Microsoft.Office.Interop.Excel.Workbook workbook =
                workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet); //创建一个Excel文件
            Microsoft.Office.Interop.Excel.Worksheet worksheet =
                (Microsoft.Office.Interop.Excel.Worksheet) workbook.Worksheets[1];

            //拿到那个工作表
            //foreach (DataColumn dc in tmDataTable.Columns)
            //{
            //    ColumnIndex++;
            //    worksheet.Cells[RowIndex, ColumnIndex] = dc.ColumnName;
            //}

            //给两列写列名
            worksheet.Columns.HorizontalAlignment = XlHAlign.xlHAlignCenter; //水平居中
            worksheet.Columns.ColumnWidth = 20;

            Microsoft.Office.Interop.Excel.Range mergeRange;
            mergeRange = worksheet.get_Range("A1", "B1");
            mergeRange.Merge(0);
            if (comboBox4.Text=="")
            {
                worksheet.Cells[1, 1] = comboBox1.Text + "-" + comboBox2.Text + "-" + comboBox3.Text;
            }
            else
            {
                worksheet.Cells[1, 1] = comboBox1.Text + "-" + comboBox2.Text + "-" + comboBox3.Text + "-" + comboBox4.Text;
            }
           
            worksheet.Cells[2, 1] = "值";
            worksheet.Cells[2, 2] = "时间";

            //添加寄存器对应的监控点名称
            for (int i = 0; i < RowNum; i++)
            {
                RowIndex++;
                ColumnIndex = 0;
                for (int j = 1; j < 3; j++)
                {
                    ColumnIndex++;
                    worksheet.Cells[RowIndex, ColumnIndex] = tmDataTable.Rows[i][j].ToString();
                }
            }
            waitingBool = false;
            workbook.SaveCopyAs(strFileName + ".xlsx");


            MessageBox.Show("Excle表格导出成功,保存为" + strFileName);

            //退出关闭EXCLE.EXE线程
            xlapp.Quit();
            IntPtr t = new IntPtr(xlapp.Hwnd);
            int k = 0;
            GetWindowThreadProcessId(t, out k);
            System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById(k);
            p.Kill();
        }

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);


        //导出Excle按钮方法
        private void button4_Click(object sender, EventArgs e)
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
            try
            {
                string path = string.Empty;
                System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    path = fbd.SelectedPath;
                }
                
                DataTableToExcel(ResultTable,path+"/Des");
            }
            catch (Exception exception)
            {
                MessageBox.Show("请关闭打开的excle文档\n\r" + exception.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
             
        }
    }
}
