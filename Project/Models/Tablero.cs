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
            cronometro = 0;
            cronometro_oponente = 0;

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

        public void XIniciar(bool apertura_personalizada)
        {
            if (apertura_personalizada)
            {
                this.apertura_personalizada = "true";
            }
            else
            {
                this.apertura_personalizada = "false";
                int x = columnas, y = filas;

                fichas[(x / 2) * (y - 1) - 1].color = colores[0];

                try
                {
                    fichas[(x / 2) * (y - 1)].color = colores[1];
                }
                catch
                {
                    fichas[(x / 2) * (y - 1)].color = colores[0];
                }

                fichas[(x / 2) * (y + 1) - 1].color = colores_oponente[0];

                try
                {
                    fichas[(x / 2) * (y + 1)].color = colores_oponente[1];
                }
                catch
                {
                    fichas[(x / 2) * (y + 1)].color = colores_oponente[0];
                }
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

        public void XActualizar(string usuario, int movimientos, int movimientos_oponente)
        {
            int acc = 0;

            if (colores.Contains(color))
            {
                foreach (string color_temp in colores)
                {
                    if (color_temp == color)
                    {
                        color = colores_oponente[acc];
                    }
                    acc++;
                }
            }
            else
            {
                foreach (string color_temp in colores_oponente)
                {
                    if (color_temp == color)
                    {
                        try
                        {
                            color = colores[acc + 1];
                        }
                        catch
                        {
                            color = colores[0];
                        }
                    }
                    acc++;
                }
            }
            
            this.usuario = usuario;
            this.movimientos = movimientos;
            this.movimientos_oponente = movimientos_oponente;
        }


        public List<Ficha> fichas { get; set; }

        public int filas { get; set; }

        public int columnas { get; set; }

        public string usuario { get; set; }

        public string color { get; set; }

        public List<string> colores { get; set; }

        public List<string> colores_oponente { get; set; }

        public int movimientos { get; set; }

        public int movimientos_oponente { get; set; }


        public string modalidad { get; set; }

        public string apertura_personalizada { get; set; }


        public int cronometro { get; set; }

        public int cronometro_oponente { get; set; }
    }
}