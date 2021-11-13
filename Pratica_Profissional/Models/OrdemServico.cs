using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pratica_Profissional.Models
{
    public class OrdemServico
    {
        public int idOrdemServico { get; set; }
        public DateTime dtSituacao { get; set; }
        public DateTime? dtFinalizado { get; set; }
        public int idFuncionario { get; set; }
        public int idCliente { get; set; }
        public int idProduto { get; set; }
        public int? idCondicaoPagamento { get; set; }
        public string dsProduto { get; set; }
        public string dsProblema { get; set; }
        public decimal vlDesconto { get; set; }
        public decimal vlTotal { get; set; }
        public string flSituacao { get; set; }
        public DateTime dtCadastro { get; set; }
        public DateTime dtAtualizacao { get; set; }
   
        public CondicaoPagamento CondicaoPagamento { get; set; }
        public Cliente Cliente { get; set; }
        public Funcionario Funcionario { get; set; }
        public Produto Produto { get; set; }

        public OrdemServico()
        {
            this.ItensOrdemServico = new List<ItemOrdemServico>();
            this.ServicosOrdemServico = new List<ServicosOrdemServico>();
            this.contasReceber = new List<ContasReceber>();
            this.historico = new List<HistoricoOrdemServico>();
        }

        public List<ItemOrdemServico> ItensOrdemServico { get; set; }
        public List<ServicosOrdemServico> ServicosOrdemServico { get; set; }
        public List<ContasReceber> contasReceber { get; set; }
        public List<HistoricoOrdemServico> historico { get; set; }
    }
}