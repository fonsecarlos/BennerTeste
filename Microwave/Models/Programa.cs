using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microwave.Models
{
    public class Programa
    {
        public int Cod { get; set; }
        public string Alimento { get; set; }
        public int Potencia { get; set; }
        public int Segundos { get; set; }
        public string Descricao { get; set; }
        public char Caracter { get; set; }
    }
}