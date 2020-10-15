using IPC2_P1.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace IPC2_P1.Controllers
{
    //IMPRIMIR EN CONSOLA
    // System.Diagnostics.Debug.WriteLine("PRUEBA: " + tablero.Count);
    public class GameController : Controller
    {
        public static string sql = "Data Source=PCP-PC;Initial Catalog=Othello_db;User ID=pabloc54;Password=pablo125";
        public SqlConnection con = new SqlConnection(sql);
        

        // SOLO, VERSUS, TORNEO

        public ActionResult Solo(string ficha_inicial)
        {
            if (Globals.logged_in == true)
            {
                Tablero tablero;

                if (ficha_inicial == "blanco")
                {
                    tablero = new Tablero(8,8, "blanco", Globals.usuario_activo);
                    tablero.Iniciar();

                    List<int> celdas_vacias = new List<int>();
                    List<int> celdas_validas = new List<int>();
                    List<int> list_temp = new List<int>();

                    int acc = 0;
                    foreach(Ficha ficha in tablero.fichas)
                    {
                        if (ficha.color==null)                        
                            celdas_vacias.Add(acc);

                        acc++;
                    }

                    int max = 0;
                    int len = 0;

                    foreach(int i in celdas_vacias)
                    {
                        list_temp = Flanquear(tablero, i, "negro", "blanco");

                        if (list_temp.Count > len)
                        {
                            len = list_temp.Count;
                            max = i;
                        }
                    }

                    Reemplazar(tablero, max, "negro");
                    List<int> lista_temp = Flanquear(tablero, max, "negro", "blanco");

                    Reemplazar_lista(tablero, lista_temp, "negro");

                }
                else
                {
                    if (ficha_inicial == "negro")
                    {
                        tablero = new Tablero(8, 8, "negro", Globals.usuario_activo);
                        tablero.Iniciar();
                    }
                    else
                    {
                        Random random = new Random();
                        double num = random.NextDouble();

                        if (num < 0.5)
                        {
                            tablero = new Tablero(8, 8, "negro", Globals.usuario_activo);
                            tablero.Iniciar();
                        }
                        else
                        {
                            tablero = new Tablero(8, 8, "blanco", Globals.usuario_activo);
                            tablero.Iniciar();

                            List<int> celdas_vacias = new List<int>();
                            List<int> celdas_validas = new List<int>();
                            List<int> list_temp = new List<int>();

                            int acc = 0;
                            foreach (Ficha ficha in tablero.fichas)
                            {
                                if (ficha.color == null)
                                    celdas_vacias.Add(acc);

                                acc++;
                            }

                            int max = 0;
                            int len = 0;

                            foreach (int i in celdas_vacias)
                            {
                                list_temp = Flanquear(tablero, i, "negro", "blanco");

                                if (list_temp.Count > len)
                                {
                                    len = list_temp.Count;
                                    max = i;
                                }
                            }

                            Reemplazar(tablero, max, "negro");
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

        public ActionResult Versus(string ficha_inicial)
        {
            if (Globals.logged_in == true)
            {
                Tablero tablero;

                if (ficha_inicial == "blanco")
                {
                    tablero = new Tablero(8, 8, "negro", "Oponente");
                    tablero.Iniciar();
                }
                else
                {
                    if (ficha_inicial == "negro")
                    {
                        tablero = new Tablero(8, 8, "negro", Globals.usuario_activo);
                        tablero.Iniciar();
                    }
                    else
                    {
                        Random random = new Random();
                        double num = random.NextDouble();

                        if (num < 0.5)
                        {
                            tablero = new Tablero(8, 8, "negro", Globals.usuario_activo);
                            tablero.Iniciar();
                        }
                        else
                        {
                            tablero = new Tablero(8, 8, "negro", "Oponente");
                            tablero.Iniciar();
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
            int index = -1;
            int num1 = tablero.movimientos;
            int num2 = tablero.movimientos_oponente;

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


            // RECONOCIMIENTO DE LA FICHA SELECCIONADA

            int acc = 0;
            foreach (Ficha ficha in tablero.fichas)
            {
                if (ficha.presionado=="true")
                {
                    index = acc;
                }

                acc++;
            }


            // VALIDANDO SI EL TIRO ES VÁLIDO

            List<int> lista = Flanquear(tablero, index, color, color_opuesto);

            if (lista.Count > 0)
            {
                // EJECUTANDO CAMBIOS

                Reemplazar_lista(tablero, lista, color);

                // VALIDANDO SI HAY TIROS VALIDOS EN EL SIGUIENTE TURNO

                List<int> lista_temp = new List<int>();
                List<int> celdas_vacias = new List<int>();
                List<int> celdas_validas = new List<int>();

                acc = 0;
                foreach (Ficha ficha in tablero.fichas) //reconociendo celdas vacias
                {
                    if (ficha.color==null)
                        celdas_vacias.Add(acc);

                    acc++;
                }
                
                foreach (int celda in celdas_vacias) //iterando en las celdas vacias, para ver si son celdas validas (generan cambios)
                {
                    lista_temp = Flanquear(tablero, celda, color_opuesto, color);

                    if (lista_temp.Count > 0)
                    {
                        celdas_validas.Add(celda);
                    }
                }

                if (celdas_validas.Count > 0) //si existen celdas validas en el turno siguiente
                {
                    //EJECUTANDO CAMBIOS DEL CPU    

                    Random random = new Random();
                    int num = random.Next(celdas_validas.Count);

                    Reemplazar(tablero, celdas_validas[num], color_opuesto);
                    lista_temp=Flanquear(tablero, celdas_validas[num], color_opuesto, color);

                    Reemplazar_lista(tablero, lista_temp, color_opuesto);

                    tablero.movimientos += 1;
                    tablero.movimientos_oponente += 1;
                }
                else //no hay celdas validas en el siguiente turno
                {
                    if (celdas_vacias.Count > 0) //si todavia hay celdas vacias
                    {

                        // VALIDANDO SI HAY TIROS VÁLIDOS EN EL SIGUIENTE SIGUIENTE TURNO

                        lista_temp = new List<int>();
                        celdas_validas = new List<int>();

                        foreach (int celda in celdas_vacias) //iterando en las celdas vacias, para ver si son celdas validas (generan cambios)
                        {
                            lista_temp = Flanquear(tablero, celda, color, color_opuesto);
                            if (lista_temp.Count > 0)
                                celdas_validas.Add(celda);
                        }

                        if (celdas_validas.Count > 0) //SI HAY TIROS VALIDOS EN El TURNO SIGUIENTE, SIGUIENTE
                        {

                            ViewBag.Message = usuario_opuesto + " no tiene movimientos válidos";
                            ViewBag.MessageType = "error-message";

                        }
                        else //NO HAY TIROS VALIDOS EN LOS DOS TURNOS SIGUIENTES (SE TERMINA LA PARTIDA)
                        {
                            con.Open();

                            int count = 0, count2 = 0;

                            foreach (Ficha ficha in tablero.fichas) //CONTANDO LAS FICHAS
                            {
                                if (usuario == Globals.usuario_activo)
                                {
                                    if (ficha.color == color) //usuario                                    
                                        count++;
                                    else //oponente                                    
                                        if (ficha.color == color_opuesto)
                                        count2++;
                                }
                                else
                                {
                                    if (ficha.color == color) //oponente                                    
                                        count2++;
                                    else //usuario                                    
                                        if (ficha.color == color_opuesto)
                                        count++;
                                }
                            }

                            string txt = "";
                            SqlCommand cmd = null;
                            SqlDataReader dr = null;

                            ViewBag.MessageType = "neutral-message";
                            if (count > count2) //usuario gano
                            {
                                ViewBag.Message = "¡No hay movimientos válidos!, el ganador es: " + Globals.usuario_activo + " con " + count + " fichas";
                                txt = "select partidas_ganadas from Reporte where username='" + Globals.usuario_activo + "'";
                            }
                            else
                            {
                                if (count < count2) //usuario perdio
                                {
                                    ViewBag.Message = "¡No hay movimientos válidos!, el ganador es Oponente con " + count2 + " fichas";
                                    txt = "select partidas_perdidas from Reporte where username='" + Globals.usuario_activo + "'";
                                }
                                else //usuario empato
                                {
                                    ViewBag.Message = "¡No hay movimientos válidos!, hubo un empate con " + count + " fichas";
                                    txt = "select partidas_empatadas from Reporte where username='" + Globals.usuario_activo + "'";
                                }
                            }

                            cmd = new SqlCommand(txt, con);
                            dr = cmd.ExecuteReader();
                            dr.Read();
                            int num = dr.GetInt32(0) + 1;
                            dr.Close();

                            if (count > count2) //usuario gano
                            {
                                txt = "update Reporte set partidas_ganadas=" + num + " where username='" + Globals.usuario_activo + "'";
                            }
                            else
                            {
                                if (count < count2) //usuario perdio
                                {
                                    txt = "update Reporte set partidas_perdidas=" + num + " where username='" + Globals.usuario_activo + "'";
                                }
                                else //usuario empato
                                {
                                    txt = "update Reporte set partidas_empatadas=" + num + " where username='" + Globals.usuario_activo + "'";
                                }
                            }

                            cmd = new SqlCommand(txt, con);
                            dr = cmd.ExecuteReader();
                            dr.Close();

                        }

                    }
                    else //si ya no quedan celdas
                    {

                        con.Open();

                        int count = 0, count2 = 0;

                        foreach (Ficha ficha in tablero.fichas) //CONTANDO LAS FICHAS
                        {
                            if (usuario == Globals.usuario_activo)
                            {
                                if (ficha.color == color) //usuario                                    
                                    count++;
                                else //oponente                                    
                                    if (ficha.color == color_opuesto)
                                    count2++;
                            }
                            else
                            {
                                if (ficha.color == color) //oponente                                    
                                    count2++;
                                else //usuario                                    
                                    if (ficha.color == color_opuesto)
                                    count++;
                            }
                        }

                        string txt = "";
                        SqlCommand cmd = null;
                        SqlDataReader dr = null;

                        ViewBag.MessageType = "success-message";
                        if (count > count2) //usuario gano
                        {
                            ViewBag.Message = "¡El juego ha terminado!, el ganador es: " + Globals.usuario_activo + " con " + count + " fichas";
                            txt = "select partidas_ganadas from Reporte where username='" + Globals.usuario_activo + "'";
                        }
                        else
                        {
                            if (count < count2) //usuario perdio
                            {
                                ViewBag.Message = "¡El juego ha terminado!, el ganador es Oponente con " + count2 + " fichas";
                                txt = "select partidas_perdidas from Reporte where username='" + Globals.usuario_activo + "'";
                            }
                            else //usuario empato
                            {
                                ViewBag.Message = "¡¡El juego ha terminado!, hubo un empate con " + count + " fichas";
                                txt = "select partidas_empatadas from Reporte where username='" + Globals.usuario_activo + "'";
                            }
                        }

                        cmd = new SqlCommand(txt, con);
                        dr = cmd.ExecuteReader();
                        dr.Read();
                        int num = dr.GetInt32(0) + 1;
                        dr.Close();
                        System.Diagnostics.Debug.WriteLine("NUM " + num);

                        if (count > count2) //usuario gano
                        {
                            txt = "update Reporte set partidas_ganadas=" + num + " where username='" + Globals.usuario_activo + "'";
                        }
                        else
                        {
                            if (count < count2) //usuario perdio
                            {
                                txt = "update Reporte set partidas_perdidas=" + num + " where username='" + Globals.usuario_activo + "'";
                            }
                            else //usuario empato
                            {
                                txt = "update Reporte set partidas_empatadas=" + num + " where username='" + Globals.usuario_activo + "'";
                            }
                        }

                        cmd = new SqlCommand(txt, con);
                        dr = cmd.ExecuteReader();
                        dr.Close();
                    }
                }

                return View(tablero);
            }
            else
            {
                Reemplazar(tablero, index, "");
                return View(tablero);
            }
        }

        [HttpPost]
        public ActionResult Versus(Tablero tablero)
        {
            int index = -1;
            int num1 = tablero.movimientos;
            int num2 = tablero.movimientos_oponente;

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


            // RECONOCIMIENTO DE LA FICHA SELECCIONADA

            int acc = 0;
            foreach (Ficha ficha in tablero.fichas)
            {
                if (ficha.presionado == "true")
                {
                    index = acc;
                }

                acc++;
            }


            // VALIDANDO SI EL TIRO ES VÁLIDO

            List<int> lista = Flanquear(tablero, index, color, color_opuesto);

            if (lista.Count > 0)
            {
                // EJECUTANDO CAMBIOS

                Reemplazar_lista(tablero, lista, color);

                // VALIDANDO SI HAY TIROS VALIDOS EN EL SIGUIENTE TURNO

                List<int> lista_temp = new List<int>();
                List<int> celdas_vacias = new List<int>();
                List<int> celdas_validas = new List<int>();

                acc = 0;
                foreach (Ficha ficha in tablero.fichas) //reconociendo celdas vacias
                {
                    if (ficha.color==null)
                        celdas_vacias.Add(acc);

                    acc++;
                }

                foreach (int celda in celdas_vacias) //iterando en las celdas vacias, para ver si son celdas validas (generan cambios)
                {
                    lista_temp = Flanquear(tablero, celda, color_opuesto, color);
                    if (lista_temp.Count > 0)
                        celdas_validas.Add(celda);
                }

                if (celdas_validas.Count > 0) //si existen celdas validas en el turno siguiente
                {
                    if (usuario == "Oponente")
                        tablero.Actualizar(color_opuesto, Globals.usuario_activo, num1, num2 + 1);
                    else
                        tablero.Actualizar(color_opuesto, "Oponente", num1+1, num2);
                }
                else //no hay celdas validas en el siguiente turno
                {
                    if (celdas_vacias.Count > 0) //si todavia hay celdas vacias
                    {

                        // VALIDANDO SI HAY TIROS VÁLIDOS EN EL SIGUIENTE SIGUIENTE TURNO

                        lista_temp = new List<int>();
                        celdas_validas = new List<int>();

                        foreach (int celda in celdas_vacias) //iterando en las celdas vacias, para ver si son celdas validas (generan cambios)
                        {
                            lista_temp = Flanquear(tablero, celda, color, color_opuesto);
                            if (lista_temp.Count > 0)
                                celdas_validas.Add(celda);
                        }

                        if (celdas_validas.Count > 0) //SI HAY TIROS VALIDOS EN El TURNO SIGUIENTE, SIGUIENTE
                        {

                            ViewBag.Message = usuario_opuesto + " no tiene movimientos válidos";
                            ViewBag.MessageType = "error-message";

                        }
                        else //NO HAY TIROS VALIDOS EN LOS DOS TURNOS SIGUIENTES (SE TERMINA LA PARTIDA)
                        {
                            con.Open();

                            int count = 0, count2 = 0;

                            foreach (Ficha ficha in tablero.fichas) //CONTANDO LAS FICHAS
                            {
                                if (usuario == Globals.usuario_activo)
                                {
                                    if (ficha.color == color) //usuario                                    
                                        count++;
                                    else //oponente                                    
                                        if (ficha.color == color_opuesto)
                                        count2++;
                                }
                                else
                                {
                                    if (ficha.color == color) //oponente                                    
                                        count2++;
                                    else //usuario                                    
                                        if (ficha.color == color_opuesto)
                                        count++;
                                }
                            }

                            string txt = "";
                            SqlCommand cmd = null;
                            SqlDataReader dr = null;

                            ViewBag.MessageType = "neutral-message";
                            if (count > count2) //usuario gano
                            {
                                ViewBag.Message = "¡No hay movimientos válidos!, el ganador es: " + Globals.usuario_activo + " con " + count + " fichas";
                                txt = "select partidas_ganadas from Reporte where username='" + Globals.usuario_activo + "'";
                            }
                            else
                            {
                                if (count < count2) //usuario perdio
                                {
                                    ViewBag.Message = "¡No hay movimientos válidos!, el ganador es Oponente con " + count2 + " fichas";
                                    txt = "select partidas_perdidas from Reporte where username='" + Globals.usuario_activo + "'";
                                }
                                else //usuario empato
                                {
                                    ViewBag.Message = "¡No hay movimientos válidos!, hubo un empate con " + count + " fichas";
                                    txt = "select partidas_empatadas from Reporte where username='" + Globals.usuario_activo + "'";
                                }
                            }

                            cmd = new SqlCommand(txt, con);
                            dr = cmd.ExecuteReader();
                            dr.Read();
                            int num = dr.GetInt32(0) + 1;
                            dr.Close();

                            if (count > count2) //usuario gano
                            {
                                txt = "update Reporte set partidas_ganadas=" + num + " where username='" + Globals.usuario_activo + "'";
                            }
                            else
                            {
                                if (count < count2) //usuario perdio
                                {
                                    txt = "update Reporte set partidas_perdidas=" + num + " where username='" + Globals.usuario_activo + "'";
                                }
                                else //usuario empato
                                {
                                    txt = "update Reporte set partidas_empatadas=" + num + " where username='" + Globals.usuario_activo + "'";
                                }
                            }

                            cmd = new SqlCommand(txt, con);
                            dr = cmd.ExecuteReader();
                            dr.Close();

                        }

                    }
                    else //si ya no quedan celdas
                    {

                        con.Open();

                        int count = 0, count2 = 0;

                        foreach (Ficha ficha in tablero.fichas) //CONTANDO LAS FICHAS
                        {
                            if (usuario == Globals.usuario_activo)
                            {
                                if (ficha.color == color) //usuario                                    
                                    count++;
                                else //oponente                                    
                                    if (ficha.color == color_opuesto)
                                    count2++;
                            }
                            else
                            {
                                if (ficha.color == color) //oponente                                    
                                    count2++;
                                else //usuario                                    
                                    if (ficha.color == color_opuesto)
                                    count++;
                            }
                        }

                        string txt = "";
                        SqlCommand cmd = null;
                        SqlDataReader dr = null;

                        ViewBag.MessageType = "success-message";
                        if (count > count2) //usuario gano
                        {
                            ViewBag.Message = "¡El juego ha terminado!, el ganador es: " + Globals.usuario_activo + " con " + count + " fichas";
                            txt = "select partidas_ganadas from Reporte where username='" + Globals.usuario_activo + "'";
                        }
                        else
                        {
                            if (count < count2) //usuario perdio
                            {
                                ViewBag.Message = "¡El juego ha terminado!, el ganador es Oponente con " + count2 + " fichas";
                                txt = "select partidas_perdidas from Reporte where username='" + Globals.usuario_activo + "'";
                            }
                            else //usuario empato
                            {
                                ViewBag.Message = "¡¡El juego ha terminado!, hubo un empate con " + count + " fichas";
                                txt = "select partidas_empatadas from Reporte where username='" + Globals.usuario_activo + "'";
                            }
                        }

                        cmd = new SqlCommand(txt, con);
                        dr = cmd.ExecuteReader();
                        dr.Read();
                        int num = dr.GetInt32(0) + 1;
                        dr.Close();
                        System.Diagnostics.Debug.WriteLine("NUM " + num);

                        if (count > count2) //usuario gano
                        {
                            txt = "update Reporte set partidas_ganadas=" + num + " where username='" + Globals.usuario_activo + "'";
                        }
                        else
                        {
                            if (count < count2) //usuario perdio
                            {
                                txt = "update Reporte set partidas_perdidas=" + num + " where username='" + Globals.usuario_activo + "'";
                            }
                            else //usuario empato
                            {
                                txt = "update Reporte set partidas_empatadas=" + num + " where username='" + Globals.usuario_activo + "'";
                            }
                        }

                        cmd = new SqlCommand(txt, con);
                        dr = cmd.ExecuteReader();
                        dr.Close();
                        System.Diagnostics.Debug.WriteLine("TODO FINE BBGG BD");
                    }
                }

                return View(tablero);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("NO SE HICIERON CAMBIOS");
                Reemplazar(tablero, index, "");
                return View(tablero);
            }
        }


        public List<int> Flanquear(Tablero tablero, int index, string color, string color_opuesto)
        {
            List<int> lista = new List<int>();

            bool izq_f = (index - 1 >= 0) && (index / 8 == (index - 1) / 8);
            bool der_f = (index + 1 < 64) && (index / 8 == (index + 1) / 8);
            bool sup_f = (index - 8 >= 0);
            bool inf_f = (index + 8 < 64);


            // CAMBIANDO FICHAS ENCERRADAS

            if (index >= 0)
            {                

                // FICHAS A LA IZQUIERDA
                if (izq_f)
                {
                    if (tablero.fichas[index - 1].color == color_opuesto)
                    {
                        int acc = -2;
                        List<int> lista_temp = new List<int>();
                        lista_temp.Add(index - 1);

                        bool salir = false;
                        while (salir == false)
                        {
                            bool izq = (index + acc >= 0) && (index / 8 == (index + acc) / 8);
                            if (izq)
                            {
                                if (tablero.fichas[index + acc].color == color_opuesto)
                                {
                                    lista_temp.Add(index + acc);

                                }
                                else
                                {
                                    if (tablero.fichas[index + acc].color == color)
                                    {
                                        lista.AddRange(lista_temp);
                                    }
                                    else
                                    {
                                        salir = true;
                                    }
                                }
                            }
                            else
                            {
                                salir = true;
                            }

                            acc -= 1;
                        }
                    }
                }

                // FICHAS A LA DERECHA
                if (der_f)
                {
                    if (tablero.fichas[index + 1].color == color_opuesto)
                    {
                        int acc = 2;
                        List<int> lista_temp = new List<int>();
                        lista_temp.Add(index + 1);

                        bool salir = false;
                        while (salir == false)
                        {
                            bool der = (index + acc < 64) && (index / 8 == (index + acc) / 8);
                            if (der)
                            {
                                if (tablero.fichas[index + acc].color == color_opuesto)
                                {
                                    lista_temp.Add(index + acc);

                                }
                                else
                                {
                                    if (tablero.fichas[index + acc].color == color)
                                    {
                                        lista.AddRange(lista_temp);
                                    }
                                    else
                                    {
                                        salir = true;
                                    }
                                }
                            }
                            else
                            {
                                salir = true;
                            }

                            acc += 1;
                        }
                    }
                }

                // FICHAS SUPERIOR
                if (sup_f)
                {
                    if (tablero.fichas[index - 8].color == color_opuesto)
                    {
                        int acc = -16;
                        List<int> lista_temp = new List<int>();
                        lista_temp.Add(index - 8);

                        bool salir = false;
                        while (salir == false)
                        {
                            bool sup = (index + acc >= 0);
                            if (sup)
                            {
                                if (tablero.fichas[index + acc].color == color_opuesto)
                                {
                                    lista_temp.Add(index + acc);

                                }
                                else
                                {
                                    if (tablero.fichas[index + acc].color == color)
                                    {
                                        lista.AddRange(lista_temp);
                                    }
                                    else
                                    {
                                        salir = true;
                                    }
                                }
                            }
                            else
                            {
                                salir = true;
                            }

                            acc -= 8;
                        }
                    }
                }

                // FICHAS INFERIOR
                if (inf_f)
                {
                    if (tablero.fichas[index + 8].color == color_opuesto)
                    {
                        int acc = 16;
                        List<int> lista_temp = new List<int>();
                        lista_temp.Add(index + 8);

                        bool salir = false;
                        while (salir == false)
                        {
                            bool inf = (index + acc < 64);
                            if (inf)
                            {
                                if (tablero.fichas[index + acc].color == color_opuesto)
                                {
                                    lista_temp.Add(index + acc);

                                }
                                else
                                {
                                    if (tablero.fichas[index + acc].color == color)
                                    {
                                        lista.AddRange(lista_temp);
                                    }
                                    else
                                    {
                                        salir = true;
                                    }
                                }
                            }
                            else
                            {
                                salir = true;
                            }

                            acc += 8;
                        }
                    }
                }

                // FICHAS IZQ SUP
                if (izq_f && sup_f)
                {
                    if (tablero.fichas[index - 9].color == color_opuesto)
                    {
                        int acc = -2; //izq
                        int acc2 = -16; //sup
                        List<int> lista_temp = new List<int>();
                        lista_temp.Add(index - 9);

                        bool salir = false;
                        while (salir == false)
                        {
                            bool izq = (index + acc >= 0) && (index / 8 == (index + acc) / 8);
                            bool sup = (index + acc2 >= 0);

                            if (izq && sup)
                            {
                                if (tablero.fichas[index + acc + acc2].color == color_opuesto)
                                {
                                    lista_temp.Add(index + acc + acc2);

                                }
                                else
                                {
                                    if (tablero.fichas[index + acc + acc2].color == color)
                                    {
                                        lista.AddRange(lista_temp);
                                    }
                                    else
                                    {
                                        salir = true;
                                    }
                                }
                            }
                            else
                            {
                                salir = true;
                            }

                            acc -= 1;
                            acc2 -= 8;
                        }
                    }
                }

                // FICHAS DER SUP
                if (der_f && sup_f)
                {
                    if (tablero.fichas[index - 7].color == color_opuesto)
                    {
                        int acc = 2; //der
                        int acc2 = -16; //sup
                        List<int> lista_temp = new List<int>();
                        lista_temp.Add(index - 7);

                        bool salir = false;
                        while (salir == false)
                        {
                            bool der = (index + acc < 64) && (index / 8 == (index + acc) / 8);
                            bool sup = (index + acc2 >= 0);

                            if (der && sup)
                            {
                                if (tablero.fichas[index + acc + acc2].color == color_opuesto)
                                {
                                    lista_temp.Add(index + acc + acc2);
                                }
                                else
                                {
                                    if (tablero.fichas[index + acc + acc2].color == color)
                                    {
                                        lista.AddRange(lista_temp);
                                    }
                                    else
                                    {
                                        salir = true;
                                    }
                                }
                            }
                            else
                            {
                                salir = true;
                            }

                            acc += 1;
                            acc2 -= 8;
                        }
                    }
                }

                // FICHAS IZQ INF
                if (izq_f && inf_f)
                {
                    if (tablero.fichas[index + 7].color == color_opuesto)
                    {
                        int acc = -2; //izq
                        int acc2 = 16; //inf
                        List<int> lista_temp = new List<int>();
                        lista_temp.Add(index + 7);

                        bool salir = false;
                        while (salir == false)
                        {
                            bool izq = (index + acc >= 0) && (index / 8 == (index + acc) / 8);
                            bool inf = (index + acc2 < 64);

                            if (izq && inf)
                            {
                                if (tablero.fichas[index + acc + acc2].color == color_opuesto)
                                {
                                    lista_temp.Add(index + acc + acc2);
                                }
                                else
                                {
                                    if (tablero.fichas[index + acc + acc2].color == color)
                                    {
                                        lista.AddRange(lista_temp);
                                    }
                                    else
                                    {
                                        salir = true;
                                    }
                                }
                            }
                            else
                            {
                                salir = true;
                            }

                            acc -= 1;
                            acc2 += 8;
                        }
                    }
                }

                // FICHAS DER INF
                if (der_f && inf_f)
                {
                    if (tablero.fichas[index + 9].color == color_opuesto)
                    {
                        int acc = 2; //der
                        int acc2 = 16; //inf
                        List<int> lista_temp = new List<int>();
                        lista_temp.Add(index + 9);

                        bool salir = false;
                        while (salir == false)
                        {
                            bool der = (index + acc < 64) && (index / 8 == (index + acc) / 8);
                            bool inf = (index + acc2 < 64);

                            if (der && inf)
                            {
                                if (tablero.fichas[index + acc + acc2].color == color_opuesto)
                                {
                                    lista_temp.Add(index + acc + acc2);
                                }
                                else
                                {
                                    if (tablero.fichas[index + acc + acc2].color == color)
                                    {
                                        lista.AddRange(lista_temp);
                                    }
                                    else
                                    {
                                        salir = true;
                                    }
                                }
                            }
                            else
                            {
                                salir = true;
                            }

                            acc += 1;
                            acc2 += 8;
                        }
                    }
                }
                
            }

            return lista;
        }

        public void Reemplazar(Tablero tablero, int pos, string valor)
        {
            if (pos >= 0)            
                tablero.fichas[pos].color = valor;            
        }

        public void Reemplazar_lista(Tablero tablero, List<int> pos_list, string valor)
        {
            foreach (int pos in pos_list)
                if(pos>=0)
                    tablero.fichas[pos].color = valor;
            
        }
        

        //CARGAR TABLERO

        [HttpPost]
        public ActionResult Load(HttpPostedFileBase archivo, string color_temp, string usuario_temp, string action)
        {
            if (archivo != null)
            {
                Tablero tablero = ReadXML(archivo, color_temp, usuario_temp);

                int num1 = tablero.movimientos;
                int num2 = tablero.movimientos_oponente;

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
                    lista_temp = Flanquear(tablero, celda, color, color_opuesto);
                    if (lista_temp.Count > 0)
                        celdas_validas.Add(celda);
                }
                

                if (celdas_validas.Count == 0)  //no hay celdas validas en el siguiente turno
                {

                    if (usuario == "Oponente")
                        tablero.Actualizar(color_opuesto, Globals.usuario_activo, num1, num2 + 1);
                    else
                        tablero.Actualizar(color_opuesto, "Oponente", num1+1, num2);


                    if (celdas_vacias.Count > 0) //si todavia hay celdas vacias
                    {

                        // VALIDANDO SI HAY TIROS VÁLIDOS EN EL SIGUIENTE SIGUIENTE TURNO

                        lista_temp = new List<int>();
                        celdas_validas = new List<int>();

                        foreach (int celda in celdas_vacias) //iterando en las celdas vacias, para ver si son celdas validas (generan cambios)
                        {
                            lista_temp = Flanquear(tablero, celda, color_opuesto, color);
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
                            return View(action, new Tablero(8, 8, color_temp, usuario_temp));
                        }

                    }
                    else //si ya no quedan celdas
                    {
                        ViewBag.Message = "El tablero cargado está lleno";
                        ViewBag.MessageType = "error-message";

                        return View(action, new Tablero(8, 8, color_temp, usuario_temp));
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
                return View(action, new Tablero(8, 8, color_temp, usuario_temp));
        }

        public Tablero ReadXML(HttpPostedFileBase archivo, string color, string usuario)
        {
            string result = "";
            using (BinaryReader b = new BinaryReader(archivo.InputStream))   // FUENTE: https://stackoverflow.com/questions/16030034/asp-net-mvc-read-file-from-httppostedfilebase-without-save/16030326
            {
                byte[] binData = b.ReadBytes(archivo.ContentLength);
                result = System.Text.Encoding.UTF8.GetString(binData);
            }

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(result);

            int tam = 64; // Para diferentes tamaños de tablero
            int lado = Convert.ToInt32(Math.Sqrt(tam));

            Tablero tablero_cargado = new Tablero(lado, lado, "", "");
            
            
            foreach(XmlNode nodo in doc.SelectNodes("tablero")[0].SelectNodes("ficha"))
            {
                int pos=-1;
                string color_temp = nodo.ChildNodes[0].InnerText;

                switch (nodo.ChildNodes[2].InnerText)
                {
                    case "1":
                        pos = lado*0;
                        break;

                    case "2":
                        pos = lado * 1;
                        break;

                    case "3":
                        pos = lado * 2;
                        break;

                    case "4":
                        pos = lado * 3;
                        break;

                    case "5":
                        pos = lado * 4;
                        break;

                    case "6":
                        pos = lado * 5;
                        break;

                    case "7":
                        pos = lado * 6;
                        break;

                    case "8":
                        pos = lado * 7;
                        break;

                }
                switch (nodo.ChildNodes[1].InnerText)
                {
                    case "A":
                        break;

                    case "B":
                        pos += 1;
                        break;

                    case "C":
                        pos += 2;
                        break;

                    case "D":
                        pos += 3;
                        break;

                    case "E":
                        pos += 4;
                        break;

                    case "F":
                        pos += 5;
                        break;

                    case "G":
                        pos += 6;
                        break;

                    case "H":
                        pos += 7;
                        break;

                }
                
                Reemplazar(tablero_cargado, pos, color_temp);
            }

            int count = 0;

            foreach (Ficha ficha in tablero_cargado.fichas)
            {
                if (ficha.color != null)
                    count++;
            }

            string color_temp2 = doc.SelectNodes("tablero")[0].SelectNodes("siguienteTiro")[0].InnerText;

            if (color != color_temp2)
            {
                if (usuario == Globals.usuario_activo)
                    usuario = "Oponente";
                else
                    usuario = Globals.usuario_activo;
            }

            tablero_cargado.Actualizar(color_temp2, usuario, count / 2-2, count / 2-2);

            return tablero_cargado;
        }


        //GUARDAR TABLERO        

        [HttpPost]
        public ActionResult Descargar(Tablero tablero)
        {

            WriteXML(tablero);
            return File("../temp.xml", "text/xml", "partida.xml");
        }

        public void WriteXML(Tablero tablero)
        {
            var ruta = Server.MapPath("../temp.xml");

            XmlTextWriter writer = null;            
            writer = new XmlTextWriter(ruta, null);

            writer.WriteStartDocument();
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 3;

            writer.WriteStartElement("tablero");

            int acc = 0;
            for (int i = 0; i < tablero.filas; i++)
            {
                for (int j = 0; j < tablero.columnas; j++)
                {
                    if (tablero.fichas[acc].color != null)
                    {
                        writer.WriteStartElement("ficha");
                        writer.WriteStartElement("color");
                        writer.WriteString(tablero.fichas[acc].color); //COLOR
                        writer.WriteEndElement();
                        writer.WriteStartElement("columna");
                        writer.WriteString(Globals.columnas[j]);//COLUMNA
                        writer.WriteEndElement();
                        writer.WriteStartElement("fila");
                        writer.WriteString(Globals.filas[i]);//FILA
                        writer.WriteEndElement();
                        writer.WriteEndElement();
                    }

                    acc++;
                }
            }

            writer.WriteStartElement("siguienteTiro"); //Poner el siguiente tiro
            writer.WriteStartElement("color");
            writer.WriteString(tablero.color);
            writer.WriteEndElement();
            writer.WriteEndElement();

            writer.WriteEndDocument();
            writer.Close();
        }
                
    }
}
