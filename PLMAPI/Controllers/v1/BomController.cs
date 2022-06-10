using NLog;
using PLMAPI.Models;
using PLMAPI.Models.Request;
using PLMAPI.Models.Response;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Security;
using System.Web;
using System.Web.Mvc;

namespace PLMAPI.Controllers.v1
{
    [RoutePrefix("v1/BOM")]
    public class BomController : JsonNetController
    {
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();
        [Route("BPM")]
        [HttpPost]
        public ActionResult getBomListByMaterials(string plant, BomByMaterialsRequest requestBody)
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
                jresult = Json(new ApiResult<List<BomByMaterialResponse>>(BomService.GetBomListByMaterials(plant,
                                         requestBody)), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return Json(new ApiError("500", ex.Message));
            }
            return jresult;
        }
    }
}