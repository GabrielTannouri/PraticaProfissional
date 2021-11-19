using Newtonsoft.Json;
using Pratica_Profissional.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pratica_Profissional.ViewModel
{
    public class ContaReceberVM
    {
        public Models.ContasReceber VM2E(Models.ContasReceber bean)
        {
            bean.modNota = this.modNota;
            bean.serieNota = this.serieNota;
            bean.nrNota = this.nrNota;
            bean.vlPago = this.vlParcela;
            bean.dtPagamento = Convert.ToDateTime(this.dtPagamento);
            bean.idConta = this.Conta.idConta;
            bean.flSituacao = "P";
            bean.nrParcela = this.nrParcela;
            bean.dtAtualizacao = Convert.ToDateTime(this.dtAtualizacao);
            return bean;
        }

        [Display(Name = "Modelo")]
        public string modNota { get; set; }

        [Display(Name = "Série")]
        public string serieNota { get; set; }

        [Display(Name = "Número nota")]
        public int nrNota { get; set; }

        public int nrParcela { get; set; }

        [Display(Name = "Data pagamento")]
        public string dtPagamento { get; set; }
        public string dtVencimento { get; set; }

        [Display(Name = "Cadastro")]
        public string dtCadastro { get; set; }

        [Display(Name = "Últ. atualização")]
        public string dtAtualizacao { get; set; }

        public decimal vlRecebido { get; set; }
        public decimal vlJuros { get; set; }
        public decimal vlDesconto { get; set; }
        public decimal vlMulta { get; set; }
        public decimal vlParcela { get; set; }

        public ClienteVM Cliente { get; set; }
        public ProdutoVM Produto { get; set; }
        public ContaVM Conta { get; set; }

        

    }
}