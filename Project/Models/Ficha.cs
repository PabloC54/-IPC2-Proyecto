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

        public Ficha(string valor, string presionado)
        {
            this.valor = valor;
            this.presionado = presionado;
        }


        public string valor { get; set; }

        public string presionado { get; set; }

    }
}