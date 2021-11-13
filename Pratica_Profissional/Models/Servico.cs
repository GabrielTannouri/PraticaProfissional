using System;
using System.ComponentModel.DataAnnotations;

namespace Pratica_Profissional.Models
{
    public class Servico
    {
        [Display(Name = "Código")]
        public int idServico { get; set; }

        [Display(Name = "Serviço")]
        public string nmServico { get; set; }

        [Display(Name = "Valor")]
        public decimal? vlServico { get; set; }

        [Display(Name = "Cadastro")]
        public DateTime dtCadastro { get; set; }

        [Display(Name = "Últ. atualização")]
        public DateTime dtAtualizacao { get; set; }

        public string text { get; set; }
    }
}