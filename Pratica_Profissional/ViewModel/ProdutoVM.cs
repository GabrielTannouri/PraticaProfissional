using Pratica_Profissional.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pratica_Profissional.ViewModel
{
    public class ProdutoVM
    {
        public Models.Produto VM2E(Models.Produto bean)
        {
            bean.nmProduto = this.nmProduto.ToUpper();
            bean.flUnidade = this.flUnidade.ToUpper();
            bean.nrEstoque = this.nrEstoque;
            bean.vlPrecoCusto = this.vlPrecoCusto ?? 0;
            bean.vlPrecoVenda = this.vlPrecoVenda ?? 0;
            bean.vlPrecoUltCompra = this.vlPrecoUltCompra;
            bean.dtCadastro = Convert.ToDateTime(this.dtCadastro);
            bean.dtAtualizacao = Convert.ToDateTime(this.dtAtualizacao);
            bean.idCategoria = this.Categoria.idCategoria ?? 0;
            bean.idFornecedor = this.Fornecedor.idFornecedor ?? 0;
            return bean;
        }

        public int? idProduto { get; set; }
        public string nmProduto { get; set; }
        public string flUnidade { get; set; }
        public int? idCategoria { get; set; }
        public int nrEstoque { get; set; }

        [Display(Name = "Valor de custo")]
        public decimal? vlPrecoCusto { get; set; }

        [Display(Name = "Valor de venda")]
        public decimal? vlPrecoVenda { get; set; }
        public decimal? vlPrecoUltCompra { get; set; }
        public string dtCadastro { get; set; }
        public string dtAtualizacao { get; set; }

        //Utilizado no select do estado
        public string text { get; set; }
        public CategoriaVM Categoria { get; set; }
        public FornecedorVM Fornecedor { get; set; }

        public static SelectListItem[] unidade
        {
            get
            {
                return new[]
                {
                    new SelectListItem {Text = "Unidade", Value = "U" },
                    new SelectListItem {Text = "Grama", Value = "G" },
                };
            }
        }
    }
}