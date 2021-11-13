using System;
using System.ComponentModel.DataAnnotations;

namespace Pratica_Profissional.Models
{
    public class Produto
    {
        [Display(Name = "Código")]
        public int idProduto { get; set; }

        [Display(Name = "Produto")]
        public string nmProduto { get; set; }

        [Display(Name = "Unidade")]
        public string flUnidade { get; set; }
        public int idCategoria { get; set; }
        public int idFornecedor { get; set; }

        [Display(Name = "Estoque")]
        public int nrEstoque { get; set; }

        [Display(Name = "Custo (R$)")]
        public decimal? vlPrecoCusto { get; set; }

        [Display(Name = "Venda (R$)")]
        public decimal? vlPrecoVenda { get; set; }
        public decimal? vlPrecoUltCompra { get; set; }

        [Display(Name = "Cadastro")]
        public DateTime dtCadastro { get; set; }

        [Display(Name = "Últ. atualização")]
        public DateTime dtAtualizacao { get; set; }

        public Categoria Categoria { get; set; }
        public Fornecedor Fornecedor { get; set; }
        public string text { get; set; }
    }
}