using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pratica_Profissional.Models
{
    public class Venda
    {
        public string modNota { get; set; }
        public string serieNota { get; set; }
        public int? nrNota { get; set; }
        public int idFuncionario { get; set; }
        public int idCliente { get; set; }
        public int idCondPagamento { get; set; }
        public DateTime dtVenda { get; set; }
        public decimal vlFrete { get; set; }
        public decimal vlDespesas { get; set; }
        public decimal vlDesconto { get; set; }
        public decimal vlTotal { get; set; }
        public DateTime dtCadastro { get; set; }
        public DateTime dtAtualizacao { get; set; }

        public CondicaoPagamento CondicaoPagamento { get; set; }
        public Cliente Cliente { get; set; }
        public Funcionario Funcionario { get; set; }

        public Venda()
        {
            this.ItensVenda = new List<ItemVenda>();
        }

        public List<ItemVenda> ItensVenda { get; set; }
    }
}