using IPC2_P1.Models;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web;
using System;
using System.Linq;

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
            List<string> Equipos_temp = new List<string>();
            
            foreach(Equipo equipo in campeonato.Equipos)
            {
                if (Equipos_temp.Contains(equipo.Nombre_equipo))
                {
                    ViewBag.Message = "El equipo '" + equipo.Nombre_equipo + "' se encuentra repetido";
                    ViewBag.MessageType = "error-message";

                    return View(campeonato);
                }
                else
                {
                    Equipos_temp.Add(equipo.Nombre_equipo);
                }
            }
            
                
            

            con.Open();

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

            Partida partida = new Partida() { Jugador1=campeonato.Equipos[0].j1, Jugador2=campeonato.Equipos[1].j1, index=0 };
            Ronda ronda = new Ronda
            {
                Nombre_campeonato = campeonato.Nombre_campeonato,
                index = 0,
                Numero_equipos = campeonato.Numero_equipos,
                Equipos = campeonato.Equipos,
                Equipo1 = campeonato.Equipos[0],
                Equipo2 = campeonato.Equipos[1],
                Partidas = new List<Partida>() { },
                Partida_actual = partida,
                Numero_partidas=(campeonato.Numero_equipos / 2) * 3
            };            

            campeonato.Ronda_actual = ronda;

            Globals.campeonato = campeonato;

            ViewBag.Message = "Ronda 1, se disputarán " + ronda.Numero_partidas + " partidos";
            ViewBag.MessageType = "neutral-message";

            return RedirectToAction("Sala","Torneo",ronda);
        }

        public ActionResult Sala(Campeonato campeonato, string message, string messagetype)
        {
            ViewBag.Message = message;
            ViewBag.MessageType = messagetype;

            return View(Globals.campeonato.Ronda_actual);
        }

        [HttpPost]
        public ActionResult Sala(Ronda ronda)
        {
            bool temp = false;
            /*
            if (apertura == "Personalizada")
            {
                temp = true;
                ViewBag.MessageType = "neutral-message";
                ViewBag.Message = "Apertura personalizada en progreso, faltan 4 fichas por colocar";

            }*/
            Tablero tablero_temp = new Tablero(8, 8, "negro", Globals.campeonato.Ronda_actual.Partida_actual.Jugador1, Globals.campeonato.Ronda_actual.Partida_actual.Jugador2);
            ronda.Partida_actual.tablero = tablero_temp;
            ronda.Partida_actual.tablero.Iniciar(temp);

            Globals.campeonato.Ronda_actual = ronda;

            return View("Juego", ronda.Partida_actual);
        }

        [HttpPost]
        public ActionResult Pasar(Ronda ronda, string ganador)
        {
            Campeonato campeonato = Globals.campeonato;

            if (ganador == "1")
            {
                ganador = ronda.Partida_actual.Jugador1;
            }
            else
            {
                ganador = ronda.Partida_actual.Jugador2;
            }

            Partida partida = new Partida()
            {
                Jugador1 = ronda.Partida_actual.Jugador1,
                Jugador2 = ronda.Partida_actual.Jugador2,
                Ganador = ganador,
                index = ronda.index + 1,
                Puntos1 = 40,
                Puntos2 = 24
            };

            try
            {
                campeonato.Ronda_actual.Partidas.Add(partida);
            }
            catch
            {
                campeonato.Ronda_actual.Partidas = new List<Partida>();
                campeonato.Ronda_actual.Partidas.Add(partida);
            }

            campeonato.Ronda_actual.Equipo1.puntos += partida.Puntos1;
            campeonato.Ronda_actual.Equipo2.puntos += partida.Puntos2;

            if (partida.index == campeonato.Ronda_actual.Numero_partidas - 1) //Es la ultima partida
            {
                if (campeonato.Ronda_actual.index == campeonato.Numero_rondas - 1) //Es la ultima ronda
                {
                    //terminar juego

                }
                else //No es la ultima ronda
                {
                    campeonato.Rondas[campeonato.Ronda_actual.index] = campeonato.Ronda_actual;

                    List<Equipo> equipos_temp = new List<Equipo>();

                    for (int i = 0; i < campeonato.Ronda_actual.Numero_equipos; i += 2)
                    {
                        Equipo equipo1 = campeonato.Ronda_actual.Equipos[i];
                        Equipo equipo2 = campeonato.Ronda_actual.Equipos[i + 1];

                        if (equipo1.puntos > equipo2.puntos)
                        {
                            equipos_temp.Add(equipo1);
                        }
                        else if (equipo1.puntos < equipo2.puntos)
                        {
                            equipos_temp.Add(equipo2);
                        }
                        else //empate, es posible que no vaya aquí
                        {

                        }

                        //reseteando los puntos
                        equipo1.puntos = 0;
                        equipo2.puntos = 0;
                    }

                    Partida partida_temp = new Partida() { Jugador1 = equipos_temp[0].j1, Jugador2 = equipos_temp[1].j1, index = 0 };

                    Ronda ronda_temp = new Ronda
                    {
                        Nombre_campeonato = campeonato.Nombre_campeonato,
                        index = campeonato.Ronda_actual.index + 1,
                        Numero_equipos = campeonato.Ronda_actual.Numero_equipos / 2,
                        Equipos = equipos_temp,
                        Equipo1 = equipos_temp[0],
                        Equipo2 = equipos_temp[1],
                        Partidas = new List<Partida>() { },
                        Partida_actual = partida_temp,
                        Numero_partidas = (campeonato.Ronda_actual.Numero_equipos / 4) * 3
                    };

                    campeonato.Ronda_actual = ronda_temp;
                }
            }
            else //No es la ultima partida
            {
                if (campeonato.Ronda_actual.Partida_actual.Jugador1 == campeonato.Ronda_actual.Equipo1.j1 || campeonato.Ronda_actual.Partida_actual.Jugador2 == campeonato.Ronda_actual.Equipo1.j1) //No se cambia de equipos
                {
                    if (campeonato.Ronda_actual.Partida_actual.Jugador1 == campeonato.Ronda_actual.Equipo1.j1)
                    {
                        campeonato.Ronda_actual.Partida_actual.Jugador1 = campeonato.Ronda_actual.Equipo1.j2;
                        campeonato.Ronda_actual.Partida_actual.Jugador2 = campeonato.Ronda_actual.Equipo2.j2;
                    }
                    else
                    {
                        campeonato.Ronda_actual.Partida_actual.Jugador1 = campeonato.Ronda_actual.Equipo1.j3;
                        campeonato.Ronda_actual.Partida_actual.Jugador2 = campeonato.Ronda_actual.Equipo2.j3;
                    }

                    Partida partida_temp = new Partida() { Jugador1 = campeonato.Ronda_actual.Partida_actual.Jugador1, Jugador2 = campeonato.Ronda_actual.Partida_actual.Jugador2, index = partida.index + 1 };
                }
                else //Cambio de equipos
                {
                    int index = 0, acc = 0;
                    foreach (Equipo equipo in campeonato.Ronda_actual.Equipos)
                    {
                        if (campeonato.Ronda_actual.Equipo1.Nombre_equipo == equipo.Nombre_equipo)
                            index = acc;

                        acc++;
                    }
                    //int index2 = campeonato.Ronda_actual.Equipos.FindIndex(x => x == campeonato.Ronda_actual.Equipo2);

                    campeonato.Ronda_actual.Equipos[index] = campeonato.Ronda_actual.Equipo1;
                    campeonato.Ronda_actual.Equipos[index + 1] = campeonato.Ronda_actual.Equipo2;

                    campeonato.Ronda_actual.Equipo1 = campeonato.Ronda_actual.Equipos[index + 2];
                    campeonato.Ronda_actual.Equipo2 = campeonato.Ronda_actual.Equipos[index + 3];

                    Partida partida_temp = new Partida() { Jugador1 = campeonato.Ronda_actual.Equipo1.j1, Jugador2 = campeonato.Ronda_actual.Equipo2.j1, index = partida.index + 1 };
                }

            }

            Globals.campeonato = campeonato;

            return View("Sala", campeonato.Ronda_actual);
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
                    if (usuario == partida.Jugador1)
                        tablero.Actualizar(color_opuesto, partida.Jugador2, tablero.movimientos + 1, tablero.movimientos_oponente);
                    else
                        tablero.Actualizar(color_opuesto, partida.Jugador1, tablero.movimientos, tablero.movimientos_oponente + 1);

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

        [HttpPost]
        public ActionResult JuegoTerminado(Partida partida)
        {
            Campeonato campeonato = Globals.campeonato;

            try
            {
                campeonato.Ronda_actual.Partidas.Add(partida);
            }
            catch
            {
                campeonato.Ronda_actual.Partidas = new List<Partida>();
                campeonato.Ronda_actual.Partidas.Add(partida);
            }

            campeonato.Ronda_actual.Equipo1.puntos += partida.Puntos1;
            campeonato.Ronda_actual.Equipo2.puntos += partida.Puntos2;

            if (partida.index == campeonato.Ronda_actual.Numero_partidas-1) //Es la ultima partida
            {
                if (campeonato.Ronda_actual.index == campeonato.Numero_rondas-1) //Es la ultima ronda
                {
                    //terminar juego

                }
                else //No es la ultima ronda
                {
                    campeonato.Rondas[campeonato.Ronda_actual.index] = campeonato.Ronda_actual;

                    List<Equipo> equipos_temp = new List<Equipo>();

                    for(int i = 0; i < campeonato.Ronda_actual.Numero_equipos; i += 2)
                    {
                        Equipo equipo1 = campeonato.Ronda_actual.Equipos[i];
                        Equipo equipo2 = campeonato.Ronda_actual.Equipos[i+1];

                        if (equipo1.puntos > equipo2.puntos)
                        {
                            equipos_temp.Add(equipo1);
                        }
                        else if (equipo1.puntos < equipo2.puntos)
                        {
                            equipos_temp.Add(equipo2);
                        }
                        else //empate, es posible que no vaya aquí
                        {

                        }

                        //reseteando los puntos
                        equipo1.puntos = 0;
                        equipo2.puntos = 0;
                    }

                    Partida partida_temp = new Partida() { Jugador1 = equipos_temp[0].j1, Jugador2 = equipos_temp[1].j1, index = 0 };

                    Ronda ronda_temp = new Ronda
                    {
                        Nombre_campeonato = campeonato.Nombre_campeonato,
                        index = campeonato.Ronda_actual.index+1,
                        Numero_equipos = campeonato.Ronda_actual.Numero_equipos/2,
                        Equipos = equipos_temp,
                        Equipo1 = equipos_temp[0],
                        Equipo2 = equipos_temp[1],
                        Partidas = new List<Partida>() { },
                        Partida_actual = partida_temp,
                        Numero_partidas = (campeonato.Ronda_actual.Numero_equipos / 4) * 3
                    };

                    campeonato.Ronda_actual = ronda_temp;                    
                }
            }
            else //No es la ultima partida
            {
                if (campeonato.Ronda_actual.Partida_actual.Jugador1 == campeonato.Ronda_actual.Equipo1.j1 || campeonato.Ronda_actual.Partida_actual.Jugador2 == campeonato.Ronda_actual.Equipo1.j1) //No se cambia de equipos
                {
                    if (campeonato.Ronda_actual.Partida_actual.Jugador1 == campeonato.Ronda_actual.Equipo1.j1)
                    {
                        campeonato.Ronda_actual.Partida_actual.Jugador1 = campeonato.Ronda_actual.Equipo1.j2;
                        campeonato.Ronda_actual.Partida_actual.Jugador2 = campeonato.Ronda_actual.Equipo2.j2;
                    }
                    else
                    {
                        campeonato.Ronda_actual.Partida_actual.Jugador1 = campeonato.Ronda_actual.Equipo1.j3;
                        campeonato.Ronda_actual.Partida_actual.Jugador2 = campeonato.Ronda_actual.Equipo2.j3;
                    }

                    Partida partida_temp = new Partida() { Jugador1 = campeonato.Ronda_actual.Partida_actual.Jugador1, Jugador2 = campeonato.Ronda_actual.Partida_actual.Jugador2, index = partida.index + 1 };
                }
                else //Cambio de equipos
                {
                    int index = 0, acc = 0;
                    foreach (Equipo equipo in campeonato.Ronda_actual.Equipos)
                    {
                        if (campeonato.Ronda_actual.Equipo1.Nombre_equipo == equipo.Nombre_equipo)
                            index = acc;

                        acc++;
                    }
                    //int index2 = campeonato.Ronda_actual.Equipos.FindIndex(x => x == campeonato.Ronda_actual.Equipo2);

                    campeonato.Ronda_actual.Equipos[index] = campeonato.Ronda_actual.Equipo1;
                    campeonato.Ronda_actual.Equipos[index + 1] = campeonato.Ronda_actual.Equipo2;
                
                    campeonato.Ronda_actual.Equipo1 = campeonato.Ronda_actual.Equipos[index + 2];
                    campeonato.Ronda_actual.Equipo2 = campeonato.Ronda_actual.Equipos[index + 3];
                    
                    Partida partida_temp = new Partida() { Jugador1 = campeonato.Ronda_actual.Equipo1.j1, Jugador2 = campeonato.Ronda_actual.Equipo2.j1, index = partida.index + 1 };
                }
                
            }

            Globals.campeonato = campeonato;

            return View("Sala", campeonato.Ronda_actual);
        }

    }
}