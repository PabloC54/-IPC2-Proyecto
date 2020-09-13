using IPC2_P1.Models;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace IPC2_P1.Controllers
{
    public class GameController : Controller
    {
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

        public ActionResult Versus()
        {
            if (Globals.logged_in == true)
            {
                return View(new Tablero
                {
                    D4 = "blanco",
                    E4 = "negro",
                    D5 = "negro",
                    E5 = "blanco"
                });
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

            Tablero tablero_cargado = new Tablero
            {
                A1 = doc.ChildNodes[1].ChildNodes[0].ChildNodes[0].InnerText,
                B1 = doc.ChildNodes[1].ChildNodes[1].ChildNodes[0].InnerText,
                C1 = doc.ChildNodes[1].ChildNodes[2].ChildNodes[0].InnerText,
                D1 = doc.ChildNodes[1].ChildNodes[3].ChildNodes[0].InnerText,
                E1 = doc.ChildNodes[1].ChildNodes[4].ChildNodes[0].InnerText,
                F1 = doc.ChildNodes[1].ChildNodes[5].ChildNodes[0].InnerText,
                G1 = doc.ChildNodes[1].ChildNodes[6].ChildNodes[0].InnerText,
                H1 = doc.ChildNodes[1].ChildNodes[7].ChildNodes[0].InnerText,

                A2 = doc.ChildNodes[1].ChildNodes[8].ChildNodes[0].InnerText,
                B2 = doc.ChildNodes[1].ChildNodes[9].ChildNodes[0].InnerText,
                C2 = doc.ChildNodes[1].ChildNodes[10].ChildNodes[0].InnerText,
                D2 = doc.ChildNodes[1].ChildNodes[11].ChildNodes[0].InnerText,
                E2 = doc.ChildNodes[1].ChildNodes[12].ChildNodes[0].InnerText,
                F2 = doc.ChildNodes[1].ChildNodes[13].ChildNodes[0].InnerText,
                G2 = doc.ChildNodes[1].ChildNodes[14].ChildNodes[0].InnerText,
                H2 = doc.ChildNodes[1].ChildNodes[15].ChildNodes[0].InnerText,

                A3 = doc.ChildNodes[1].ChildNodes[16].ChildNodes[0].InnerText,
                B3 = doc.ChildNodes[1].ChildNodes[17].ChildNodes[0].InnerText,
                C3 = doc.ChildNodes[1].ChildNodes[18].ChildNodes[0].InnerText,
                D3 = doc.ChildNodes[1].ChildNodes[19].ChildNodes[0].InnerText,
                E3 = doc.ChildNodes[1].ChildNodes[20].ChildNodes[0].InnerText,
                F3 = doc.ChildNodes[1].ChildNodes[21].ChildNodes[0].InnerText,
                G3 = doc.ChildNodes[1].ChildNodes[22].ChildNodes[0].InnerText,
                H3 = doc.ChildNodes[1].ChildNodes[23].ChildNodes[0].InnerText,

                A4 = doc.ChildNodes[1].ChildNodes[24].ChildNodes[0].InnerText,
                B4 = doc.ChildNodes[1].ChildNodes[25].ChildNodes[0].InnerText,
                C4 = doc.ChildNodes[1].ChildNodes[26].ChildNodes[0].InnerText,
                D4 = doc.ChildNodes[1].ChildNodes[27].ChildNodes[0].InnerText,
                E4 = doc.ChildNodes[1].ChildNodes[28].ChildNodes[0].InnerText,
                F4 = doc.ChildNodes[1].ChildNodes[29].ChildNodes[0].InnerText,
                G4 = doc.ChildNodes[1].ChildNodes[30].ChildNodes[0].InnerText,
                H4 = doc.ChildNodes[1].ChildNodes[31].ChildNodes[0].InnerText,

                A5 = doc.ChildNodes[1].ChildNodes[32].ChildNodes[0].InnerText,
                B5 = doc.ChildNodes[1].ChildNodes[33].ChildNodes[0].InnerText,
                C5 = doc.ChildNodes[1].ChildNodes[34].ChildNodes[0].InnerText,
                D5 = doc.ChildNodes[1].ChildNodes[35].ChildNodes[0].InnerText,
                E5 = doc.ChildNodes[1].ChildNodes[36].ChildNodes[0].InnerText,
                F5 = doc.ChildNodes[1].ChildNodes[37].ChildNodes[0].InnerText,
                G5 = doc.ChildNodes[1].ChildNodes[38].ChildNodes[0].InnerText,
                H5 = doc.ChildNodes[1].ChildNodes[39].ChildNodes[0].InnerText,

                A6 = doc.ChildNodes[1].ChildNodes[40].ChildNodes[0].InnerText,
                B6 = doc.ChildNodes[1].ChildNodes[41].ChildNodes[0].InnerText,
                C6 = doc.ChildNodes[1].ChildNodes[42].ChildNodes[0].InnerText,
                D6 = doc.ChildNodes[1].ChildNodes[43].ChildNodes[0].InnerText,
                E6 = doc.ChildNodes[1].ChildNodes[44].ChildNodes[0].InnerText,
                F6 = doc.ChildNodes[1].ChildNodes[45].ChildNodes[0].InnerText,
                G6 = doc.ChildNodes[1].ChildNodes[46].ChildNodes[0].InnerText,
                H6 = doc.ChildNodes[1].ChildNodes[47].ChildNodes[0].InnerText,

                A7 = doc.ChildNodes[1].ChildNodes[48].ChildNodes[0].InnerText,
                B7 = doc.ChildNodes[1].ChildNodes[49].ChildNodes[0].InnerText,
                C7 = doc.ChildNodes[1].ChildNodes[50].ChildNodes[0].InnerText,
                D7 = doc.ChildNodes[1].ChildNodes[51].ChildNodes[0].InnerText,
                E7 = doc.ChildNodes[1].ChildNodes[52].ChildNodes[0].InnerText,
                F7 = doc.ChildNodes[1].ChildNodes[53].ChildNodes[0].InnerText,
                G7 = doc.ChildNodes[1].ChildNodes[54].ChildNodes[0].InnerText,
                H7 = doc.ChildNodes[1].ChildNodes[55].ChildNodes[0].InnerText,

                A8 = doc.ChildNodes[1].ChildNodes[56].ChildNodes[0].InnerText,
                B8 = doc.ChildNodes[1].ChildNodes[57].ChildNodes[0].InnerText,
                C8 = doc.ChildNodes[1].ChildNodes[58].ChildNodes[0].InnerText,
                D8 = doc.ChildNodes[1].ChildNodes[59].ChildNodes[0].InnerText,
                E8 = doc.ChildNodes[1].ChildNodes[60].ChildNodes[0].InnerText,
                F8 = doc.ChildNodes[1].ChildNodes[61].ChildNodes[0].InnerText,
                G8 = doc.ChildNodes[1].ChildNodes[62].ChildNodes[0].InnerText,
                H8 = doc.ChildNodes[1].ChildNodes[63].ChildNodes[0].InnerText
            };
            
            return tablero_cargado;
        }


        //GUARDAR TABLERO
        public void WriteXML(string[][] tablero)
        {
            int num = 1;
            /*while (System.IO.File.Exists("~/wwwroot/xml/tablero" + num + ".xml") == true)
            {
                num += 1;
            }*/
            //string directorio = Directory.GetCurrentDirectory("~/wwwroot/");
            XmlTextWriter writer = null;
            writer = new XmlTextWriter("C:/Users/pablo/Downloads/archivo" + num + ".xml", null);

            writer.WriteStartDocument();
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 3;

            writer.WriteStartElement("tablero");

            for (int i = 0; i < 64; i++)
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
