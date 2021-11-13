using Pratica_Profissional.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pratica_Profissional.ViewModel
{
    public class EstadoVM
    {
        public Models.Estado VM2E(Models.Estado bean)
        {
            bean.nmEstado = this.nmEstado.ToUpper();
            bean.uf = this.uf.ToUpper();
            bean.dtCadastro = Convert.ToDateTime(this.dtCadastro);
            bean.dtAtualizacao = Convert.ToDateTime(this.dtAtualizacao);
            bean.idPais = this.Pais.idPais ?? 0;

            return bean;
        }

        public int? idEstado { get; set; }
        public int? idPais { get; set; }
        public string nmEstado { get; set; }
        public string uf { get; set; }
        public string dtCadastro { get; set; }
        public string dtAtualizacao { get; set; }

        //Utilizado no select do estado
        public string text { get; set; }
        public PaisVM Pais { get; set; }
    }
}