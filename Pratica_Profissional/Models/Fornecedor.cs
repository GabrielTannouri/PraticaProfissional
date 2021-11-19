using System;
using System.ComponentModel.DataAnnotations;

namespace Pratica_Profissional.Models
{
    public class Fornecedor : Pessoa
    {
        public string nrCel { get; set; }
        public string site { get; set; }
        public string dsObservacao { get; set; }
        public string flTipo { get; set; }
        public int? idCondicaoPagamento { get; set; }
        public decimal limiteCredito { get; set; }

        public CondicaoPagamento condicaoPagamento { get; set; }
    }
}