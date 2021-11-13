using System;

namespace Pratica_Profissional.Models
{
    public class ItemOrdemServico
    {
        public int idOrdemServico { get; set; }
        public int idProduto { get; set; }
        public string nmProduto { get; set; }
        public string flUnidade { get; set; }
        public int qtProduto { get; set; }
        public decimal vlUnitario { get; set; }
        public decimal? vlTotalProduto { get; set; }
        public DateTime dtCadastro { get; set; }
        public DateTime dtAtualizacao { get; set; }
      

        public Fornecedor Fornecedores { get; set; }
    }
}