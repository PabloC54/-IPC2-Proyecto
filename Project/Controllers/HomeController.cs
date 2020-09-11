using IPC2_P1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace IPC2_P1.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View("Menu");
        }

        public ActionResult Menu()
        {
            return View();
        }

        /*LOGIN*/

        [HttpGet]
        public ActionResult Login()
        {
            return View(new Usuario { Username = "201901698", Contraseña = "pepe3343" });
        }

        [HttpPost]
        public ActionResult Login(Usuario user)
        {

            ViewBag.Message = "La combinación usuario-contraseña no existe";
            return View();
        }

        /*REGISTRO*/

        public ActionResult Registro()
        {           
                return View();
        }

        [HttpPost]
        public ActionResult Registro(Usuario user)
        {// código de https://www.youtube.com/watch?v=1FB_X3adKpQ 
            
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connDB"].ConnectionString);
            con.Open();

            string txt= "insert into Usuario values ('" + user.Username + "','" + user.Nombres + "','" + user.Apellidos + "','" + user.Email + "','" + user.Contraseña + "',04-04-2004,'" + user.Pais + "',1";
            Console.WriteLine("texto:  " + txt);
            SqlCommand cmd = new SqlCommand(txt,con);
            //cmd.ExecuteReader();
            ViewBag.Message = "¡Registro exitoso!";   
            return View();

            

            //return Content("Se registró al usuario: "+user.Username+", Nombres: "+user.Nombres+", Apellidos: "+user.Apellidos+", Email: "+user.Email+", Contraseña: "+user.Contrasena+", Fecha de nacimiento: "+user.Fecha_Nacimiento+", Pais: "+user.Pais);
            
            /*if (ModelState.IsValid)            {

                ViewBag.Message = "¡Registro exitoso!";
                return View();
            }
            else
                return View(user);*/
        }

    }
}