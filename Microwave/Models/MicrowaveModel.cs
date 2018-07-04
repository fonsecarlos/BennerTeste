using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Microwave.Models
{
    public class MicrowaveModel
    {

        public MicrowaveModel()
        {
            Programas = new List<Programa>();
            ListaProgramas = new List<SelectListItem>();
            Programas.Add(new Programa() { Cod = 1, Alimento = "Frango", Potencia = 3, Segundos = 120, Descricao = "Programa Frango: Aquece o alimento na potência 3 por 02:00", Caracter = '-' });
            Programas.Add(new Programa() { Cod = 2, Alimento = "Pipoca", Potencia = 6, Segundos = 60, Descricao = "Programa Pipoca: Aquece o alimento na potência 6 por 01:00", Caracter = '*'});
            Programas.Add(new Programa() { Cod = 3, Alimento = "Carne", Potencia = 10, Segundos = 120, Descricao = "Programa Carne: Aquece o alimento na potência 10 por 02:00", Caracter = '+' });
            Programas.Add(new Programa() { Cod = 4, Alimento = "Brigadeiro", Potencia = 5, Segundos = 90, Descricao = "Programa Brigadeiro: Aquece o alimento na potência 5 por 01:30", Caracter = '/' });
            Programas.Add(new Programa() { Cod = 5, Alimento = "Arroz", Potencia = 7, Segundos = 100, Descricao = "Programa Arroz: Aquece o alimento na potência 73 por 01:40", Caracter = ',' });

            ListaProgramas.Add(new SelectListItem() { Text = "Selecione", Value = "0", Selected = true });
            foreach (var item in Programas)
            {
                ListaProgramas.Add(new SelectListItem() { Text = item.Alimento, Value = item.Cod.ToString() });
            }

            Identificador = Guid.NewGuid().ToString();

            SimulaTempo = false;

        }

        [Display(Name = "Entrada")]
        [Required]
        public string Entrada { get; set; }
        

        [Display(Name = "Programa")]
        public string ProgramaSelecionado { get; set; }
        public List<SelectListItem> ListaProgramas { get; set; }

        public List<Programa> Programas { get; set; }

        [Display(Name = "Resultado")]
        public string Resultado;

        public string Identificador { get; set; }

        [Display(Name = "Simular Tempo")]
        public Boolean SimulaTempo { get; set; }


    }
}