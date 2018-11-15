using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLC_POSITION.Class
{
    class PageClass
    {
        public static DataTable DtSelectTop(int TopItem, DataTable oDt)
        {
            if (oDt.Rows.Count < TopItem) return oDt;

            DataTable NewTable = oDt.Clone();
            DataRow[] rows = oDt.Select("1=1");
            for (int i = 0; i < TopItem; i++)
            {
                NewTable.ImportRow((DataRow)rows[i]);
            }
            return NewTable;
        }
    }
}
