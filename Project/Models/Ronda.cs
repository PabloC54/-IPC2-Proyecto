using System.Collections.Generic;

namespace IPC2_P1.Models
{
    public class Ronda
    {

        public int Numero_ronda { get; set; }

        public List<Equipo> Equipos { get; set; }

        public Equipo Equipo1 { get; set; }

        public Equipo Equipo2 { get; set; }

        public int Numero_equipos { get; set; }

        public List<Partida> Partidas{ get; set; }

        public Partida Partida_siguiente{ get; set; }
        
    }
}