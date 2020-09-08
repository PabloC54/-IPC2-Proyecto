using IPC2_P1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IPC2_P1.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View(new Usuario { Nombres = "Leonardo Alejandro", Apellidos = "Jona Urque" });
        }

        [HttpPost]
        public ActionResult Login(Usuario user)
        {
            return Content($"User {user.Nombres} updated!");
        }


        public ActionResult Menu()
        {
            return View();
        }
    }
}