using System;
using System.ComponentModel.DataAnnotations;

namespace Pratica_Profissional.Models
{
    public class Categoria
    {
        [Display(Name = "ID")]
        public int idCategoria { get; set; }

        [Display(Name = "Categoria")]
        public string nmCategoria { get; set; }

        [Display(Name = "Cadastro")]
        public DateTime dtCadastro { get; set; }

        [Display(Name = "Últ. atualização")]
        public DateTime dtAtualizacao { get; set; }

        public string text { get; set; }
    }
}