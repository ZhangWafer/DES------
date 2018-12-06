using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using PLC_POSITION.Class;

namespace PLC_POSITION.VIEW
{
    public partial class RealTimeChart : Form
    {
        public RealTimeChart()
        {
            InitializeComponent();
        }

        private DataTable referDataTable;

        //窗口默认读取方法，
        private void Chart_Show_Load(object sender, EventArgs e)
        {
            //读取出参考表
            referDataTable = SqlHelper.ExecuteDataTable("select * from Refer_Table");
        }

        //combobox选择器选择改变方法
        //公共“流量”或者“压力”后缀选择
        private string commomTitleString = null;

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
                    chart1.ChartAreas[0].AxisY.Title = "流量(L/min)";
                    
                    commomTitleString = "流量";
                    break;
                case 1:
                    comboBox2.DataSource = FDJK;
                    comboBox4.Enabled = true;
                    chart1.ChartAreas[0].AxisY.Title = "压力(kPa)";
                    
                    commomTitleString = "压力";
                    break;
                case 2:
                    comboBox2.DataSource = PLYL;
                    comboBox4.Enabled = false;
                    comboBox4.Text = null;
                    chart1.ChartAreas[0].AxisY.Title = "压力(MPa)";
                    
                    commomTitleString = "压力";
                    break;
                case 3:
                    comboBox2.DataSource = PLXY;
                    comboBox4.Enabled = true;
                    chart1.ChartAreas[0].AxisY.Title = "流量(L/min)";
                    
                    commomTitleString = "流量";
                    break;
                case 4:
                    comboBox2.DataSource = PLSKQ;
                    comboBox4.Enabled = true;
                    chart1.ChartAreas[0].AxisY.Title = "流量(L/min)";
                   
                    commomTitleString = "流量";
                    break;
                case 5:
                    comboBox2.DataSource = PLSKH;
                    comboBox4.Enabled = true;
                    chart1.ChartAreas[0].AxisY.Title = "流量(L/min)";
                   
                    commomTitleString = "流量";
                    break;
                case 6:
                    comboBox2.DataSource = QMQX;
                    comboBox4.Enabled = true;
                    chart1.ChartAreas[0].AxisY.Title = "流量(L/min)";
                    
                    commomTitleString = "流量";
                    break;
                case 7:
                    comboBox2.DataSource = QM;
                    comboBox4.Enabled = true;
                    chart1.ChartAreas[0].AxisY.Title = "流量(L/min)";
                    
