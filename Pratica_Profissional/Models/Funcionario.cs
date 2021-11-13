using System;
using System.ComponentModel.DataAnnotations;

namespace Pratica_Profissional.Models
{
    public class Funcionario : Pessoa
    {

        public string cargo { get; set; }
        public string dsObservacao { get; set; }
        public string nrCel { get; set; }
        public decimal salario { get; set; }
        public string dtAdmissao { get; set; }
        public string dtDemissao { get; set; }

        public string text { get; set; }
    }
}