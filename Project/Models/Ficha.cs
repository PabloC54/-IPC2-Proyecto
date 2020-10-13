using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IPC2_P1.Models
{
    public class Ficha
    {

        public Ficha()
        {
            this.valor = "";
            this.presionado = "false";
        }

        public Ficha(string valor)
        {
            this.valor = valor;
            this.presionado = "false";
        }

        public Ficha(string valor, string presionado, int num1, int num2)
        {
            this.valor = valor;
            this.presionado = presionado;
            this.mov1 = num1;
            this.mov2 = num2;
        }


        public string valor { get; set; }

        public string presionado { get; set; }

        public int mov1 { get; set; }

        public int mov2 { get; set; }

    }
}