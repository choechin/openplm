using System;
using System.Collections.Generic;

namespace PLMAPI.Models.Response
{
  public class WpartItemResponse
  {
    public string id { get; set; }
    public string TempPartNo { get; set; }
    public string WpartStatus { get; set; }
    public DateTime ApplyDate { get; set; }
    public DateTime RenotifyDate { get; set; }
    public DateTime RecycleDate { get; set; }
    public string Applicant { get; set; }
    public string Department { get; set; }
    public string Cycle { get; set; }
    public string Applicationtype { get; set; }
    public string UseDates { get; set; }
    public string CEOwner { get; set; }
    public double UnitPrice { get; set; }
    public string ApplicationReason { get; set; }
    public string Memo { get; set; }
    public string MaterialDescription { get; set; }
    public string Size { get; set; }
    public string TranSap { get; set; }
  }
}