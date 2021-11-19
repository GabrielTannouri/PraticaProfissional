using System;

namespace Pratica_Profissional.Models
{
    public class ItemVenda
    {
        public string modNota { get; set; }
        public string serieNota { get; set; }
        public string nrNota { get; set; }
        public int idProduto { get; set; }
        public string nmProduto { get; set; }
        public int qtProduto { get; set; }
        public decimal vlUnitario { get; set; }
        public decimal? vlTotalProduto { get; set; }
        public string flUnidade { get; set; }
        public DateTime dtCadastro { get; set; }
        public DateTime dtAtualizacao { get; set; }

        public Fornecedor Fornecedores { get; set; }
        public CondicaoPagamento CondicaoPagamentos { get; set; }
    }
}