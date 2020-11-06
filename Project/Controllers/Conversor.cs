using IPC2_P1.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Xml;

namespace IPC2_P1.Controllers
{
    public class Conversor
    {
        Othello Juego = new Othello();

        public Tablero ReadXML(HttpPostedFileBase archivo)
        {
            string result = "";
            using (BinaryReader b = new BinaryReader(archivo.InputStream))   // FUENTE: https://stackoverflow.com/questions/16030034/asp-net-mvc-read-file-from-httppostedfilebase-without-save/16030326
            {
                byte[] binData = b.ReadBytes(archivo.ContentLength);
                result = System.Text.Encoding.UTF8.GetString(binData);
            }

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(result);
                        
            Tablero tablero_cargado = new Tablero(8, 8, "", "", "Normal");
            int lado = 8;

            foreach (XmlNode nodo in doc.SelectNodes("tablero")[0].SelectNodes("ficha"))
            {
                int pos = -1;
                string color_temp = nodo.ChildNodes[0].InnerText;
                string fila = nodo.ChildNodes[2].InnerText, columna = nodo.ChildNodes[1].InnerText;

                switch (fila)
                {
                    case "1":
                        pos = lado * 0;
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

                switch (columna)
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

                try
                { 
                    if (tablero_cargado.fichas[pos].color == null)
                        Juego.Reemplazar(tablero_cargado, pos, color_temp);
                }
                catch { }
            }
            
            int count = 0;

            foreach (Ficha ficha in tablero_cargado.fichas)
            {
                if (ficha.color != null)
                    count++;
            }

            string color = doc.SelectNodes("tablero")[0].SelectNodes("siguienteTiro")[0].InnerText;                       

            tablero_cargado.Actualizar(color, Globals.usuario_activo, count / 2 - 2, count / 2 - 2);


            return tablero_cargado;
        }

        public Tablero XReadXML(HttpPostedFileBase archivo)
        {
            string result = "";
            using (BinaryReader b = new BinaryReader(archivo.InputStream))   // FUENTE: https://stackoverflow.com/questions/16030034/asp-net-mvc-read-file-from-httppostedfilebase-without-save/16030326
            {
                byte[] binData = b.ReadBytes(archivo.ContentLength);
                result = System.Text.Encoding.UTF8.GetString(binData);
            }

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(result);

            int filas = Int32.Parse(doc.SelectNodes("partida")[0].SelectNodes("filas")[0].InnerText);
            int columnas = Int32.Parse(doc.SelectNodes("partida")[0].SelectNodes("columnas")[0].InnerText);
            string modalidad = doc.SelectNodes("partida")[0].SelectNodes("Modalidad")[0].InnerText;


            Tablero tablero_cargado = new Tablero(filas, columnas,"", "", modalidad);

            foreach (XmlNode nodo in doc.SelectNodes("partida")[0].SelectNodes("tablero")[0].SelectNodes("ficha"))
            {
                int pos = -1;
                string color_temp = nodo.ChildNodes[0].InnerText;
                string fila = nodo.ChildNodes[2].InnerText, columna = nodo.ChildNodes[1].InnerText;

                switch (fila)
                {
                    case "1":
                        pos = columnas * 0;
                        break;

                    case "2":
                        pos = columnas * 1;
                        break;

                    case "3":
                        pos = columnas * 2;
                        break;

                    case "4":
                        pos = columnas * 3;
                        break;

                    case "5":
                        pos = columnas * 4;
                        break;

                    case "6":
                        pos = columnas * 5;
                        break;

                    case "7":
                        pos = columnas * 6;
                        break;

                    case "8":
                        pos = columnas * 7;
                        break;

                    case "9":
                        pos = columnas * 8;
                        break;

                    case "10":
                        pos = columnas * 9;
                        break;

                    case "11":
                        pos = columnas * 10;
                        break;

                    case "12":
                        pos = columnas * 11;
                        break;

                    case "13":
                        pos = columnas * 12;
                        break;

                    case "14":
                        pos = columnas * 13;
                        break;

                    case "15":
                        pos = columnas * 14;
                        break;

                    case "16":
                        pos = columnas * 15;
                        break;

                    case "17":
                        pos = columnas * 16;
                        break;

                    case "18":
                        pos = columnas * 17;
                        break;

                    case "19":
                        pos = columnas * 18;
                        break;

                    case "20":
                        pos = columnas * 19;
                        break;

                }

                switch (columna)
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

                    case "I":
                        pos += 8;
                        break;

                    case "J":
                        pos += 9;
                        break;

                    case "K":
                        pos += 10;
                        break;

                    case "L":
                        pos += 11;
                        break;

                    case "M":
                        pos += 12;
                        break;

                    case "N":
                        pos += 13;
                        break;

                    case "O":
                        pos += 14;
                        break;

                    case "P":
                        pos += 15;
                        break;

                    case "Q":
                        pos += 16;
                        break;

                    case "R":
                        pos += 17;
                        break;

                    case "S":
                        pos += 18;
                        break;

                    case "T":
                        pos += 19;
                        break;

                }

                try
                {
                    if (tablero_cargado.fichas[pos].color == null)
                        Juego.Reemplazar(tablero_cargado, pos, color_temp);
                }
                catch { }
            }

            int count = 0;
            foreach (Ficha ficha in tablero_cargado.fichas)
            {
                if (ficha.color != null)
                    count++;
            }

            List<string> colores=new List<string>(), colores2=new List<string>();

            foreach (XmlNode nodo in doc.SelectNodes("partida")[0].SelectNodes("Jugador1")[0].SelectNodes("color"))
            {
                colores.Add(nodo.InnerText);
            }

            foreach (XmlNode nodo in doc.SelectNodes("partida")[0].SelectNodes("Jugador2")[0].SelectNodes("color"))
            {
                colores2.Add(nodo.InnerText);
            }
            tablero_cargado.colores = colores;
            tablero_cargado.colores_oponente = colores2;
            
            string color = doc.SelectNodes("partida")[0].SelectNodes("tablero")[0].SelectNodes("siguienteTiro")[0].InnerText;
            
            tablero_cargado.Actualizar(color, Globals.usuario_activo, count / 2 - 2, count / 2 - 2);
            
            return tablero_cargado;
        }


