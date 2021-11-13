using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pratica_Profissional.ViewModel
{
    public class PaisVM
    {
        public Models.Pais VM2E(Models.Pais bean)
        {
            bean.nmPais = this.nmPais.ToUpper();
            bean.sigla = this.sigla.ToUpper();
            bean.ddi = this.ddi;
            bean.dtCadastro = Convert.ToDateTime(this.dtCadastro);
            bean.dtAtualizacao = Convert.ToDateTime(this.dtAtualizacao);

            return bean;
        }

        public int? idPais { get; set; }

        public string nmPais { get; set; }

        public string sigla { get; set; }

        public string ddi { get; set; }
        
        public string dtCadastro { get; set; }

        public string dtAtualizacao { get; set; }

        //Utilizado no select do país
        public string text { get; set; }
       
    }
}