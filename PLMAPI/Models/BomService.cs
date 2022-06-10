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
    public class BomService
    {
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static List<BomByMaterialResponse> GetBomListByMaterials(string plant, BomByMaterialsRequest requestBody)
        {
            List<BomByMaterialResponse> list = new List<BomByMaterialResponse>();
            foreach (string material in requestBody.materialNumber)
            {
                BomByMaterialResponse bomByMaterialResponse = new BomByMaterialResponse();
                bomByMaterialResponse.materialNumber = material;
                bomByMaterialResponse.plant = plant;
                bomByMaterialResponse.bom = BomData(plant, material);
                if (bomByMaterialResponse.bom.Count() > 0)
                {
                    bomByMaterialResponse.msgCode = "OK";
                }
                else if (bomByMaterialResponse.bom.Count() == 0)
                {
                    bomByMaterialResponse.msgCode = "ERROR";
                    bomByMaterialResponse.msgDescription = "Not found material or material no bom";
                }
                else
                {
                    bomByMaterialResponse.msgCode = "ERROR";
                    bomByMaterialResponse.msgDescription = "Bom data error";
                }
                list.Add(bomByMaterialResponse);
            }
            return list;
        }

        private static List<BomItemResponse> BomData(string plant, string material)
        {
            List<BomItemResponse> bomItemResponseList = new List<BomItemResponse>();
            Dictionary<string, string> map = new Dictionary<string, string>();

            string sql = "select d._sort_order 'itemNumber',e._number 'component',e._temp_number 'tempComponent',e._description 'description' "
                       + " ,e.state 'state',e.major_rev 'rev',e._h_class 'materialType'"
                       + " ,(select _id from innovator.list_settings where id = d._ict) 'itemCategory',d._qty 'componentQuantity' "
                       + " ,(select _name from innovator.list_settings where id = d._unit) 'componentUnit' "
                       + " ,d._bom_item_text_1 'bomItemText1',d._bom_item_text_2 'bomItemText2',d._alternative_group 'alternativeItemGroup' "
                       + " ,(select _name from innovator.list_settings where id = d._strategy)'alternativeItemStrategy' "
                       + " ,d._probability 'usageProbability' "
                       + " from innovator.ATEN_YPART a "
                       + " left join innovator.ATEN_YPART_BOM b on a.id = b.SOURCE_ID "
                       + " left join innovator.ATEN_YBOM c on b.RELATED_ID = c.id "
                       + " left join innovator.ATEN_YBOM_BOM d on c.id = d.SOURCE_ID "
                       + " left join innovator.ATEN_YPART e on d.RELATED_ID = e.id "
                       + " where 1 = 1 "
                       + " and a.IS_CURRENT='1' "
                       + " and ( a._number=@material or a._temp_number=@material) "
                       + " and c._plant = (select id from innovator.list_settings where _setting_id = 'SAP' and _setting_type = 'PLANT' "
                       + " and _id = @plant)  order by d._sort_order,e._number ";
            map.Add("@material", material);
            map.Add("@plant", plant);

            DBTool dbtool = new DBTool();
            DataTable dt = dbtool.PLMDataTable(sql, map);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                BomItemResponse bomItemResponse = new BomItemResponse();
                bomItemResponse.itemNumber = dt.Rows[i]["itemNumber"].ToString();
                bomItemResponse.component = dt.Rows[i]["component"].ToString();
                bomItemResponse.tempComponent = dt.Rows[i]["tempComponent"].ToString();
                bomItemResponse.description = dt.Rows[i]["description"].ToString();
                bomItemResponse.state = dt.Rows[i]["state"].ToString();
                bomItemResponse.rev = dt.Rows[i]["rev"].ToString();
                bomItemResponse.materialType = dt.Rows[i]["materialType"].ToString();
                bomItemResponse.itemCategory = dt.Rows[i]["itemCategory"].ToString();
                if (!string.IsNullOrEmpty(dt.Rows[i]["componentQuantity"].ToString()))
                {
                    bomItemResponse.componentQuantity = Convert.ToDouble(dt.Rows[i]["componentQuantity"].ToString());
                }
                bomItemResponse.componentUnit = dt.Rows[i]["componentUnit"].ToString();
                bomItemResponse.bomItemText1 = dt.Rows[i]["bomItemText1"].ToString();
                bomItemResponse.bomItemText2 = dt.Rows[i]["bomItemText2"].ToString();
                bomItemResponse.alternativeItemGroup = dt.Rows[i]["alternativeItemGroup"].ToString();
                bomItemResponse.alternativeItemStrategy = dt.Rows[i]["alternativeItemStrategy"].ToString();
                bomItemResponse.usageProbability = dt.Rows[i]["usageProbability"].ToString();
                bomItemResponseList.Add(bomItemResponse);

                logger.Info("Get Material BomData material={}, itemNumber={},component={},tempComponent={}"
                    + ",description={},state={},rev={},materialType={},itemCategory={},componentQuantity={}"
                    + ",componentUnit={},bomItemText1={},bomItemText2={},alternativeItemGroup={}"
                    + ",alternativeItemStrategy={},usageProbability={}"
                    , material
                    , bomItemResponse.itemNumber
                    , bomItemResponse.component
                    , bomItemResponse.tempComponent
                    , bomItemResponse.description
                    , bomItemResponse.state
                    , bomItemResponse.rev
                    , bomItemResponse.materialType
                    , bomItemResponse.itemCategory
                    , bomItemResponse.componentQuantity
                    , bomItemResponse.componentUnit
                    , bomItemResponse.bomItemText1
                    , bomItemResponse.bomItemText2
                    , bomItemResponse.alternativeItemGroup
                    , bomItemResponse.alternativeItemStrategy
                    , bomItemResponse.usageProbability);
            }
            return bomItemResponseList;
        }
    }
}