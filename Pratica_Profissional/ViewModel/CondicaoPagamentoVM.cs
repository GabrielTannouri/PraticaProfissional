using Newtonsoft.Json;
using Pratica_Profissional.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pratica_Profissional.ViewModel
{
    public class CondicaoPagamentoVM
    {
        public Models.CondicaoPagamento VM2E(Models.CondicaoPagamento bean)
        {
            bean.nmCondicaoPagamento = this.nmCondicaoPagamento;
            bean.txJuros = this.txJuros;
            bean.multa = this.multa;
            bean.desconto = this.desconto;
            bean.dtCadastro = Convert.ToDateTime(this.dtCadastro);
            bean.dtAtualizacao = Convert.ToDateTime(this.dtAtualizacao);
            bean.CondicaoParcelas = this.ListCondicao;
            return bean;
        }

        public int? idCondicaoPagamento { get; set; }
        public string nmCondicaoPagamento { get; set; }
        public decimal txJuros { get; set; }
        public decimal multa { get; set; }
        public decimal desconto { get; set; }
        public string dtCadastro { get; set; }
        public string dtAtualizacao { get; set; }
        public FormaPagamentoVM formaPagamento { get; set; }

        public string text { get; set; }

        public string jsList { get; set; }

        public List<CondicaoPagamentoParcela> ListCondicao
        {
            get
            {
                if (string.IsNullOrEmpty(jsList))
                    return new List<CondicaoPagamentoParcela>();
                return JsonConvert.DeserializeObject<List<CondicaoPagamentoParcela>>(jsList);
            }
            set
            {
                jsList = JsonConvert.SerializeObject(value);
            }
        }
    }
}