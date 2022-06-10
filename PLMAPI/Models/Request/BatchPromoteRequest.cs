using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLMAPI.Models.Request
{
    public class BatchPromoteRequest
    {
        public string itemType { get; set; }
        public string number { get; set; }
    }
}