using IPC2_P1.Models;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web;
using System;

namespace IPC2_P1.Controllers
{
    public class TorneoController : Controller
    {
        public static string sql = "Data Source=PCP-PC;Initial Catalog=Othello_db;User ID=pabloc54;Password=pablo125";
        private SqlConnection con = new SqlConnection(sql);
        private Othello Game = new Othello();

        public ActionResult Index(string message, string messagetype)
        {
            if (Globals.logged_in == true)
            {
                ViewBag.Message = message;
                ViewBag.MessageType = messagetype;

                return View();
            }
            else
            {
                message = "Primero debes iniciar sesión";
                messagetype = "error-message";

                return RedirectToAction("Login", "Home", new { message, messagetype });
            }
        }

        [HttpPost]
        public ActionResult Index(string nombre_campeonato, string num_equipos, HttpPostedFileBase archivo)
        {
            if (archivo != null)
            {
                //CARGAR
                return View();
            }
            else
            {
                return RedirectToAction("CrearCampeonato","Torneo", new { nombre_campeonato, num_equipos });
            }
        }

        [HttpPost]
        public ActionResult CrearEquipo(string nombre_equipo, string j1, string j2, string j3)
        {
            if (j1 == j2 || j1 == j3 || j2 == j3)
            {
                string message = "Ingresa usuarios diferentes";
                string messagetype = "error-message";

                return RedirectToAction("Index", "Torneo", new { message, messagetype });
            }


            con.Open();

            List<string> jugadores = new List<string> { j1, j2, j3 };

            string txt = "";
            SqlCommand cmd;
            SqlDataReader dr;

            foreach (string jugador in jugadores)
            {
                txt = "select * from Usuario where username='" + jugador + "'";

                cmd = new SqlCommand(txt, con);
                dr = cmd.ExecuteReader();

                if (!dr.Read())
                {
                    string message = "El usuario " + jugador + " no está registrado";
                    string messagetype = "error-message";

                    return RedirectToAction("Index", new { message, messagetype });
                }

                dr.Close();
            }

            txt = "insert into Equipo values ('" + nombre_equipo + "','" + j1 + "','" + j2 + "','" + j3 + "')";
            cmd = new SqlCommand(txt, con);

            try
            {
                int n = cmd.ExecuteNonQuery();

                if (n > 0)
                {
                    con.Close();

                    string message = "¡Equipo creado con éxito!";
                    string messagetype = "success-message";

                    return RedirectToAction("Index", "Torneo", new { message, messagetype });
                }
                else
                {
                    con.Close();

                    string message = "Nombre de equipo ocupado";
                    string messagetype = "error-message";

                    return RedirectToAction("Index", "Torneo", new { message, messagetype });
                }
            }
            catch
            {
                con.Close();

                string message = "Nombre de equipo ocupado";
                string messagetype = "error-message";

                return RedirectToAction("Index", "Torneo", new { message, messagetype });
            }

        }

        public ActionResult CrearCampeonato(string nombre_campeonato, string num_equipos)
        {

            con.Open();

            string txt = "select * from Campeonato where nombre_campeonato='" + nombre_campeonato + "'";
            SqlCommand cmd = new SqlCommand(txt, con);
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                dr.Close();
                con.Close();

                string message = "El nombre de campeonato '" + nombre_campeonato + "' está ocupado";
                string messagetype = "error-message";

                return RedirectToAction("Index", "Torneo", new { message, messagetype });
            }

            dr.Close();
            con.Close();

            int temp = 0;

            if (num_equipos == "4")
            {
                temp = 2;
            }
            else if (num_equipos == "8")
            {
                temp = 3;
            }
            else if (num_equipos == "16")
            {
                temp = 4;
            }


            Campeonato campeonato = new Campeonato()
            {
                Nombre_campeonato = nombre_campeonato,
                Numero_equipos = int.Parse(num_equipos),
                Numero_rondas = temp,
                Equipos = new List<Equipo>()
            };

            for (int i = 0; i < int.Parse(num_equipos); i++)
            {
                campeonato.Equipos.Add(new Equipo());
            }

            Globals.campeonato = campeonato;
            
            return View(campeonato);
        }

        [HttpPost]
        public ActionResult CrearCampeonato(Campeonato campeonato)
        {
            con.Open();

            // VALIDAR QUE NO SE REPITA EQUIPO

            foreach (Equipo equipo in campeonato.Equipos)
            {
                string txt = "select username, username2, username3 from Equipo where nombre_equipo='" + equipo.Nombre_equipo + "'";
                SqlCommand cmd = new SqlCommand(txt, con);
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    equipo.j1 = dr.GetString(0);
                    equipo.j2 = dr.GetString(1);
                    equipo.j3 = dr.GetString(2);
                }
                else
                {
                    dr.Close();
                    con.Close();

                    ViewBag.Message = "El equipo '" + equipo.Nombre_equipo + "' no ha sido registrado";
                    ViewBag.MessageType = "error-message";

                    return View(campeonato);
                }

                dr.Close();
            }

