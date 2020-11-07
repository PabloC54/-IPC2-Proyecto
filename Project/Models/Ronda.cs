using System.Collections.Generic;

namespace IPC2_P1.Models
{
    public class Ronda
    {

        public string Nombre_campeonato { get; set; }

        public List<Equipo> Equipos { get; set; }

        public Equipo Equipo1 { get; set; }

        public Equipo Equipo2 { get; set; }

        public int Numero_equipos { get; set; }

        public List<Partida> Partidas{ get; set; }

        public Partida Partida_actual{ get; set; }

        public int Numero_partidas { get; set; }

        public int index { get; set; }
    }
}