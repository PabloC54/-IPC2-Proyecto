using System.Collections.Generic;

namespace IPC2_P1.Models
{
    public class Campeonato
    {

        public string Nombre_campeonato { get; set; }

        public List<Equipo> Equipos { get; set; }

        public int Numero_equipos { get; set; }

        public List<Ronda> Rondas { get; set; }

        public Ronda Ronda_actual { get; set; }

        public int Numero_rondas { get; set; }

    }
}