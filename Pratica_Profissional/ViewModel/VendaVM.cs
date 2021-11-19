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
            bean.modNota = this.modNota;
            bean.modNotaServico = this.modNotaServico;
            bean.serieNota = this.serieNota;
            bean.serieNotaServico = this.serieNotaServico;
            bean.idCliente = this.Cliente.idCliente ?? 0;
            bean.idFuncionario = this.Funcionario.idFuncionario ?? 0;
            bean.idCondPagamento = this.CondicaoPagamento.idCondicaoPagamento ?? null;
            bean.idCondPagamentoServico = this.CondicaoPagamentoServico.idCondicaoPagamento ?? null;
            bean.dtVenda = Convert.ToDateTime(this.dtVenda);
            bean.dtVendaServico = Convert.ToDateTime(this.dtVendaServico);
            bean.vlDesconto = this.vlDesconto;
            bean.vlDescontoServico = this.vlDescontoServico;
            bean.vlTotal = this.vlTotal;
            bean.vlTotalServico = this.vlTotalVendaServicos;
            bean.ItensVenda = this.ListItemVenda;
            bean.ServicosVenda = this.ListServicosVenda;
            bean.ParcelasVenda = this.ListVendaParcelas;
            bean.ParcelasVendaServico = this.ListVendaParcelasServico;
            bean.dtCadastro = Convert.ToDateTime(this.dtCadastro);
            bean.dtAtualizacao = Convert.ToDateTime(this.dtAtualizacao);
            return bean;
        }

        [Display(Name = "Número nota")]
        public int? nrNota { get; set; }
        public int? nrNotaServico { get; set; }
        public string modNota { get; set; }
        public string modNotaServico { get; set; }
        public string serieNota { get; set; }
        public string serieNotaServico { get; set; }

        public int? idFornecedor { get; set; }
        public int? idCondPagamento { get; set; }
        public int? idCondPagamentoServico { get; set; }

        [Display(Name = "Data venda")]
        public string dtVenda { get; set; }
        public string dtVendaServico { get; set; }

        [Display(Name = "Desconto")]
        public decimal vlDesconto { get; set; }
        public decimal vlDescontoServico { get; set; }

        [Display(Name = "Total")]
        public decimal vlTotal { get; set; }
        public decimal vlTotalVendaServicos { get; set; }

        [Display(Name = "Cadastro")]
        public string dtCadastro { get; set; }

        [Display(Name = "Últ. atualização")]
        public string dtAtualizacao { get; set; }
        public string flSituacao { get; set; }

        public string jsListItemVenda { get; set; }
        public string jsListServicosVenda { get; set; }
        public string jsListParcela { get; set; }
        public string jsListParcelaServico { get; set; }

        public ServicoVM Servico { get; set; }
        public ClienteVM Cliente { get; set; }
        public ClienteVM ClienteServico { get; set; }
        public ProdutoVM Produto { get; set; }
        public CondicaoPagamentoVM CondicaoPagamento { get; set; }
        public CondicaoPagamentoVM CondicaoPagamentoServico { get; set; }
        public FuncionarioVM Funcionario { get; set; }
        public FuncionarioVM FuncionarioServico { get; set; }
        public FuncionarioVM FuncionarioExecutante { get; set; }

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

        public List<ServicosVenda> ListServicosVenda
        {
            get
            {
                if (string.IsNullOrEmpty(jsListServicosVenda))
                    return new List<ServicosVenda>();
                return JsonConvert.DeserializeObject<List<ServicosVenda>>(jsListServicosVenda);
            }
            set
            {
                jsListServicosVenda = JsonConvert.SerializeObject(value);
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

        public List<ContasReceber> ListVendaParcelasServico
        {
            get
            {
                if (string.IsNullOrEmpty(jsListParcelaServico))
                    return new List<ContasReceber>();
                return JsonConvert.DeserializeObject<List<ContasReceber>>(jsListParcelaServico);
            }
            set
            {
                jsListParcelaServico = JsonConvert.SerializeObject(value);
            }
        }
    }
}