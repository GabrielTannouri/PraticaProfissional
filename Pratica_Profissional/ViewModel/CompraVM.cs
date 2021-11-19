using Newtonsoft.Json;
using Pratica_Profissional.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pratica_Profissional.ViewModel
{
    public class CompraVM
    {
        public Models.Compra VM2E(Models.Compra bean)
        {
            bean.modNota = this.modNota;
            bean.serieNota = this.serieNota;
            bean.nrNota = this.nrNota;
            bean.idFornecedor = this.Fornecedor.idFornecedor ?? 0;
            bean.idCondPagamento = this.CondicaoPagamento.idCondicaoPagamento ?? null;
            bean.dtEmissao = Convert.ToDateTime(this.dtEmissao);
            if (this.dtEntrega != null)
            {
                bean.dtEntrega = Convert.ToDateTime(this.dtEntrega);
            }
            bean.vlFrete = this.vlFrete;
            bean.vlSeguro = this.vlSeguro;
            bean.vlDespesas = this.vlDespesas;
            bean.vlTotal = this.vlTotal;
            bean.dtCadastro = Convert.ToDateTime(this.dtCadastro);
            bean.dtAtualizacao = Convert.ToDateTime(this.dtAtualizacao);
            bean.ItensCompra = this.ListItemCompra;
            bean.ContasPagar = this.ListCompraParcelas;
            return bean;
        }

        [Display(Name = "Modelo")]
        public string modNota { get; set; }

        [Display(Name = "Série")]
        public string serieNota { get; set; }

        [Display(Name = "Número nota")]
        public int nrNota { get; set; }

        public int idFornecedor { get; set; }
        public int idCondPagamento { get; set; }

        [Display(Name = "Data emissão")]
        public string dtEmissao { get; set; }

        [Display(Name = "Data entrega")]
        public string dtEntrega { get; set; }

        [Display(Name = "Frete")]
        public decimal vlFrete { get; set; }

        [Display(Name = "Seguro")]
        public decimal vlSeguro { get; set; }

        [Display(Name = "Despesas")]
        public decimal vlDespesas { get; set; }

        [Display(Name = "Total")]
        public decimal vlTotal { get; set; }

        [Display(Name = "Cadastro")]
        public string dtCadastro { get; set; }

        [Display(Name = "Últ. atualização")]
        public string dtAtualizacao { get; set; }

        public string jsListItemCompra { get; set; }
        public string jsListParcela { get; set; }

        public FornecedorVM Fornecedor { get; set; }
        public ProdutoVM Produto { get; set; }
        public CondicaoPagamentoVM CondicaoPagamento { get; set; }

        public List<ItemCompra> ListItemCompra
        {
            get
            {
                if (string.IsNullOrEmpty(jsListItemCompra))
                    return new List<ItemCompra>();
                return JsonConvert.DeserializeObject<List<ItemCompra>>(jsListItemCompra);
            }
            set
            {
                jsListItemCompra = JsonConvert.SerializeObject(value);
            }
        }

        public List<ContasPagar> ListCompraParcelas
        {
            get
            {
                if (string.IsNullOrEmpty(jsListParcela))
                    return new List<ContasPagar>();
                return JsonConvert.DeserializeObject<List<ContasPagar>>(jsListParcela);
            }
            set
            {
                jsListParcela = JsonConvert.SerializeObject(value);
            }
        }

    }
}