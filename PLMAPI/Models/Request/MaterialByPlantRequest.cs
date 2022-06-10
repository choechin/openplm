using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PLMAPI.Models.Request
{
    public class MaterialByPlantRequest
    {
        [Required]
        public List<string> materialNumber { get; set; }
    }
}