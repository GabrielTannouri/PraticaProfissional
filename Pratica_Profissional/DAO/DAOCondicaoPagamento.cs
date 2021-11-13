using Pratica_Profissional.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Pratica_Profissional.DAO
{
    public class DAOCondicaoPagamento : DAO
    {

        public bool Create(CondicaoPagamento condicaoPagamento)
        {

            AbrirConexao();
            SqlCommand comando1 = con.CreateCommand();
            SqlCommand comando2 = con.CreateCommand();

            comando1.CommandText = "INSERT INTO tbCondicaoPagamentos (nmcondicaopagamento, txjuros, multa, desconto, dtcadastro, dtatualizacao) VALUES " +
                "(@nmcondicaopagamento, @txjuros, @multa, @desconto, @dtcadastro, @dtatualizacao);SELECT CAST(SCOPE_IDENTITY() AS int)";

            comando2.CommandText = "INSERT INTO tbCondicaoPagamentoParcelas (idFormaPagamento, idCondicaoPagamento, nrParcela, nrPrazo, nrPorcentagem) VALUES " +
                "(@idFormaPagamento, @idCondicaoPagamento, @nrParcela, @nrPrazo, @nrPorcentagem);";

            using (con)
            {
                SqlTransaction sqlTrans = con.BeginTransaction();

                try
                {
                    comando1.Transaction = sqlTrans;
                    comando1.Parameters.AddWithValue("@nmcondicaopagamento", condicaoPagamento.nmCondicaoPagamento);
                    comando1.Parameters.AddWithValue("@txjuros", condicaoPagamento.txJuros);
                    comando1.Parameters.AddWithValue("@multa", condicaoPagamento.multa);
                    comando1.Parameters.AddWithValue("@desconto", condicaoPagamento.desconto);
                    comando1.Parameters.AddWithValue("@dtcadastro", condicaoPagamento.dtCadastro);
                    comando1.Parameters.AddWithValue("@dtatualizacao", condicaoPagamento.dtAtualizacao);
                    Int32 idRetorno = Convert.ToInt32(comando1.ExecuteScalar());

                    comando2.Transaction = sqlTrans;

                    foreach (var item in condicaoPagamento.CondicaoParcelas)
                    {
                        comando2.Parameters.Clear();
                        comando2.Parameters.AddWithValue("@idFormaPagamento", item.idFormaPagamento);
                        comando2.Parameters.AddWithValue("@idCondicaoPagamento", idRetorno);
                        comando2.Parameters.AddWithValue("@nrParcela", item.nrParcela);
                        comando2.Parameters.AddWithValue("@nrPrazo", item.nrPrazo);
                        comando2.Parameters.AddWithValue("@nrPorcentagem", item.nrPorcentagem.ToString().Replace(",", "."));
                        comando2.ExecuteNonQuery();
                    }

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

        public List<CondicaoPagamento> GetCondicaoPagamentos()
        {
            try
            {
                AbrirConexao();
                SqlQuery = new SqlCommand("SELECT * FROM tbCondicaoPagamentos", con);
                reader = SqlQuery.ExecuteReader();

                var lista = new List<CondicaoPagamento>();

                while (reader.Read())
                {
                    var condicaoPagamento = new CondicaoPagamento
                    {
                        idCondicaoPagamento = Convert.ToInt32(reader["idcondicaopagamento"]),
                        nmCondicaoPagamento = Convert.ToString(reader["nmcondicaopagamento"]),
                        txJuros = Convert.ToDecimal(reader["txjuros"]),
                        multa = Convert.ToDecimal(reader["multa"]),
                        desconto = Convert.ToDecimal(reader["desconto"]),
                        dtCadastro = Convert.ToDateTime(reader["dtcadastro"]),
                        dtAtualizacao = Convert.ToDateTime(reader["dtAtualizacao"]),
                    };

                    lista.Add(condicaoPagamento);
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

        public CondicaoPagamento GetCondicaoPagamentosByID(int? idCondicaoPagamento)
        {
            try
            {
                AbrirConexao();
                var _where = string.Empty;
                _where = " WHERE idcondicaopagamento = " + idCondicaoPagamento;
                SqlQuery = new SqlCommand("SELECT * FROM tbCondicaoPagamentos" + _where, con);
                reader = SqlQuery.ExecuteReader();
                var objCondPagamento = new CondicaoPagamento();
                while (reader.Read())
                {
                    objCondPagamento = new CondicaoPagamento()
                    {
                        idCondicaoPagamento = Convert.ToInt32(reader["idcondicaopagamento"]),
                        nmCondicaoPagamento = Convert.ToString(reader["nmcondicaopagamento"]),
                        txJuros = Convert.ToDecimal(reader["txjuros"]),
                        multa = Convert.ToDecimal(reader["multa"]),
                        desconto = Convert.ToDecimal(reader["desconto"]),
                        dtCadastro = Convert.ToDateTime(reader["dtcadastro"]),
                        dtAtualizacao = Convert.ToDateTime(reader["dtAtualizacao"]),
                        CondicaoParcelas = this.GetParcelasByID(idCondicaoPagamento),
                    };
                }
                return objCondPagamento;
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

        public List<CondicaoPagamentoParcela> GetParcelasByID(int? idCondicaoPagamento)
        {
            try
            {
                AbrirConexao();
                var _where = string.Empty;
                _where = " WHERE idcondicaopagamento = " + idCondicaoPagamento;
                SqlQuery = new SqlCommand("SELECT * FROM tbCondicaoPagamentoParcelas INNER JOIN tbFormaPagamentos on tbCondicaoPagamentoParcelas.idformapagamento = tbFormaPagamentos.idformapagamento" + _where, con);
                reader = SqlQuery.ExecuteReader();
                List<CondicaoPagamentoParcela> parcelas = new List<CondicaoPagamentoParcela>();
                while (reader.Read())
                {
                    var obj = new CondicaoPagamentoParcela()
                    {
                        idFormaPagamento = Convert.ToInt32(reader["idformapagamento"]),
                        nmFormaPagamento = Convert.ToString(reader["nmformapagamento"]),
                        idCondicaoPagamento = Convert.ToInt32(reader["idcondicaopagamento"]),
                        nrParcela = Convert.ToInt32(reader["nrparcela"]),
                        nrPrazo = Convert.ToInt32(reader["nrprazo"]),
                        nrPorcentagem = Convert.ToDecimal(reader["nrporcentagem"]),
                    };
                    parcelas.Add(obj);
                }

                return parcelas;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }

        }

        public bool Edit(CondicaoPagamento condicaoPagamento)
        {

            AbrirConexao();
            SqlCommand comando1 = con.CreateCommand();
            SqlCommand comando2 = con.CreateCommand();
            SqlCommand comando3 = con.CreateCommand();

            comando1.CommandText = "UPDATE tbCondicaoPagamentos SET nmcondicaopagamento=@nmcondicaopagamento, txjuros=@txjuros, multa=@multa, desconto=@desconto, " +
                "dtatualizacao=@dtAtualizacao WHERE idcondicaopagamento=@idcondicaopagamento;";

            comando2.CommandText = "DELETE FROM tbCondicaoPagamentoParcelas WHERE idcondicaopagamento = " + condicaoPagamento.idCondicaoPagamento;

            comando3.CommandText = "INSERT INTO tbCondicaoPagamentoParcelas (idFormaPagamento, idCondicaoPagamento, nrParcela, nrPrazo, nrPorcentagem) VALUES " +
           "(@idFormaPagamento, @idCondicaoPagamento, @nrParcela, @nrPrazo, @nrPorcentagem);";

            using (con)
            {
                SqlTransaction sqlTrans = con.BeginTransaction();

                try
                {
                    comando1.Transaction = sqlTrans;
                    comando1.Parameters.AddWithValue("@idcondicaopagamento", condicaoPagamento.idCondicaoPagamento);
                    comando1.Parameters.AddWithValue("@nmcondicaopagamento", condicaoPagamento.nmCondicaoPagamento);
                    comando1.Parameters.AddWithValue("@txjuros", condicaoPagamento.txJuros);
                    comando1.Parameters.AddWithValue("@multa", condicaoPagamento.multa);
                    comando1.Parameters.AddWithValue("@desconto", condicaoPagamento.desconto);
                    comando1.Parameters.AddWithValue("@dtatualizacao", condicaoPagamento.dtAtualizacao);
                    comando1.ExecuteNonQuery();

                    comando2.Transaction = sqlTrans;
                    comando2.Parameters.Clear();
                    comando2.ExecuteNonQuery();

                    comando3.Transaction = sqlTrans;

                    foreach (var item in condicaoPagamento.CondicaoParcelas)
                    {
                        comando3.Parameters.Clear();
                        comando3.Parameters.AddWithValue("@idFormaPagamento", item.idFormaPagamento);
                        comando3.Parameters.AddWithValue("@idCondicaoPagamento", condicaoPagamento.idCondicaoPagamento);
                        comando3.Parameters.AddWithValue("@nrParcela", item.nrParcela);
                        comando3.Parameters.AddWithValue("@nrPrazo", item.nrPrazo);
                        comando3.Parameters.AddWithValue("@nrPorcentagem", item.nrPorcentagem);
                        comando3.ExecuteNonQuery();
                    }
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

        public bool Delete(int id)
        {
            try
            {
                AbrirConexao();
                SqlQuery = new SqlCommand("DELETE FROM tbCondicaoPagamentos WHERE idcondicaopagamento=@idcondicaopagamento", con);

                SqlQuery.Parameters.AddWithValue("@idcondicaopagamento", id);
                // Validação para saber se a linha foi alterada no BD
                int i = SqlQuery.ExecuteNonQuery();
                if (i >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
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
