using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PLC_POSITION.Class;

namespace PLC_POSITION
{
    public partial class Sql_query : Form
    {
        public Sql_query()
        {
            InitializeComponent();
        }

        private void Sql_query_Load(object sender, EventArgs e)
        {
            dateTimePicker1.CustomFormat = "yyyy-MM-dd";
            dateTimePicker2.CustomFormat = "yyyy-MM-dd";
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var LLJK = new[]
            {
                "放流1", "显影清洗", "放流2-1", "放流2-5", "比重水", "放流3-1", "酸洗1", "水洗3-4", "去膜定量", "放流4-1", "汤洗4-3", "酸洗2",
                "水洗4-5", "防锈", "放流5-4"
            };
            var FDJK = new[] { "风刀-1", "风刀-2", "风刀-3" };
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
                    break;
                case 1:
                    comboBox2.DataSource = FDJK;
                    break;
                case 2:
                    comboBox2.DataSource = PLYL;
                    break;
                case 3:
                    comboBox2.DataSource = PLXY;
                    break;
                case 4:
                    comboBox2.DataSource = PLSKQ;
                    break;
                case 5:
                    comboBox2.DataSource = PLSKH;
                    break;
                case 6:
                    comboBox2.DataSource = QMQX;
                    break;
                case 7:
                    comboBox2.DataSource = QM;
                    break;
            }
        }

        string sqlString = null;
        private DataTable FirstTable;
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "" || comboBox2.Text == "" || comboBox3.Text == "" || comboBox4.Text == "")
            {
                MessageBox.Show("必填项不得为空");
            }
            else
            {


                string BeforeDate = dateTimePicker1.Text;
                string AfterDate = dateTimePicker2.Text;
                if (comboBox4.Text != "无")
                {
                    sqlString =string.Format(
                        "SELECT * FROM Record_Table where Name='{0}'  and UpDown='{1}' and Time>='{2}' and Time<'{3}'",
                            comboBox2.Text, comboBox4.Text, BeforeDate, AfterDate);
                }
                else if (comboBox3.Text != "无")
                {
                    sqlString =string.Format(
                        "SELECT * FROM Record_Table where Name='{0}'  and LeftRight='{1}' and Time>='{2}' and Time<'{3}'",
                            comboBox2.Text, comboBox3.Text, BeforeDate, AfterDate);
                }
                else if (comboBox3.Text == "无" && comboBox4.Text == "无")
                {
                    sqlString = string.Format(
                        "SELECT * FROM Record_Table where Name='{0}' and Time>='{1}' and Time<'{2}'",
                        comboBox2.Text, BeforeDate, AfterDate);
                }
                else
                {
                    sqlString = string.Format(
                        "SELECT * FROM Record_Table where Name='{0}' and UpDown='{1}' and LeftRight='{2}' and Time>='{3}' and Time<'{4}'",
                        comboBox2.Text, comboBox3.Text, comboBox4.Text, BeforeDate, AfterDate);
                }
                FirstTable = SqlHelper.ExecuteDataTable(sqlString);
                dataGridView1.DataSource = FirstTable;

            }

        }

        
        private void button4_Click(object sender, EventArgs e)
        {
            this.Owner.Visible = true;
            this.Close();
        }

        private int pageNum = 1;
        private void button2_Click(object sender, EventArgs e)
        {
            if (FirstTable != null)
            { dataGridView1.DataSource = DtSelectTop((pageNum - 1) * 20, pageNum * 20, FirstTable); }
            //string pageUpString =
            //    "select top 50 * from MAT_Chemical_Line.dbo.Record_Table \r\nwhere id not in (select top "+50*(pageNum-1)+" id from MAT_Chemical_Line.dbo.Record_Table  order by id)\r\norder by id";
            //dataGridView1.DataSource = SqlHelper.ExecuteDataTable(pageUpString);
            if (pageNum!=1)
            {
                pageNum--;
            }
            textBox1.Text = pageNum.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (FirstTable != null)
            { dataGridView1.DataSource = DtSelectTop((pageNum - 1) * 20, pageNum * 20, FirstTable); }
            //string pageUpString =
            //"select top 50 * from MAT_Chemical_Line.dbo.Record_Table \r\nwhere id not in (select top " + 50 * (pageNum - 1) + " id from MAT_Chemical_Line.dbo.Record_Table  order by id)\r\norder by id";
            //dataGridView1.DataSource = SqlHelper.ExecuteDataTable(pageUpString);
            
            if (dataGridView1.RowCount-(pageNum*20)>20)
            {
                pageNum++;
            }

            textBox1.Text = pageNum.ToString();
        }

        public static DataTable DtSelectTop(int BottomItem, int TopItem, DataTable oDt)
        {

                if (oDt.Rows.Count < TopItem) return oDt;

                DataTable NewTable = oDt.Clone();
                DataRow[] rows = oDt.Select("1=1");
                for (int i = BottomItem; i < TopItem; i++)
                {
                    NewTable.ImportRow((DataRow)rows[i]);
                }
                return NewTable;

        }

        private void Sql_query_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

    }
}
