using NLog;
using NLog.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PLMAPI.Controllers
{
    public class HomeController : Controller
    {
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
    }
}