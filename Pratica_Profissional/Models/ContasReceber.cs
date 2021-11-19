using System;

namespace Pratica_Profissional.Models
{
    public class ContasReceber
    {
        public const string SITUACAO_PENDENTE = "P";

        public string modNota { get; set; }
        public string serieNota { get; set; }
        public int nrNota { get; set; }
        public int idCliente { get; set; }
        public int? idOrdemServico { get; set; }
        public int? idFormaPagamento { get; set; }
        public string nmFormaPagamento { get; set; }
        public int? nrParcela { get; set; }
        public DateTime dtVencimento { get; set; }
        public DateTime dtPagamento { get; set; }
        public decimal vlParcela { get; set; }
        public string flSituacao { get; set; }
        public decimal vlDesconto { get; set; }
        public decimal vlJuros { get; set; }
        public decimal vlMulta { get; set; }
        public decimal vlPago { get; set; }
        public DateTime dtCadastro { get; set; }
        public DateTime dtAtualizacao { get; set; }
        public int? idConta { get; set; }

        public FormaPagamento formaPagamento { get; set; }
        public CondicaoPagamento condicaoPagamento { get; set; }
        public Cliente cliente { get; set; }
        public ContaContabil Conta { get; set; }

    }
}