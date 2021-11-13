using System;
using System.ComponentModel.DataAnnotations;

namespace Pratica_Profissional.Models
{
    public class Estado
    {
        [Display(Name = "Código")]
        public int idEstado { get; set; }
        public int idPais { get; set; }

        [Display(Name = "Estado")]
        public string nmEstado { get; set; }

        [Display(Name = "UF")]
        public string uf { get; set; }

        [Display(Name = "Cadastro")]
        public DateTime dtCadastro { get; set; }

        [Display(Name = "Últ. atualização")]
        public DateTime dtAtualizacao { get; set; }

        public Pais Pais { get; set; }
    }
}