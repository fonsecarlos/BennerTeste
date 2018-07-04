using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microwave.Models;

namespace Microwave.Controllers
{
    public class MicrowaveController : Controller
    {
        public ActionResult Index()
        {

            MicrowaveModel model = new MicrowaveModel();

            SalvarProgramaSessao(model);

            return View(model);
        }

        

        public ActionResult Iniciar()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Iniciar(MicrowaveModel model)
        {
            try
            {
                string entrada = model.Entrada;
                char caracter = '.';
                int potencia = 1;
                int tempo = 0;
                List<string> param = entrada.Split(' ').ToList();
                string alimento;
                if (model.ProgramaSelecionado == "0")
                {
                    switch (param.Count)
                    {
                        case 1:
                            alimento = param[0];
                            if (alimento != "")
                            {
                                tempo = 30;
                                potencia = 8;
                                model.Resultado = Aquece(param[0], tempo, potencia, caracter, model.SimulaTempo);
                            }
                            break;
                        case 2:
                            break;
                        case 3:
                            alimento = param[0];
                            List<string> auxTempo = param[1].Split(':').ToList();
                            int minutos = 0;
                            minutos = Int32.Parse(auxTempo[0]);
                            tempo = Int32.Parse(auxTempo[1]);
                            potencia = Int32.Parse(param[2]);
                            tempo = tempo + (minutos * 60);
                            if (tempo > 120 || tempo < 1)
                            {
                                throw new Exception("Tempo Inválido! O tempo máximo deve estar entre 00:01 e  02:00");
                            }
                            if (potencia < 1 || potencia > 10)
                            {
                                throw new Exception("A potencia deve estar entre 1 e 10");
                            }
                            model.Resultado = Aquece(alimento, tempo, potencia, caracter, model.SimulaTempo);
                            break;

                    }
                }
                else
                {
                    var lista = RetornaProgramaSessao(model.Identificador);
                    Programa programa = null;
                    if (lista.Any(x => x.Cod == Int32.Parse(model.ProgramaSelecionado)))
                    {
                        programa = lista.FirstOrDefault(x => x.Cod == Int32.Parse(model.ProgramaSelecionado));
                    }
                    if (entrada.ToLower().Contains(programa.Alimento.ToLower()))
                    {
                        model.Resultado = Aquece(programa.Alimento, programa.Segundos, programa.Potencia, programa.Caracter, model.SimulaTempo);
                    }
                    else
                    {
                        throw new Exception("Programa inválido para o tipo de alimento");
                    }
                }


            }

            catch (ArgumentOutOfRangeException e)
            {
                model.Resultado = "Formato de tempo inválido";
            }
            catch (Exception e)
            {
                model.Resultado = e.Message;
            }



            return PartialView("Resultado", model);
        }

        [HttpPost]
        public JsonResult BuscaDescricao(MicrowaveModel model)
        {
            var lista = RetornaProgramaSessao(model.Identificador);
            string descricao = lista.FirstOrDefault(x => x.Cod == Int32.Parse(model.ProgramaSelecionado)).Descricao;
            return new JsonResult { Data = descricao };
        }

        
        public string Aquece(string entrada, int segundos, int potencia, char caracter, bool simulaTempo)
        {
            int i = 0;
            string fatorAquec = "";
            while (i < potencia)
            {
                fatorAquec += caracter;
                i++;
            }
            i = 0;
            while (i < segundos)
            {
                    if (simulaTempo)
                    {
                        System.Threading.Thread.Sleep(1000); //Delay de 1 segundo para simular o tempo de aquecimento
                    }


                    entrada += fatorAquec;
                
                i++;

            }
            return entrada;
        }

        #region ProgramaSessao
        public JsonResult SalvaPrograma(MicrowaveModel model)
        {
            List<string> param = model.Entrada.Split(' ').ToList();
            if (param.Count() == 4)
            {
                var lista = RetornaProgramaSessao(model.Identificador);
                List<string> auxTempo = param[1].Split(':').ToList();
                int minutos = 0;
                minutos = Int32.Parse(auxTempo[0]);
                int segundos = Int32.Parse(auxTempo[1]);
                int cod = lista.Count()+1;
                string alimento = param[0];
                int potencia = Int32.Parse(param[2]);
                string descricao = "Programa "+alimento+": Aquece o alimento em potência "+potencia+" por "+minutos+":"+segundos;
                segundos += (minutos * 60);
                char caracter = param[3][0];



                lista.Add(new Programa { Cod = cod, Alimento = alimento, Segundos = segundos, Potencia = potencia, Descricao = descricao, Caracter = caracter });
                

                return new JsonResult { Data = new SelectListItem{ Text = alimento, Value = cod.ToString() } };

            }
            return new JsonResult { Data = "ERRO" };
        }

        public void SalvarProgramaSessao(MicrowaveModel model)
        {

            Session[ChaveSessao(model.Identificador)] = model.Programas;
        }

        public List<Programa> RetornaProgramaSessao(string identificador)
        {
            if ((List<Programa>)Session[ChaveSessao(identificador)] == null)
            {
                return new List<Programa>();
            }
            return (List<Programa>)Session[ChaveSessao(identificador)];
        }

        public string ChaveSessao(string identificador)
        {
            return "programas" + identificador;
        }
        #endregion //Métodos para salavar e recuperar os Programas em Sessão
    }
}