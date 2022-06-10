using PLMAPI.Connection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PLMAPI.Models
{
    public class Material
    {
        static DBTool dbtool = new DBTool();

        public static DataTable PartBOM(string partno)
        {
            DataTable dt = new DataTable();
            Dictionary<string, string> map = new Dictionary<string, string>();

            //由此開始設定API要做的事情
            string sql = "select a._number 'part',c._number 'bom',c._DESCRIPTION 'discription',c.STATE 'state',c.MAJOR_REV 'rev',c.CLASSIFICATION 'class' " +
                         " from innovator.ATEN_PART a " +
                         " left join innovator.ATEN_PART_BOM b on a.id = b.SOURCE_ID " +
                         " left join innovator.aten_part c on b.RELATED_ID = c.id " +
                         " where a._number =@partno ";
            map.Add("@partno", partno);
            dt = dbtool.PLMDataTable(sql, map);
            return dt;
        }
    }
}