using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PLMAPI.Models.Request
{
    public class WpartRequest
    {
        public string ID { get; set; }
        public string Status { get; set; }
    }
}