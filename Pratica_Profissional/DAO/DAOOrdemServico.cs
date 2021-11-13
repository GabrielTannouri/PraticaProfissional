using Pratica_Profissional.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Pratica_Profissional.DAO
{
    public class DAOOrdemServico : DAO
    {

        public string Situacao (string flSituacao)
        {
            if (flSituacao == "A")
                return "ABERTA";
            if (flSituacao == "R")
                return "ORÇAMENTO REALIZADO";
            if (flSituacao == "O")
                return "ORÇAMENTO APROVADO";
            if (flSituacao == "F")
                return "FECHADO";
            if (flSituacao == "I")
                return "FINALIZADO";
            if (flSituacao == "C")
                return "CANCELADO";

            return "";
        }
        public bool Create(OrdemServico ordemServico)
        {
            AbrirConexao();
            SqlCommand comando1 = con.CreateCommand();
            SqlCommand comando2 = con.CreateCommand();
            SqlCommand comando3 = con.CreateCommand();
            SqlCommand comando4 = con.CreateCommand();

            comando1.CommandText = "INSERT INTO tbOrdensServico (dtsituacao, idfuncionario, idcliente, idproduto, idcondicaoPagamento, dsproduto, dsproblema," +
                    "vldesconto, vltotal, flsituacao, dtcadastro, dtatualizacao) VALUES " +
                    "(@dtsituacao, @idfuncionario, @idcliente, @idproduto, @idcondicaoPagamento, @dsproduto, @dsproblema, @vldesconto, @vltotal, @flsituacao," +
                    "@dtcadastro, @dtatualizacao);SELECT CAST(SCOPE_IDENTITY() AS int)";

            comando2.CommandText = "INSERT INTO tbItensOrdemServico (idordemservico, idproduto, flunidade, qtproduto, vlunitario, dtcadastro, dtatualizacao) VALUES " +
                    "(@idordemservico, @idproduto, @flunidade, @qtproduto, @vlunitario, @dtcadastro, @dtatualizacao)";

            comando3.CommandText = "INSERT INTO tbServicosOrdemServico (idordemservico, idservico, idfuncionario, qtservico, vlservico, dtcadastro, dtatualizacao) VALUES " +
                    "(@idordemservico, @idservico, @idfuncionario, @qtservico, @vlservico, @dtcadastro, @dtatualizacao)";

            comando4.CommandText = "INSERT INTO tbHistoricoOrdemServico (idordemservico, flsituacao, dtsituacao, idfuncionario) VALUES " +
                    "(@idordemservico, @flsituacao, @dtsituacao, @idfuncionario)";

            using (con)
            {
                SqlTransaction sqlTrans = con.BeginTransaction();

                try
                {
                    comando1.Transaction = sqlTrans;
                    comando1.Parameters.AddWithValue("@dtsituacao", ordemServico.dtSituacao);
                    comando1.Parameters.AddWithValue("@idfuncionario", ordemServico.idFuncionario);
                    comando1.Parameters.AddWithValue("@idcliente", ordemServico.idCliente);
                    comando1.Parameters.AddWithValue("@idproduto", ordemServico.idProduto);
                    comando1.Parameters.AddWithValue("@idcondicaoPagamento", (object)ordemServico.idCondicaoPagamento ?? DBNull.Value);
                    comando1.Parameters.AddWithValue("@dsproduto", (object)ordemServico.dsProduto ?? DBNull.Value);
                    comando1.Parameters.AddWithValue("@dsproblema", (object)ordemServico.dsProblema ?? DBNull.Value);
                    comando1.Parameters.AddWithValue("@vldesconto", (object)ordemServico.vlDesconto ?? DBNull.Value);
                    comando1.Parameters.AddWithValue("@vltotal", ordemServico.vlTotal);
                    comando1.Parameters.AddWithValue("@flsituacao", ordemServico.flSituacao);
                    comando1.Parameters.AddWithValue("@dtcadastro", ordemServico.dtCadastro);
                    comando1.Parameters.AddWithValue("@dtatualizacao", ordemServico.dtAtualizacao);
                    Int32 idOrdemServico = Convert.ToInt32(comando1.ExecuteScalar());


                    if (ordemServico.ItensOrdemServico.Count > 0)
                    {
                        comando2.Transaction = sqlTrans;

                        foreach (var item in ordemServico.ItensOrdemServico)
                        {
                            comando2.Parameters.Clear();
                            comando2.Parameters.AddWithValue("@idordemservico", idOrdemServico);
                            comando2.Parameters.AddWithValue("@idproduto", item.idProduto);
                            comando2.Parameters.AddWithValue("@flunidade", item.flUnidade == "UNIDADE" ? "U" : "G");
                            comando2.Parameters.AddWithValue("@qtproduto", item.qtProduto);
                            comando2.Parameters.AddWithValue("@vlunitario", item.vlUnitario);
                            comando2.Parameters.AddWithValue("@dtcadastro", ordemServico.dtCadastro);
                            comando2.Parameters.AddWithValue("@dtatualizacao", ordemServico.dtAtualizacao);
                            comando2.ExecuteNonQuery();
                        }
                    }

                    if (ordemServico.ServicosOrdemServico.Count > 0)
                    {
                        comando3.Transaction = sqlTrans;

                        foreach (var item in ordemServico.ServicosOrdemServico)
                        {
                            comando3.Parameters.Clear();
                            comando3.Parameters.AddWithValue("@idordemservico", idOrdemServico);
                            comando3.Parameters.AddWithValue("@idservico", item.idServico);
                            comando3.Parameters.AddWithValue("@idfuncionario", item.idFuncionario);
                            comando3.Parameters.AddWithValue("@qtservico", item.qtServico);
                            comando3.Parameters.AddWithValue("@vlservico", item.vlUnitarioServico);
                            comando3.Parameters.AddWithValue("@dtcadastro", ordemServico.dtCadastro);
                            comando3.Parameters.AddWithValue("@dtatualizacao", ordemServico.dtAtualizacao);
                            comando3.ExecuteNonQuery();
                        }
                    }

                    comando4.Transaction = sqlTrans;
                    comando4.Parameters.Clear();
                    comando4.Parameters.AddWithValue("@idordemservico", idOrdemServico);
                    comando4.Parameters.AddWithValue("@flsituacao", ordemServico.flSituacao);
                    comando4.Parameters.AddWithValue("@dtsituacao", ordemServico.dtSituacao);
                    comando4.Parameters.AddWithValue("@idfuncionario", ordemServico.idFuncionario);
                    comando4.ExecuteNonQuery();
                   
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


        public List<OrdemServico> GetOrdemServicos()
        {
            try
            {
                AbrirConexao();
                SqlQuery = new SqlCommand("SELECT * FROM tbOrdensServico INNER JOIN tbClientes on tbOrdensServico.idcliente = tbClientes.idcliente " +
                                                                        "INNER JOIN tbFuncionarios on tbOrdensServico.idfuncionario = tbFuncionarios.idfuncionario", con);
                reader = SqlQuery.ExecuteReader();

                var lista = new List<OrdemServico>();

                while (reader.Read())
                {
                    var ordemServico = new OrdemServico
                    {
                        idOrdemServico = Convert.ToInt32(reader["idordemservico"]),
                        dtSituacao = Convert.ToDateTime(reader["dtsituacao"]),
                        Cliente = new Cliente
                        {
                            idPessoa = Convert.ToInt32(reader["idcliente"]),
                            nmPessoa = Convert.ToString(reader["nmcliente"]),
                        },
                        Funcionario = new Funcionario
                        {
                            idPessoa = Convert.ToInt32(reader["idfuncionario"]),
                            nmPessoa = Convert.ToString(reader["nmfuncionario"]),
                        },
                        vlTotal = Convert.ToDecimal(reader["vltotal"]),
                        flSituacao = Convert.ToString(reader["flSituacao"]),
                        dtCadastro = Convert.ToDateTime(reader["dtcadastro"]),
                        dtAtualizacao = Convert.ToDateTime(reader["dtAtualizacao"]),
                    };

                    lista.Add(ordemServico);
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

        public OrdemServico GetOrdemServicoByID(int idOrdemServico)
        {
            try
            {
                AbrirConexao();
                var _where = string.Empty;
                _where = " WHERE idordemservico = " + idOrdemServico;
                SqlQuery = new SqlCommand("SELECT * FROM tbOrdensServico INNER JOIN tbClientes on tbOrdensServico.idcliente = tbClientes.idcliente " +
                                          "INNER JOIN tbFuncionarios on tbOrdensServico.idfuncionario = tbFuncionarios.idfuncionario " +
                                          "INNER JOIN tbProdutos on tbOrdensServico.idproduto = tbProdutos.idproduto " + _where, con);
                reader = SqlQuery.ExecuteReader();
                var objOrdemServico = new OrdemServico();
                while (reader.Read())
                {
                    objOrdemServico = new OrdemServico()
                    {
                        idOrdemServico = Convert.ToInt32(reader["idordemservico"]),
                        dtSituacao = Convert.ToDateTime(reader["dtsituacao"]),
                        dtFinalizado = Convert.ToDateTime(reader["dtfinalizado"] != DBNull.Value ? reader["dtfinalizado"] : null),
                        Funcionario = new Funcionario
                        {
                            idPessoa = Convert.ToInt32(reader["idfuncionario"]),
                            nmPessoa = Convert.ToString(reader["nmfuncionario"]),
                        },
                        Cliente = new Cliente
                        {
                            idPessoa = Convert.ToInt32(reader["idcliente"]),
                            nmPessoa = Convert.ToString(reader["nmcliente"]),
                        },
                        Produto = new Produto
                        {
                            idProduto = Convert.ToInt32(reader["idproduto"]),
                            nmProduto = Convert.ToString(reader["nmproduto"]),
                        },
                        dsProduto = Convert.ToString(reader["dsproduto"] != DBNull.Value ? reader["dsproduto"] : null),
                        dsProblema = Convert.ToString(reader["dsproblema"]),
                        vlDesconto = Convert.ToDecimal(reader["vldesconto"] != DBNull.Value ? reader["vldesconto"] : null),
                        vlTotal = Convert.ToDecimal(reader["vltotal"]),
                        flSituacao = Convert.ToString(reader["flsituacao"]),
                        dtCadastro = Convert.ToDateTime(reader["dtcadastro"]),
                        dtAtualizacao = Convert.ToDateTime(reader["dtatualizacao"]),
                        ItensOrdemServico = this.GetItensByID(idOrdemServico),
                        ServicosOrdemServico = this.GetServicosOrdemServico(idOrdemServico),
                        historico = this.GetHistorico(idOrdemServico),
                        //contasReceber = this.GetParcelasByID(idOrdemServico),
                    };
                }
                return objOrdemServico;
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

        public List<ItemOrdemServico> GetItensByID(int idOrdemServico)
        {
            try
            {
                AbrirConexao();
                var _where = string.Empty;
                _where = " WHERE A.idordemservico  = " + idOrdemServico;
                SqlQuery = new SqlCommand("SELECT * FROM tbItensOrdemServico A INNER JOIN tbProdutos B on A.idproduto = B.idproduto" + _where, con);
                reader = SqlQuery.ExecuteReader();
                List<ItemOrdemServico> itens = new List<ItemOrdemServico>();
                while (reader.Read())
                {
                    var obj = new ItemOrdemServico()
                    {
                        idOrdemServico = Convert.ToInt32(reader["idordemservico"]),
                        idProduto = Convert.ToInt32(reader["idproduto"]),
                        nmProduto = Convert.ToString(reader["nmproduto"]),
                        flUnidade = Convert.ToString(reader["flunidade"]),
                        qtProduto = Convert.ToInt32(reader["qtproduto"]),
                        vlUnitario = Convert.ToDecimal(reader["vlunitario"]),
                        vlTotalProduto = Convert.ToDecimal(reader["vlunitario"]) * Convert.ToInt32(reader["qtproduto"]),
                        dtCadastro = Convert.ToDateTime(reader["dtcadastro"]),
                        dtAtualizacao = Convert.ToDateTime(reader["dtatualizacao"]),
                      
                    };
                    itens.Add(obj);
                }

                return itens;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }

        }

        public List<ServicosOrdemServico> GetServicosOrdemServico(int idOrdemServico)
        {
            try
            {
                AbrirConexao();
                var _where = string.Empty;
                _where = " WHERE A.idordemservico  = " + idOrdemServico;
                SqlQuery = new SqlCommand("SELECT * FROM tbServicosOrdemServico A INNER JOIN tbServicos B on A.idservico = B.idservico " +
                    "                      INNER JOIN tbFuncionarios C on A.idfuncionario = C.idfuncionario" + _where, con);
                reader = SqlQuery.ExecuteReader();
                List<ServicosOrdemServico> servicos = new List<ServicosOrdemServico>();
                while (reader.Read())
                {
                    var obj = new ServicosOrdemServico()
                    {
                        idOrdemServico = Convert.ToInt32(reader["idordemservico"]),
                        idServico = Convert.ToInt32(reader["idservico"]),
                        nmServico = Convert.ToString(reader["nmservico"]),
                        qtServico = Convert.ToInt32(reader["qtservico"]),
                        vlUnitarioServico = Convert.ToDecimal(reader["vlservico"]),
                        idFuncionario = Convert.ToInt32(reader["idfuncionario"]),
                        nmFuncionario = Convert.ToString(reader["nmfuncionario"]),
                        dtCadastro = Convert.ToDateTime(reader["dtcadastro"]),
                        dtAtualizacao = Convert.ToDateTime(reader["dtatualizacao"]),
                        vlTotalServico = Convert.ToDecimal(reader["vlservico"]) * Convert.ToInt32(reader["qtservico"]),

                    };
                    servicos.Add(obj);
                }

                return servicos;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }

        }

        //public List<ContasReceber> GetParcelasByID(int idOrdemServico)
        //{
        //    try
        //    {
        //        AbrirConexao();
        //        var _where = string.Empty;
        //        _where = " WHERE A.modnota = '" + modNota + "' AND A.serienota  = '" + serieNota + "' AND A.nrnota  = " + nrNota + " AND A.idfornecedor =" + idFornecedor;
        //        SqlQuery = new SqlCommand("SELECT * FROM tbServicosOrdemServico A INNER JOIN tbServicos B on A.idservico = B.idservico" + _where, con);
        //        reader = SqlQuery.ExecuteReader();
        //        List<ContasPagar> parcelas = new List<ContasPagar>();
        //        while (reader.Read())
        //        {
        //            var obj = new ContasPagar()
        //            {
        //                modNota = Convert.ToString(reader["modnota"]),
        //                serieNota = Convert.ToString(reader["serienota"]),
        //                nrNota = Convert.ToInt32(reader["nrnota"]),
        //                idFornecedor = Convert.ToInt32(reader["idfornecedor"]),
        //                nrParcela = Convert.ToInt32(reader["nrparcela"]),
        //                dtVencimento = Convert.ToDateTime(reader["dtvencimento"]),
        //                flSituacao = Convert.ToString(reader["flsituacao"]),
        //                vlDesconto = Convert.ToDecimal(reader["vldesconto"] != DBNull.Value ? reader["vldesconto"] : null),
        //                vlJuros = Convert.ToDecimal(reader["vljuros"] != DBNull.Value ? reader["vljuros"] : null),
        //                vlMulta = Convert.ToDecimal(reader["vlmulta"] != DBNull.Value ? reader["vlmulta"] : null),
        //                vlPago = Convert.ToDecimal(reader["vlpago"] != DBNull.Value ? reader["vlpago"] : null),
        //                dtPagamento = Convert.ToDateTime(reader["dtpagamento"] != DBNull.Value ? reader["dtpagamento"] : null),
        //                vlParcela = Convert.ToDecimal(reader["vlparcela"]),
        //                dtCadastro = Convert.ToDateTime(reader["dtcadastro"]),
        //                dtAtualizacao = Convert.ToDateTime(reader["dtatualizacao"]),
        //                idFormaPagamento = Convert.ToInt32(reader["idformapagamento"]),
        //                nmFormaPagamento = Convert.ToString(reader["nmformapagamento"]),
        //            };
        //            parcelas.Add(obj);
        //        }

        //        return parcelas;
        //    }
        //    catch (Exception error)
        //    {
        //        throw new Exception(error.Message);
        //    }

        //}

        public List<HistoricoOrdemServico> GetHistorico(int idOrdemServico)
        {
            try
            {
                AbrirConexao();
                var _where = string.Empty;
                _where = " WHERE A.idordemservico  = " + idOrdemServico;
                SqlQuery = new SqlCommand("SELECT * FROM tbHistoricoOrdemServico A INNER JOIN tbFuncionarios B on A.idfuncionario = B.idfuncionario" + _where, con);
                reader = SqlQuery.ExecuteReader();
                List<HistoricoOrdemServico> historico = new List<HistoricoOrdemServico>();
                while (reader.Read())
                {
                    var obj = new HistoricoOrdemServico()
                    {
                        idOrdemServico = Convert.ToInt32(reader["idordemservico"]),
                        flSituacao = Convert.ToString(reader["flsituacao"]),
                        dtSituacao = Convert.ToDateTime(reader["dtsituacao"]),
                        idFuncionario = Convert.ToInt32(reader["idfuncionario"]),
                        nmFuncionario = Convert.ToString(reader["nmfuncionario"]),

                    };
                    historico.Add(obj);
                }

                return historico;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }

        }

        public bool Edit(OrdemServico ordemServico)
        {

            AbrirConexao();
            SqlCommand comando1 = con.CreateCommand();
            SqlCommand comando2 = con.CreateCommand();
            SqlCommand comando3 = con.CreateCommand();
            SqlCommand comando4 = con.CreateCommand();
            SqlCommand comando5 = con.CreateCommand();
            SqlCommand comando6 = con.CreateCommand();

            comando1.CommandText = "UPDATE tbOrdensServico SET dtsituacao=@dtsituacao, vldesconto=@vldesconto, vltotal=@vltotal, flsituacao=@flsituacao, " +
                "dtatualizacao=@dtatualizacao WHERE idordemservico=@idordemservico;";

            comando2.CommandText = "DELETE FROM tbItensOrdemServico WHERE idordemservico = " + ordemServico.idOrdemServico;

            comando3.CommandText = "DELETE FROM tbServicosOrdemServico WHERE idordemservico = " + ordemServico.idOrdemServico;

            comando4.CommandText = "INSERT INTO tbItensOrdemServico (idordemservico, idproduto, flunidade, qtproduto, vlunitario, dtcadastro, dtatualizacao) VALUES " +
                    "(@idordemservico, @idproduto, @flunidade, @qtproduto, @vlunitario, @dtcadastro, @dtatualizacao)";

            comando5.CommandText = "INSERT INTO tbServicosOrdemServico (idordemservico, idservico, idfuncionario, qtservico, vlservico, dtcadastro, dtatualizacao) VALUES " +
                    "(@idordemservico, @idservico, @idfuncionario, @qtservico, @vlservico, @dtcadastro, @dtatualizacao)";

            comando6.CommandText = "INSERT INTO tbHistoricoOrdemServico (idordemservico, flsituacao, dtsituacao, idfuncionario) VALUES " +
                    "(@idordemservico, @flsituacao, @dtsituacao, @idfuncionario)";

            using (con)
            {
                SqlTransaction sqlTrans = con.BeginTransaction();

                try
                {
                    comando1.Transaction = sqlTrans;
                    comando1.Parameters.AddWithValue("@idordemservico", ordemServico.idOrdemServico);
                    comando1.Parameters.AddWithValue("@dtsituacao", ordemServico.dtSituacao);
                    comando1.Parameters.AddWithValue("@vldesconto", ordemServico.vlDesconto);
                    comando1.Parameters.AddWithValue("@vltotal", ordemServico.vlTotal);
                    comando1.Parameters.AddWithValue("@flsituacao", ordemServico.flSituacao);
                    comando1.Parameters.AddWithValue("@dtatualizacao", ordemServico.dtAtualizacao);
                    comando1.ExecuteNonQuery();

                    comando2.Transaction = sqlTrans;
                    comando2.Parameters.Clear();
                    comando2.ExecuteNonQuery();


                    comando3.Transaction = sqlTrans;
                    comando3.Parameters.Clear();
                    comando3.ExecuteNonQuery();

                    if (ordemServico.ItensOrdemServico.Count > 0)
                    {
                        comando4.Transaction = sqlTrans;

                        foreach (var item in ordemServico.ItensOrdemServico)
                        {
                            comando4.Parameters.Clear();
                            comando4.Parameters.AddWithValue("@idordemservico", ordemServico.idOrdemServico);
                            comando4.Parameters.AddWithValue("@idproduto", item.idProduto);
                            comando4.Parameters.AddWithValue("@flunidade", item.flUnidade == "UNIDADE" ? "U" : "G");
                            comando4.Parameters.AddWithValue("@qtproduto", item.qtProduto);
                            comando4.Parameters.AddWithValue("@vlunitario", item.vlUnitario);
                            comando4.Parameters.AddWithValue("@dtcadastro", ordemServico.dtAtualizacao);
                            comando4.Parameters.AddWithValue("@dtatualizacao", ordemServico.dtAtualizacao);
                            comando4.ExecuteNonQuery();
                        }
                    }

                    if (ordemServico.ServicosOrdemServico.Count > 0)
                    {
                        comando5.Transaction = sqlTrans;

                        foreach (var item in ordemServico.ServicosOrdemServico)
                        {
                            comando5.Parameters.Clear();
                            comando5.Parameters.AddWithValue("@idordemservico", ordemServico.idOrdemServico);
                            comando5.Parameters.AddWithValue("@idservico", item.idServico);
                            comando5.Parameters.AddWithValue("@idfuncionario", item.idFuncionario);
                            comando5.Parameters.AddWithValue("@qtservico", item.qtServico);
                            comando5.Parameters.AddWithValue("@vlservico", item.vlUnitarioServico);
                            comando5.Parameters.AddWithValue("@dtcadastro", ordemServico.dtAtualizacao);
                            comando5.Parameters.AddWithValue("@dtatualizacao", ordemServico.dtAtualizacao);
                            comando5.ExecuteNonQuery();
                        }
                    }

                    comando6.Transaction = sqlTrans;
                    comando6.Parameters.Clear();
                    comando6.Parameters.AddWithValue("@idordemservico", ordemServico.idOrdemServico);
                    comando6.Parameters.AddWithValue("@flsituacao", ordemServico.flSituacao);
                    comando6.Parameters.AddWithValue("@dtsituacao", ordemServico.dtSituacao);
                    comando6.Parameters.AddWithValue("@idfuncionario", ordemServico.idFuncionario);
                    comando6.ExecuteNonQuery();

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

    }
}