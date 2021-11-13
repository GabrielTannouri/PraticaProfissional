using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pratica_Profissional.ViewModel
{
    public class ServicoVM
    {
        public Models.Servico VM2E(Models.Servico bean)
        {
            bean.nmServico = this.nmServico.ToUpper();
            bean.vlServico = this.vlServico;
            bean.dtCadastro = Convert.ToDateTime(this.dtCadastro);
            bean.dtAtualizacao = Convert.ToDateTime(this.dtAtualizacao);

            return bean;
        }

        public int? idServico { get; set; }

        public string nmServico { get; set; }

        public decimal? vlServico { get; set; }
        
        public string dtCadastro { get; set; }

        public string dtAtualizacao { get; set; }

        //Utilizado no select do serviço
        public string text { get; set; }
       
    }
}