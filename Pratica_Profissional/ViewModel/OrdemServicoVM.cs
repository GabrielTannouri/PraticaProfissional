using Newtonsoft.Json;
using Pratica_Profissional.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Pratica_Profissional.ViewModel
{
    public class OrdemServicoVM
    {
        public Models.OrdemServico VM2E(Models.OrdemServico bean)
        {
            bean.flSituacao = this.flSituacao;
            bean.dtSituacao = Convert.ToDateTime(this.dtSituacao);
            if (this.dtFinalizado != null)
            {
                bean.dtFinalizado = this.dtFinalizado;
            }
            bean.idFuncionario = this.Funcionario.idFuncionario ?? 0;
            bean.idCondicaoPagamento = this.CondicaoPagamento.idCondicaoPagamento ?? 0;
            bean.idCliente = this.Cliente.idCliente ?? 0;
            bean.idProduto = this.Produto.idProduto ?? 0;
            bean.dsProduto = this.dsProduto;
            bean.dsProblema = this.dsProblema;
            bean.ServicosOrdemServico = this.ListServicosItem;
            bean.ItensOrdemServico = this.ListProdutosItem;
            bean.contasReceber = this.ListParcelas;
            bean.vlDesconto = this.vlDesconto ?? 0;
            bean.vlTotal = this.vlTotal;
            bean.dtCadastro = Convert.ToDateTime(this.dtCadastro);
            bean.dtAtualizacao = Convert.ToDateTime(this.dtAtualizacao);
            return bean;
        }

        public int? idOrdemServico { get; set; }

        [Display(Name = "Data abertura")]
        public string dtSituacao { get; set; }

        [Display(Name = "Data fechamento")]
        public DateTime dtFinalizado { get; set; }

        public int? idFuncionario { get; set; }
        public int? idCliente { get; set; }
        public int? idProduto { get; set; }
        public int? idCondicaoPagamento { get; set; }

        [Display(Name = "Descrição do problema")]
        public string dsProblema { get; set; }

        [Display(Name = "Descrição do produto")]
        public string dsProduto { get; set; }

        public string flSituacao { get; set; }
        public string flSituacaoAux { get; set; }

        [Display(Name = "Desconto")]
        public decimal? vlDesconto { get; set; }

        [Display(Name = "Total")]
        public decimal vlTotal { get; set; }

        [Display(Name = "Cadastro")]
        public string dtCadastro { get; set; }

        [Display(Name = "Últ. atualização")]
        public string dtAtualizacao { get; set; }

        public string flSituacaoAberta { get; set; }
        public string flSituacaoRealizado { get; set; }
        public string flSituacaoAprovado { get; set; }
        public string flSituacaoFechado { get; set; }

        public string jsListProdutosOS { get; set; }
        public string jsListServicosOS { get; set; }
        public string jsListParcelasOS { get; set; }
        public string jsListHistoricoOS { get; set; }

        public CondicaoPagamentoVM CondicaoPagamento { get; set; }
        public ClienteVM Cliente { get; set; }
        public FuncionarioVM Funcionario { get; set; }
        public FuncionarioVM FuncionarioExecutante { get; set; }
        public ProdutoVM Produto { get; set; }
        public ProdutoVM ProdutoUtilizado { get; set; }
        public ServicoVM Servico { get; set; }

        public static SelectListItem[] SituacaoCreate
        {
            get
            {
                return new[]
                {
                    new SelectListItem {Text = "ABERTA", Value = "A" },
                    new SelectListItem {Text = "ORÇAMENTO REALIZADO", Value = "R" },
                    new SelectListItem {Text = "ORÇAMENTO APROVADO", Value = "O" },
                    new SelectListItem {Text = "FECHADO", Value = "F" },
                    new SelectListItem {Text = "FINALIZADO", Value = "I" },
                    new SelectListItem {Text = "CANCELADO", Value = "C" },
                };
            }
        }

        public static SelectListItem[] SituacaoAberta
        {
            get
            {
                return new[]
                {
                    new SelectListItem {Text = "ORÇAMENTO REALIZADO", Value = "R" },
                    new SelectListItem {Text = "ORÇAMENTO APROVADO", Value = "O" },
                    new SelectListItem {Text = "FECHADO", Value = "F" },
                    new SelectListItem {Text = "FINALIZADO", Value = "I" },
                    new SelectListItem {Text = "CANCELADO", Value = "C" },
                };
            }
        }

        public static SelectListItem[] SituacaoOrcamentoRealizado
        {
            get
            {
                return new[]
                {
                    new SelectListItem {Text = "ORÇAMENTO APROVADO", Value = "O" },
                    new SelectListItem {Text = "FECHADO", Value = "F" },
                    new SelectListItem {Text = "FINALIZADO", Value = "I" },
                    new SelectListItem {Text = "CANCELADO", Value = "C" },
                };
            }
        }

        public static SelectListItem[] SituacaoOrcamentoAprovado
        {
            get
            {
                return new[]
                {
                    new SelectListItem {Text = "FECHADO", Value = "F" },
                    new SelectListItem {Text = "FINALIZADO", Value = "I" },
                    new SelectListItem {Text = "CANCELADO", Value = "C" },
                };
            }
        }

        public static SelectListItem[] SituacaoFechado
        {
            get
            {
                return new[]
                {
                    new SelectListItem {Text = "FINALIZADO", Value = "I" },
                    new SelectListItem {Text = "CANCELADO", Value = "C" },
                };
            }
        }

        public List<ItemOrdemServico> ListProdutosItem
        {
            get
            {
                if (string.IsNullOrEmpty(jsListProdutosOS))
                    return new List<ItemOrdemServico>();
                return JsonConvert.DeserializeObject<List<ItemOrdemServico>>(jsListProdutosOS);
            }
            set
            {
                jsListProdutosOS = JsonConvert.SerializeObject(value);
            }
        }

        public List<ServicosOrdemServico> ListServicosItem
        {
            get
            {
                if (string.IsNullOrEmpty(jsListServicosOS))
                    return new List<ServicosOrdemServico>();
                return JsonConvert.DeserializeObject<List<ServicosOrdemServico>>(jsListServicosOS);
            }
            set
            {
                jsListServicosOS = JsonConvert.SerializeObject(value);
            }
        }

        public List<ContasReceber> ListParcelas
        {
            get
            {
                if (string.IsNullOrEmpty(jsListParcelasOS))
                    return new List<ContasReceber>();
                return JsonConvert.DeserializeObject<List<ContasReceber>>(jsListParcelasOS);
            }
            set
            {
                jsListParcelasOS = JsonConvert.SerializeObject(value);
            }
        }

        public List<HistoricoOrdemServico> ListHistoricoOrdemServico
        {
            get
            {
                if (string.IsNullOrEmpty(jsListHistoricoOS))
                    return new List<HistoricoOrdemServico>();
                return JsonConvert.DeserializeObject<List<HistoricoOrdemServico>>(jsListHistoricoOS);
            }
            set
            {
                jsListHistoricoOS = JsonConvert.SerializeObject(value);
            }
        }
    }
}