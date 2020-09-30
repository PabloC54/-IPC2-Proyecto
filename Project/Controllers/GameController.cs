using IPC2_P1.Models;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace IPC2_P1.Controllers
{
    public class GameController : Controller
    {

        public ActionResult GetMessage()
        {
            string message = "Welcome";
            return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }




        public ActionResult Solo()
        {
            if (Globals.logged_in == true)
            {
                return View(new Tablero {
                    D4 = "blanco",
                    E4 = "negro",
                    D5 = "negro",
                    E5 = "blanco"});
            }
            else
            {
                ViewBag.Message = "Primero debes iniciar sesión";
                ViewBag.MessageType = "error-message";

                return View("../Home/Login");
            }
        }

        [HttpPost]
        public ActionResult Solo(HttpPostedFileBase archivo, Tablero tablero)
        {
            if (archivo != null) { 
                return View(ReadXML(archivo));
            }
            else
            {
                WriteXML(ToArray(tablero));
                return View(tablero);
            }
        } 

        public ActionResult Versus()
        {
            if (Globals.logged_in == true)
            {
                Tablero tablero = new Tablero
                {
                    D4 = "blanco",
                    D5 = "negro",
                    E4 = "blanco",
                    E5 = "negro"
                };

                return View(tablero);
            }
            else
            {
                ViewBag.Message = "Primero debes iniciar sesión";
                ViewBag.MessageType = "error-message";

                return View("../Home/Login");
            }
        }
      
        //CARGAR TABLERO
        public Tablero ReadXML(HttpPostedFileBase archivo)
        {
            string result = string.Empty;
            using (BinaryReader b = new BinaryReader(archivo.InputStream))   // FUENTE: https://stackoverflow.com/questions/16030034/asp-net-mvc-read-file-from-httppostedfilebase-without-save/16030326
            {
                byte[] binData = b.ReadBytes(archivo.ContentLength);
                result = System.Text.Encoding.UTF8.GetString(binData);
            }

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(result);
            
            string[] Ids=new string[64];
            foreach(XmlNode nodo in doc.SelectNodes("tablero")[0].SelectNodes("ficha"))
            {
                int pos=0;

                switch (nodo.ChildNodes[2].InnerText)
                {
                    case "1":
                        pos = 0;
                        break;

                    case "2":
                        pos = 8;
                        break;

                    case "3":
                        pos = 16;
                        break;

                    case "4":
                        pos = 24;
                        break;

                    case "5":
                        pos = 32;
                        break;

                    case "6":
                        pos = 40;
                        break;

                    case "7":
                        pos = 48;
                        break;

                    case "8":
                        pos = 56;
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

                Ids[pos] = nodo.ChildNodes[0].InnerText;
            }

            Tablero tablero_cargado = new Tablero
            {
                A1 = Ids[0],
                B1 = Ids[1],
                C1 = Ids[2],
                D1 = Ids[3],
                E1 = Ids[4],
                F1 = Ids[5],
                G1 = Ids[6],
                H1 = Ids[7],
 
                A2 = Ids[8],
                B2 = Ids[9],
                C2 = Ids[10],
                D2 = Ids[11],
                E2 = Ids[12],
                F2 = Ids[13],
                G2 = Ids[14],
                H2 = Ids[15],

                A3 = Ids[16],
                B3 = Ids[17],
                C3 = Ids[18],
                D3 = Ids[19],
                E3 = Ids[20],
                F3 = Ids[21],
                G3 = Ids[22],
                H3 = Ids[23],

                A4 = Ids[24],
                B4 = Ids[25],
                C4 = Ids[26],
                D4 = Ids[27],
                E4 = Ids[28],
                F4 = Ids[29],
                G4 = Ids[30],
                H4 = Ids[31],

                A5 = Ids[32],
                B5 = Ids[33],
                C5 = Ids[34],
                D5 = Ids[35],
                E5 = Ids[36],
                F5 = Ids[37],
                G5 = Ids[38],
                H5 = Ids[39],

                A6 = Ids[40],
                B6 = Ids[41],
                C6 = Ids[42],
                D6 = Ids[43],
                E6 = Ids[44],
                F6 = Ids[45],
                G6 = Ids[46],
                H6 = Ids[47],

                A7 = Ids[48],
                B7 = Ids[49],
                C7 = Ids[50],
                D7 = Ids[51],
                E7 = Ids[52],
                F7 = Ids[53],
                G7 = Ids[54],
                H7 = Ids[55],

                A8 = Ids[56],
                B8 = Ids[57],
                C8 = Ids[58],
                D8 = Ids[59],
                E8 = Ids[60],
                F8 = Ids[61],
                G8 = Ids[62],
                H8 = Ids[63]
            };
            
            return tablero_cargado;
        }


        //GUARDAR TABLERO
        public void WriteXML(string[][] tablero)
        {
            XmlTextWriter writer = null;

            int num = 1;
            while (System.IO.File.Exists("C:/Users/pablo/Downloads/archivo" + num + ".xml") == true)
            {
                num += 1;
            }

            writer = new XmlTextWriter("C:/Users/pablo/Downloads/archivo" + num + ".xml", null);

            writer.WriteStartDocument();
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 3;

            writer.WriteStartElement("tablero");

            for (int i = 0; i < 64; i++)
            {
                if (tablero[i][0]=="blanco" || tablero[i][0] == "negro")
                {

                    writer.WriteStartElement("ficha");
                    writer.WriteStartElement("color");
                    writer.WriteString(tablero[i][0]); //COLOR
                    writer.WriteEndElement();
                    writer.WriteStartElement("columna");
                    writer.WriteString(tablero[i][1]);//COLUMNA
                    writer.WriteEndElement();
                    writer.WriteStartElement("fila");
                    writer.WriteString(tablero[i][2]);//FILA
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
            }

            writer.WriteStartElement("siguienteTiro");
            writer.WriteStartElement("color");
            writer.WriteString("blanco");
            writer.WriteEndElement();
            writer.WriteEndElement();

            writer.WriteEndDocument();
            writer.Close();
        }

        public string[][] ToArray(Tablero tablero)
        {
            string[][] array =
            {
                new string[]{tablero.A1,"A","1"},
                new string[]{tablero.B1,"B","1"},
                new string[]{tablero.C1,"C","1"},
                new string[]{tablero.D1,"D","1"},
                new string[]{tablero.E1,"E","1"},
                new string[]{tablero.F1,"F","1"},
                new string[]{tablero.G1,"G","1"},
                new string[]{tablero.H1,"H","1"},

                new string[]{tablero.A2,"A","2"},
                new string[]{tablero.B2,"B","2"},
                new string[]{tablero.C2,"C","2"},
                new string[]{tablero.D2,"D","2"},
                new string[]{tablero.E2,"E","2"},
                new string[]{tablero.F2,"F","2"},
                new string[]{tablero.G2,"G","2"},
                new string[]{tablero.H2,"H","2"},

                new string[]{tablero.A3,"A","3"},
                new string[]{tablero.B3,"B","3"},
                new string[]{tablero.C3,"C","3"},
                new string[]{tablero.D3,"D","3"},
                new string[]{tablero.E3,"E","3"},
                new string[]{tablero.F3,"F","3"},
                new string[]{tablero.G3,"G","3"},
                new string[]{tablero.H3,"H","3"},

                new string[]{tablero.A4,"A","4"},
                new string[]{tablero.B4,"B","4"},
                new string[]{tablero.C4,"C","4"},
                new string[]{tablero.D4,"D","4"},
                new string[]{tablero.E4,"E","4"},
                new string[]{tablero.F4,"F","4"},
                new string[]{tablero.G4,"G","4"},
                new string[]{tablero.H4,"H","4"},

                new string[]{tablero.A5,"A","5"},
                new string[]{tablero.B5,"B","5"},
                new string[]{tablero.C5,"C","5"},
                new string[]{tablero.D5,"D","5"},
                new string[]{tablero.E5,"E","5"},
                new string[]{tablero.F5,"F","5"},
                new string[]{tablero.G5,"G","5"},
                new string[]{tablero.H5,"H","5"},

                new string[]{tablero.A6,"A","6"},
                new string[]{tablero.B6,"B","6"},
                new string[]{tablero.C6,"C","6"},
                new string[]{tablero.D6,"D","6"},
                new string[]{tablero.E6,"E","6"},
                new string[]{tablero.F6,"F","6"},
                new string[]{tablero.G6,"G","6"},
                new string[]{tablero.H6,"H","6"},

                new string[]{tablero.A7,"A","7"},
                new string[]{tablero.B7,"B","7"},
                new string[]{tablero.C7,"C","7"},
                new string[]{tablero.D7,"D","7"},
                new string[]{tablero.E7,"E","7"},
                new string[]{tablero.F7,"F","7"},
                new string[]{tablero.G7,"G","7"},
                new string[]{tablero.H7,"H","7"},

                new string[]{tablero.A8,"A","8"},
                new string[]{tablero.B8,"B","8"},
                new string[]{tablero.C8,"C","8"},
                new string[]{tablero.H8,"H","8"},
                new string[]{tablero.D8,"D","8"},
                new string[]{tablero.E8,"E","8"},
                new string[]{tablero.F8,"F","8"},
                new string[]{tablero.G8,"G","8"},
            };

            return array;
        }
    }
}
