using System;
using System.Collections.Generic;

namespace IPC2_P1.Models
{
    public class Tablero
    {
        public Tablero(){}

        public Tablero(int filas, int columnas, string color, string usuario)
        {
            this.filas = filas;
            this.columnas = columnas;
            this.color = color;
            this.usuario = usuario;

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

        public void Iniciar()
        {
            int x = columnas, y = filas;
            fichas[(x / 2) * (y - 1)-1].color = "blanco";
            fichas[(x / 2) * (y - 1)].color = "negro";
            fichas[(x/2)*(y+1)-1].color = "negro";
            fichas[(x / 2) * (y + 1)].color = "blanco";
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

    }
}