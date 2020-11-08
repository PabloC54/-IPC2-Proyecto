using IPC2_P1.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace IPC2_P1.Controllers
{
    public class TorneoController : Controller
    {
        public static string sql = "Data Source=PCP-PC;Initial Catalog=Othello_db;User ID=pabloc54;Password=pablo125";
        private SqlConnection con = new SqlConnection(sql);
        private Othello Game = new Othello();

        /*         
            string txt = "insert into Partida(modalidad, resultado, username) values ('" + tablero.modalidad + "','" + resultado + "','" + Globals.usuario_activo + "')";

            con.Open();
            SqlCommand cmd = new SqlCommand(txt, con);
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();

            dr.Close();
            con.Close();
             */

        // MENU INICIAL

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
        public ActionResult Index(string nombre_campeonato, string num_equipos, string action)
        {
            return RedirectToAction("CrearCampeonato","Torneo", new { nombre_campeonato, num_equipos });            
        }


        // CREAR EQUIPO

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


        // CAMPEONATO

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
            else //insertando el torneo a la DB
            {
                dr.Close();

                txt = "insert into Campeonato values ('" + nombre_campeonato + "'," + num_equipos + ")";

                cmd = new SqlCommand(txt, con);
                dr = cmd.ExecuteReader();
                dr.Read();

                dr.Close();
                con.Close();
            }
            

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
            string txt = "";
            SqlCommand cmd;
            SqlDataReader dr;

            foreach (Equipo equipo in campeonato.Equipos)
            {
                txt = "select username, username2, username3 from Equipo where nombre_equipo='" + equipo.Nombre_equipo + "'";
                cmd = new SqlCommand(txt, con);
                dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    equipo.j1 = dr.GetString(0);
                    equipo.j2 = dr.GetString(1);
                    equipo.j3 = dr.GetString(2);

                    dr.Close();
                }
                else
                {
                    dr.Close();
                    con.Close();

                    ViewBag.Message = "El equipo '" + equipo.Nombre_equipo + "' no ha sido registrado";
                    ViewBag.MessageType = "error-message";

                    return View(campeonato);
                }                
            }

            //registrando los equipos a registro campeonato
            foreach(Equipo equipo in campeonato.Equipos)
            {
                txt = "insert into Registro_Campeonato values ('" + equipo.Nombre_equipo + "','"+campeonato.Nombre_campeonato+"','pendiente',0)";
                
                cmd = new SqlCommand(txt, con);
                dr = cmd.ExecuteReader();
                dr.Read();
                dr.Close();
            }

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

            txt = "insert into Ronda(idRonda, numero_ronda, cantidad_equipos, nombre_campeonato) values ('"+campeonato.Nombre_campeonato+"_1',1," + ronda.Numero_equipos + ",'" + campeonato.Nombre_campeonato + "')";
            
            cmd = new SqlCommand(txt, con);
            dr = cmd.ExecuteReader();
            dr.Read();

            dr.Close();
            con.Close();

            campeonato.Ronda_actual = ronda;
            
            Globals.campeonato = campeonato;

            ViewBag.Message = "Ronda 1";
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
            Tablero tablero_temp = new Tablero(8, 8, "negro", Globals.campeonato.Ronda_actual.Partida_actual.Jugador1, Globals.campeonato.Ronda_actual.Partida_actual.Jugador2);
            tablero_temp.modalidad = "Normal";
            tablero_temp.Iniciar(false);

            ronda.Partida_actual.tablero = tablero_temp;

            Globals.campeonato.Ronda_actual = ronda;

            ViewBag.Message = Globals.campeonato.Ronda_actual.Partida_actual.Jugador1+" VS "+Globals.campeonato.Ronda_actual.Partida_actual.Jugador2;
            ViewBag.MessageType = "neutral-message";

            return View("Juego", ronda.Partida_actual);
        }

        [HttpPost]
        public ActionResult Pasar(Ronda ronda, string ganador)
        {
            Campeonato campeonato = Globals.campeonato;

            int puntos1 = 0, puntos2 = 0;
            string resultado1 = "", resultado2 = "";
            string txt = "", txt2 = "";
            SqlCommand cmd;
            SqlDataReader dr;

            if (ganador == "1")
            {
                ganador = ronda.Partida_actual.Jugador1;
                puntos1 = 40;
                puntos2 = 24;
                resultado1 = "victoria";
                resultado2 = "derrota";
            }
            else
            {
                ganador = ronda.Partida_actual.Jugador2;
                puntos1 = 24;
                puntos2 = 40;
                resultado1 = "derrota";
                resultado2 = "victoria";
            }

            Partida partida = new Partida()
            {
                Jugador1 = ronda.Partida_actual.Jugador1,
                Jugador2 = ronda.Partida_actual.Jugador2,
                Ganador = ganador,
                index = campeonato.Ronda_actual.Partidas.Count,
                Puntos1 = puntos1,
                Puntos2 = puntos2
            };

            txt = "insert into Partida(modalidad, resultado, username, idRonda) values ('Normal','" + resultado1 + "','" + partida.Jugador1 + "','"+ronda.Nombre_campeonato+"_"+(ronda.index+1)+"')";
            txt2 = "insert into Partida(modalidad, resultado, username, idRonda) values ('Normal','" + resultado2 + "','" + partida.Jugador2 + "','"+ronda.Nombre_campeonato+"_"+(ronda.index+1)+"')";
         
            con.Open();

            cmd = new SqlCommand(txt, con);
            dr = cmd.ExecuteReader();
            dr.Read();

            dr.Close();

            cmd = new SqlCommand(txt2, con);
            dr = cmd.ExecuteReader();
            dr.Read();

            dr.Close();
            con.Close();

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
                    //validar que no haya empate
                    List<Equipo> equipos_temp = new List<Equipo>()
                    {
                        campeonato.Equipos[0]
                    };

                    for(int i = 1;i < campeonato.Numero_equipos;i++)
                    {
                        Equipo equipo = campeonato.Equipos[i];

                        if(equipo.puntos < equipos_temp[0].puntos)
                        {
                            equipos_temp.Add(equipo);
                        }
                        else
                        {
                            equipos_temp.Insert(0, equipo);
                        }
                    }
                    
                    con.Open();

                    string txtt = "update Registro_Campeonato set resultado='victoria', puntos="+equipos_temp[0].puntos+" where nombre_equipo = '" + equipos_temp[0].Nombre_equipo + "'";

                    cmd = new SqlCommand(txtt, con);
                    dr = cmd.ExecuteReader();
                    dr.Read();
                    dr.Close();

                    for (int i = 1; i < campeonato.Numero_equipos; i++)
                    {
                        txtt = "update Registro_Campeonato set resultado='derrota', puntos=" + equipos_temp[i].puntos + " where nombre_equipo = '" + equipos_temp[i].Nombre_equipo + "'";

                        cmd = new SqlCommand(txtt, con);
                        dr = cmd.ExecuteReader();
                        dr.Read();
                        dr.Close();
                    }
                    
                    con.Close();

                    ViewBag.Message = "Campeonato finalizado";
                    ViewBag.MessageType = "neutral-message";

                    return View("Salon", equipos_temp);                    
                }
                else //No es la ultima ronda
                {
                    campeonato.Rondas.Add(campeonato.Ronda_actual);
                    
                    List<Equipo> equipos_temp = new List<Equipo>();

                    for (int i = 0; i < campeonato.Ronda_actual.Numero_equipos; i += 2)
                    {
                        Equipo equipo1 = campeonato.Ronda_actual.Equipos[i];
                        Equipo equipo2 = campeonato.Ronda_actual.Equipos[i + 1];

                        if (equipo1.puntos >= equipo2.puntos)
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
                    }
                    
                    foreach (Equipo equipo in equipos_temp)
                    {
                        equipo.puntos = 0;
                    }
                    
                    Partida partida_temp = new Partida()
                    {
                        Jugador1 = equipos_temp[0].j1,
                        Jugador2 = equipos_temp[1].j1,
                        Puntos1 = 0,
                        Puntos2 = 0,
                        index = 0
                    };

                    Ronda ronda_temp = new Ronda
                    {
                        Nombre_campeonato = campeonato.Nombre_campeonato,
                        index = campeonato.Ronda_actual.index + 1,
                        Numero_equipos = campeonato.Ronda_actual.Numero_equipos / 2,
                        Equipos = equipos_temp,
                        Equipo1 = equipos_temp[0],
                        Equipo2 = equipos_temp[1],
                        Partidas = new List<Partida>(),
                        Partida_actual = partida_temp,
                        Numero_partidas = (campeonato.Ronda_actual.Numero_equipos / 4) * 3
                    };

                    campeonato.Ronda_actual = ronda_temp;

                    txt = "insert into Ronda(idRonda, numero_ronda, cantidad_equipos, nombre_campeonato) values ('" + campeonato.Nombre_campeonato + "_"+ (ronda_temp.index+1) +"',"+ (ronda_temp.index+1)+"," + ronda_temp.Numero_equipos + ",'" + ronda.Nombre_campeonato + "')";
                    
                    con.Open();

                    cmd = new SqlCommand(txt, con);
                    dr = cmd.ExecuteReader();
                    dr.Read();
                    
                    dr.Close();
                    con.Close();

                    ViewBag.Message = "Ronda "+(ronda_temp.index+1);
                    ViewBag.MessageType = "neutral-message";
                }
            }
            else //No es la ultima partida
            {
                if (campeonato.Ronda_actual.Partida_actual.Jugador1 == campeonato.Ronda_actual.Equipo1.j3) //Cambio de equipos
                {
                    int index = 0, acc = 0;
                    foreach (Equipo equipo in campeonato.Ronda_actual.Equipos)
                    {
                        if (campeonato.Ronda_actual.Equipo1.Nombre_equipo == equipo.Nombre_equipo)
                            index = acc;

                        acc++;
                    }

                    campeonato.Ronda_actual.Equipos[index].puntos = campeonato.Ronda_actual.Equipo1.puntos;
                    campeonato.Ronda_actual.Equipos[index + 1].puntos = campeonato.Ronda_actual.Equipo2.puntos;

                    campeonato.Ronda_actual.Equipo1 = campeonato.Ronda_actual.Equipos[index + 2];
                    campeonato.Ronda_actual.Equipo2 = campeonato.Ronda_actual.Equipos[index + 3];

                    campeonato.Ronda_actual.Partida_actual = new Partida()
                    {
                        Jugador1 = campeonato.Ronda_actual.Equipo1.j1,
                        Jugador2 = campeonato.Ronda_actual.Equipo2.j1,
                        Puntos1 = 0,
                        Puntos2 = 0,
                        index = partida.index + 1
                    };
                }
                else //No se cambia de equipos
                {
                    if (campeonato.Ronda_actual.Partida_actual.Jugador1 == campeonato.Ronda_actual.Equipo1.j1)
                    {
                        campeonato.Ronda_actual.Partida_actual = new Partida()
                        {
                            Jugador1 = campeonato.Ronda_actual.Equipo1.j2,
                            Jugador2 = campeonato.Ronda_actual.Equipo2.j2,
                            Puntos1 = 0,
                            Puntos2 = 0,
                            index = partida.index + 1
                        };
                    }
                    else
                    {
                        campeonato.Ronda_actual.Partida_actual = new Partida()
                        {
                            Jugador1 = campeonato.Ronda_actual.Equipo1.j3,
                            Jugador2 = campeonato.Ronda_actual.Equipo2.j3,
                            Puntos1 = 0,
                            Puntos2 = 0,
                            index = partida.index + 1
                        };
                    }
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

            /*APERTURA PERSONALIZADA
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
            */

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
            
            string resultado1 = "", resultado2 = "";
            string txt = "", txt2 = "";
            SqlCommand cmd;
            SqlDataReader dr;
            
            txt = "insert into Partida(modalidad, resultado, username, idRonda) values ('Normal','" + resultado1 + "','" + partida.Jugador1 + "','" + campeonato.Nombre_campeonato + "_" + (campeonato.Ronda_actual.index + 1) + "')";
            txt2 = "insert into Partida(modalidad, resultado, username, idRonda) values ('Normal','" + resultado2 + "','" + partida.Jugador2 + "','" + campeonato.Ronda_actual.Nombre_campeonato + "_" + (campeonato.Ronda_actual.index + 1) + "')";

            con.Open();

            cmd = new SqlCommand(txt, con);
            dr = cmd.ExecuteReader();
            dr.Read();

            dr.Close();

            cmd = new SqlCommand(txt2, con);
            dr = cmd.ExecuteReader();
            dr.Read();

            dr.Close();
            con.Close();

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
                    //validar que no haya empate
                    List<Equipo> equipos_temp = new List<Equipo>()
                    {
                        campeonato.Equipos[0]
                    };

                    for (int i = 1; i < campeonato.Numero_equipos; i++)
                    {
                        Equipo equipo = campeonato.Equipos[i];

                        if (equipo.puntos < equipos_temp[0].puntos)
                        {
                            equipos_temp.Add(equipo);
                        }
                        else
                        {
                            equipos_temp.Insert(0, equipo);
                        }
                    }

                    con.Open();

                    string txtt = "update Registro_Campeonato set resultado='victoria', puntos=" + equipos_temp[0].puntos + " where nombre_equipo = '" + equipos_temp[0].Nombre_equipo + "'";

                    cmd = new SqlCommand(txtt, con);
                    dr = cmd.ExecuteReader();
                    dr.Read();
                    dr.Close();

                    for (int i = 1; i < campeonato.Numero_equipos; i++)
                    {
                        txtt = "update Registro_Campeonato set resultado='derrota', puntos=" + equipos_temp[i].puntos + " where nombre_equipo = '" + equipos_temp[i].Nombre_equipo + "'";

                        cmd = new SqlCommand(txtt, con);
                        dr = cmd.ExecuteReader();
                        dr.Read();
                        dr.Close();
                    }

                    con.Close();

                    ViewBag.Message = "Campeonato finalizado";
                    ViewBag.MessageType = "neutral-message";

                    return View("Salon", equipos_temp);
                }
                else //No es la ultima ronda
                {
                    campeonato.Rondas.Add(campeonato.Ronda_actual);

                    List<Equipo> equipos_temp = new List<Equipo>();

                    for (int i = 0; i < campeonato.Ronda_actual.Numero_equipos; i += 2)
                    {
                        Equipo equipo1 = campeonato.Ronda_actual.Equipos[i];
                        Equipo equipo2 = campeonato.Ronda_actual.Equipos[i + 1];

                        if (equipo1.puntos >= equipo2.puntos)
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
                    }

                    foreach (Equipo equipo in equipos_temp)
                    {
                        equipo.puntos = 0;
                    }

                    Partida partida_temp = new Partida()
                    {
                        Jugador1 = equipos_temp[0].j1,
                        Jugador2 = equipos_temp[1].j1,
                        Puntos1 = 0,
                        Puntos2 = 0,
                        index = 0
                    };

                    Ronda ronda_temp = new Ronda
                    {
                        Nombre_campeonato = campeonato.Nombre_campeonato,
                        index = campeonato.Ronda_actual.index + 1,
                        Numero_equipos = campeonato.Ronda_actual.Numero_equipos / 2,
                        Equipos = equipos_temp,
                        Equipo1 = equipos_temp[0],
                        Equipo2 = equipos_temp[1],
                        Partidas = new List<Partida>(),
                        Partida_actual = partida_temp,
                        Numero_partidas = (campeonato.Ronda_actual.Numero_equipos / 4) * 3
                    };

                    campeonato.Ronda_actual = ronda_temp;

                    txt = "insert into Ronda(idRonda, numero_ronda, cantidad_equipos, nombre_campeonato) values ('" + campeonato.Nombre_campeonato + "_" + (ronda_temp.index + 1) + "'," + (ronda_temp.index + 1) + "," + ronda_temp.Numero_equipos + ",'" + campeonato.Nombre_campeonato + "')";

                    con.Open();

                    cmd = new SqlCommand(txt, con);
                    dr = cmd.ExecuteReader();
                    dr.Read();

                    dr.Close();
                    con.Close();

                    ViewBag.Message = "Ronda " + (ronda_temp.index + 1);
                    ViewBag.MessageType = "neutral-message";
                }
            }
            else //No es la ultima partida
            {
                if (campeonato.Ronda_actual.Partida_actual.Jugador1 == campeonato.Ronda_actual.Equipo1.j3) //Cambio de equipos
                {
                    int index = 0, acc = 0;
                    foreach (Equipo equipo in campeonato.Ronda_actual.Equipos)
                    {
                        if (campeonato.Ronda_actual.Equipo1.Nombre_equipo == equipo.Nombre_equipo)
                            index = acc;

                        acc++;
                    }

                    campeonato.Ronda_actual.Equipos[index].puntos = campeonato.Ronda_actual.Equipo1.puntos;
                    campeonato.Ronda_actual.Equipos[index + 1].puntos = campeonato.Ronda_actual.Equipo2.puntos;

                    campeonato.Ronda_actual.Equipo1 = campeonato.Ronda_actual.Equipos[index + 2];
                    campeonato.Ronda_actual.Equipo2 = campeonato.Ronda_actual.Equipos[index + 3];

                    campeonato.Ronda_actual.Partida_actual = new Partida()
                    {
                        Jugador1 = campeonato.Ronda_actual.Equipo1.j1,
                        Jugador2 = campeonato.Ronda_actual.Equipo2.j1,
                        Puntos1 = 0,
                        Puntos2 = 0,
                        index = partida.index + 1
                    };
                }
                else //No se cambia de equipos
                {
                    if (campeonato.Ronda_actual.Partida_actual.Jugador1 == campeonato.Ronda_actual.Equipo1.j1)
                    {
                        campeonato.Ronda_actual.Partida_actual = new Partida()
                        {
                            Jugador1 = campeonato.Ronda_actual.Equipo1.j2,
                            Jugador2 = campeonato.Ronda_actual.Equipo2.j2,
                            Puntos1 = 0,
                            Puntos2 = 0,
                            index = partida.index + 1
                        };
                    }
                    else
                    {
                        campeonato.Ronda_actual.Partida_actual = new Partida()
                        {
                            Jugador1 = campeonato.Ronda_actual.Equipo1.j3,
                            Jugador2 = campeonato.Ronda_actual.Equipo2.j3,
                            Puntos1 = 0,
                            Puntos2 = 0,
                            index = partida.index + 1
                        };
                    }
                }
            }

            Globals.campeonato = campeonato;

            return View("Sala", campeonato.Ronda_actual);
        }

        
        // CARGAR CAMPEONATO

        [HttpPost]
        public ActionResult CargarCampeonato(HttpPostedFileBase archivo)
        {
            if (archivo != null)
            {
                string result = "";
                using (BinaryReader b = new BinaryReader(archivo.InputStream))   // FUENTE: https://stackoverflow.com/questions/16030034/asp-net-mvc-read-file-from-httppostedfilebase-without-save/16030326
                {
                    byte[] binData = b.ReadBytes(archivo.ContentLength);
                    result = System.Text.Encoding.UTF8.GetString(binData);
                }

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(result);
                
          /*      try
                {*/
                    string nombre_temp = doc.SelectNodes("campeonato")[0].SelectNodes("nombre")[0].InnerText;

                    List<Equipo> equipos_temp = new List<Equipo>();

                    foreach (XmlNode nodo in doc.SelectNodes("campeonato")[0].SelectNodes("equipo"))
                    {
                        equipos_temp.Add(new Equipo()
                        {
                            Nombre_equipo = nodo.SelectNodes("nombreEquipo")[0].InnerText,
                            j1 = nodo.SelectNodes("jugador")[0].InnerText,
                            j2 = nodo.SelectNodes("jugador")[1].InnerText,
                            j3 = nodo.SelectNodes("jugador")[2].InnerText
                        });
                    }

                    int temp = 0;

                    if (equipos_temp.Count == 4)
                    {
                        temp = 2;
                    }
                    else if (equipos_temp.Count == 8)
                    {
                        temp = 3;
                    } 
                    else if (equipos_temp.Count == 16)
                    {
                        temp = 4;
                    }
                    else
                    {
                        string message = "El tamaño del campeonato cargado no es válido";
                        string messagetype = "error-message";

                        return RedirectToAction("Index", new { message, messagetype });
                    }

                    Campeonato campeonato = new Campeonato() {
                        Nombre_campeonato = nombre_temp,
                        Equipos = equipos_temp,
                        Numero_equipos = equipos_temp.Count,
                        Rondas=new List<Ronda>(),
                        Numero_rondas=temp
                    };

                    con.Open();
                
                    string txt = "";
                    SqlCommand cmd;
                    SqlDataReader dr;

                txt = "insert into Campeonato values ('" + campeonato.Nombre_campeonato + "'," + campeonato.Numero_equipos + ")";

                cmd = new SqlCommand(txt, con);

                try
                {
                    dr = cmd.ExecuteReader();
                    dr.Read();
                    dr.Close();
                }
                catch{}


                //validando si los equipos existen
                foreach (Equipo equipo in campeonato.Equipos)
                    {
                        txt = "select * from Equipo where nombre_equipo='" + equipo.Nombre_equipo + "'";
                        cmd = new SqlCommand(txt, con);
                                                
                        dr = cmd.ExecuteReader();
                        

                        if (!dr.Read())
                        {
                            dr.Close();

                            List<string> jugadores = new List<string> { equipo.j1, equipo.j2, equipo.j3 };
                            
                            //validando que los usuarios estén registrados
                            foreach (string jugador in jugadores)
                            {
                                txt = "select * from Usuario where username='" + jugador + "'";

                                cmd = new SqlCommand(txt, con);
                                dr = cmd.ExecuteReader();

                                if (!dr.Read())
                                {
                                    dr.Close();
                                    
                                    string message = "El usuario " + jugador + " no está registrado";
                                    string messagetype = "error-message";

                                    return RedirectToAction("Index", new { message, messagetype });
                                }

                                dr.Close();
                            }

                            txt = "insert into Equipo values ('" + equipo.Nombre_equipo + "','" + equipo.j1 + "','" + equipo.j2 + "','" + equipo.j3 + "')";
                            cmd = new SqlCommand(txt, con);
                            dr = cmd.ExecuteReader();

                            dr.Read();
                        }

                    dr.Close();
                }

                    //registrando los equipos a registro campeonato
                    foreach (Equipo equipo in campeonato.Equipos)
                    {
                        txt = "insert into Registro_Campeonato values ('" + equipo.Nombre_equipo + "','" + campeonato.Nombre_campeonato + "','pendiente',0)";

                        cmd = new SqlCommand(txt, con);
                        dr = cmd.ExecuteReader();
                        dr.Read();
                        dr.Close();
                    }


                campeonato.Rondas = new List<Ronda>();

                Partida partida = new Partida() { Jugador1 = campeonato.Equipos[0].j1, Jugador2 = campeonato.Equipos[1].j1, index = 0 };
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
                    Numero_partidas = (campeonato.Numero_equipos / 2) * 3
                };

                txt = "insert into Ronda(idRonda, numero_ronda, cantidad_equipos, nombre_campeonato) values ('" + campeonato.Nombre_campeonato + "_1',1," + ronda.Numero_equipos + ",'" + campeonato.Nombre_campeonato + "')";

                cmd = new SqlCommand(txt, con);

                try
                {
                    dr = cmd.ExecuteReader();
                    dr.Read();

                    dr.Close();
                }
                catch { }
                con.Close();

                campeonato.Ronda_actual = ronda;


                Globals.campeonato = campeonato;

                    ViewBag.Message = "Campeonato de "+ equipos_temp.Count +" equipos cargado con éxito";
                    ViewBag.MessageType = "success-message";

                    return View("Sala", campeonato.Ronda_actual);
             /*   }
                catch
                {
                    string message = "El archivo cargado no es válido";
                    string messagetype = "error-message";

                    return RedirectToAction("Index", new { message, messagetype });
                }*/
            }
            else //no se cargó un archivo
            {
                string message = "No se cargó un archivo";
                 string messagetype = "error-message";

                return RedirectToAction("Index", new { message, messagetype });
            }
        }

    }
}