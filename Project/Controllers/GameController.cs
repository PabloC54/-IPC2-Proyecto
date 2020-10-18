using IPC2_P1.Models;
using IPC2_P1.Controllers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;
using System.Web.Mvc;

namespace IPC2_P1.Controllers
{
    // IMPRIMIR EN CONSOLA
    // System.Diagnostics.Debug.WriteLine("PRUEBA: ");
    public class GameController : Controller
    {
        public Conversor Conversor = new Conversor();
        public Othello Juego = new Othello();


        // I N I C I O

        public ActionResult Index(string go_to, string ficha_inicial)
        {
            string[] temp = { go_to, ficha_inicial };
            return View(temp);
        }

        [HttpPost]
        public RedirectToRouteResult Index(string go_to, string ficha_inicial, string apertura, string modalidad)
        {
            return RedirectToAction(go_to, new {ficha_inicial, apertura, modalidad });
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
                    tablero = new Tablero(8,8, "blanco", Globals.usuario_activo, modalidad);
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
                ViewBag.Message = "Primero debes iniciar sesión";
                ViewBag.MessageType = "error-message";

                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult Versus(string ficha_inicial, string apertura, string modalidad)
        {
            if (Globals.logged_in == true)
            {
                Tablero tablero;
                bool temp=false;

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
                ViewBag.Message = "Primero debes iniciar sesión";
                ViewBag.MessageType = "error-message";

                return RedirectToAction("Login", "Home");
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
                        tablero.cronometro_agotado = "false";
                    }
                    else
                    {
                        Juego.Movimiento_cpu_apertura(tablero, color_opuesto);

                        if (Juego.Celdas_ocupadas(tablero).Count == 4)
                        {
                            ViewBag.MessageType = "success-message";
                            ViewBag.Message = "¡Apertura personalizada finalizada!";
                            tablero.apertura_personalizada = "false";
                            tablero.cronometro_agotado = "false";                            
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

            //CRONOMETRO AGOTADO
            if (tablero.cronometro_agotado == "true")
            {
                //EJECUTANDO CAMBIOS DEL CPU    
                Juego.Movimiento_cpu(tablero, color_opuesto, color);
                tablero.Actualizar(color, usuario, tablero.movimientos, tablero.movimientos_oponente + 1);
                tablero.cronometro_agotado = "false";
                               
                List<int> celdas_validas = Juego.Celdas_validas(tablero, color, color_opuesto);

                bool salir = true;
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
                        tablero.cronometro_agotado = "false";
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

            //CRONOMETRO AGOTADO
            if (tablero.cronometro_agotado == "true")
            {
                tablero.cronometro_agotado = "false";
                tablero.Actualizar(color_opuesto, usuario_opuesto, tablero.movimientos, tablero.movimientos_oponente);

                return View(tablero);
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


        //CARGAR TABLERO

        [HttpPost]
        public ActionResult Cargar(HttpPostedFileBase archivo, string color_temp, string usuario_temp, string tablero_variable, string action)
        {
            if (archivo != null)
            {
                Tablero tablero;
                if(tablero_variable=="true")
                    tablero = Conversor.XReadXML(archivo, color_temp, usuario_temp);
                else
                    tablero = Conversor.ReadXML(archivo, color_temp, usuario_temp);
                
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


             
                // VALIDANDO SI HAY TIROS VALIDOS EN EL SIGUIENTE TURNO

                List<int> lista_temp = new List<int>();
                List<int> celdas_vacias = new List<int>();
                List<int> celdas_validas = new List<int>();

                int acc = 0;
                foreach (Ficha ficha in tablero.fichas) //reconociendo celdas vacias
                {
                    if (ficha.color== null)
                        celdas_vacias.Add(acc);

                    acc++;
                }

                foreach (int celda in celdas_vacias) //iterando en las celdas vacias, para ver si son celdas validas (generan cambios)
                {
                    lista_temp = Juego.Flanquear(tablero, celda, color, color_opuesto);
                    if (lista_temp.Count > 0)
                        celdas_validas.Add(celda);
                }
                

                if (celdas_validas.Count == 0)  //no hay celdas validas en el siguiente turno
                {

                    if (usuario == "Oponente")
                        tablero.Actualizar(color_opuesto, Globals.usuario_activo, tablero.movimientos, tablero.movimientos_oponente + 1);
                    else
                        tablero.Actualizar(color_opuesto, "Oponente", tablero.movimientos+1, tablero.movimientos_oponente);


                    if (celdas_vacias.Count > 0) //si todavia hay celdas vacias
                    {

                        // VALIDANDO SI HAY TIROS VÁLIDOS EN EL SIGUIENTE SIGUIENTE TURNO

                        lista_temp = new List<int>();
                        celdas_validas = new List<int>();

                        foreach (int celda in celdas_vacias) //iterando en las celdas vacias, para ver si son celdas validas (generan cambios)
                        {
                            lista_temp = Juego.Flanquear(tablero, celda, color_opuesto, color);
                            if (lista_temp.Count > 0)
                                celdas_validas.Add(celda);
                        }

                        if (celdas_validas.Count > 0) //SI HAY TIROS VALIDOS EN El TURNO SIGUIENTE, SIGUIENTE
                        {
                            ViewBag.Message = usuario + " no tiene movimientos válidos";
                            ViewBag.MessageType = "error-message";
                            return View(action, tablero);
                        }
                        else //NO HAY TIROS VALIDOS EN LOS DOS TURNOS SIGUIENTES (SE TERMINA LA PARTIDA)
                        {
                            ViewBag.Message = "El tablero cargado no es válido";
                            ViewBag.MessageType = "error-message";
                            return View(action, new Tablero(8, 8, color_temp, usuario_temp,"Normal"));
                        }

                    }
                    else //si ya no quedan celdas
                    {
                        ViewBag.Message = "El tablero cargado está lleno";
                        ViewBag.MessageType = "error-message";

                        return View(action, new Tablero(8, 8, color_temp, usuario_temp, "Normal"));
                    }
                }
                else
                {
                    ViewBag.Message = "Tablero cargado exitosamente";
                    ViewBag.MessageType = "neutral-message";
                    return View(action, tablero);
                }
                
            } //no se cargó un archivo
            else
                return View(action, new Tablero(8, 8, color_temp, usuario_temp, "Normal"));
        }


        //GUARDAR TABLERO        

        [HttpPost]
        public ActionResult Descargar(Tablero tablero)
        {
            Conversor.WriteXML(tablero,Server.MapPath("../temp.xml"));
            return File("../temp.xml", "text/xml", "partida.xml");
        }
   
    }
}
