using Pratica_Profissional.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Pratica_Profissional.DAO
{
    public class DAOVenda : DAO
    {

        public string Situacao(string flSituacao)
        {
            if (flSituacao == "A")
                return "ABERTA";
            if (flSituacao == "F")
                return "FINALIZADA";

            return "";
        }

        public bool Create(Venda venda, CondicaoPagamento condicaoPagamento)
        {
            AbrirConexao();
            SqlCommand comando1 = con.CreateCommand();
            SqlCommand comando2 = con.CreateCommand();
            SqlCommand comando3 = con.CreateCommand();
            SqlCommand comando4 = con.CreateCommand();
            SqlCommand comando5 = con.CreateCommand();
            SqlCommand comando6 = con.CreateCommand();
            SqlCommand comando7 = con.CreateCommand();

            //VENDA DE PRODUTOS
            comando1.CommandText = "INSERT INTO tbVendas (modnota, serieNota, idcliente, idfuncionario, idordemservico, idcondicaopagamento, dtvenda," +
                    "vltotal, vldesconto, dtcadastro, dtatualizacao, flsituacao) VALUES " +
                    "(@modnota, @serieNota, @idcliente, @idfuncionario, @idordemservico, @idcondicaopagamento, @dtvenda, @vltotal, @vldesconto," +
                    "@dtcadastro, @dtatualizacao, @flsituacao);SELECT CAST(SCOPE_IDENTITY() AS int)";

            comando2.CommandText = "INSERT INTO tbItensVenda (modnota, serieNota, nrnota, idcliente, idproduto, qtproduto, vlvenda, dtcadastro, dtatualizacao, flunidade) VALUES " +
                    "(@modnota, @serieNota, @nrnota, @idcliente, @idproduto, @qtproduto, @vlvenda, @dtcadastro, @dtatualizacao, @flunidade)";

            comando3.CommandText = "INSERT INTO tbContasReceber (modnota, serienota, nrnota, nrparcela, idcliente, idordemservico, idformapagamento, dtvencimento, vldesconto," +
                    "vljuros, vlmulta, vlparcela, flsituacao) VALUES " +
                    "(@modnota, @serienota, @nrnota, @nrparcela, @idcliente, @idordemservico, @idformapagamento, @dtvencimento, @vldesconto, @vljuros, @vlmulta, " +
                    "@vlparcela, @flsituacao)";

            comando4.CommandText = "UPDATE tbProdutos SET nrestoque=nrestoque-@nrEstoque WHERE idproduto=@idProduto";

            //VENDA DE SERVIÇOS
            comando5.CommandText = "INSERT INTO tbVendas (modnota, serieNota, idcliente, idfuncionario, idordemservico, idcondicaopagamento, dtvenda," +
                    "vltotal, vldesconto, dtcadastro, dtatualizacao, flsituacao) VALUES " +
                    "(@modnota, @serieNota, @idcliente, @idfuncionario, @idordemservico, @idcondicaopagamento, @dtvenda, @vltotal, @vldesconto," +
                    "@dtcadastro, @dtatualizacao, @flsituacao);SELECT CAST(SCOPE_IDENTITY() AS int)";

            comando6.CommandText = "INSERT INTO tbServicosVenda (modnota, serieNota, nrnota, idcliente, idservico, qtservico, vlservico, dtcadastro, dtatualizacao) VALUES " +
                    "(@modnota, @serieNota, @nrnota, @idcliente, @idservico, @qtservico, @vlservico, @dtcadastro, @dtatualizacao)";

            comando7.CommandText = "INSERT INTO tbContasReceber (modnota, serienota, nrnota, nrparcela, idcliente, idordemservico, idformapagamento, dtvencimento, vldesconto," +
                   "vljuros, vlmulta, vlparcela, flsituacao) VALUES " +
                   "(@modnota, @serienota, @nrnota, @nrparcela, @idcliente, @idordemservico, @idformapagamento, @dtvencimento, @vldesconto, @vljuros, @vlmulta, " +
                   "@vlparcela, @flsituacao)";

            using (con)
            {
                SqlTransaction sqlTrans = con.BeginTransaction();

                try
                {
                    if (venda.ItensVenda.Count > 0)
                    {
                        comando1.Transaction = sqlTrans;
                        comando1.Parameters.AddWithValue("@modnota", venda.modNota);
                        comando1.Parameters.AddWithValue("@serieNota", venda.serieNota);
                        comando1.Parameters.AddWithValue("@idcliente", venda.idCliente);
                        comando1.Parameters.AddWithValue("@idfuncionario", venda.idFuncionario);
                        comando1.Parameters.AddWithValue("@idordemservico", (object)venda.idOrdemServico ?? DBNull.Value);
                        comando1.Parameters.AddWithValue("@idcondicaopagamento", venda.idCondPagamento);
                        comando1.Parameters.AddWithValue("@dtvenda", venda.dtVenda);
                        comando1.Parameters.AddWithValue("@vltotal", venda.vlTotal);
                        comando1.Parameters.AddWithValue("@vldesconto", venda.vlDesconto);
                        comando1.Parameters.AddWithValue("@dtcadastro", venda.dtCadastro);
                        comando1.Parameters.AddWithValue("@dtatualizacao", venda.dtAtualizacao);
                        comando1.Parameters.AddWithValue("@flsituacao", venda.flSituacao);
                        Int32 nrnota = Convert.ToInt32(comando1.ExecuteScalar());

                        comando2.Transaction = sqlTrans;

                        foreach (var item in venda.ItensVenda)
                        {
                            comando2.Parameters.Clear();
                            comando2.Parameters.AddWithValue("@modnota", venda.modNota);
                            comando2.Parameters.AddWithValue("@serieNota", venda.serieNota);
                            comando2.Parameters.AddWithValue("@nrnota", nrnota);
                            comando2.Parameters.AddWithValue("@idcliente", venda.idCliente);
                            comando2.Parameters.AddWithValue("@idproduto", item.idProduto);
                            comando2.Parameters.AddWithValue("@qtproduto", item.qtProduto);
                            comando2.Parameters.AddWithValue("@vlvenda", item.vlUnitario);
                            comando2.Parameters.AddWithValue("@dtcadastro", venda.dtCadastro);
                            comando2.Parameters.AddWithValue("@dtatualizacao", venda.dtAtualizacao);
                            comando2.Parameters.AddWithValue("@flunidade", item.flUnidade == "UNIDADE" ? "U" : "G");
                            comando2.ExecuteNonQuery();
                        }


                        comando3.Transaction = sqlTrans;

                        foreach (var item in venda.ParcelasVenda)
                        {
                            comando3.Parameters.Clear();
                            comando3.Parameters.AddWithValue("@modnota", venda.modNota);
                            comando3.Parameters.AddWithValue("@serienota", venda.serieNota);
                            comando3.Parameters.AddWithValue("@nrnota", nrnota);
                            comando3.Parameters.AddWithValue("@nrparcela", item.nrParcela);
                            comando3.Parameters.AddWithValue("@idcliente", venda.idCliente);
                            comando3.Parameters.AddWithValue("@idordemservico", (object)venda.idOrdemServico ?? DBNull.Value);
                            comando3.Parameters.AddWithValue("@idformapagamento", item.idFormaPagamento);
                            comando3.Parameters.AddWithValue("@dtvencimento", item.dtVencimento);
                            comando3.Parameters.AddWithValue("@vldesconto", (object)condicaoPagamento.desconto ?? DBNull.Value);
                            comando3.Parameters.AddWithValue("@vljuros", (object)condicaoPagamento.txJuros ?? DBNull.Value);
                            comando3.Parameters.AddWithValue("@vlmulta", (object)condicaoPagamento.multa ?? DBNull.Value);
                            comando3.Parameters.AddWithValue("@vlparcela", item.vlParcela);
                            comando3.Parameters.AddWithValue("@flsituacao", "A");
                            comando3.ExecuteNonQuery();
                        }


                        comando4.Transaction = sqlTrans;

                        foreach (var item in venda.ItensVenda)
                        {
                            comando4.Parameters.Clear();
                            comando4.Parameters.AddWithValue("@idProduto", item.idProduto);
                            comando4.Parameters.AddWithValue("@nrestoque", item.qtProduto);
                            comando4.ExecuteNonQuery();
                        }
                    }

                    if (venda.ServicosVenda.Count > 0)
                    {
                        comando5.Transaction = sqlTrans;
                        comando5.Parameters.Clear();
                        comando5.Parameters.AddWithValue("@modnota", venda.modNotaServico);
                        comando5.Parameters.AddWithValue("@serieNota", venda.serieNotaServico);
                        comando5.Parameters.AddWithValue("@idcliente", venda.idCliente);
                        comando5.Parameters.AddWithValue("@idfuncionario", venda.idFuncionario);
                        comando5.Parameters.AddWithValue("@idordemservico", (object)venda.idOrdemServico ?? DBNull.Value);
                        comando5.Parameters.AddWithValue("@idcondicaopagamento", venda.idCondPagamentoServico);
                        comando5.Parameters.AddWithValue("@dtvenda", venda.dtVendaServico);
                        comando5.Parameters.AddWithValue("@vltotal", venda.vlTotalServico);
                        comando5.Parameters.AddWithValue("@vldesconto", venda.vlDescontoServico);
                        comando5.Parameters.AddWithValue("@dtcadastro", venda.dtCadastro);
                        comando5.Parameters.AddWithValue("@dtatualizacao", venda.dtAtualizacao);
                        comando5.Parameters.AddWithValue("@flsituacao", venda.flSituacao);
                        Int32 nrnotaServico = Convert.ToInt32(comando5.ExecuteScalar());

                        comando6.Transaction = sqlTrans;
                        foreach (var item in venda.ServicosVenda)
                        {
                            comando6.Parameters.Clear();
                            comando6.Parameters.AddWithValue("@modnota", venda.modNotaServico);
                            comando6.Parameters.AddWithValue("@serieNota", venda.serieNotaServico);
                            comando6.Parameters.AddWithValue("@nrnota", nrnotaServico);
                            comando6.Parameters.AddWithValue("@idcliente", venda.idCliente);
                            comando6.Parameters.AddWithValue("@idservico", item.idServico);
                            comando6.Parameters.AddWithValue("@qtservico", item.qtServico);
                            comando6.Parameters.AddWithValue("@vlservico", item.vlUnitarioServico);
                            comando6.Parameters.AddWithValue("@dtcadastro", venda.dtCadastro);
                            comando6.Parameters.AddWithValue("@dtatualizacao", venda.dtAtualizacao);
                            comando6.ExecuteNonQuery();
                        }

                        comando7.Transaction = sqlTrans;

                        foreach (var item in venda.ParcelasVenda)
                        {
                            comando7.Parameters.Clear();
                            comando7.Parameters.AddWithValue("@modnota", venda.modNotaServico);
                            comando7.Parameters.AddWithValue("@serieNota", venda.serieNotaServico);
                            comando7.Parameters.AddWithValue("@nrnota", nrnotaServico);
                            comando7.Parameters.AddWithValue("@nrparcela", item.nrParcela);
                            comando7.Parameters.AddWithValue("@idcliente", venda.idCliente);
                            comando7.Parameters.AddWithValue("@idordemservico", (object)venda.idOrdemServico ?? DBNull.Value);
                            comando7.Parameters.AddWithValue("@idformapagamento", item.idFormaPagamento);
                            comando7.Parameters.AddWithValue("@dtvencimento", item.dtVencimento);
                            comando7.Parameters.AddWithValue("@vldesconto", (object)condicaoPagamento.desconto ?? DBNull.Value);
                            comando7.Parameters.AddWithValue("@vljuros", (object)condicaoPagamento.txJuros ?? DBNull.Value);
                            comando7.Parameters.AddWithValue("@vlmulta", (object)condicaoPagamento.multa ?? DBNull.Value);
                            comando7.Parameters.AddWithValue("@vlparcela", item.vlParcela);
                            comando7.Parameters.AddWithValue("@flsituacao", "A");
                            comando7.ExecuteNonQuery();
                        }

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

        public List<Venda> GetVendas()
        {
            try
            {
                AbrirConexao();
                SqlQuery = new SqlCommand("SELECT * FROM tbVendas INNER JOIN tbClientes on tbVendas.idcliente = tbClientes.idcliente " +
                                                                        "INNER JOIN tbFuncionarios on tbVendas.idfuncionario = tbFuncionarios.idfuncionario", con);
                reader = SqlQuery.ExecuteReader();

                var lista = new List<Venda>();

                while (reader.Read())
                {
                    var venda = new Venda
                    {
                        modNota = Convert.ToString(reader["modnota"]),
                        serieNota = Convert.ToString(reader["serienota"]),
                        nrNota = Convert.ToInt32(reader["nrnota"]),
                        dtVenda = Convert.ToDateTime(reader["dtvenda"]),
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

                    lista.Add(venda);
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