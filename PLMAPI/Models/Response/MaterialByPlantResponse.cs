using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLMAPI.Models.Response
{
    public class MaterialByPlantResponse
    {
        public string msgCode { get; set; }
        public string msgDescription { get; set; }
        public string number { get; set; }
        public string tempNumber { get; set; }
        public string hClass { get; set; }
        public string description { get; set; }
        public string state { get; set; }
        public string rev { get; set; }
        public string spec { get; set; }
        public double? grossWeight { get; set; }
        public double? netWeight { get; set; }
        public string unitOfWeight { get; set; }
        public string volume { get; set; }
        public string unit { get; set; }
        public string materialGroup { get; set; }
        public string model { get; set; }
        public string exMaterialGroup { get; set; }
        public string sapProductCategory { get; set; }
        public string productHierarchy { get; set; }
        public double? length { get; set; }
        public double? width { get; set; }
        public double? high { get; set; }
        public string procurement { get; set; }
    }
}