﻿using Newtonsoft.Json;
using PLMAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace PLMAPI.Controllers
{
    public class JsonNetController : Controller
    {
        protected override JsonResult Json(object data, string contentType,
                   Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            if (behavior == JsonRequestBehavior.DenyGet
                && string.Equals(this.Request.HttpMethod, "GET",
                                 StringComparison.OrdinalIgnoreCase))
                //Call JsonResult to throw the same exception as JsonResult
                return new JsonResult();
            return new JsonNetResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding
            };
        }
        public class JsonNetResult : JsonResult
        {
            public JsonSerializerSettings SerializerSettings { get; set; }
            public Formatting Formatting { get; set; }
            public JsonNetResult()
            {
                SerializerSettings = new JsonSerializerSettings();
            }
            public override void ExecuteResult(ControllerContext context)
            {
                if (context == null)
                    throw new ArgumentNullException("context");
                HttpResponseBase response = context.HttpContext.Response;
                response.ContentType =
                    !string.IsNullOrEmpty(ContentType) ? ContentType : "application/json";
                if (ContentEncoding != null)
                    response.ContentEncoding = ContentEncoding;
                if (Data != null)
                {
                    JsonTextWriter writer = new JsonTextWriter(response.Output)
                    {
                        Formatting = Formatting
                    };
                    JsonSerializer serializer = JsonSerializer.Create(SerializerSettings);
                    serializer.Serialize(writer, Data); writer.Flush();
                }
            }
        }

        private string GetToken()
        {
            return System.Configuration.ConfigurationManager.AppSettings["x-aten-api-token"];
        }

        public string ValidateApiToken(string[] value)
        {
            string restr="";
            if (null == value)
            {
                restr = "Required request header 'x-aten-api-token'";
            }

            else if (!value.Contains(GetToken()))
            {
                restr = "Invalid API token";
            }
            return restr;
        }
    }
}