using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLMAPI.Models.Response
{
    public class BatchPromoteResponse
    {
        public string msgCode { get; set; }
        public string msgDescription { get; set; }
        public string itemType { get; set; }
        public string number { get; set; }
    }
}