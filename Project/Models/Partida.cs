
namespace IPC2_P1.Models
{
    public class Partida
    {

        public Tablero tablero { get; set; }

        public string Jugador1 { get; set; }

        public string Jugador2 { get; set; }

        public int Puntos1 { get; set; }

        public int Puntos2 { get; set; }

        public string Ganador { get; set; }

        public int index { get; set; }
        
    }
}