using System;
using System.ComponentModel.DataAnnotations;

namespace Pratica_Profissional.Models
{
    public class Cliente : Pessoa
    {
        public int idCondicaoPagamento { get; set; }
        public decimal limiteCredito { get; set; }
        public string flTipo { get; set; }
        public string dsObservacao { get; set; }
        public CondicaoPagamento condicaoPagamento { get; set; }
    }
}