            con.Close();
            campeonato.Rondas = new List<Ronda>();
            Partida partida = new Partida() { Jugador1=campeonato.Equipos[0].j1, Jugador2=campeonato.Equipos[0].j1 };
            Ronda ronda = new Ronda
            {
                Numero_ronda = 1,
                Numero_equipos = campeonato.Numero_equipos,
                Equipos = campeonato.Equipos,
                Equipo1 = campeonato.Equipos[0],
                Equipo2 = campeonato.Equipos[1],
                Partidas = new List<Partida>() { partida },
                Partida_siguiente = partida
            };

            campeonato.Rondas.Add(ronda);

            campeonato.Ronda_actual = campeonato.Rondas[0];

            Globals.campeonato = campeonato;

            ViewBag.Message = "Ronda 1, se disputarán " + (campeonato.Numero_equipos / 2)*3 + " partidos";
            ViewBag.MessageType = "neutral-message";

            return RedirectToAction("Sala","Torneo",campeonato);
        }

        public ActionResult Sala(Campeonato campeonato, string message, string messagetype)
        {
            ViewBag.Message = message;
            ViewBag.MessageType = messagetype;

            return View(Globals.campeonato);
        }

        [HttpPost]
        public ActionResult Sala(Campeonato campeonato)
        {
            bool temp = false;
            /*
            if (apertura == "Personalizada")
            {
                temp = true;
                ViewBag.MessageType = "neutral-message";
                ViewBag.Message = "Apertura personalizada en progreso, faltan 4 fichas por colocar";

            }*/

            campeonato.Ronda_actual.Partida_siguiente.tablero = new Tablero(8, 8, "negro", Globals.campeonato.Ronda_actual.Partida_siguiente.Jugador1, Globals.campeonato.Ronda_actual.Partida_siguiente.Jugador2);
            campeonato.Ronda_actual.Partida_siguiente.tablero.Iniciar(temp);

            Globals.campeonato = campeonato;

            return View("Juego", campeonato.Ronda_actual.Partida_siguiente);            
        }
        
        [HttpPost]
        public ActionResult Juego(Partida partida)
        {
            Tablero tablero = partida.tablero;
            string usuario = tablero.usuario, usuario_opuesto = "";

            if (usuario == partida.Jugador1)
                usuario_opuesto = partida.Jugador2;
            else
                usuario_opuesto = partida.Jugador1;


            string color = tablero.color, color_opuesto = "";

            if (color == "blanco")
                color_opuesto = "negro";
            else
                color_opuesto = "blanco";


            int index = Game.Ficha_seleccionada(tablero);

            //APERTURA PERSONALIZADA
            if (tablero.apertura_personalizada == "true")
            {
                if (tablero.Apertura(index))
                {
                    Game.Reemplazar(tablero, index, color);
                    tablero.Actualizar(color_opuesto, usuario_opuesto, 0, 0);

                    if (Game.Celdas_ocupadas(tablero).Count == 4)
                    {
                        ViewBag.MessageType = "success-message";
                        ViewBag.Message = "¡Apertura personalizada finalizada!";
                        tablero.apertura_personalizada = "false";
                    }
                    else
                    {
                        ViewBag.MessageType = "neutral-message";
                        ViewBag.Message = "Apertura personalizada en progreso, faltan " + (4 - Game.Celdas_ocupadas(tablero).Count) + " fichas para finalizar";
                    }

                    return View(partida);
                }
                else
                {
                    ViewBag.MessageType = "error-message";
                    ViewBag.Message = "¡Posición de apertura no válida!";
                    Game.Reemplazar(tablero, index, "");

                    return View(partida);
                }
            }

            List<int> lista = Game.Flanquear(tablero, index, color, color_opuesto);

            if (lista.Count > 0)
            {
                Game.Reemplazar_lista(tablero, lista, color);

                List<int> celdas_validas = Game.Celdas_validas(tablero, color_opuesto, color);

                if (celdas_validas.Count > 0)
                {
                    if (usuario == "Oponente")
                        tablero.Actualizar(color_opuesto, Globals.usuario_activo, tablero.movimientos, tablero.movimientos_oponente + 1);
                    else
                        tablero.Actualizar(color_opuesto, "Oponente", tablero.movimientos + 1, tablero.movimientos_oponente);

                    return View(partida);
                }
                else
                {
                    celdas_validas = Game.Celdas_validas(tablero, color, color_opuesto);

                    if (celdas_validas.Count > 0)
                    {
                        ViewBag.Message = usuario_opuesto + " no tiene movimientos válidos";
                        ViewBag.MessageType = "error-message";

                        return View(partida);
                    }
                    else
                    {
                        //JUEGO TERMINADO
                        ViewBag.MessageType = "success-message";
                        ViewBag.Message = Game.TJuego_terminado(partida, color_opuesto);

                        return View(partida);
                    }
                }
            }
            else
            {
                Game.Reemplazar(tablero, index, "");
                return View(partida);
            }
        }
    }
}