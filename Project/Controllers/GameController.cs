using IPC2_P1.Models;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace IPC2_P1.Controllers
{
    public class GameController : Controller
    {
        public string[] ids = {"A1", "B1", "C1", "D1", "E1", "F1", "G1", "H1",
            "A2", "B2", "C2", "D2", "E2", "F2", "G2", "H2",
            "A3", "B3", "C3", "D3", "E3", "F3", "G3", "H3",
            "A4", "B4", "C4", "D4", "E4", "F4", "G4", "H4",
            "A5", "B5", "C5", "D5", "E5", "F5", "G5", "H5",
            "A6", "B6", "C6", "D6", "E6", "F6", "G6", "H6",
            "A7", "B7", "C7", "D7", "E7", "F7", "G7", "H7",
            "A8", "B8", "C8", "D8", "E8", "F8", "G8", "H8" };

        public string ficha_inicial;

        // SOLO, VERSUS, TORNEO

        public ActionResult Solo(string ficha_inicial)
        {
            if (Globals.logged_in == true)
            {
                List<Ficha> tablero = new List<Ficha>();
                this.ficha_inicial = ficha_inicial;

                int tam=64; // Para diferentes tamaños de tablero

                for (int i = 0; i < tam; i++)                
                    tablero.Add(new Ficha(""));
                
        
                Reemplazar(tablero, 27, "blanca");
                Reemplazar(tablero, 28, "negra");
                Reemplazar(tablero, 35, "negra");
                Reemplazar(tablero, 36, "blanca");                           
                

                if (ficha_inicial == "blanca")
                    tablero.Add(new Ficha("negra","Oponente"));
                else
                    tablero.Add(new Ficha("negra",Globals.usuario_activo));

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
        public ActionResult Solo(List<Ficha> tablero)
        {
            int index=-1;
            string color=tablero[64].valor, color_opuesto="";
            bool izq_f = false, der_f = false, sup_f = false, inf_f = false;

            List<int> lista = new List<int>();


            // RECONOCIMIENTO DE LA FICHA SELECCIONADA

            for (int i = 0; i < tablero.Count; i++)
            {
                if (tablero[i].presionado == "true")
                {
                    System.Diagnostics.Debug.WriteLine("FICHA EN: " +i);
                    index = i;

                    izq_f = (index - 1 >= 0)&& (index/8==(index-1)/8);
                    der_f = (index + 1 < 64)&& (index/8==(index+1)/8);
                    sup_f = (index - 8 >= 0);
                    inf_f = (index + 8 < 64);                                 
                }
            }


            // CAMBIANDO FICHAS ENCERRADAS

            if (index >= 0)
            {
                // CAMBIANDO EL COLOR DE LA FICHA SIGUIENTE

                if (color == "blanca")
                    color_opuesto = "negra";
                else
                    color_opuesto = "blanca";


                System.Diagnostics.Debug.WriteLine("SIMON DICE: '" + tablero[64].presionado+"'");
                if (tablero[64].presionado == "Oponente")
                {
                    tablero.RemoveAt(64);
                    tablero.Add(new Ficha(color_opuesto, Globals.usuario_activo));
                }
                else
                { 
                    tablero.RemoveAt(64);
                    tablero.Add(new Ficha(color_opuesto, "Oponente"));
                }

                // FICHAS A LA IZQUIERDA
                if (izq_f)
                {
                    if (tablero[index - 1].valor == color_opuesto)
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
                                if (tablero[index + acc].valor == color_opuesto)
                                {
                                    lista_temp.Add(index + acc);

                                }
                                else
                                {
                                    if (tablero[index + acc].valor == color)
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

                            acc-=1;
                        }
                    }
                }

                // FICHAS A LA DERECHA
                if (der_f)
                {
                    if (tablero[index + 1].valor == color_opuesto)
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
                                if (tablero[index + acc].valor == color_opuesto)
                                {
                                    lista_temp.Add(index + acc);

                                }
                                else
                                {
                                    if (tablero[index + acc].valor == color)
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

                            acc+=1;
                        }
                    }
                }

                // FICHAS SUPERIOR
                if (sup_f)
                {
                    if (tablero[index - 8].valor == color_opuesto)
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
                                if (tablero[index + acc].valor == color_opuesto)
                                {
                                    lista_temp.Add(index + acc);

                                }
                                else
                                {
                                    if (tablero[index + acc].valor == color)
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
                    if (tablero[index + 8].valor == color_opuesto)
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
                                if (tablero[index + acc].valor == color_opuesto)
                                {
                                    lista_temp.Add(index + acc);

                                }
                                else
                                {
                                    if (tablero[index + acc].valor == color)
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
                    if (tablero[index - 9].valor == color_opuesto)
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

                            if (izq&&sup)
                            {
                                if (tablero[index + acc + acc2].valor == color_opuesto)
                                {
                                    lista_temp.Add(index + acc+acc2);

                                }
                                else
                                {
                                    if (tablero[index + acc+acc2].valor == color)
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

                            acc-=1;
                            acc2-=8;
                        }
                    }
                }

                // FICHAS DER SUP
                if (der_f && sup_f)
                {
                    if (tablero[index - 7].valor == color_opuesto)
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

                            if (der&&sup)
                            {
                                if (tablero[index + acc + acc2].valor == color_opuesto)
                                {
                                    lista_temp.Add(index + acc + acc2);
                                }
                                else
                                {
                                    if (tablero[index + acc + acc2].valor == color)
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
                    if (tablero[index +7].valor == color_opuesto)
                    {
                        int acc = -2; //izq
                        int acc2 = 16; //inf
                        List<int> lista_temp = new List<int>();
                        lista_temp.Add(index +7);

                        bool salir = false;
                        while (salir == false)
                        {
                            bool izq = (index + acc >= 0) && (index / 8 == (index + acc) / 8);
                            bool inf = (index + acc2 < 64);

                            if (izq&&inf)
                            {
                                if (tablero[index + acc + acc2].valor == color_opuesto)
                                {
                                    lista_temp.Add(index + acc + acc2);
                                }
                                else
                                {
                                    if (tablero[index + acc + acc2].valor == color)
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
                    if (tablero[index + 9].valor == color_opuesto)
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

                            if (der&&inf)
                            {
                                if (tablero[index + acc + acc2].valor == color_opuesto)
                                {
                                    lista_temp.Add(index + acc + acc2);
                                }
                                else
                                {
                                    if (tablero[index + acc + acc2].valor == color)
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

                Reemplazar_list(tablero, lista, color);

            }

            return View(tablero);
        }

        public void Reemplazar(List<Ficha> tablero, int pos, string valor)
        {            
            tablero.RemoveAt(pos);
            tablero.Insert(pos, new Ficha(valor));            
        }

        public void Reemplazar_list(List<Ficha> tablero, List<int> pos_list, string valor)
        {
            foreach (int pos in pos_list)
            {
                tablero.RemoveAt(pos);
                tablero.Insert(pos, new Ficha(valor));
            }
        }


        //CARGAR TABLERO

        [HttpPost]
        public ActionResult Load(HttpPostedFileBase archivo)
        {
            if (archivo != null)
                return View("Solo",ReadXML(archivo));
            else
                return View("Solo");
        }

        public List<Ficha> ReadXML(HttpPostedFileBase archivo)
        {
            string result = string.Empty;
            using (BinaryReader b = new BinaryReader(archivo.InputStream))   // FUENTE: https://stackoverflow.com/questions/16030034/asp-net-mvc-read-file-from-httppostedfilebase-without-save/16030326
            {
                byte[] binData = b.ReadBytes(archivo.ContentLength);
                result = System.Text.Encoding.UTF8.GetString(binData);
            }

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(result);

            List<Ficha> tablero_cargado = new List<Ficha>();

            int tam = 64; // Para diferentes tamaños de tablero
            for (int i = 0; i < tam; i++)
            {
                tablero_cargado.Add(new Ficha(""));
            }
            
            foreach(XmlNode nodo in doc.SelectNodes("tablero")[0].SelectNodes("ficha"))
            {
                int pos=0;
                switch (nodo.ChildNodes[2].InnerText)
                {
                    case "1":
                        pos = 8*0;
                        break;

                    case "2":
                        pos = 8*1;
                        break;

                    case "3":
                        pos = 8*2;
                        break;

                    case "4":
                        pos = 8*3;
                        break;

                    case "5":
                        pos = 8*4;
                        break;

                    case "6":
                        pos = 8*5;
                        break;

                    case "7":
                        pos = 8*6;
                        break;

                    case "8":
                        pos = 8*7;
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

                Reemplazar(tablero_cargado, pos, nodo.ChildNodes[0].InnerText);
            }
                       
            return tablero_cargado;
        }


        //GUARDAR TABLERO        
        
        [HttpPost]
        public ActionResult Descargar(List<Ficha> tablero)
        {

            System.Diagnostics.Debug.WriteLine("PRUEBA: " + tablero.Count);
            WriteXML(tablero);
            return File("../temp.xml", "text/xml", "partida.xml");
        }

        public void WriteXML(List<Ficha> tablero)
        {
            var ruta = Server.MapPath("../temp.xml");

            XmlTextWriter writer = null;            
            writer = new XmlTextWriter(ruta, null);

            writer.WriteStartDocument();
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 3;

            writer.WriteStartElement("tablero");

            System.Diagnostics.Debug.WriteLine("TAMAÑO 2: " + tablero.Count);
            for (int i = 0; i < tablero.Count; i++)
            {
                if (tablero[i].valor=="blanca" || tablero[i].valor == "negra")
                {
                    writer.WriteStartElement("ficha");
                    writer.WriteStartElement("color");
                    writer.WriteString(tablero[i].valor); //COLOR
                    writer.WriteEndElement();
                    writer.WriteStartElement("columna");
                    writer.WriteString(ids[i].Substring(0,1));//COLUMNA
                    writer.WriteEndElement();
                    writer.WriteStartElement("fila");
                    writer.WriteString(ids[i].Substring(1,1));//FILA
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
            }

            writer.WriteStartElement("siguienteTiro"); //Poner el siguiente tiro
            writer.WriteStartElement("color");
            writer.WriteString("blanco");
            writer.WriteEndElement();
            writer.WriteEndElement();

            writer.WriteEndDocument();
            writer.Close();
        }
                
    }
}
