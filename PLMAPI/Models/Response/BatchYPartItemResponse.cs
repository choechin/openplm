using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLMAPI.Models.Response
{
    public class BatchYPartItemResponse
    {
        public string materialNumber { get; set; }
        public string baseUnitOfMeasure { get; set; }
        public string materialType { get; set; }
        public string materialDescription { get; set; }
        public string sizeOrDimensions { get; set; }
        public double? netWeight { get; set; }
        public double? grossWeight { get; set; }
        public string weightUnit { get; set; }
        public string volumeUnit { get; set; }
        public string materialGroup { get; set; }
        public string externalMaterialGroup { get; set; }
        public string division { get; set; }
        public string productHierarchy { get; set; }
        public double? length { get; set; }
        public double? width { get; set; }
        public double? height { get; set; }
        public string procurementIndicator { get; set; }
        public string specialProcurementType { get; set; }
        public bool @virtual { get; set; }
    }
}