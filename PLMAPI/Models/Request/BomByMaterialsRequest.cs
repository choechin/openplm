using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PLMAPI.Models.Request
{
    public class BomByMaterialsRequest
    {
        [Required]
        public List<string> materialNumber { get; set; }
    }
}