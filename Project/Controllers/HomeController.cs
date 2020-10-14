using IPC2_P1.Models;
using System.Web.Mvc;
using System.Data.SqlClient;

namespace IPC2_P1.Controllers
{
    public class HomeController : Controller
    {
        public static string sql = "Data Source=PCP-PC;Initial Catalog=Othello_db;User ID=pabloc54;Password=pablo125";
        public SqlConnection con = new SqlConnection(sql);

        public RedirectToRouteResult Index()
        {
            return RedirectToAction("Menu","Home");
        }

        public ActionResult Menu()
        {                
            return View();
        }

        /*LOGIN*/

        [HttpGet]
        public ActionResult Login()
        {
            if (Globals.logged_in == true)
            {
                ViewBag.Message = "Sesión iniciada como <b>" + Globals.usuario_activo + "</b>";
                ViewBag.MessageType = "neutral-message";
                return RedirectToAction("Menu", "Home");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Login(Usuario user)
        {
            con.Open();

            string txt = "select * from Usuario where username='"+user.Username+"' and contraseña='"+user.Contraseña+"'";
            SqlCommand cmd = new SqlCommand(txt, con);
            SqlDataReader dr = cmd.ExecuteReader();

            try
            {
                if (dr.Read())
                {
                    Globals.logged_in = true;
                    Globals.usuario_activo = user.Username;

                    ViewBag.Message = "¡Inicio de sesión exitoso!";
                    ViewBag.MessageType = "neutral-message";

                    con.Close();
                    return RedirectToAction("Menu", "Home");
                }
                else
                {
                    ViewBag.Message = "La combinación usuario-contraseña no existe";
                    ViewBag.MessageType = "error-message";

                    con.Close();
                    return View(user);
                }
            }
            catch
            {
                ViewBag.Message = "Se produjo un error";
                ViewBag.MessageType = "error-message";

                con.Close();
                return View();
            }            
        }

        /*REGISTRO*/

        public ActionResult Registro()
        {           
                return View();
        }

        [HttpPost]
        public ActionResult Registro(Usuario user)
        {// código de https://www.youtube.com/watch?v=1FB_X3adKpQ 

            con.Open();

            string txt= "insert into Usuario values ('" + user.Username + "','" + user.Nombres + "','" + user.Apellidos + "','" + user.Email + "','" + user.Contraseña + "','"+user.Fecha_Nacimiento.ToString("yyyy-MM-dd")+"','" + user.Pais + "',1); insert into Reporte values ('" + user.Username + "', 0, 0, 0)";
            SqlCommand cmd = new SqlCommand(txt,con);

            int n = cmd.ExecuteNonQuery();

            try
            {
                if (n > 0)
                {
                    ViewBag.Message = "¡Registro exitoso!";
                    ViewBag.MessageType = "success-message";

                    con.Close();
                    return RedirectToAction("Menu", "Home");
                }
                else
                {
                    ViewBag.Message = "¡Ocurrió un error!";
                    ViewBag.MessageType = "error-message";

                    con.Close();
                    return View();
                }
            }
            catch
            {
                ViewBag.Message = "Se produjo un error";
                ViewBag.MessageType = "error-message";

                con.Close();
                return View();
            }
        }
        
    }
}