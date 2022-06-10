using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Aras.IOM;
using NLog;
using PLMAPI.Aras;
using PLMAPI.Connection;
using PLMAPI.Models.Request;
using PLMAPI.Models.Response;

namespace PLMAPI.Models
{
    public class WpartService
    {
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static List<WpartItemResponse> GetWpart2Sap()
        {
            List<WpartItemResponse> wpartItemList = new List<WpartItemResponse>();
            DataTable partDt = new DataTable();
            partDt = wpartData();
            int dtCnt = partDt.Rows.Count;
            for (int i = 0; i < dtCnt; i++)
            {
                WpartItemResponse wpartItemResponse = new WpartItemResponse();
                wpartItemResponse.id = partDt.Rows[i]["ID"] == null ? "" : partDt.Rows[i]["ID"].ToString();
                wpartItemResponse.TempPartNo= partDt.Rows[i]["_TEMP_PART_NO"] == null ? "" : partDt.Rows[i]["_TEMP_PART_NO"].ToString(); //虛擬料號
                wpartItemResponse.MaterialDescription = partDt.Rows[i]["_Material_Description"] == null ? "" : partDt.Rows[i]["_Material_Description"].ToString(); //物料說明
                wpartItemResponse.Size = partDt.Rows[i]["_Size"] == null ? "" : partDt.Rows[i]["_Size"].ToString(); //尺寸
                string strUNIT_PRICE = partDt.Rows[i]["_UNIT_PRICE"] == null ? "" : partDt.Rows[i]["_UNIT_PRICE"].ToString(); // 單價
                wpartItemResponse.UnitPrice = string.IsNullOrEmpty(strUNIT_PRICE) ? 0 : Convert.ToDouble(partDt.Rows[i]["_UNIT_PRICE"]);
                wpartItemList.Add(wpartItemResponse);
            }

            return wpartItemList;
        }

        /// <summary>
        /// 更新WPart轉SAP狀態
        /// </summary>
        /// <param name="Wpartbody"></param>
        /// <returns></returns>
        public static string EditWpartStatus(WpartRequest Wpartbody)
        {
            string strStatus = string.Empty;

            if (wpartEdit(Wpartbody))
            {
                strStatus = "Success";
            }
            else
            {
                strStatus = "Fail";
            }
            return strStatus;
        }

        /// <summary>
        /// WPART待轉SAP資料
        /// </summary>
        /// <returns></returns>
        private static DataTable wpartData()
        {
            DataTable dtWpart = new DataTable();
            Dictionary<string, string> map = new Dictionary<string, string>();
            string sqlcmd = "Select ID,_TEMP_PART_NO,_Material_Description,_Size,_UNIT_PRICE From [innovator].[ATEN_WPART] WHERE _TRAN_SAP='W' ";
            DBTool dbtool = new DBTool();
            dtWpart = dbtool.PLMDataTable(sqlcmd, map);
            return dtWpart;
        }

        /// <summary>
        /// Update Wpart Status
        /// </summary>
        /// <param name="Wpartbody"></param>
        /// <returns></returns>
        private static bool wpartEdit(WpartRequest Wpartbody)
        {
            bool booEdit = false;
            int editCnt = 0;
            string sqlcmd = "Update [innovator].[ATEN_WPART] Set _TRAN_SAP=@Status Where ID=@ID ";
            Dictionary<string, string> map = new Dictionary<string, string>();
            map.Add("@ID", Wpartbody.ID);
            map.Add("@Status", Wpartbody.Status);
            DBTool dbtool = new DBTool();
            booEdit = dbtool.EditPLM(sqlcmd, map, ref editCnt);
            return booEdit;
        }
    }
}
