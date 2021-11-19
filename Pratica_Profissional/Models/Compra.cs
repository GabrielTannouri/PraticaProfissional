using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pratica_Profissional.Models
{
    public class Compra
    {
        public string modNota { get; set; }
        public string serieNota { get; set; }
        public int nrNota { get; set; }
        public int idFornecedor { get; set; }
        public int? idCondPagamento { get; set; }
        public DateTime dtEmissao { get; set; }
        public DateTime? dtEntrega { get; set; }
        public decimal vlFrete { get; set; }
        public decimal vlSeguro { get; set; }
        public decimal vlDespesas { get; set; }
        public decimal vlTotal { get; set; }
        public DateTime dtCadastro { get; set; }
        public DateTime dtAtualizacao { get; set; }

        public Fornecedor Fornecedor { get; set; }
        public CondicaoPagamento CondicaoPagamento { get; set; }

        public Compra()
        {
            this.ItensCompra = new List<ItemCompra>();
            this.ContasPagar = new List<ContasPagar>();
        }

        public List<ItemCompra> ItensCompra { get; set; }
        public List<ContasPagar> ContasPagar { get; set; }
    }
}