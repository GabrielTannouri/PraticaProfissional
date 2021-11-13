using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pratica_Profissional.Models
{
    public class CondicaoPagamento
    {
        [Display(Name = "Código")]
        public int idCondicaoPagamento { get; set; }

        [Display(Name = "Condição pagamento")]
        public string nmCondicaoPagamento { get; set; }

        [Display(Name = "Tx. juros")]
        public decimal txJuros { get; set; }

        [Display(Name = "Multa")]
        public decimal multa { get; set; }
        public decimal desconto { get; set; }

        [Display(Name = "Cadastro")]
        public DateTime dtCadastro { get; set; }

        [Display(Name = "Últ. atualização")]
        public DateTime dtAtualizacao { get; set; }

        public CondicaoPagamento()
        {
            this.CondicaoParcelas = new List<CondicaoPagamentoParcela>();
        }

        public List<CondicaoPagamentoParcela> CondicaoParcelas { get; set; }
    }
}