using System;
using System.ComponentModel.DataAnnotations;

namespace Pratica_Profissional.Models
{
    public class Cidade
    {
        [Display(Name = "ID")]
        public int idCidade { get; set; }

        public int idEstado { get; set; }

        [Display(Name = "Cidade")]
        public string nmCidade { get; set; }

        [Display(Name = "DDD")]
        public string ddd { get; set; }

        [Display(Name = "Cadastro")]
        public DateTime dtCadastro { get; set; }

        [Display(Name = "Últ. atualização")]
        public DateTime dtAtualizacao { get; set; }

        public Estado Estado { get; set; }
    }
}