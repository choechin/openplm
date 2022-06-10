using Aras.IOM;
using NLog;
using PLMAPI.Aras;
using PLMAPI.Connection;
using PLMAPI.Models.Request;
using PLMAPI.Models.Response;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PLMAPI.Models
{
    public class BatchService
    {
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static BatchYPartResponse GetMaterialDataByMaterials(BatchYPartRequest requestBody)
        {
            BatchYPartResponse batchYPartResponse = new BatchYPartResponse();

            List<BatchYPartItemResponse> batchYPartItemList = new List<BatchYPartItemResponse>();

            int quryCount = requestBody.materialNumberId.Count;

            DataTable partDt = YPartData(requestBody);

            if (quryCount != partDt.Rows.Count)
            {
                return batchYPartResponse;
            }

            for (int i = 0; i < partDt.Rows.Count; i++)
            {
                BatchYPartItemResponse batchYPartItemResponse = new BatchYPartItemResponse();

                batchYPartItemResponse.materialNumber = partDt.Rows[i]["materialNumber"].ToString();
                batchYPartItemResponse.baseUnitOfMeasure = partDt.Rows[i]["baseUnitOfMeasure"].ToString();
                batchYPartItemResponse.materialType = partDt.Rows[i]["materialType"].ToString();
                batchYPartItemResponse.materialDescription = partDt.Rows[i]["materialDescription"].ToString();
                batchYPartItemResponse.sizeOrDimensions = partDt.Rows[i]["sizeOrDimensions"].ToString();
                if (!DBNull.Value.Equals(partDt.Rows[i]["netWeight"]))
                {
                    batchYPartItemResponse.netWeight = Convert.ToDouble(partDt.Rows[i]["netWeight"]);
                }
                if (!DBNull.Value.Equals(partDt.Rows[i]["grossWeight"]))
                {
                    batchYPartItemResponse.grossWeight = Convert.ToDouble(partDt.Rows[i]["grossWeight"]);
                }
                batchYPartItemResponse.weightUnit = partDt.Rows[i]["weightUnit"].ToString();
                batchYPartItemResponse.volumeUnit = partDt.Rows[i]["volumeUnit"].ToString();
                batchYPartItemResponse.materialGroup = partDt.Rows[i]["materialGroup"].ToString();
                batchYPartItemResponse.externalMaterialGroup = partDt.Rows[i]["externalMaterialGroup"].ToString();
                batchYPartItemResponse.division = partDt.Rows[i]["division"].ToString();
                batchYPartItemResponse.productHierarchy = partDt.Rows[i]["productHierarchy"].ToString();
                if (!DBNull.Value.Equals(partDt.Rows[i]["length"]))
                {
                    batchYPartItemResponse.length = Convert.ToDouble(partDt.Rows[i]["length"]);
                }
                if (!DBNull.Value.Equals(partDt.Rows[i]["width"]))
                {
                    batchYPartItemResponse.width = Convert.ToDouble(partDt.Rows[i]["width"]);
                }
                if (!DBNull.Value.Equals(partDt.Rows[i]["height"]))
                {
                    batchYPartItemResponse.height = Convert.ToDouble(partDt.Rows[i]["height"]);
                }
                batchYPartItemResponse.procurementIndicator = partDt.Rows[i]["procurementIndicator"].ToString();
                batchYPartItemResponse.specialProcurementType = partDt.Rows[i]["specialProcurementType"].ToString();
                batchYPartItemResponse.@virtual = false;
                batchYPartItemList.Add(batchYPartItemResponse);
            }
            batchYPartResponse.materialList = batchYPartItemList;

            return batchYPartResponse;
        }

        /// <summary>
        /// 取得料號資料
        /// </summary>
        /// <returns></returns>
        private static DataTable YPartData(BatchYPartRequest requestBody)
        {
            string partPare = "";
            Dictionary<string, string> partMap = new Dictionary<string, string>();
            int partInt = 0;
            string partValue = "@part";

            foreach (string part in requestBody.materialNumberId)
            {
                string partPara = partValue + partInt.ToString();
                partPare += partPara + ",";
                partMap.Add(partPara, part);
                partInt++;
            }
            partPare = partPare.TrimEnd(',');

            Dictionary<string, string> map = new Dictionary<string, string>();
            string sql = "select "
                       + "a.id 'materialNumberId' "
                       + ",a._number 'materialNumber' "
                       + ",(select _name from innovator.list_settings where id = a._unit) 'baseUnitOfMeasure' "
                       + ", a._h_class 'materialType' "
                       + ", a._description 'materialDescription' "
                       + ", a._spec 'sizeOrDimensions' "
                       + ", a._net_weight 'netWeight' "
                       + ", a._gross_weight 'grossWeight' "
                       + ",(select _name from innovator.list_settings where id = a._unit_of_weight) 'weightUnit' "
                       + ",(select _name from innovator.list_settings where id = a._volume) 'volumeUnit' "
                       + ",(select _name from innovator.list_settings where id = a._material_group) 'materialGroup' "
                       + ",(select _model_group from innovator.ATEN_MODEL_GROUP where id = a._ex_material_group) 'externalMaterialGroup' "
                       + ",(select _id from innovator.list_settings where id = a._sap_product_category) 'division' "
                       + ", a._product_hierarchy_sap 'productHierarchy' "
                       + ", a._length 'length',a._width 'width',a._high 'height' "
                       + ",(select(select _name from innovator.list_settings where id = mrp._procurement) from innovator.ATEN_YPART_MRP mrp where mrp.id = b.id) 'procurementIndicator' "
                       + " ,(select(select _id from innovator.list_settings where id = mrp._special_procurement_type) from innovator.ATEN_YPART_MRP mrp where mrp.id = b.id) 'specialProcurementType' "
                       + " from innovator.ATEN_YPART a "
                       + " inner join innovator.ATEN_YPART_MRP b on a.id = b.source_id "
                       + " where 1 = 1 "
                       + " and a.is_current='1' "
                       + " and a.id in (" + partPare + ")";

            //多料號參數
            partInt = 0;
            foreach (KeyValuePair<string, string> paraName in partMap)
            {
                map.Add(partValue + partInt.ToString(), paraName.Value.ToString().Trim());
                partInt++;
            }

            DBTool dbtool = new DBTool();
            DataTable dt = dbtool.PLMDataTable(sql, map);
            return dt;
        }

        /// <summary>
        /// 推關程式
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public static List<BatchPromoteResponse> ItemPromoteState(string nowState, string toState, List<BatchPromoteRequest> requestBody)
        {
            List<BatchPromoteResponse> batchPromoteResponseList = new List<BatchPromoteResponse>();
            BaseTool bastool = new BaseTool();
            Innovator innovator = bastool.innovator;

            foreach (BatchPromoteRequest request in requestBody)
            {
                BatchPromoteResponse batchPromoteResponse = new BatchPromoteResponse();
                string itemName = "ATEN_" + request.itemType.ToString().Trim();

                Item nowItem = innovator.newItem(itemName, "get");
                nowItem.setAttribute("where", itemName + "._number='"+ request.number+ "' and "+ itemName+".is_current='1'");
                nowItem = nowItem.apply();

                string result = innovator.applyMethod("ATEN_Promote_By_System", "<formID>" + nowItem.getID() + "</formID><formItemName>" 
                    + itemName + "</formItemName><nowstate>"+ nowState + "</nowstate><tostate>" 
                    + toState + "</tostate>").getResult();

                if("Y"== result)
                {
                    batchPromoteResponse.msgCode = "OK";
                }
                else
                {
                    batchPromoteResponse.msgCode = "ERROR";
                    batchPromoteResponse.msgDescription = "API Promote Fail";
                }
                batchPromoteResponse.itemType = request.itemType;
                batchPromoteResponse.number = request.number;
                batchPromoteResponseList.Add(batchPromoteResponse);
            }

            return batchPromoteResponseList;
        }
    }
}