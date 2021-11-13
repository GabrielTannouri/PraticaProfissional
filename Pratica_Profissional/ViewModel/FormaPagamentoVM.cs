using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pratica_Profissional.ViewModel
{
    public class FormaPagamentoVM
    {
        public Models.FormaPagamento VM2E(Models.FormaPagamento bean)
        {
            bean.nmFormaPagamento = this.nmFormaPagamento.ToUpper();
            bean.dtCadastro = Convert.ToDateTime(this.dtCadastro);
            bean.dtAtualizacao = Convert.ToDateTime(this.dtAtualizacao);

            return bean;
        }

        public int? idFormaPagamento { get; set; }

        public string nmFormaPagamento { get; set; }
        
        public string dtCadastro { get; set; }

        public string dtAtualizacao { get; set; }

        //Utilizado no select da forma de pagamento
        public string text { get; set; }
       
    }
}