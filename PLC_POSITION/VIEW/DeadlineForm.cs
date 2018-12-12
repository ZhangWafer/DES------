using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PLC_POSITION.Class;

namespace PLC_POSITION.VIEW
{
    public partial class DeadlineForm : Form
    {
        public DeadlineForm()
        {
            InitializeComponent();
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

        private DataTable referDataTable;
        private DataTable deadlineTable;
        private void DeadlineForm_Load(object sender, EventArgs e)
        {
            textBox3.Visible = false;
            button3.Visible = false;
            //读取出参考表
            referDataTable = SqlHelper.ExecuteDataTable("select * from Refer_Table");
            //读出上下限表格
            deadlineTable = SqlHelper.ExecuteDataTable("select * from DeadlineTable");
        }

        private int selectIndex = 0;

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
            DataRow[] registerNum = referDataTable.Select(TableSelect);
            //显示查出来的对应寄存器
            // MessageBox.Show(registerNum[0][4].ToString());
            //用对应寄存器的编号去找对应的记录表，到处datatable
            // DataTable ResultTable = QueryRecordFromSql(Convert.ToInt16(registerNum[0][0]));
            selectIndex = Convert.ToInt16(registerNum[0][0]);

            //查找上下限语句
            string deadLineselectNum = string.Format("id='{0}'", selectIndex);
            //从结果表查找上下限
            DataRow[] deadLineNum = deadlineTable.Select(deadLineselectNum);
            low = deadLineNum[0][1].ToString();
            high = deadLineNum[0][2].ToString();
            textBox1.Text = low;
            textBox2.Text = high;
        }

        private string low;
        private string high;
        

        //Sql查询历史记录方法
        private DataTable QueryRecordFromSql(int typeid)
        {
            string queryString =
                string.Format(
                    "select * from MAT_Chemical_Line.dbo.Simple_RecordTable where TypeId={0} and RecordTime >= '{1}' and RecordTime <='{2}'",
                    typeid, DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.AddDays(1).ToString("yy-MM-dd"));
            return SqlHelper.ExecuteDataTable(queryString);
        }
        //修改按钮
        private void button2_Click(object sender, EventArgs e)
        {
            if (enableBool)
            {
                low = textBox1.Text;
                high = textBox2.Text;
                string updatestring =
                    string.Format("UPDATE DeadlineTable SET lowDeadline = '{0}',upDeadline = '{1}' WHERE id = '{2}' ",
                        low,
                        high, selectIndex);

                SqlHelper.ExecuteNonQuery(updatestring);
                MessageBox.Show("修改成功！");
            }
            else
            {
                textBox3.Visible = true;
                button3.Visible = true;
            }
        }

        private bool enableBool=false;
        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox3.Text=="admin")
            {
                MessageBox.Show("密码正确,可开始修改数据");
                enableBool = true;
                textBox3.Visible = false;
                button3.Visible = false;
            }
            else
            {
                MessageBox.Show("密码错误！");
                enableBool = false;
            }
        }
    }
}
