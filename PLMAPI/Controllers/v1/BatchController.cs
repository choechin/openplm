using NLog;
using PLMAPI.Models;
using PLMAPI.Models.Request;
using PLMAPI.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PLMAPI.Controllers.v1
{
    [RoutePrefix("v1/Batch")]
    public class BatchController : JsonNetController
    {
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();
        [Route("YPartToSap")]
        [HttpPost]
        public ActionResult getBatchYPartList(BatchYPartRequest requestBody)
        {
            JsonResult jresult = new JsonResult();

            string valiDateApi = ValidateApiToken(Request.Headers.GetValues("x-aten-api-token"));

            if (!string.IsNullOrEmpty(valiDateApi))
            {
                return jresult = Json(new ApiError(HttpStatusCode.BadRequest.ToString(), valiDateApi), JsonRequestBehavior.AllowGet);
            }
            if (null == requestBody.materialNumberId)
            {
                return jresult = Json(new ApiError(HttpStatusCode.BadRequest.ToString(), "Request Body is null"), JsonRequestBehavior.AllowGet);
            }
            try
            {
                BatchYPartResponse responseData = BatchService.GetMaterialDataByMaterials(requestBody);
                if (responseData.materialList != null)
                {
                    jresult = Json(new ApiResult<BatchYPartResponse>(responseData), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new ApiError("ERROR", "No response data. Please check material in plm"));
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return Json(new ApiError("500", ex.Message));
            }
            return jresult;
        }

        [Route("Promote")]
        [HttpPost]
        public ActionResult getPromoteList(string nowState, string toState, List<BatchPromoteRequest> requestBody)
        {
            JsonResult jresult = new JsonResult();

            string valiDateApi = ValidateApiToken(Request.Headers.GetValues("x-aten-api-token"));

            if (!string.IsNullOrEmpty(valiDateApi))
            {
                return jresult = Json(new ApiError(HttpStatusCode.BadRequest.ToString(), valiDateApi), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(nowState))
            {
                return jresult = Json(new ApiError(HttpStatusCode.BadRequest.ToString(), "No nowState"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(toState))
            {
                return jresult = Json(new ApiError(HttpStatusCode.BadRequest.ToString(), "No toState"), JsonRequestBehavior.AllowGet);
            }
            if (null== requestBody)
            {
                return jresult = Json(new ApiError(HttpStatusCode.BadRequest.ToString(), "Request Body is null"), JsonRequestBehavior.AllowGet);
            }
            try
            {
                List<BatchPromoteResponse> responseData = BatchService.ItemPromoteState(nowState, toState, requestBody);
                if (responseData != null)
                {
                    jresult = Json(new ApiResult<List<BatchPromoteResponse>>(responseData), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new ApiError("ERROR", "No response data. Please check material in plm"));
                }
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