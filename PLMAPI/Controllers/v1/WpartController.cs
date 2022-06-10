using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using NLog;
using PLMAPI.Models;
using PLMAPI.Models.Request;
using PLMAPI.Models.Response;

namespace PLMAPI.Controllers.v1
{
    [RoutePrefix("v1/Wpart")]
    public class WpartController : JsonNetController
    {
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();

        [Route("Wpart")]
        [HttpPost]
        public ActionResult getWPartList()
        {
            JsonResult jresult = new JsonResult();
            string valiDateApi = ValidateApiToken(Request.Headers.GetValues("x-aten-api-token"));//檢核Token
            if (!string.IsNullOrEmpty(valiDateApi))
            {
                return jresult = Json(new ApiError(HttpStatusCode.BadRequest.ToString(), valiDateApi), JsonRequestBehavior.AllowGet);
            }
            try
            {
                List<WpartItemResponse> responseData = WpartService.GetWpart2Sap();//取得WPART待轉SAP資料
                jresult = Json(new ApiResult<List<WpartItemResponse>>(responseData), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return Json(new ApiError("500", ex.Message));
            }
            return jresult;
        }

        [Route("WpartSapStatus")]
        [HttpPost]
        public ActionResult edWpart(WpartRequest Wpartbody)
        {
            JsonResult jresult = new JsonResult();
            string valiDateApi = ValidateApiToken(Request.Headers.GetValues("x-aten-api-token"));//檢核Token
            if (!string.IsNullOrEmpty(valiDateApi))
            {
                return jresult = Json(new ApiError(HttpStatusCode.BadRequest.ToString(), valiDateApi), JsonRequestBehavior.AllowGet);
            }
            if (Wpartbody == null)
            {
                return jresult = Json(new ApiError(HttpStatusCode.BadRequest.ToString(), "Request Body is null"), JsonRequestBehavior.AllowGet);
            }
            try
            {
                string wpartStatus = WpartService.EditWpartStatus(Wpartbody);
                jresult = Json(new ApiResult<string>(wpartStatus), JsonRequestBehavior.AllowGet);
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