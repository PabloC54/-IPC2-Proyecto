using IPC2_P1.Models;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace IPC2_P1.Controllers
{
    public class HomeController : Controller
    {
        public static string sql = "Data Source=PCP-PC;Initial Catalog=Othello_db;User ID=pabloc54;Password=pablo125";
        private SqlConnection con = new SqlConnection(sql);

        public ActionResult Index()
        {
            if (Globals.logged_in == false)
            {
                string message = "Inicia sesión para jugar Othello";
                string messagetype = "neutral-message";
                return RedirectToAction("Menu", "Home", new { message, messagetype });
            }
            else
            {
                return RedirectToAction("Menu", "Home");
            }
        }

        public ActionResult Menu(string message, string messagetype)
        {
            ViewBag.Message = message;
            ViewBag.MessageType = messagetype;

            Globals.campeonato = new Campeonato() { };

            return View();
        }

        /*LOGIN*/
        
        public ActionResult Login(string message, string messagetype)
        {
            if (Globals.logged_in == true)
            {
                message = "Sesión iniciada como " + Globals.usuario_activo;
                messagetype = "neutral-message";

                return RedirectToAction("Menu", "Home", new { message, messagetype });
            }
            else
            {
                ViewBag.Message = message;
                ViewBag.MessageType = messagetype;

                return View();
            }
        }

        [HttpPost]
        public ActionResult Login(Usuario user)
        {
            con.Open();

            string txt = "select * from Usuario where username='" + user.Username + "' and contraseña='" + user.Contraseña + "'";
            SqlCommand cmd = new SqlCommand(txt, con);
            SqlDataReader dr = cmd.ExecuteReader();

            try
            {
                if (dr.Read())
                {
                    Globals.logged_in = true;
                    Globals.usuario_activo = user.Username;

                    con.Close();

                    string message = "¡Inicio de sesión exitoso!";
                    string messagetype = "neutral-message";

                    return RedirectToAction("Menu", "Home", new { message, messagetype });
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
        public ActionResult Registro(Usuario usuario, string confirmar_contraseña)
        {// código de https://www.youtube.com/watch?v=1FB_X3adKpQ 

            if (usuario.Contraseña == confirmar_contraseña)
            {
                con.Open();

                string txt = "insert into Usuario values ('" + usuario.Username + "','" + usuario.Nombres + "','" + usuario.Apellidos + "','" + usuario.Email + "','" + usuario.Contraseña + "','" + usuario.Fecha_Nacimiento.ToString("yyyy-MM-dd") + "','" + usuario.Pais + "',1)";
                SqlCommand cmd = new SqlCommand(txt, con);

                try
                {
                    int n = cmd.ExecuteNonQuery();

                    if (n > 0)
                    {
                        con.Close();
                        string message = "¡Registro exitoso!";
                        string messagetype = "success-message";

                        return RedirectToAction("Login", "Home", new { message, messagetype });
                    }
                    else
                    {
                        ViewBag.Message = "Usuario ya registrado";
                        ViewBag.MessageType = "error-message";

                        con.Close();
                        return View(usuario);
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
            else //contraseñas no coinciden
            {
                ViewBag.Message = "Las contraseñas no coinciden";
                ViewBag.MessageType = "error-message";
                
                return View(usuario);
            }
        }

        /*PERFIL*/

        public ActionResult Perfil()
        {
            if (Globals.logged_in == true)
            {
                con.Open();
                
                string txt = "select nombres, apellidos, correo_electronico, fecha_nacimiento, pais from Usuario where username='" + Globals.usuario_activo + "'";

                SqlCommand cmd = new SqlCommand(txt, con);
                SqlDataReader dr = cmd.ExecuteReader();

                dr.Read();

                string nombres = dr.GetString(0), apellidos = dr.GetString(1), correo = dr.GetString(2), fecha = dr.GetDateTime(3).ToString("dd-MM-yyyy"), pais = dr.GetString(4);
                
                dr.Close();


                txt = "select resultado from Partida where username='" + Globals.usuario_activo +"'";

                cmd = new SqlCommand(txt, con);
                dr = cmd.ExecuteReader();
                
                int victorias = 0, derrotas = 0, empates = 0;

                while (dr.Read())
                {
                    string resultado = dr.GetString(0);

                    if (resultado == "victoria")
                        victorias++;
                    else if (resultado == "derrota")
                        derrotas++;
                    else if (resultado == "empate")
                        empates++;
                }

                dr.Close();

                
                txt = "select nombre_equipo from Equipo where username='" + Globals.usuario_activo + "' or username2='"+Globals.usuario_activo+"' or username3='"+Globals.usuario_activo+"'";

                cmd = new SqlCommand(txt, con);
                dr = cmd.ExecuteReader();

                List<string> equipos = new List<string>();
                while (dr.Read())
                {
                    equipos.Add(dr.GetString(0));
                }

                dr.Close();

                int victorias_c = 0, puntos=0, cantidad=0;

                foreach(string equipo in equipos)
                {
                    txt = "select resultado, puntos from Registro_Campeonato where nombre_equipo='" + equipo + "'";

                    cmd = new SqlCommand(txt, con);
                    dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        cantidad++;
                        puntos += int.Parse(dr.GetSqlInt32(1).ToString());
                        if (dr.GetString(0) == "victoria")
                        {
                            victorias += 1;
                        }

                    }
                    dr.Close();
                }
                
                dr.Close();

                List<string> temp = new List<string>
                {
                    nombres,
                    apellidos,
                    correo,
                    fecha,
                    pais,
                    victorias.ToString(),
                    derrotas.ToString(),
                    empates.ToString(),
                    cantidad.ToString(),
                    victorias_c.ToString(),
                    puntos.ToString()
                };
                
                return View(temp);                             
            }
            else
            {
                string message = "Primero debes iniciar sesión";
                string messagetype = "error-message";

                return RedirectToAction("Login", "Home", new { message, messagetype });
            }
        }

    }
}