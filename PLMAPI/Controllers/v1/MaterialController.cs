using NLog;
using PLMAPI.Models;
using PLMAPI.Models.Request;
using PLMAPI.Models.Response;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PLMAPI.Controllers.v1
{
    [RoutePrefix("v1/Material")]
    public class MaterialController : JsonNetController
    {
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 取得料號資料
        /// 備註:BPM宏電楊梅工程處理單有使用
        /// </summary>
        /// <param name="plant"></param>
        /// <param name="materialNumberType"></param>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        public ActionResult getMaterialDataByMaterials(string plant, MaterialByPlantRequest requestBody)
        {
            JsonResult jresult = new JsonResult();

            string valiDateApi = ValidateApiToken(Request.Headers.GetValues("x-aten-api-token"));
            if (!string.IsNullOrEmpty(valiDateApi))
            {
                return jresult = Json(new ApiError(HttpStatusCode.BadRequest.ToString(), valiDateApi), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(plant))
            {
                return jresult = Json(new ApiError(HttpStatusCode.BadRequest.ToString(), "No plant"), JsonRequestBehavior.AllowGet);
            }
            if (null == requestBody.materialNumber)
            {
                return jresult = Json(new ApiError(HttpStatusCode.BadRequest.ToString(), "Request Body is null"), JsonRequestBehavior.AllowGet);
            }
            try
            {
                jresult = Json(new ApiResult<List<MaterialByPlantResponse>>(MaterialService.GetMaterialDataByMaterials(plant,
                                         requestBody)), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return Json(new ApiError("500", ex.Message));
            }
            return jresult;

        }

        [Route("PartData")]
        [HttpGet]
        public ActionResult PartData(string partno)
        {
            JsonResult jresult = new JsonResult();

            /*****Add Token Authorize*****/
            string valiDateApi = ValidateApiToken(Request.Headers.GetValues("x-aten-api-token"));
            if (!string.IsNullOrEmpty(valiDateApi))
            {
                return jresult = Json(new ApiError(HttpStatusCode.BadRequest.ToString(), valiDateApi), JsonRequestBehavior.AllowGet);
            }

            if (!string.IsNullOrEmpty(valiDateApi))
            {
                return jresult = Json(new ApiError(HttpStatusCode.BadRequest.ToString(), valiDateApi), JsonRequestBehavior.AllowGet);
            }

            try
            {
                //jresult = Json(new ApiResult<DataTable>(Material.PartBOM(part)));//POST
                jresult = Json(new ApiResult<DataTable>(Material.PartBOM(partno)), JsonRequestBehavior.AllowGet); //GET
            }
            catch (Exception ex)
            {
                jresult = Json(new ApiError("0003", ex.ToString()), JsonRequestBehavior.AllowGet);
            }
            return jresult;
        }
    }
}