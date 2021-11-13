using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pratica_Profissional.ViewModel
{
    public class FuncionarioVM
    {
        public Models.Funcionario VM2E(Models.Funcionario bean)
        {
            bean.nmPessoa = this.nmFuncionario;
            bean.nmApelido = this.nmApelido;
            bean.rg = this.rg;
            bean.documento = this.cpf;
            bean.genero = this.genero;
            bean.email = this.email;
            bean.endereco = this.endereco;
            bean.bairro = this.bairro;
            bean.nrEndereco = this.nrEndereco;
            bean.complemento = this.complemento;
            bean.cep = this.cep;
            bean.idCidade = this.cidade.idCidade ?? 0;
            bean.dtNascimento = this.dtNascimento;
            bean.nrCel = this.nrCel;
            bean.cargo = this.cargo;
            bean.dsObservacao = this.dsObservacao;
            bean.salario = this.salario;
            bean.dtAdmissao = this.dtAdmissao;
            bean.dtDemissao = this.dtDemissao;
            bean.dtCadastro = Convert.ToDateTime(this.dtCadastro);
            bean.dtAtualizacao = Convert.ToDateTime(this.dtAtualizacao);

            return bean;
        }

        [Display(Name = "ID")]
        public int? idFuncionario { get; set; }

        public int? idCidade { get; set; }

        public string nmFuncionario { get; set; }

        public string nmApelido { get; set; }

        [Display(Name = "RG")]
        public string rg { get; set; }

        [Display(Name = "CPF")]
        public string cpf { get; set; }

        [Display(Name = "CNPJ")]
        public string cnpj { get; set; }

        [Display(Name = "Inscrição estadual")]
        public string inscricaoEstadual { get; set; }

        [Display(Name = "Gênero")]
        public string genero { get; set; }

        [Display(Name = "E-mail")]
        public string email { get; set; }

        [Display(Name = "Site")]
        public string site { get; set; }

        [Display(Name = "Endereço")]
        public string endereco { get; set; }

        [Display(Name = "Bairro")]
        public string bairro { get; set; }

        [Display(Name = "Número endereço")]
        public string nrEndereco { get; set; }

        [Display(Name = "Complemento")]
        public string complemento { get; set; }

        public int? idCondicaoPagamento { get; set; }

        [Display(Name = "CEP")]
        public string cep { get; set; }

        [Display(Name = "Telefone")]
        public string nrTel { get; set; }

        [Display(Name = "Telefone")]
        public string nrCel { get; set; }

        [Display(Name = "Limite de crédito")]
        public decimal limiteCredito { get; set; }

        [Display(Name = "Observação")]
        public string dsObservacao { get; set; }

        [Display(Name = "Data de cadastro")]
        public string dtCadastro { get; set; }

        [Display(Name = "Data de atualização")]
        public string dtAtualizacao { get; set; }
        public string dtNascimento { get; set; }
        public string dtAdmissao { get; set; }
        public string dtDemissao { get; set; }
        public string cargo { get; set; }
        public decimal salario { get; set; }

        public string flTipo { get; set; }
        public string text { get; set; }

        public CondicaoPagamentoVM condicaoPagamento { get; set; }
        public CidadeVM cidade { get; set; }


        public static SelectListItem[] Tipo
        {
            get
            {
                return new[]
                {
                    new SelectListItem {Text = "Físico", Value = "F" },
                    new SelectListItem {Text = "Jurídico", Value = "J" },
                };
            }
        }

        public static SelectListItem[] Genero
        {
            get
            {
                return new[]
                {
                    new SelectListItem {Text = "Masculino", Value = "M" },
                    new SelectListItem {Text = "Feminino", Value = "F" },
                };
            }
        }
    }
}