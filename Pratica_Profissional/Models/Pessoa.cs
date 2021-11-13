using System;
using System.ComponentModel.DataAnnotations;

namespace Pratica_Profissional.Models
{
    public class Pessoa
    {
        [Display(Name = "Código")]
        public int idPessoa { get; set; }
        public int idCidade { get; set; }

        public string nmPessoa { get; set; }
        public string nmApelido { get; set; }
        public string rg { get; set; }
        public string documento { get; set; }
        public string genero { get; set; }

        [Display(Name = "E-mail")]
        public string email { get; set; }
        public string endereco { get; set; }
        public string bairro { get; set; }
        public string nrEndereco { get; set; }
        public string complemento { get; set; }
        public string cep { get; set; }

        [Display(Name = "Contato")]
        public string nrTel { get; set; }
        public string dtNascimento { get; set; }

        [Display(Name = "Cadastro")]
        public DateTime dtCadastro { get; set; }

        [Display(Name = "Últ. atualização")]
        public DateTime dtAtualizacao { get; set; }

        public Cidade cidade { get; set; }

    }
}