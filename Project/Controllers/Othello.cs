using IPC2_P1.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace IPC2_P1.Controllers
{
    public class Othello
    {
        public static string sql = "Data Source=PCP-PC;Initial Catalog=Othello_db;User ID=pabloc54;Password=pablo125";
        public SqlConnection con = new SqlConnection(sql);

        public List<int> Flanquear(Tablero tablero, int index, string color, string color_opuesto)
        {
            List<int> lista = new List<int>();

            bool izq_f = (index - 1 >= 0) && (index / 8 == (index - 1) / 8);
            bool der_f = (index + 1 < 64) && (index / 8 == (index + 1) / 8);
            bool sup_f = (index - 8 >= 0);
            bool inf_f = (index + 8 < 64);


            // CAMBIANDO FICHAS ENCERRADAS

            if (index >= 0)
            {

                // FICHAS A LA IZQUIERDA
                if (izq_f)
                {
                    if (tablero.fichas[index - 1].color == color_opuesto)
                    {
                        int acc = -2;
                        List<int> lista_temp = new List<int>();
                        lista_temp.Add(index - 1);

                        bool salir = false;
                        while (salir == false)
                        {
                            bool izq = (index + acc >= 0) && (index / 8 == (index + acc) / 8);
                            if (izq)
                            {
                                if (tablero.fichas[index + acc].color == color_opuesto)
                                {
                                    lista_temp.Add(index + acc);

                                }
                                else
                                {
                                    if (tablero.fichas[index + acc].color == color)
                                    {
                                        lista.AddRange(lista_temp);
                                    }
                                    else
                                    {
                                        salir = true;
                                    }
                                }
                            }
                            else
                            {
                                salir = true;
                            }

                            acc -= 1;
                        }
                    }
                }

                // FICHAS A LA DERECHA
                if (der_f)
                {
                    if (tablero.fichas[index + 1].color == color_opuesto)
                    {
                        int acc = 2;
                        List<int> lista_temp = new List<int>();
                        lista_temp.Add(index + 1);

                        bool salir = false;
                        while (salir == false)
                        {
                            bool der = (index + acc < 64) && (index / 8 == (index + acc) / 8);
                            if (der)
                            {
                                if (tablero.fichas[index + acc].color == color_opuesto)
                                {
                                    lista_temp.Add(index + acc);

                                }
                                else
                                {
                                    if (tablero.fichas[index + acc].color == color)
                                    {
                                        lista.AddRange(lista_temp);
                                    }
                                    else
                                    {
                                        salir = true;
                                    }
                                }
                            }
                            else
                            {
                                salir = true;
                            }

                            acc += 1;
                        }
                    }
                }

                // FICHAS SUPERIOR
                if (sup_f)
                {
                    if (tablero.fichas[index - 8].color == color_opuesto)
                    {
                        int acc = -16;
                        List<int> lista_temp = new List<int>();
                        lista_temp.Add(index - 8);

                        bool salir = false;
                        while (salir == false)
                        {
                            bool sup = (index + acc >= 0);
                            if (sup)
                            {
                                if (tablero.fichas[index + acc].color == color_opuesto)
                                {
                                    lista_temp.Add(index + acc);

                                }
                                else
                                {
                                    if (tablero.fichas[index + acc].color == color)
                                    {
                                        lista.AddRange(lista_temp);
                                    }
                                    else
                                    {
                                        salir = true;
                                    }
                                }
                            }
                            else
                            {
                                salir = true;
                            }

                            acc -= 8;
                        }
                    }
                }

                // FICHAS INFERIOR
                if (inf_f)
                {
                    if (tablero.fichas[index + 8].color == color_opuesto)
                    {
                        int acc = 16;
                        List<int> lista_temp = new List<int>();
                        lista_temp.Add(index + 8);

                        bool salir = false;
                        while (salir == false)
                        {
                            bool inf = (index + acc < 64);
                            if (inf)
                            {
                                if (tablero.fichas[index + acc].color == color_opuesto)
                                {
                                    lista_temp.Add(index + acc);

                                }
                                else
                                {
                                    if (tablero.fichas[index + acc].color == color)
                                    {
                                        lista.AddRange(lista_temp);
                                    }
                                    else
                                    {
                                        salir = true;
                                    }
                                }
                            }
                            else
                            {
                                salir = true;
                            }

                            acc += 8;
                        }
                    }
                }

                // FICHAS IZQ SUP
                if (izq_f && sup_f)
                {
                    if (tablero.fichas[index - 9].color == color_opuesto)
                    {
                        int acc = -2; //izq
                        int acc2 = -16; //sup
                        List<int> lista_temp = new List<int>();
                        lista_temp.Add(index - 9);

                        bool salir = false;
                        while (salir == false)
                        {
                            bool izq = (index + acc >= 0) && (index / 8 == (index + acc) / 8);
                            bool sup = (index + acc2 >= 0);

                            if (izq && sup)
                            {
                                if (tablero.fichas[index + acc + acc2].color == color_opuesto)
                                {
                                    lista_temp.Add(index + acc + acc2);

                                }
                                else
                                {
                                    if (tablero.fichas[index + acc + acc2].color == color)
                                    {
                                        lista.AddRange(lista_temp);
                                    }
                                    else
                                    {
                                        salir = true;
                                    }
                                }
                            }
                            else
                            {
                                salir = true;
                            }

                            acc -= 1;
                            acc2 -= 8;
                        }
                    }
                }

                // FICHAS DER SUP
                if (der_f && sup_f)
                {
                    if (tablero.fichas[index - 7].color == color_opuesto)
                    {
                        int acc = 2; //der
                        int acc2 = -16; //sup
                        List<int> lista_temp = new List<int>();
                        lista_temp.Add(index - 7);

                        bool salir = false;
                        while (salir == false)
                        {
                            bool der = (index + acc < 64) && (index / 8 == (index + acc) / 8);
                            bool sup = (index + acc2 >= 0);

                            if (der && sup)
                            {
                                if (tablero.fichas[index + acc + acc2].color == color_opuesto)
                                {
                                    lista_temp.Add(index + acc + acc2);
                                }
                                else
                                {
                                    if (tablero.fichas[index + acc + acc2].color == color)
                                    {
                                        lista.AddRange(lista_temp);
                                    }
                                    else
                                    {
                                        salir = true;
                                    }
                                }
                            }
                            else
                            {
                                salir = true;
                            }

                            acc += 1;
                            acc2 -= 8;
                        }
                    }
                }

                // FICHAS IZQ INF
                if (izq_f && inf_f)
                {
                    if (tablero.fichas[index + 7].color == color_opuesto)
                    {
                        int acc = -2; //izq
                        int acc2 = 16; //inf
                        List<int> lista_temp = new List<int>();
                        lista_temp.Add(index + 7);

                        bool salir = false;
                        while (salir == false)
                        {
                            bool izq = (index + acc >= 0) && (index / 8 == (index + acc) / 8);
                            bool inf = (index + acc2 < 64);

                            if (izq && inf)
                            {
                                if (tablero.fichas[index + acc + acc2].color == color_opuesto)
                                {
                                    lista_temp.Add(index + acc + acc2);
                                }
                                else
                                {
                                    if (tablero.fichas[index + acc + acc2].color == color)
                                    {
                                        lista.AddRange(lista_temp);
                                    }
                                    else
                                    {
                                        salir = true;
                                    }
                                }
                            }
                            else
                            {
                                salir = true;
                            }

                            acc -= 1;
                            acc2 += 8;
                        }
                    }
                }

                // FICHAS DER INF
                if (der_f && inf_f)
                {
                    if (tablero.fichas[index + 9].color == color_opuesto)
                    {
                        int acc = 2; //der
                        int acc2 = 16; //inf
                        List<int> lista_temp = new List<int>();
                        lista_temp.Add(index + 9);

                        bool salir = false;
                        while (salir == false)
                        {
                            bool der = (index + acc < 64) && (index / 8 == (index + acc) / 8);
                            bool inf = (index + acc2 < 64);

                            if (der && inf)
                            {
                                if (tablero.fichas[index + acc + acc2].color == color_opuesto)
                                {
                                    lista_temp.Add(index + acc + acc2);
                                }
                                else
                                {
                                    if (tablero.fichas[index + acc + acc2].color == color)
                                    {
                                        lista.AddRange(lista_temp);
                                    }
                                    else
                                    {
                                        salir = true;
                                    }
                                }
                            }
                            else
                            {
                                salir = true;
                            }

                            acc += 1;
                            acc2 += 8;
                        }
                    }
                }

            }

            return lista;
        }

        public void Reemplazar(Tablero tablero, int pos, string valor)
        {
            if (pos >= 0)
                tablero.fichas[pos].color = valor;
        }

        public void Reemplazar_lista(Tablero tablero, List<int> pos_list, string valor)
        {
            foreach (int pos in pos_list)
                if (pos >= 0)
                    tablero.fichas[pos].color = valor;

        }

        public int Ficha_seleccionada(Tablero tablero)
        {
            int index = 0, acc = 0;
            
            foreach (Ficha ficha in tablero.fichas)
            {
                if (ficha.presionado == "true")
                    index = acc;

                acc++;
            }

            return index;
        }

        public List<int> Celdas_validas(Tablero tablero, string color, string color_opuesto)
        {
            List<int> celdas_vacias = Celdas_vacias(tablero);

            List<int> lista_temp = new List<int>();
            List<int> celdas_validas = new List<int>();            

            foreach (int celda in celdas_vacias) //iterando en las celdas vacias, para ver si son celdas validas (generan cambios)
            {
                lista_temp = Flanquear(tablero, celda, color, color_opuesto);
                if (lista_temp.Count > 0)
                    celdas_validas.Add(celda);
            }

            return celdas_validas;
        }
        
        public List<int> Celdas_vacias(Tablero tablero)
        {
            List<int> celdas_vacias = new List<int>();

            int acc = 0;
            foreach (Ficha ficha in tablero.fichas) //reconociendo celdas vacias
            {
                if (ficha.color == null)
                    celdas_vacias.Add(acc);

                acc++;
            }

            return celdas_vacias;
        }

        public List<int> Celdas_ocupadas(Tablero tablero)
        {
            List<int> celdas_vacias = new List<int>();

            int acc = 0;
            foreach (Ficha ficha in tablero.fichas) //reconociendo celdas vacias
            {
                if (ficha.color != null)
                    celdas_vacias.Add(acc);

                acc++;
            }

            return celdas_vacias;
        }

        public void Movimiento_cpu(Tablero tablero, string color, string color_opuesto)
        {
            System.Random random = new System.Random();
            List<int> celdas_validas = Celdas_validas(tablero, color, color_opuesto);

            int num = random.Next(celdas_validas.Count);
            List<int> lista_temp = Flanquear(tablero, celdas_validas[num], color, color_opuesto);

            Reemplazar(tablero, celdas_validas[num], color);

            Reemplazar_lista(tablero, lista_temp, color);
            
        }

        public void Movimiento_cpu_apertura(Tablero tablero, string color)
        {
            int x = tablero.columnas, y = tablero.filas;

            List<int> temp = new List<int>();
            temp.Add((x / 2) * (y - 1) - 1);
            temp.Add((x / 2) * (y - 1));
            temp.Add((x / 2) * (y + 1) - 1);
            temp.Add((x / 2) * (y + 1));

            List<int> celdas_validas = new List<int>();

            foreach (int pos in temp)
            {
                if (tablero.fichas[pos].color == null)
                {
                    celdas_validas.Add(pos);
                }
            }

            System.Random random = new System.Random();
            int num = random.Next(celdas_validas.Count);

            Reemplazar(tablero, celdas_validas[num], color);            
        }
        
        public string Juego_terminado(Tablero tablero, string color_opuesto)
        {
            int count = 0, count2 = 0;

            foreach (Ficha ficha in tablero.fichas) //CONTANDO LAS FICHAS
            {
                if (tablero.usuario == Globals.usuario_activo)
                {
                    if (ficha.color == tablero.color) //usuario                                    
                        count++;
                    else //oponente                                    
                        if (ficha.color == color_opuesto)
                        count2++;
                }
                else
                {
                    if (ficha.color == tablero.color) //oponente                                    
                        count2++;
                    else //usuario                                    
                        if (ficha.color == color_opuesto)
                        count++;
                }
            }

            con.Open();

            string txt = "", mensaje = "";
            SqlCommand cmd = null;
            SqlDataReader dr = null;

            if (tablero.modalidad == "Normal")
            {
                if (count > count2) //usuario gano
                {
                    mensaje = "¡El juego ha terminado! El ganador es: " + Globals.usuario_activo + " con " + count + " fichas";
                    txt = "select partidas_ganadas from Reporte where username='" + Globals.usuario_activo + "'";
                }
                else
                {
                    if (count < count2) //usuario perdio
                    {
                        mensaje = "¡El juego ha terminado! El ganador es Oponente con " + count2 + " fichas";
                        txt = "select partidas_perdidas from Reporte where username='" + Globals.usuario_activo + "'";
                    }
                    else //usuario empato
                    {
                        mensaje = "¡¡El juego ha terminado! Hubo un empate con " + count + " fichas";
                        txt = "select partidas_empatadas from Reporte where username='" + Globals.usuario_activo + "'";
                    }
                }

            }
            else
            {
                if (count < count2) //usuario gano
                {
                    mensaje = "¡El juego ha terminado! El ganador es: " + Globals.usuario_activo + " con " + count + " fichas";
                    txt = "select partidas_ganadas from Reporte where username='" + Globals.usuario_activo + "'";
                }
                else
                {
                    if (count > count2) //usuario perdio
                    {
                        mensaje = "¡El juego ha terminado! El ganador es Oponente con " + count2 + " fichas";
                        txt = "select partidas_perdidas from Reporte where username='" + Globals.usuario_activo + "'";
                    }
                    else //usuario empato
                    {
                        mensaje = "¡¡El juego ha terminado! Hubo un empate con " + count + " fichas";
                        txt = "select partidas_empatadas from Reporte where username='" + Globals.usuario_activo + "'";
                    }
                }
            }

            cmd = new SqlCommand(txt, con);
            dr = cmd.ExecuteReader();
            dr.Read();
            int num = dr.GetInt32(0) + 1;
            dr.Close();

            if (tablero.modalidad == "Normal")
            {
                if (count > count2) //usuario gano
                {
                    txt = "update Reporte set partidas_ganadas=" + num + " where username='" + Globals.usuario_activo + "'";
                }
                else
                {
                    if (count < count2) //usuario perdio
                    {
                        txt = "update Reporte set partidas_perdidas=" + num + " where username='" + Globals.usuario_activo + "'";
                    }
                    else //usuario empato
                    {
                        txt = "update Reporte set partidas_empatadas=" + num + " where username='" + Globals.usuario_activo + "'";
                    }
                }
            }
            else
            {
                if (count < count2) //usuario gano
                {
                    txt = "update Reporte set partidas_ganadas=" + num + " where username='" + Globals.usuario_activo + "'";
                }
                else
                {
                    if (count > count2) //usuario perdio
                    {
                        txt = "update Reporte set partidas_perdidas=" + num + " where username='" + Globals.usuario_activo + "'";
                    }
                    else //usuario empato
                    {
                        txt = "update Reporte set partidas_empatadas=" + num + " where username='" + Globals.usuario_activo + "'";
                    }
                }
            }

            cmd = new SqlCommand(txt, con);
            dr = cmd.ExecuteReader();
            dr.Close();
            con.Close();

            return mensaje;
        }

    }
}