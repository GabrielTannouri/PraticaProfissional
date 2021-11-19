using System;
using System.ComponentModel.DataAnnotations;

namespace Pratica_Profissional.Models
{
    public class Movimento
    {
        [Display(Name = "Código")]
        public int idMovimento { get; set; }
        public int idConta { get; set; }
        public decimal vlPagamento { get; set; }
        public string flTipo { get; set; }
        public string descricao { get; set; }
        public DateTime dtPagamento { get; set; }

        public ContaContabil conta { get; set; }
    }
}