                    commomTitleString = "流量";
                    break;
            }
          
        }

        //Sql查询历史记录方法
        private DataTable QueryRecordFromSql(int typeid)
        {
            string queryString =
                string.Format(
                    "select * from MAT_Chemical_Line.dbo.Simple_RecordTable where TypeId={0} and RecordTime >= '{1}' and RecordTime <='{2}'",
                    typeid, DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.AddDays(1).ToString("yy-MM-dd"));
            return SqlHelper.ExecuteDataTable(queryString);
        }

        //判断是否为数字
        public bool IsNumberic(string oText)
        {
            try
            {
                double var1 = Convert.ToDouble(oText);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //确定按钮执行方法
        private void button1_Click(object sender, EventArgs e)
        {
            //填入必填项
            if (comboBox1.Text == "" || comboBox2.Text == "" || comboBox3.Text == "" || comboBox5.Text == "" ||
                textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("必选项不得为空", "提示");
                return;
            }
            //判断是否为数字
            if (!(IsNumberic(textBox1.Text) && (IsNumberic(textBox2.Text))))
            {
                MessageBox.Show("输入了非法上下限的值，上下限应该为数字");
                return;
            }
            //添加曲线进曲线名字列表
            SelectedName.Add(comboBox1.Text + "-" + comboBox2.Text + "-" + comboBox3.Text + "-" + comboBox4.Text);
            //屏蔽选择
            comboBox1.Enabled = false;
            //设置曲线标题
            chart1.Series[0].LegendText = comboBox1.Text + "-" + comboBox2.Text + "-" + comboBox3.Text+"-"+comboBox4.Text;

            //设置网格线为透明
            chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Transparent; //透明
            //查询Table语句
            string TableSelect = string.Format("Name='{0}' and  LeftRight='{1}' and UpDown='{2}'", comboBox2.Text,
                comboBox3.Text, comboBox4.Text);
            if (comboBox4.Text == "")
            {
                TableSelect = string.Format("Name='{0}' and  LeftRight='{1}'", comboBox2.Text,
                    comboBox3.Text);
            }
            //把筛选行的数组挑出来
            DataRow[] registerNum = referDataTable.Select(TableSelect);
            //显示查出来的对应寄存器
            // MessageBox.Show(registerNum[0][4].ToString());
            //用对应寄存器的编号去找对应的记录表，到处datatable
            DataTable ResultTable = QueryRecordFromSql(Convert.ToInt16(registerNum[0][0]));
            selectIndex = Convert.ToInt16(registerNum[0][0]);
            //新建第一条折线
            Series series = chart1.Series[0];
            seriesArray.Add(series);
            SelectedIDArray.Add(selectIndex);
            //表格初始化
            ChartInit();




            //根据选择的时间间隔设定定时器频率
            timer1.Interval = Convert.ToInt16(comboBox5.Text)*1000;
            //启动定时器
            timer1.Enabled = true;
            timer1.Start();
            //按钮变灰
            button1.Enabled = false;
            button4.Enabled = true;
            comboBox5.Enabled = false;
        }


        //图表初始化
        private void ChartInit()
        {
            Series series = chart1.Series[0];
            series.ChartType = SeriesChartType.Line;
            // chart1.Series[0].LegendText = "温度";
            chart1.ChartAreas[0].AxisX.Title = "时间";
            // chart1.ChartAreas[0].AxisY.Title = "压力";
            //chart1.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Seconds;
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";
            //chart1.ChartAreas[0].AxisX.LabelStyle.Interval = 60;
            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            chart1.ChartAreas[0].AxisX.ScrollBar.Enabled = true;
            chart1.Series[0].Points.Clear();

            #region 画上下阈值

            //下限
            StripLine sl1 = new StripLine();
            sl1.BackColor = System.Drawing.Color.Red; //默认为白色，看不见
            sl1.IntervalOffset = double.Parse(textBox1.Text); //
            sl1.StripWidth = 0.001;
            sl1.Text = "下限: " + textBox1.Text;
            sl1.TextAlignment = StringAlignment.Far;
            chart1.ChartAreas[0].AxisY.StripLines.Add(sl1);
            //设置y轴的范围
            chart1.ChartAreas[0].AxisY.Maximum = Convert.ToDouble(textBox2.Text) + Convert.ToDouble(textBox2.Text)/4;
            chart1.ChartAreas[0].AxisY.Minimum = Convert.ToDouble(textBox1.Text) - 1;

            //上限
            StripLine sl2 = new StripLine();
            sl2.BackColor = System.Drawing.Color.Red; //默认为白色，看不见
            sl2.IntervalOffset = double.Parse(textBox2.Text); //
            sl2.StripWidth = 0.001;
            sl2.Text = "上限: " + textBox2.Text;
            sl2.TextAlignment = StringAlignment.Far;
            chart1.ChartAreas[0].AxisY.StripLines.Add(sl2);

            #endregion
        }

        //从Sqlserver取数据
        private string[] float_temp;

        private void GetDataFromSql()
        {
            string GetDataString = "select value from Temp_Table";
            SqlDataReader myDataReader = SqlHelper.ExecuteReader(GetDataString);
            myDataReader.Read();
            string tempStirng = myDataReader.GetString(myDataReader.GetOrdinal("value"));
            float_temp = new string[250];
            float_temp = tempStirng.Split('*');
        }



        //计算平均值
        public void CalAverage(Series seriesInput)
        {
            double sumPoints = 0;
            for (int i = 0; i < seriesInput.Points.Count; i++)
            {

                sumPoints += seriesInput.Points[i].YValues[0];
            }
            double averagePoint = sumPoints / (seriesInput.Points.Count);
            if (averagePoint.ToString().Length >= 4)
            {
                label9.Text = averagePoint.ToString().Substring(0, 4);
            }
            else
            {
                label9.Text = averagePoint.ToString();
            }
        }

        //timer1执行方法

        private int selectIndex = 0;


        private void timer1_Tick(object sender, EventArgs e)
        {
            //控制时间画一次图
            //if (timercount_60s>=1)
            //{
            GetDataFromSql();
            //遍历曲线个数
            for (int i = 0; i < seriesArray.Count; i++)
            {
                seriesArray[i].Points.AddXY(DateTime.Now, float_temp[SelectedIDArray[i]]);
            }
            //计算平均值
            CalAverage(seriesArray[0]);

            // timercount_60s = 0;
            //}
            //timercount_60s++;
        }

        //停止按钮方法
        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            //this.Owner.Visible = true;
            this.Visible = false;
            Monitor mainpage=new Monitor();
            mainpage.Show();
        }

        //清楚按钮方法
        private void button3_Click(object sender, EventArgs e)
        {
            //chart1.Series[i].Points.Clear();
            // chart1.ChartAreas[0].AxisY.StripLines.Clear();
            RealTimeChart newformChart=new RealTimeChart();
            newformChart.Show();
            this.Hide();
        }

        //鼠标移动事件 显示实时坐标值
        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            var area = chart1.ChartAreas[0];
            try
            {
                double yValue = area.AxisY.PixelPositionToValue(e.Y);
                label6.Text = string.Format("{0}", yValue.ToString().Substring(0, 4));
            }
            catch (Exception)
            {
                Console.Write("读取出画图界面数据错误");
            }
        }

        //窗口退出方法
        private void RealTimeChart_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        //取消按键方法
        private void button4_Click(object sender, EventArgs e)
        {
            comboBox5.Enabled = true;
            button1.Enabled = true;
            timer1.Stop();
            button4.Enabled = false;
        }

        List<Series> seriesArray = new List<Series>();
        List<int> SelectedIDArray = new List<int>();
        List<string> SelectedName = new List<string>();
        //新增复选曲线
        private void button5_Click(object sender, EventArgs e)
        {
            //判断是否已有该曲线
            if (SelectedName.Contains(comboBox1.Text + "-" + comboBox2.Text + "-" + comboBox3.Text + "-" + comboBox4.Text))
            {
                MessageBox.Show("已存在该线体");
                return;
            }
            //新建实例
            Series series2 = new Series();
            series2.ChartType = SeriesChartType.Line;
            //增加一条到list
            seriesArray.Add(series2);
            //增加到图表中
            chart1.Series.Add(series2);
            chart1.Series[seriesArray.Count-1].LegendText = comboBox1.Text + "-" + comboBox2.Text + "-" + comboBox3.Text+"-"+comboBox4.Text;
            SelectedIDArray.Add(SeachTypeId()); 
            //增加名字到列表
            SelectedName.Add(comboBox1.Text + "-" + comboBox2.Text + "-" + comboBox3.Text + "-" + comboBox4.Text);
        }

        //查找TypeId号
        public int SeachTypeId()
        {
            //查询Table语句
            string TableSelect = string.Format("Name='{0}' and  LeftRight='{1}' and UpDown='{2}'", comboBox2.Text,
                comboBox3.Text, comboBox4.Text);
            if (comboBox4.Text == "")
            {
                TableSelect = string.Format("Name='{0}' and  LeftRight='{1}'", comboBox2.Text,
                    comboBox3.Text);
            }
            //把筛选行的数组挑出来
            DataRow[] registerNum = referDataTable.Select(TableSelect);
            //显示查出来的对应寄存器
            // MessageBox.Show(registerNum[0][4].ToString());
            //用对应寄存器的编号去找对应的记录表，到处datatable
            DataTable ResultTable = QueryRecordFromSql(Convert.ToInt16(registerNum[0][0]));
            selectIndex = Convert.ToInt16(registerNum[0][0]);
            return selectIndex;
        }

    }
}
