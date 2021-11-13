using System;

namespace Pratica_Profissional.Models
{
    public class ServicosVenda
    {
        public string modNota { get; set; }
        public string serieNota { get; set; }
        public int nrNota { get; set; }
        public int idServico { get; set; }
        public string nmServico { get; set; }
        public int idFuncionario { get; set; }
        public string nmFuncionario { get; set; }
        public int qtServico { get; set; }
        public decimal vlUnitarioServico { get; set; }
        public DateTime dtCadastro { get; set; }
        public DateTime dtAtualizacao { get; set; }
        public decimal? vlTotalServico { get; set; }


        public Fornecedor Fornecedores { get; set; }
    }
}