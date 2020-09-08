using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IPC2_P1.Controllers
{
    public class GameController : Controller
    {
        public ActionResult Solo()
        {
            return View();
        }

        public ActionResult Versus()
        {
            return View();
        }
    }
}