using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pratica_Profissional.Models
{
    public class Venda
    {
        public string modNota { get; set; }
        public string modNotaServico { get; set; }
        public string serieNota { get; set; }
        public string serieNotaServico { get; set; }
        public int? nrNota { get; set; }
        public int? nrNotaServico { get; set; }
        public int? idFuncionario { get; set; }
        public int? idCliente { get; set; }
        public int? idCondPagamento { get; set; }
        public int? idCondPagamentoServico { get; set; }
        public int? idOrdemServico { get; set; }
        public DateTime dtVenda { get; set; }
        public DateTime dtVendaServico { get; set; }
        public decimal? vlDesconto { get; set; }
        public decimal? vlDescontoServico { get; set; }
        public decimal? vlTotal { get; set; }
        public decimal? vlTotalServico { get; set; }
        public DateTime dtCadastro { get; set; }
        public DateTime dtAtualizacao { get; set; }
        public string flSituacao { get; set; }

        public CondicaoPagamento CondicaoPagamento { get; set; }
        public Cliente Cliente { get; set; }
        public Funcionario Funcionario { get; set; }

        public Venda()
        {
            this.ItensVenda = new List<ItemVenda>();
            this.ServicosVenda = new List<ServicosVenda>();
            this.ParcelasVenda = new List<ContasReceber>();
            this.ParcelasVendaServico = new List<ContasReceber>();
        }

        public List<ItemVenda> ItensVenda { get; set; }
        public List<ServicosVenda> ServicosVenda { get; set; }
        public List<ContasReceber> ParcelasVenda { get; set; }
        public List<ContasReceber> ParcelasVendaServico { get; set; }
    }
}