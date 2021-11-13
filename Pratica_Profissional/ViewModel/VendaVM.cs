using Newtonsoft.Json;
using Pratica_Profissional.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pratica_Profissional.ViewModel
{
    public class VendaVM
    {
        public Models.Venda VM2E(Models.Venda bean)
        {
            bean.nrNota = this.nrNota;
            bean.idFuncionario = this.Funcionario.idFuncionario ?? 0;
            bean.idCondPagamento = this.CondicaoPagamento.idCondicaoPagamento ?? 0;
            bean.idCliente = this.Cliente.idCliente ?? 0;
            bean.dtVenda = Convert.ToDateTime(this.dtVenda);
            bean.vlDesconto = this.vlDesconto;
            bean.vlTotal = this.vlTotal;
            bean.dtCadastro = Convert.ToDateTime(this.dtCadastro);
            bean.dtAtualizacao = Convert.ToDateTime(this.dtAtualizacao);
            return bean;
        }

        [Display(Name = "Número nota")]
        public int? nrNota { get; set; }
        public string modNota { get; set; }
        public string serieNota { get; set; }

        public int? idFornecedor { get; set; }
        public int? idCondPagamento { get; set; }

        [Display(Name = "Data venda")]
        public string dtVenda { get; set; }

        [Display(Name = "Desconto")]
        public decimal vlDesconto { get; set; }

        [Display(Name = "Total")]
        public decimal vlTotal { get; set; }

        [Display(Name = "Cadastro")]
        public DateTime dtCadastro { get; set; }

        [Display(Name = "Últ. atualização")]
        public DateTime dtAtualizacao { get; set; }

        public string jsListItemVenda { get; set; }
        public string jsListServicoVenda { get; set; }
        public string jsListParcela { get; set; }

        public ClienteVM Cliente { get; set; }
        public ProdutoVM Produto { get; set; }
        public CondicaoPagamentoVM CondicaoPagamento { get; set; }
        public FuncionarioVM Funcionario { get; set; }

        public List<ItemVenda> ListItemVenda
        {
            get
            {
                if (string.IsNullOrEmpty(jsListItemVenda))
                    return new List<ItemVenda>();
                return JsonConvert.DeserializeObject<List<ItemVenda>>(jsListItemVenda);
            }
            set
            {
                jsListItemVenda = JsonConvert.SerializeObject(value);
            }
        }

        public List<ContasReceber> ListVendaParcelas
        {
            get
            {
                if (string.IsNullOrEmpty(jsListParcela))
                    return new List<ContasReceber>();
                return JsonConvert.DeserializeObject<List<ContasReceber>>(jsListParcela);
            }
            set
            {
                jsListParcela = JsonConvert.SerializeObject(value);
            }
        }

        public List<ServicosVenda> ListServicosItem
        {
            get
            {
                if (string.IsNullOrEmpty(jsListServicoVenda))
                    return new List<ServicosVenda>();
                return JsonConvert.DeserializeObject<List<ServicosVenda>>(jsListServicoVenda);
            }
            set
            {
                jsListServicoVenda = JsonConvert.SerializeObject(value);
            }
        }
    }
}