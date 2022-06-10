using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PLMAPI.Models.Request
{
    public class BatchYPartRequest
    {
        [Required]
        public List<string> materialNumberId { get; set; }
    }
}