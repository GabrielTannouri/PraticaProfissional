using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pratica_Profissional.Models
{
    public class Paises
    {
        [Display(Name="ID")]
        public int idPais { get; set; }

        [Display(Name = "Nome")]
        public string nmPais { get; set; }

        [Display(Name = "Sigla")]
        public string sigla { get; set; }

        [Display(Name = "DDI")]
        public string ddi { get; set; }

        [Display(Name = "Data de cadastro")]
        public DateTime dtCadastro { get; set; }

        [Display(Name = "Data de atualização")]
        public DateTime dtAtualizacao { get; set; }
    }
}