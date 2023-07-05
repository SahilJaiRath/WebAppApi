using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Einv_EwayBill_WebApp.Controllers
{
    public class EwaybillController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
