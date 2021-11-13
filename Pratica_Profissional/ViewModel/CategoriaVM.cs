using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pratica_Profissional.ViewModel
{
    public class CategoriaVM
    {
        public Models.Categoria VM2E(Models.Categoria bean)
        {
            bean.nmCategoria = this.nmCategoria.ToUpper();
            bean.dtCadastro = Convert.ToDateTime(this.dtCadastro);
            bean.dtAtualizacao = Convert.ToDateTime(this.dtAtualizacao);

            return bean;
        }

        public int? idCategoria { get; set; }

        public string nmCategoria { get; set; }
        
        public string dtCadastro { get; set; }

        public string dtAtualizacao { get; set; }

        //Utilizado no select do país
        public string text { get; set; }
       
    }
}