using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pratica_Profissional.ViewModel
{
    public class ContaVM
    {
        public Models.ContaContabil VM2E(Models.ContaContabil bean)
        {
            bean.nmConta = this.nmConta.ToUpper();
            bean.vlSaldo = this.vlSaldo;
            bean.dtCadastro = Convert.ToDateTime(this.dtCadastro);
            bean.dtAtualizacao = Convert.ToDateTime(this.dtAtualizacao);

            return bean;
        }

        public int? idConta { get; set; }

        public string nmConta { get; set; }

        public decimal? vlSaldo { get; set; }

        public string dtCadastro { get; set; }

        public string dtAtualizacao { get; set; }

        //Utilizado no select do país
        public string text { get; set; }
       
    }
}