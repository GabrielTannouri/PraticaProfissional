using System;
using System.ComponentModel.DataAnnotations;

namespace Pratica_Profissional.Models
{
    public class CondicaoPagamentoParcela
    {
        public int? idParcela { get; set; }
        public int? idFormaPagamento { get; set; }
        public string nmFormaPagamento { get; set; }
        public int? idCondicaoPagamento { get; set; }
        public int? nrParcela { get; set; }
        public decimal nrPrazo { get; set; }
        public decimal nrPorcentagem { get; set; }

        public FormaPagamento formaPagamento { get; set; }
        public CondicaoPagamento condicaoPagamento { get; set; }
    }
}