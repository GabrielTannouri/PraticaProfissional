using System.Collections.Generic;

namespace Pratica_Profissional.Models
{
    public class DataTablesList<T>
    {
        public DataTablesList() { }
        public DataTablesList(List<T> itens) { }

        public string js { get; set; }
        public string hash { get; set; }
        public List<T> Get { get; }
        public List<T> Set { get; set; }
    }
}