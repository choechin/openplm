using NLog;
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
    public class MaterialService
    {
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static List<MaterialByPlantResponse> GetMaterialDataByMaterials(string plant, MaterialByPlantRequest requestBody)
        {
            List<MaterialByPlantResponse> list = new List<MaterialByPlantResponse>();
            string materialList = "";
            foreach (string material in requestBody.materialNumber)
            {
                materialList += material + ",";
            }
            materialList = materialList.TrimEnd(',');

            DataTable partDt = MaterialData(plant, materialList);
            foreach (string part in materialList.Split(',').ToList())
            {
                MaterialByPlantResponse materialByPlantResponse = new MaterialByPlantResponse();
                DataRow[] rows = partDt.Select("number='" + part + "' or tempNumber='"+ part + "'");
                if (rows.Count() == 0)
                {
                    materialByPlantResponse.msgCode = "ERROR";
                    materialByPlantResponse.msgDescription = "Material not exists.";
                    materialByPlantResponse.number = part;
                }
                else
                {
                    for (int i = 0; i < rows.Length; i++)
                    {
                        materialByPlantResponse.number = rows[i]["number"].ToString();
                        materialByPlantResponse.tempNumber = rows[i]["tempNumber"].ToString();
                        materialByPlantResponse.hClass = rows[i]["hClass"].ToString();
                        materialByPlantResponse.description = rows[i]["description"].ToString();
                        materialByPlantResponse.state = rows[i]["state"].ToString();
                        materialByPlantResponse.rev = rows[i]["rev"].ToString();
                        materialByPlantResponse.spec = rows[i]["spec"].ToString();
                        if (!string.IsNullOrEmpty(rows[i]["grossWeight"].ToString()))
                        {
                            materialByPlantResponse.grossWeight = Convert.ToDouble(rows[i]["grossWeight"]);
                        }
                        if (!string.IsNullOrEmpty(rows[i]["netWeight"].ToString()))
                        {
                            materialByPlantResponse.netWeight = Convert.ToDouble(rows[i]["netWeight"]);
                        }

                        materialByPlantResponse.unitOfWeight = rows[i]["unitOfWeight"].ToString();
                        materialByPlantResponse.volume = rows[i]["volume"].ToString();
                        materialByPlantResponse.unit = rows[i]["unit"].ToString();
                        materialByPlantResponse.materialGroup = rows[i]["materialGroup"].ToString();
                        materialByPlantResponse.model = rows[i]["model"].ToString();
                        materialByPlantResponse.exMaterialGroup = rows[i]["exMaterialGroup"].ToString();
                        materialByPlantResponse.sapProductCategory = rows[i]["sapProductCategory"].ToString();
                        materialByPlantResponse.productHierarchy = rows[i]["productHierarchy"].ToString();
                        if (!string.IsNullOrEmpty(rows[i]["length"].ToString()))
                        {
                            materialByPlantResponse.length = Convert.ToDouble(rows[i]["length"]);
                        }
                        if (!string.IsNullOrEmpty(rows[i]["width"].ToString()))
                        {
                            materialByPlantResponse.width = Convert.ToDouble(rows[i]["width"]);
                        }
                        if (!string.IsNullOrEmpty(rows[i]["high"].ToString()))
                        {
                            materialByPlantResponse.high = Convert.ToDouble(rows[i]["high"]);
                        }
                        materialByPlantResponse.procurement = rows[i]["procurement"].ToString();
                        materialByPlantResponse.msgCode = "OK";
                    }
                }
                list.Add(materialByPlantResponse);
            }
            return list;
        }

        /// <summary>
        /// 取得料號資料
        /// </summary>
        /// <param name="plant">廠別</param>
        /// <param name="materials">多筆料號用逗號分開</param>
        /// <returns></returns>
        private static DataTable MaterialData(string plant, string materials)
        {
            string partPare = "";

            Dictionary<string, string> partMap = new Dictionary<string, string>();
            int partInt = 0;
            string partValue = "@part";
            foreach (string part in materials.Split(',').ToList())
            {
                string partPara = partValue + partInt.ToString();
                partPare += partPara + ",";
                partMap.Add(partPara, part);
                partInt++;
            }
            partPare = partPare.TrimEnd(',');

            Dictionary<string, string> map = new Dictionary<string, string>();
            string sql = "select a._number 'number',a._temp_number 'tempNumber',a._h_class 'hClass',a._description 'description' "
                       + " ,a.state 'state',a.major_rev 'rev' "
                       + " , a._spec 'spec',a._gross_weight 'grossWeight',a._net_weight 'netWeight' "
                       + " ,(select _name from innovator.list_settings where id = a._unit_of_weight) 'unitOfWeight' "
                       + " ,(select _name from innovator.list_settings where id = a._volume) 'volume' "
                       + " ,(select _name from innovator.list_settings where id = a._unit) 'unit' "
                       + " ,(select _name from innovator.list_settings where id = a._material_group) 'materialGroup' "
                       + " ,(select _number from innovator.ATEN_MODEL where id = a._model) 'model' "
                       + " ,(select _model_group from innovator.ATEN_MODEL_GROUP where id = a._ex_material_group) 'exMaterialGroup' "
                       + " ,(select _name from innovator.list_settings where id = a._sap_product_category) 'sapProductCategory' "
                       + " ,(select _code from innovator.ATEN_Product_Hierarchy where id = a._product_hierarchy) 'productHierarchy' "
                       + " , a._length 'length',a._width 'width',a._high 'high' "
                       + " ,(select(select _name from innovator.list_settings where id = mrp._procurement) from innovator.ATEN_YPART_MRP mrp where mrp.id = b.id) 'procurement' "
                       + " from innovator.ATEN_YPART a "
                       + " left join innovator.ATEN_YPART_MRP b on a.id = b.source_id "
                       + " where 1 = 1 "
                       + " and a.IS_CURRENT = '1' "
                       + " and (a._number in (" + partPare + ") or a._temp_number in (" + partPare + ") ) "
                       + " and b._plant = (select id from innovator.list_settings list "
                       + " where list._setting_id = 'SAP' and list._setting_type = 'PLANT' and list._id = @plant) ";
            map.Add("@plant", plant);

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
    }
}