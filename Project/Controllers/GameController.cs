using IPC2_P1.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace IPC2_P1.Controllers
{
    public class GameController : Controller
    {
        public ActionResult Solo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Solo(Tablero tablero)
        {
            WriteXML(ToArray(tablero));

            return View();
        }

        [HttpPost]
        public ActionResult Solo_UploadFile(Archivo archivo)
        {
            Console.Write(archivo.archivo);
            
            return View("Solo");
        }

        public ActionResult Versus()
        {
            return View();
        }


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
