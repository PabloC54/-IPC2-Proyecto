using System.Collections.Generic;

namespace IPC2_P1.Models
{
    public class Tablero
    {
        public Tablero(){}

        public Tablero(int filas, int columnas, string color, string usuario, string modalidad)
        {
            this.filas = filas;
            this.columnas = columnas;
            this.color = color;
            this.usuario = usuario;
            this.modalidad = modalidad;
            cronometro_agotado = "false";

            fichas = new List<Ficha>();

            //verificando si es par
            if (filas % 2 == 1)
            {
                filas--;
            }
            if (columnas % 2 == 1)
            {
                columnas--;
            }

            //verificando que esté dentro de los límites (min=6, max=20)
            if (filas < 6)
                filas = 6;
            else
                if (filas > 20)
                filas = 20;

            if (columnas < 6)
                columnas = 6;
            else
                if (columnas > 20)
                columnas = 20;

            for (int i = 0; i < filas * columnas; i++)
            {
                fichas.Add(new Ficha());
            }
                 
        }
               


        public void Iniciar(bool apertura_personalizada)
        {
            if (apertura_personalizada)
            {
                this.apertura_personalizada = "true";
                cronometro_agotado = "true";
            }
            else
            {
                this.apertura_personalizada = "false";
                int x = columnas, y = filas;
                fichas[(x / 2) * (y - 1) - 1].color = "blanco";
                fichas[(x / 2) * (y - 1)].color = "negro";
                fichas[(x / 2) * (y + 1) - 1].color = "negro";
                fichas[(x / 2) * (y + 1)].color = "blanco";
            }
        }

        public bool Apertura(int pos)
        {
            int x = columnas, y = filas;

            List<int> temp = new List<int>();
            temp.Add((x / 2) * (y - 1) - 1);
            temp.Add((x / 2) * (y - 1));
            temp.Add((x / 2) * (y + 1) - 1);
            temp.Add((x / 2) * (y + 1));

            if (temp.Contains(pos))            
                return true;            
            else
                return false;
        }

        public void Actualizar(string color, string usuario, int movimientos, int movimientos_oponente)
        {
            this.color = color;
            this.usuario = usuario;
            this.movimientos = movimientos;
            this.movimientos_oponente = movimientos_oponente;
        }



        public List<Ficha> fichas { get; set; }

        public int filas { get; set; }

        public int columnas { get; set; }

        public string usuario { get; set; }

        public string color { get; set; }

        public int movimientos { get; set; }

        public int movimientos_oponente { get; set; }


        public string modalidad { get; set; }

        public string apertura_personalizada { get; set; }


        public string cronometro_agotado { get; set; }
    }
}