        public void WriteXML(Tablero tablero, string ruta)
        {

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

            writer.WriteEndElement();

            writer.WriteEndDocument();
            writer.Close();
        }

        public void XWriteXML(Tablero tablero, string ruta)
        {

            XmlTextWriter writer = null;
            writer = new XmlTextWriter(ruta, null);

            writer.WriteStartDocument();
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 3;

            writer.WriteStartElement("partida");

            writer.WriteStartElement("filas");
            writer.WriteString(tablero.filas.ToString()); //FILAS
            writer.WriteEndElement();

            writer.WriteStartElement("columnas");
            writer.WriteString(tablero.columnas.ToString()); //COLUMNAS
            writer.WriteEndElement();
            
            writer.WriteStartElement("Jugador1");

            foreach(string color in tablero.colores)
            {
                writer.WriteStartElement("color");
                writer.WriteString(color); //COLOR
                writer.WriteEndElement();
            }

            writer.WriteEndElement();

            writer.WriteStartElement("Jugador2");

            foreach (string color in tablero.colores_oponente)
            {
                writer.WriteStartElement("color");
                writer.WriteString(color); //COLOR
                writer.WriteEndElement();
            }

            writer.WriteEndElement();


            writer.WriteStartElement("Modalidad");
            writer.WriteString(tablero.modalidad); //MODALIDAD
            writer.WriteEndElement();

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

            writer.WriteEndElement();
            writer.WriteEndElement();

            writer.WriteEndDocument();
            writer.Close();
        }

    }
}