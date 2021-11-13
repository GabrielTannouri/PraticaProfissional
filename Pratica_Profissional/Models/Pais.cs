using System;
using System.ComponentModel.DataAnnotations;

namespace Pratica_Profissional.Models
{
    public class Pais
    {
        [Display(Name = "Código")]
        public int idPais { get; set; }

        [Display(Name = "País")]
        public string nmPais { get; set; }

        [Display(Name = "Sigla")]
        public string sigla { get; set; }

        [Display(Name = "DDI")]
        public string ddi { get; set; }

        [Display(Name = "Cadastro")]
        public DateTime dtCadastro { get; set; }

        [Display(Name = "Últ. atualização")]
        public DateTime dtAtualizacao { get; set; }

    }
}