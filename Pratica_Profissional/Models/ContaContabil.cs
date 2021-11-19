using System;
using System.ComponentModel.DataAnnotations;

namespace Pratica_Profissional.Models
{
    public class ContaContabil
    {
        [Display(Name = "Código")]
        public int? idConta { get; set; }

        [Display(Name = "Conta")]
        public string nmConta { get; set; }

        [Display(Name = "Saldo")]
        public decimal? vlSaldo { get; set; }

        [Display(Name = "Cadastro")]
        public DateTime dtCadastro { get; set; }

        [Display(Name = "Últ. atualização")]
        public DateTime dtAtualizacao { get; set; }

        public string text { get; set; }
    }
}