using Pratica_Profissional.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Pratica_Profissional.DAO
{
    public class DAOContaReceber : DAO
    {

        public string Situacao(string flSituacao)
        {
            if (flSituacao == "A")
                return "ABERTA";
            if (flSituacao == "P")
                return "PAGA";
            if (flSituacao == "C")
                return "CANCELADA";

            return "";
        }
        public bool Pagar(ContasReceber contaReceber)
        {
            AbrirConexao();
            SqlCommand comando1 = con.CreateCommand();
            SqlCommand comando2 = con.CreateCommand();
            SqlCommand comando3 = con.CreateCommand();

            comando1.CommandText = "UPDATE tbContasReceber SET vlrecebido=@vlrecebido, dtpagamento=@dtpagamento, idconta=@idconta, flsituacao=@flsituacao " +
                "WHERE modnota=@modnota AND serienota=@serienota AND nrnota=@nrnota AND nrparcela=@nrparcela;";

            comando2.CommandText = "UPDATE tbContasContabeis SET vlsaldo=vlsaldo+@vlsaldo WHERE idconta=@idconta";

            comando3.CommandText = "INSERT INTO tbHistoricoPagamentos (idconta, vlpagamento, dtpagamento, fltipo, descricao) VALUES " +
                    "(@idconta, @vlpagamento, @dtpagamento, @fltipo, @descricao)";

            using (con)
            {
                SqlTransaction sqlTrans = con.BeginTransaction();

                try
                {
                    comando1.Transaction = sqlTrans;
                    comando1.Parameters.AddWithValue("@vlrecebido", contaReceber.vlPago);
                    comando1.Parameters.AddWithValue("@dtpagamento", contaReceber.dtPagamento);
                    comando1.Parameters.AddWithValue("@idconta", contaReceber.idConta);
                    comando1.Parameters.AddWithValue("@flsituacao", contaReceber.flSituacao);
                    comando1.Parameters.AddWithValue("@modnota", contaReceber.modNota);
                    comando1.Parameters.AddWithValue("@serienota", contaReceber.serieNota);
                    comando1.Parameters.AddWithValue("@nrnota", contaReceber.nrNota);
                    comando1.Parameters.AddWithValue("@nrparcela", contaReceber.nrParcela);
                    comando1.ExecuteNonQuery();

                    comando2.Transaction = sqlTrans;
                    comando2.Parameters.Clear();
                    comando2.Parameters.AddWithValue("@vlsaldo", contaReceber.vlPago);
                    comando2.Parameters.AddWithValue("@idconta", contaReceber.idConta);
                    comando2.ExecuteNonQuery();

                    comando3.Transaction = sqlTrans;
                    comando3.Parameters.Clear();
                    comando3.Parameters.AddWithValue("@idconta", contaReceber.idConta);
                    comando3.Parameters.AddWithValue("@dtpagamento", contaReceber.dtPagamento);
                    comando3.Parameters.AddWithValue("@vlpagamento", contaReceber.vlPago);
                    comando3.Parameters.AddWithValue("@fltipo", "R");
                    comando3.Parameters.AddWithValue("@descricao", "Esta movimentação é referente ao recebimento de uma conta.");
                    comando3.ExecuteNonQuery();

                    sqlTrans.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    sqlTrans.Rollback();
                    throw new Exception(ex.Message);
                }
                finally
                {
                    FecharConexao();
                }
            }
        }


        public List<ContasReceber> GetContasReceber()
        {
            try
            {
                AbrirConexao();
                SqlQuery = new SqlCommand("SELECT * FROM tbContasReceber INNER JOIN tbClientes on tbContasReceber.idcliente = tbClientes.idcliente ", con);
                reader = SqlQuery.ExecuteReader();

                var lista = new List<ContasReceber>();

                while (reader.Read())
                {
                    var contasReceber = new ContasReceber
                    {
                        nrParcela = Convert.ToInt32(reader["nrparcela"]),
                        modNota = Convert.ToString(reader["modnota"]),
                        serieNota = Convert.ToString(reader["serienota"]),
                        nrNota = Convert.ToInt32(reader["nrnota"]),
                        dtVencimento = Convert.ToDateTime(reader["dtvencimento"]),
                        cliente = new Cliente
                        {
                            idPessoa = Convert.ToInt32(reader["idcliente"]),
                            nmPessoa = Convert.ToString(reader["nmcliente"]),
                        },
                        vlParcela = Convert.ToDecimal(reader["vlparcela"]),
                        flSituacao = Convert.ToString(reader["flSituacao"]),
                    };

                    lista.Add(contasReceber);
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

        public ContasReceber GetContasReceberbyID(string modNota, string serieNota, int nrNota, int nrParcela, int idCliente)
        {
            try
            {
                AbrirConexao();
                var _where = string.Empty;
                _where = " WHERE modnota = '"+modNota+ "' AND serienota = '" + serieNota + "' AND nrnota = " + nrNota + " AND nrparcela = " + nrParcela + "AND idcliente = " + idCliente;
                SqlQuery = new SqlCommand("SELECT * FROM tbContasReceber" + _where, con);
                reader = SqlQuery.ExecuteReader();
                var conta = new ContasReceber();
                while (reader.Read())
                {
                    conta = new ContasReceber()
                    {
                        modNota = Convert.ToString(reader["modnota"]),
                        serieNota = Convert.ToString(reader["serienota"]),
                        nrNota = Convert.ToInt32(reader["nrnota"]),
                        nrParcela = Convert.ToInt32(reader["nrparcela"]),
                        vlDesconto = Convert.ToDecimal(reader["vldesconto"]),
                        vlJuros = Convert.ToDecimal(reader["vljuros"]),
                        vlMulta = Convert.ToDecimal(reader["vlmulta"]),
                        vlParcela = Convert.ToDecimal(reader["vlparcela"]),
                        dtVencimento = Convert.ToDateTime(reader["dtvencimento"]),
                    };
                }
                return conta;
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