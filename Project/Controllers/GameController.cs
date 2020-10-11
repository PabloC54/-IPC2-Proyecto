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

        // SOLO, VERSUS, TORNEO

        public ActionResult Solo(string ficha_inicial)
        {
            if (Globals.logged_in == true)
            {
                List<Ficha> tablero = new List<Ficha>();

                int tam=64; // Para diferentes tamaños de tablero

                for (int i = 0; i < tam; i++)
                {
                    tablero.Add(new Ficha(""));
                }

                Reemplazar(tablero, 27, "negra");
                Reemplazar(tablero, 28, "blanca");
                Reemplazar(tablero, 35, "blanca");
                Reemplazar(tablero, 36, "negra");
                                

                tablero.Add(new Ficha("negra"));              
                
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
            int index = 0;
            string color=tablero[64].valor, color_opuesto="";
            bool izq=false, der=false, sup=false, inf=false;

            if (color == "blanca")
            {
                color_opuesto = "negra";
                Reemplazar(tablero, 64, "negra");
            }
            else
            {
                color_opuesto = "blanca";
                Reemplazar(tablero, 64, "blanca");
            }

            // RECONOCIMIENTO DE LA FICHA SELECCIONADA
            for (int i = 0; i < tablero.Count; i++)
            {
                if (tablero[i].presionado == "true")
                {
                    System.Diagnostics.Debug.WriteLine("FICHA EN: " +i);
                    index = i;
                    izq = (index - 1 >= 0)&& (index/8==(index-1)/8);
                    der = (index + 1 < 64)&& (index/8==(index+1)/8);
                    sup = (index - 8 >= 0);
                    inf = (index + 8 < 64);                                 
                }
            }

            // FICHA A LA IZQUIERDA
            if (izq)
            {
                if (tablero[index - 1].valor == color_opuesto)
                {
                    izq = (index - 2 >= 0) && (index / 8 == (index - 2) / 8);
                    if (izq)
                    {
                        if (tablero[index - 2].valor == color)
                        {
                            Reemplazar(tablero, index - 1, color);
                        }
                    }
                }
            }

            // FICHA A LA DERECHA
            if (der)
            {
                if (tablero[index + 1].valor == color_opuesto)
                {
                    int acc = 2;

                    bool salir = false;
                    while (salir == false)
                    {
                        der = (index + acc < 64) && (index / 8 == (index + acc) / 8);
                        if (der)
                        {
                            if (tablero[index + acc].valor == color_opuesto)
                            {
                                Reemplazar(tablero, index + acc-1, color);

                            }
                            else
                            {
                                if (tablero[index + acc].valor == color)
                                {
                                    //Aqui me quedé
                                }
                            }
                        }
                        else
                        {
                            salir = true;
                        }

                        acc++;
                    }
                }
            }

            // FICHA SUPERIOR
            if (sup)
            {
                if (tablero[index - 8].valor == color_opuesto)
                {
                    sup = (index - 16 >= 0);
                    if (sup)
                    {
                        if (tablero[index - 16].valor == color)
                        {
                            Reemplazar(tablero, index - 8, color);
                        }
                    }
                }
            }

            // FICHA INFERIOR
            if (inf)
            {
                if (tablero[index + 8].valor == color_opuesto)
                {
                    inf = (index + 16 < 64);
                    if (inf)
                    {
                        if (tablero[index + 16].valor == color)
                        {
                            Reemplazar(tablero, index + 8, color);
                        }
                    }
                }
            }




            return View(tablero);
        }

        public void Reemplazar(List<Ficha> tablero, int pos, string valor)
        {
            System.Diagnostics.Debug.WriteLine("Se reemplazo");
            tablero.RemoveAt(pos);
            tablero.Insert(pos, new Ficha(valor));            
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
