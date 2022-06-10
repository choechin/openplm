using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLMAPI.Models.Response
{
    public class BomItemResponse
    {
        public string itemNumber { get; set; }
        public string component { get; set; }
        public string tempComponent { get; set; }
        public string description { get; set; }
        public string state { get; set; }
        public string rev { get; set; }
        public string materialType { get; set; }
        public string itemCategory { get; set; }
        public double? componentQuantity { get; set; }
        public string componentUnit { get; set; }
        public string bomItemText1 { get; set; }
        public string bomItemText2 { get; set; }
        public string alternativeItemGroup { get; set; }
        public string alternativeItemStrategy { get; set; }
        public string usageProbability { get; set; }
    }
}