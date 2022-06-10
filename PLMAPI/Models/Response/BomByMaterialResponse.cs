using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLMAPI.Models.Response
{
    public class BomByMaterialResponse
    {
        public string msgCode { get; set; }
        public string msgDescription { get; set; }
        public string materialNumber { get; set; }
        public string plant { get; set; }
        public List<BomItemResponse> bom { get; set; }
    }
}