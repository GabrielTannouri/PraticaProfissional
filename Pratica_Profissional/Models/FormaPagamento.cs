using System;
using System.ComponentModel.DataAnnotations;

namespace Pratica_Profissional.Models
{
    public class FormaPagamento
    {
        [Display(Name = "ID")]
        public int idFormaPagamento { get; set; }

        [Display(Name = "Nome")]
        public string nmFormaPagamento { get; set; }

        [Display(Name = "Cadastro")]
        public DateTime dtCadastro { get; set; }

        [Display(Name = "Últ. atualização")]
        public DateTime dtAtualizacao { get; set; }
    }
}