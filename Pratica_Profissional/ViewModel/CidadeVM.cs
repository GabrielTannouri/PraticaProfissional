using Pratica_Profissional.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pratica_Profissional.ViewModel
{
    public class CidadeVM
    {
        public Models.Cidade VM2E(Models.Cidade bean)
        {
            bean.nmCidade = this.nmCidade.ToUpper();
            bean.ddd = this.ddd;
            bean.dtCadastro = Convert.ToDateTime(this.dtCadastro);
            bean.dtAtualizacao = Convert.ToDateTime(this.dtAtualizacao);
            bean.idEstado = this.Estado.idEstado ?? 0;

            return bean;
        }

        public int? idCidade { get; set; }
        public int? idEstado { get; set; }
        public string nmCidade { get; set; }
        public string ddd { get; set; }
        public string dtCadastro { get; set; }
        public string dtAtualizacao { get; set; }

        //Utilizado no select de cidade
        public string text { get; set; }
        public EstadoVM Estado { get; set; }
    }
}