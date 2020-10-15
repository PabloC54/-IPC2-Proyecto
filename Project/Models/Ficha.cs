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
            presionado = "false";
        }

        public Ficha(string color)
        {
            this.color = color;
            presionado = "false";
        }
        

        public string color { get; set; }

        public string presionado { get; set; }
        
    }
}