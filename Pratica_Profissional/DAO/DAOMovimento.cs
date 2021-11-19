using Pratica_Profissional.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Pratica_Profissional.DAO
{
    public class DAOMovimento : DAO
    {


        public List<Movimento> GetMovimentos()
        {
            try
            {
                AbrirConexao();
                SqlQuery = new SqlCommand("SELECT * FROM tbHistoricoPagamentos INNER JOIN tbContasContabeis on tbHistoricoPagamentos.idconta = tbContasContabeis.idconta ", con);
                reader = SqlQuery.ExecuteReader();

                var lista = new List<Movimento>();

                while (reader.Read())
                {
                    var movimento = new Movimento
                    {
                        idMovimento = Convert.ToInt32(reader["idhistorico"]),
                        conta = new ContaContabil
                        {
                            idConta = Convert.ToInt32(reader["idconta"]),
                            nmConta = Convert.ToString(reader["nmconta"]),
                        },
                        vlPagamento = Convert.ToDecimal(reader["vlpagamento"]),
                        dtPagamento = Convert.ToDateTime(reader["dtpagamento"]),
                        flTipo = Convert.ToString(reader["fltipo"]),
                        descricao = Convert.ToString(reader["descricao"]),
                    };

                    lista.Add(movimento);
                }

                return lista;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
            finally
            {
                FecharConexao();
            }
        }

       



    }
}