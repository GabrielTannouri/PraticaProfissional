using System;
using System.ComponentModel.DataAnnotations;

namespace Pratica_Profissional.Models
{
    public class HistoricoOrdemServico
    {
        public int idOrdemServico { get; set; }
        public string flSituacao { get; set; }
        public DateTime dtSituacao { get; set; }
        public int idFuncionario { get; set; }
        public string nmFuncionario { get; set; }

        public Funcionario Funcionario { get; set; }
    }
}