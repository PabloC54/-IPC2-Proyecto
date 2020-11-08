using IPC2_P1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IPC2_P1.Controllers
{
    // IMPRIMIR EN CONSOLA
    // System.Diagnostics.Debug.WriteLine("PRUEBA: ");
    public class GameController : Controller
    {
        private Conversor Conversor = new Conversor();
        private Othello Juego = new Othello();

        // I N I C I O

        public ActionResult Index(string go_to, string ficha_inicial)
        {
            if (Globals.logged_in == true)
            {
                string[] temp = { go_to, ficha_inicial };
                return View(temp);
            }
            else
            {
                string message = "Primero debes iniciar sesión";
                string messagetype = "error-message";

                return RedirectToAction("Login", "Home", new { message, messagetype });
            }
        }

        public ActionResult XIndex()
        {
            if (Globals.logged_in == true)
            {
                return View();
            }
            else
            {
                string message = "Primero debes iniciar sesión";
                string messagetype = "error-message";

                return RedirectToAction("Login", "Home", new { message, messagetype });
            }
        }

        public ActionResult Torneo()
        {
            return View();
        }

        [HttpPost]
        public RedirectToRouteResult Index(string go_to, string ficha_inicial, string apertura, string modalidad)
        {
            return RedirectToAction(go_to, new { ficha_inicial, apertura, modalidad });
        }

        [HttpPost]
        public ActionResult XIndex(string juego,string apertura, string modalidad, int filas, int columnas, List<string> colores, List<string> colores2)
        {
            try
            {
                string colores_temp = "", colores2_temp = "";

                foreach(string color in colores)
                {
                    colores_temp += color+",";
                }

                foreach (string color in colores2)
                {
                    colores2_temp += color + ",";
                }

                return RedirectToAction("Xtream", new { juego,apertura, modalidad, filas, columnas, colores_temp, colores2_temp });
             
            }
            catch
            {
                ViewBag.Message = "Se debe seleccionar mínimo un color por jugador";
                ViewBag.MessageType = "error-message";
                return View();
            }
        }


        // SOLO, VERSUS, XTREME, TORNEO

        public ActionResult Solo(string ficha_inicial, string apertura, string modalidad)
        {
            if (Globals.logged_in == true)
            {
                Tablero tablero;
                bool temp = false;

                if (apertura == "Personalizada")
                {
                    temp = true;

                }

                if (ficha_inicial == "blanco")
                {
                    tablero = new Tablero(8, 8, "blanco", Globals.usuario_activo, modalidad);
                    tablero.Iniciar(temp);

                    ViewBag.MessageType = "neutral-message";
                    if (tablero.apertura_personalizada == "true")
                    {
                        ViewBag.Message = "Apertura personalizada en progreso, faltan 3 fichas por colocar";
                        Juego.Movimiento_cpu_apertura(tablero, "negro");
                    }
                    else
                    {
                        ViewBag.Message = "Apertura personalizada en progreso, faltan 4 fichas por colocar";
                        Juego.Movimiento_cpu(tablero, "negro", "blanco");
                    }

                }
                else
                {
                    if (ficha_inicial == "negro")
                    {
                        tablero = new Tablero(8, 8, "negro", Globals.usuario_activo, modalidad);
                        tablero.Iniciar(temp);
                    }
                    else
                    {
                        Random random = new Random();
                        double num = random.NextDouble();

                        if (num < 0.5)
                        {
                            tablero = new Tablero(8, 8, "negro", Globals.usuario_activo, modalidad);
                            tablero.Iniciar(temp);
                        }
                        else
                        {
                            tablero = new Tablero(8, 8, "blanco", Globals.usuario_activo, modalidad);
                            tablero.Iniciar(temp);

                            ViewBag.MessageType = "neutral-message";
                            if (tablero.apertura_personalizada == "true")
                            {
                                ViewBag.Message = "Apertura personalizada en progreso, faltan 3 fichas por colocar";
                                Juego.Movimiento_cpu_apertura(tablero, "negro");
                            }
                            else
                            {
                                ViewBag.Message = "Apertura personalizada en progreso, faltan 4 fichas por colocar";
                                Juego.Movimiento_cpu(tablero, "negro", "blanco");
                            }
                        }
                    }
                }

                return View(tablero);
            }
            else
            {
                string message = "Primero debes iniciar sesión";
                string messagetype = "error-message";

                return RedirectToAction("Login", "Home", new { message, messagetype });
            }
        }

        public ActionResult Versus(string ficha_inicial, string apertura, string modalidad)
        {
            if (Globals.logged_in == true)
            {
                Tablero tablero;
                bool temp = false;

                if (apertura == "Personalizada")
                {
                    temp = true;
                    ViewBag.MessageType = "neutral-message";
                    ViewBag.Message = "Apertura personalizada en progreso, faltan 4 fichas por colocar";

                }

                if (ficha_inicial == "blanco")
                {
                    tablero = new Tablero(8, 8, "negro", "Oponente", modalidad);
                    tablero.Iniciar(temp);
                }
                else
                {
                    if (ficha_inicial == "negro")
                    {
                        tablero = new Tablero(8, 8, "negro", Globals.usuario_activo, modalidad);
                        tablero.Iniciar(temp);
                    }
                    else
                    {
                        Random random = new Random();
                        double num = random.NextDouble();

                        if (num < 0.5)
                        {
                            tablero = new Tablero(8, 8, "negro", Globals.usuario_activo, modalidad);
                            tablero.Iniciar(temp);
                        }
                        else
                        {
                            tablero = new Tablero(8, 8, "negro", "Oponente", modalidad);
                            tablero.Iniciar(temp);
                        }
                    }
                }

                return View(tablero);
            }
            else
            {
                string message = "Primero debes iniciar sesión";
                string messagetype = "error-message";

                return RedirectToAction("Login", "Home", new { message, messagetype });
            }
        }

        public ActionResult Xtream(string juego, string apertura, string modalidad, int filas, int columnas, string colores_temp, string colores2_temp)
        {
            if (Globals.logged_in == true)
            {
                Tablero tablero;
                bool temp = false;
                
                if (apertura == "Personalizada")
                {
                    temp = true;
                    ViewBag.Message = "Apertura personalizada en progreso, faltan 4 fichas por colocar";
                    ViewBag.MessageType = "neutral-message";
                }

                List<string> colores = colores_temp.Split(',').ToList();
                colores.Remove("");
                List<string> colores2 = colores2_temp.Split(',').ToList();
                colores2.Remove("");
                
                tablero = new Tablero(filas, columnas, colores[0], Globals.usuario_activo, modalidad);
                
                tablero.juego = juego;
                tablero.colores = colores;
                tablero.colores_oponente = colores2;
                tablero.colores_index = 0;
                tablero.colores_oponente_index = 0;
                tablero.XIniciar(temp);

                return View(tablero);
            }
            else
            {
                string message = "Primero debes iniciar sesión";
                string messagetype = "error-message";

                return RedirectToAction("Login", "Home", new { message, messagetype });
            }
        }


        //LÓGICA DEL TABLERO

        [HttpPost]
        public ActionResult Solo(Tablero tablero)
        {
            string usuario = tablero.usuario, usuario_opuesto = "";

            if (usuario == Globals.usuario_activo)
                usuario_opuesto = "Oponente";
            else
                usuario_opuesto = Globals.usuario_activo;


            string color = tablero.color, color_opuesto = "";

            if (color == "blanco")
                color_opuesto = "negro";
            else
                color_opuesto = "blanco";

            
            int index = Juego.Ficha_seleccionada(tablero);

            //APERTURA PERSONALIZADA
            if (tablero.apertura_personalizada == "true")
            {
                if (tablero.Apertura(index))
                {
                    Juego.Reemplazar(tablero, index, color);

                    if (Juego.Celdas_ocupadas(tablero).Count == 4)
                    {
                        ViewBag.MessageType = "success-message";
                        ViewBag.Message = "¡Apertura personalizada finalizada!";
                        tablero.apertura_personalizada = "false";
                    }
                    else
                    {
                        Juego.Movimiento_cpu_apertura(tablero, color_opuesto);

                        if (Juego.Celdas_ocupadas(tablero).Count == 4)
                        {
                            ViewBag.MessageType = "success-message";
                            ViewBag.Message = "¡Apertura personalizada finalizada!";
                            tablero.apertura_personalizada = "false";                  
                        }
                        else
                        {
                            ViewBag.MessageType = "neutral-message";
                            ViewBag.Message = "Apertura personalizada en progreso, faltan " + (4 - Juego.Celdas_ocupadas(tablero).Count) + " fichas para finalizar";
                        }
                        
                    }

                    return View(tablero);
                }
                else
                {
                    ViewBag.MessageType = "error-message";
                    ViewBag.Message = "¡Posición de apertura no válida!";
                    Juego.Reemplazar(tablero, index, "");

                    return View(tablero);
                }
            }
           
            List<int> lista = Juego.Flanquear(tablero, index, color, color_opuesto);

            if (lista.Count > 0)
            {
                Juego.Reemplazar_lista(tablero, lista, color);
                    
                List<int> celdas_validas = Juego.Celdas_validas(tablero, color_opuesto, color);

                if (celdas_validas.Count > 0)
                {
                    //EJECUTANDO CAMBIOS DEL CPU    
                    Juego.Movimiento_cpu(tablero, color_opuesto, color);
                    tablero.Actualizar(color, usuario, tablero.movimientos + 1, tablero.movimientos_oponente + 1);

                    celdas_validas = Juego.Celdas_validas(tablero, color, color_opuesto);

                    bool salir=true;
                    if (celdas_validas.Count == 0)
                    {
                        salir = false;
                    }

                    while (salir == false)
                    {
                        celdas_validas = Juego.Celdas_validas(tablero, color_opuesto, color);

                        if (celdas_validas.Count > 0)
                        {
                            //EJECUTANDO CAMBIOS DEL CPU
                            Juego.Movimiento_cpu(tablero, color_opuesto, color);
                            tablero.Actualizar(color, usuario, tablero.movimientos, tablero.movimientos_oponente + 1);

                            celdas_validas = Juego.Celdas_validas(tablero, color, color_opuesto);
                                
                            if (celdas_validas.Count > 0)
                            {
                                salir = true;
                            }
                        }
                        else
                        {
                            //JUEGO TERMINADO
                            ViewBag.MessageType = "success-message";
                            ViewBag.Message = Juego.Juego_terminado(tablero, color_opuesto);
                            return View(tablero);
                        }
                    }

                    return View(tablero);

                }
                else
                {
                    celdas_validas = Juego.Celdas_validas(tablero, color, color_opuesto);

                    if (celdas_validas.Count > 0)
                    {
                        ViewBag.Message = usuario_opuesto + " no tiene movimientos válidos";
                        ViewBag.MessageType = "error-message";

                            return View(tablero);
                    }
                    else
                    {
                        //JUEGO TERMINADO
                        ViewBag.MessageType = "success-message";
                        ViewBag.Message = Juego.Juego_terminado(tablero, color_opuesto);
                        return View(tablero);
                    }                            
                }                
            }
            else
            {
                Juego.Reemplazar(tablero, index, "");
                return View(tablero);
            }            
        }

        [HttpPost]
        public ActionResult Versus(Tablero tablero)
        {
            string usuario = tablero.usuario, usuario_opuesto = "";

            if (usuario == Globals.usuario_activo)
                usuario_opuesto = "Oponente";
            else
                usuario_opuesto = Globals.usuario_activo;


            string color = tablero.color, color_opuesto = "";

            if (color == "blanco")
                color_opuesto = "negro";
            else
                color_opuesto = "blanco";
            

            int index = Juego.Ficha_seleccionada(tablero);

            //APERTURA PERSONALIZADA
            if (tablero.apertura_personalizada == "true")
            {
                if (tablero.Apertura(index))
                {
                    Juego.Reemplazar(tablero, index, color);
                    tablero.Actualizar(color_opuesto, usuario_opuesto, 0, 0);

                    if (Juego.Celdas_ocupadas(tablero).Count == 4)
                    {
                        ViewBag.MessageType = "success-message";
                        ViewBag.Message = "¡Apertura personalizada finalizada!";
                        tablero.apertura_personalizada = "false";
                    }
                    else
                    {
                        ViewBag.MessageType = "neutral-message";
                        ViewBag.Message = "Apertura personalizada en progreso, faltan " + (4 - Juego.Celdas_ocupadas(tablero).Count) + " fichas para finalizar";
                    }

                    return View(tablero);
                }
                else
                {
                    ViewBag.MessageType = "error-message";
                    ViewBag.Message = "¡Posición de apertura no válida!";
                    Juego.Reemplazar(tablero, index, "");

                    return View(tablero);
                }
            }
            
            List<int> lista = Juego.Flanquear(tablero, index, color, color_opuesto);

            if (lista.Count > 0)
            {
                Juego.Reemplazar_lista(tablero, lista, color);
                    
                List<int> celdas_validas = Juego.Celdas_validas(tablero, color_opuesto, color);

                if (celdas_validas.Count > 0)
                {
                    if (usuario == "Oponente")
                        tablero.Actualizar(color_opuesto, Globals.usuario_activo, tablero.movimientos, tablero.movimientos_oponente + 1);
                    else
                        tablero.Actualizar(color_opuesto, "Oponente", tablero.movimientos + 1, tablero.movimientos_oponente);

                    return View(tablero);
                }
                else
                {      
                    celdas_validas = Juego.Celdas_validas(tablero, color, color_opuesto);                            

                    if (celdas_validas.Count > 0)
                    {

                        ViewBag.Message = usuario_opuesto + " no tiene movimientos válidos";
                        ViewBag.MessageType = "error-message";

                        return View(tablero);

                    }
                    else
                    {
                        //JUEGO TERMINADO
                        ViewBag.MessageType = "success-message";
                        ViewBag.Message = Juego.Juego_terminado(tablero, color_opuesto);
                        return View(tablero);
                    }
                }
            }
            else
            {
                Juego.Reemplazar(tablero, index, "");
                return View(tablero);
            }            
        }
        
        [HttpPost]
        public ActionResult Xtream(Tablero tablero)
        {
            if (tablero.juego == "Versus")
            {
                string usuario = tablero.usuario, usuario_opuesto = "";

                if (usuario == Globals.usuario_activo)
                    usuario_opuesto = "Oponente";
                else
                    usuario_opuesto = Globals.usuario_activo;


                string color = tablero.color;

                int index = Juego.Ficha_seleccionada(tablero);

                //APERTURA PERSONALIZADA
                if (tablero.apertura_personalizada == "true")
                {
                    if (tablero.Apertura(index))
                    {
                        Juego.Reemplazar(tablero, index, color);
                        tablero.XActualizar(usuario_opuesto, 0, 0);

                        if (Juego.Celdas_ocupadas(tablero).Count == 4)
                        {
                            ViewBag.MessageType = "success-message";
                            ViewBag.Message = "¡Apertura personalizada finalizada!";
                            tablero.apertura_personalizada = "false";
                        }
                        else
                        {
                            ViewBag.MessageType = "neutral-message";
                            ViewBag.Message = "Apertura personalizada en progreso, faltan " + (4 - Juego.Celdas_ocupadas(tablero).Count) + " fichas para finalizar";
                        }

                        return View(tablero);
                    }
                    else
                    {
                        ViewBag.MessageType = "error-message";
                        ViewBag.Message = "¡Posición de apertura no válida!";
                        Juego.Reemplazar(tablero, index, "");

                        return View(tablero);
                    }
                }

                List<int> lista = Juego.XFlanquear(tablero, index, color); //ARREGLAR

                if (lista.Count > 0)
                {
                    Juego.Reemplazar_lista(tablero, lista, color);

                    List<int> celdas_validas;

                    if (usuario == "Oponente")
                    {
                        tablero.XActualizar(Globals.usuario_activo, tablero.movimientos, tablero.movimientos_oponente + 1);
                        celdas_validas = Juego.XCeldas_validas(tablero);
                    }
                    else
                    {
                        tablero.XActualizar("Oponente", tablero.movimientos + 1, tablero.movimientos_oponente);
                        celdas_validas = Juego.XCeldas_validas(tablero);
                    }


                    if (celdas_validas.Count > 0)
                    {
                        return View(tablero);
                    }
                    else
                    {
                        if (tablero.usuario == "Oponente")
                        {
                            tablero.XActualizar(Globals.usuario_activo, tablero.movimientos, tablero.movimientos_oponente);
                            celdas_validas = Juego.XCeldas_validas(tablero);
                        }
                        else
                        {
                            tablero.XActualizar("Oponente", tablero.movimientos, tablero.movimientos_oponente);
                            celdas_validas = Juego.XCeldas_validas(tablero);
                        }

                        if (celdas_validas.Count > 0)
                        {

                            ViewBag.Message = usuario_opuesto + " no tiene movimientos válidos";
                            ViewBag.MessageType = "error-message";

                            return View(tablero);

                        }
                        else
                        {
                            //JUEGO TERMINADO
                            ViewBag.MessageType = "success-message";
                            ViewBag.Message = Juego.XJuego_terminado(tablero);
                            return View(tablero);
                        }
                    }
                }
                else
                {
                    Juego.Reemplazar(tablero, index, "");
                    return View(tablero);
                }
            }
            else //SOLO
            {
                string usuario = tablero.usuario, usuario_opuesto = "";

                if (usuario == Globals.usuario_activo)
                    usuario_opuesto = "Oponente";
                else
                    usuario_opuesto = Globals.usuario_activo;


                string color = tablero.color;               


                int index = Juego.Ficha_seleccionada(tablero);

                //APERTURA PERSONALIZADA
                if (tablero.apertura_personalizada == "true")
                {
                    if (tablero.Apertura(index))
                    {
                        Juego.Reemplazar(tablero, index, color);
                        tablero.XActualizar(usuario_opuesto, tablero.movimientos + 1, tablero.movimientos_oponente);

                        if (Juego.Celdas_ocupadas(tablero).Count == 4)
                        {
                            ViewBag.MessageType = "success-message";
                            ViewBag.Message = "¡Apertura personalizada finalizada!";
                            tablero.apertura_personalizada = "false";
                        }
                        else
                        {
                            Juego.Movimiento_cpu_apertura(tablero,tablero.color);
                            tablero.XActualizar(usuario, tablero.movimientos, tablero.movimientos_oponente + 1);

                            if (Juego.Celdas_ocupadas(tablero).Count == 4)
                            {
                                ViewBag.MessageType = "success-message";
                                ViewBag.Message = "¡Apertura personalizada finalizada!";
                                tablero.apertura_personalizada = "false";
                            }
                            else
                            {
                                ViewBag.MessageType = "neutral-message";
                                ViewBag.Message = "Apertura personalizada en progreso, faltan " + (4 - Juego.Celdas_ocupadas(tablero).Count) + " fichas para finalizar";
                            }
                        }

                        return View(tablero);
                    }
                    else
                    {
                        ViewBag.MessageType = "error-message";
                        ViewBag.Message = "¡Posición de apertura no válida!";
                        Juego.Reemplazar(tablero, index, "");

                        return View(tablero);
                    }
                }

                List<int> lista = Juego.XFlanquear(tablero, index, color);

                if (lista.Count > 0)
                {
                    Juego.Reemplazar_lista(tablero, lista, color);

                    tablero.XActualizar(usuario_opuesto, tablero.movimientos + 1, tablero.movimientos_oponente);
                    List<int> celdas_validas = Juego.XCeldas_validas(tablero);

                    if (celdas_validas.Count > 0)
                    {
                        //EJECUTANDO CAMBIOS DEL CPU    
                        Juego.XMovimiento_cpu(tablero);

                        tablero.XActualizar(usuario, tablero.movimientos, tablero.movimientos_oponente + 1);
                        celdas_validas = Juego.XCeldas_validas(tablero);

                        if (celdas_validas.Count > 0)
                        {
                            return View(tablero);
                        }
                        else
                        {
                            tablero.XActualizar(usuario_opuesto, tablero.movimientos, tablero.movimientos_oponente);

                            bool salir = false;
                            while (salir == false)
                            {
                                Juego.XMovimiento_cpu(tablero);
                                tablero.XActualizar(usuario_opuesto, tablero.movimientos, tablero.movimientos_oponente);
                                tablero.XActualizar(usuario_opuesto, tablero.movimientos, tablero.movimientos_oponente + 1);

                                celdas_validas = Juego.XCeldas_validas(tablero);

                                if (celdas_validas.Count > 0)
                                {
                                    salir = true;
                                }

                            }

                            return View(tablero);
                        }                        
                    }
                    else
                    {
                        tablero.XActualizar(usuario, tablero.movimientos, tablero.movimientos_oponente);
                        celdas_validas = Juego.XCeldas_validas(tablero);

                        if (celdas_validas.Count > 0)
                        {
                            ViewBag.Message = usuario_opuesto + " no tiene movimientos válidos";
                            ViewBag.MessageType = "error-message";

                            return View(tablero);
                        }
                        else
                        {
                            //JUEGO TERMINADO
                            ViewBag.MessageType = "success-message";
                            ViewBag.Message = Juego.XJuego_terminado(tablero);
                            return View(tablero);
                        }
                    }
                }
                else
                {
                    Juego.Reemplazar(tablero, index, "");
                    return View(tablero);
                }
            }
        }

        
        //CARGAR TABLERO

        [HttpPost]
        public ActionResult Cargar(HttpPostedFileBase archivo, string action)
        {
            if (archivo != null)
            {
                Tablero tablero;
                if (action == "Xtream")
                {
                    tablero = Conversor.XReadXML(archivo);

                    string usuario = tablero.usuario, usuario_opuesto = "";

                    if (usuario == Globals.usuario_activo)
                        usuario_opuesto = "Oponente";
                    else
                        usuario_opuesto = Globals.usuario_activo;

                    string color = tablero.color;
                    if (!tablero.colores.Contains(color) && !tablero.colores_oponente.Contains(color))
                    {
                        Tablero tablero_temp = new Tablero(8, 8, "blanco", Globals.usuario_activo, "Normal");
                        tablero_temp.colores = new List<string> { "blanco", "rojo" };
                        tablero_temp.colores_oponente = new List<string> { "negro", "violeta" };
                        tablero_temp.XIniciar(false);

                        ViewBag.Message = "Tablero cargado no válido";
                        ViewBag.MessageType = "error-message";

                        return View(action,tablero_temp);
                    }
                    
                    List<int> celdas_validas = Juego.XCeldas_validas(tablero);

                    if (celdas_validas.Count > 0)
                    {
                        ViewBag.Message = "Tablero cargado con éxito";
                        ViewBag.MessageType = "success-message";

                        return View(action,tablero);
                    }
                    else
                    {
                        if (usuario == "Oponente")
                        {
                            tablero.XActualizar(Globals.usuario_activo, tablero.movimientos, tablero.movimientos_oponente + 1);
                            celdas_validas = Juego.XCeldas_validas(tablero);
                        }
                        else
                        {
                            tablero.XActualizar("Oponente", tablero.movimientos + 1, tablero.movimientos_oponente);
                            celdas_validas = Juego.XCeldas_validas(tablero);
                        }

                        if (celdas_validas.Count > 0)
                        {
                            ViewBag.Message = usuario + " no tiene movimientos válidos";
                            ViewBag.MessageType = "error-message";

                            return View(action,tablero);
                        }
                        else
                        {
                            //JUEGO TERMINADO
                            ViewBag.Message = Juego.Juego_terminado(tablero, color);
                            ViewBag.MessageType = "success-message";

                            return View(action,tablero);
                        }
                    }
                }
                else
                {
                    tablero = Conversor.ReadXML(archivo);

                    string color = tablero.color, color_opuesto="";
                                                         
                    if (color == "blanco")
                        color_opuesto = "negro";
                    else                    
                        color_opuesto = "blanco";


                    string usuario = tablero.usuario, usuario_opuesto="";

                    if (usuario == Globals.usuario_activo)
                        usuario_opuesto = "Oponente";
                    else
                        usuario_opuesto = Globals.usuario_activo;



                    List<int> celdas_validas = Juego.Celdas_validas(tablero, color, color_opuesto);

                    if (celdas_validas.Count > 0)
                    {
                        ViewBag.Message = "Tablero cargado con éxito";
                        ViewBag.MessageType = "success-message";

                        return View(tablero);
                    }
                    else
                    {
                        celdas_validas = Juego.Celdas_validas(tablero, color_opuesto, color);

                        if (celdas_validas.Count > 0)
                        {
                            ViewBag.Message = usuario + " no tiene movimientos válidos";
                            ViewBag.MessageType = "error-message";

                            return View(tablero);
                        }
                        else
                        {
                            //JUEGO TERMINADO
                            ViewBag.Message = Juego.Juego_terminado(tablero, color);
                            ViewBag.MessageType = "success-message";

                            return View(tablero);
                        }
                    }
                }
            } 
            else //no se cargó un archivo
            {
                Tablero tablero_temp = new Tablero(8, 8, "blanco", Globals.usuario_activo, "Normal");
                tablero_temp.colores = new List<string> { "blanco" };
                tablero_temp.colores_oponente = new List<string> { "negro" };
                tablero_temp.XIniciar(false);

                ViewBag.Message = "No se cargó un archivo";
                ViewBag.MessageType = "error-message";

                return View(action, tablero_temp);
            }
        }


        //GUARDAR TABLERO        

        [HttpPost]
        public ActionResult Descargar(Tablero tablero, string action)
        {
            if(action=="Xtream")
                Conversor.XWriteXML(tablero,Server.MapPath("../temp.xml"));
            else
                Conversor.WriteXML(tablero, Server.MapPath("../temp.xml"));

            return File("../temp.xml", "text/xml", "partida.xml");
        }
   
    }
}
