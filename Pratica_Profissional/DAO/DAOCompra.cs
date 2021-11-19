using Pratica_Profissional.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Pratica_Profissional.DAO
{
    public class DAOCompra : DAO
    {

        public bool Create(Compra compra, decimal vlTotalProdutos)
        {
            AbrirConexao();
            SqlCommand comando1 = con.CreateCommand();
            SqlCommand comando2 = con.CreateCommand();
            SqlCommand comando3 = con.CreateCommand();
            SqlCommand comando4 = con.CreateCommand();

            comando1.CommandText = "INSERT INTO tbCompras (modnota, serienota, nrnota, idfornecedor, idcondpagamento, dtemissao, dtentrega, vlfrete, vlseguro," +
                    "vldespesas, vltotal, dtcadastro, dtatualizacao) VALUES " +
                    "(@modnota, @serienota, @nrnota, @idfornecedor, @idcondpagamento, @dtemissao, @dtentrega, @vlfrete, @vlseguro, @vldespesas, @vltotal," +
                    "@dtcadastro, @dtatualizacao)";

            comando2.CommandText = "INSERT INTO tbItemCompra (modnota, serienota, nrnota, idfornecedor, idproduto, qtitem, vlunitario, dtcadastro, dtatualizacao, flunidade) VALUES " +
                    "(@modnota, @serienota, @nrnota, @idfornecedor, @idproduto, @qtitem, @vlunitario, @dtcadastro, @dtatualizacao, @flunidade)";

            comando3.CommandText = "INSERT INTO tbContasPagar (modnota, serienota, nrnota, idfornecedor, nrparcela, dtvencimento, flsituacao," +
                    "dtcadastro, dtatualizacao, idformapagamento, vlparcela) VALUES " +
                    "(@modnota, @serienota, @nrnota, @idfornecedor, @nrparcela, @dtvencimento, @flsituacao, @dtcadastro," +
                    "@dtatualizacao, @idformapagamento, @vlparcela)";

            comando4.CommandText = "UPDATE tbProdutos SET nrestoque=nrestoque+@nrEstoque, vlprecoultcompra=@vlPrecoUltCompra, vlprecocusto = @vlprecocusto WHERE idproduto=@idProduto";

            using (con)
            {
                SqlTransaction sqlTrans = con.BeginTransaction();

                try
                {
                    comando1.Transaction = sqlTrans;
                    comando1.Parameters.AddWithValue("@modnota", compra.modNota);
                    comando1.Parameters.AddWithValue("@serienota", compra.serieNota);
                    comando1.Parameters.AddWithValue("@nrnota", compra.nrNota);
                    comando1.Parameters.AddWithValue("@idfornecedor", compra.idFornecedor);
                    comando1.Parameters.AddWithValue("@idcondpagamento", compra.idCondPagamento);
                    comando1.Parameters.AddWithValue("@dtemissao", compra.dtEmissao);
                    comando1.Parameters.AddWithValue("@dtentrega", (object)compra.dtEntrega ?? DBNull.Value);
                    comando1.Parameters.AddWithValue("@vlfrete", (object)compra.vlFrete ?? DBNull.Value);
                    comando1.Parameters.AddWithValue("@vlseguro", (object)compra.vlSeguro ?? DBNull.Value);
                    comando1.Parameters.AddWithValue("@vldespesas", (object)compra.vlDespesas ?? DBNull.Value);
                    comando1.Parameters.AddWithValue("@vltotal", compra.vlTotal);
                    comando1.Parameters.AddWithValue("@dtcadastro", compra.dtCadastro);
                    comando1.Parameters.AddWithValue("@dtatualizacao", compra.dtAtualizacao);
                    comando1.ExecuteNonQuery();

                    comando2.Transaction = sqlTrans;

                    foreach (var item in compra.ItensCompra)
                    {
                        decimal vlUnitarioAtualizado = this.CalcValorUnitario(item.qtProduto, item.vlUnitario, compra.vlFrete, compra.vlSeguro, compra.vlDespesas, (item.qtProduto * item.vlUnitario), vlTotalProdutos);
                        comando2.Parameters.Clear();
                        comando2.Parameters.AddWithValue("@modnota", compra.modNota);
                        comando2.Parameters.AddWithValue("@serienota", compra.serieNota);
                        comando2.Parameters.AddWithValue("@nrnota", compra.nrNota);
                        comando2.Parameters.AddWithValue("@idfornecedor", compra.idFornecedor);
                        comando2.Parameters.AddWithValue("@idproduto", item.idProduto);
                        comando2.Parameters.AddWithValue("@qtitem", item.qtProduto);
                        comando2.Parameters.AddWithValue("@vlunitario", vlUnitarioAtualizado);
                        comando2.Parameters.AddWithValue("@dtcadastro", compra.dtCadastro);
                        comando2.Parameters.AddWithValue("@dtatualizacao", compra.dtAtualizacao);
                        comando2.Parameters.AddWithValue("@flunidade", item.flUnidade == "UNIDADE" ? "U" : "G");
                        comando2.ExecuteNonQuery();
                    }

                    comando3.Transaction = sqlTrans;

                    foreach (var item in compra.ContasPagar)
                    {
                        comando3.Parameters.Clear();
                        comando3.Parameters.AddWithValue("@modnota", compra.modNota);
                        comando3.Parameters.AddWithValue("@serienota", compra.serieNota);
                        comando3.Parameters.AddWithValue("@nrnota", compra.nrNota);
                        comando3.Parameters.AddWithValue("@idfornecedor", compra.idFornecedor);
                        comando3.Parameters.AddWithValue("@nrparcela", item.nrParcela);
                        comando3.Parameters.AddWithValue("@dtvencimento", item.dtVencimento);
                        comando3.Parameters.AddWithValue("@flsituacao", Models.ContasPagar.SITUACAO_PENDENTE);
                        comando3.Parameters.AddWithValue("@dtcadastro", compra.dtCadastro);
                        comando3.Parameters.AddWithValue("@dtatualizacao", compra.dtAtualizacao);
                        comando3.Parameters.AddWithValue("@idformapagamento", item.idFormaPagamento);
                        comando3.Parameters.AddWithValue("@vlparcela", item.vlParcela);
                        comando3.ExecuteNonQuery();
                    }

                    comando4.Transaction = sqlTrans;

                    foreach (var item in compra.ItensCompra)
                    {
                        decimal vlUnitarioAtualizado = this.CalcValorUnitario(item.qtProduto, item.vlUnitario, compra.vlFrete, compra.vlSeguro, compra.vlDespesas, (item.qtProduto * item.vlUnitario), vlTotalProdutos);
                        decimal? vlPrecoCusto = this.CalcPrecoCustoProduto(item.idProduto, item.qtProduto, vlUnitarioAtualizado);
                        comando4.Parameters.Clear();
                        comando4.Parameters.AddWithValue("@idProduto", item.idProduto);
                        comando4.Parameters.AddWithValue("@nrestoque", item.qtProduto);
                        comando4.Parameters.AddWithValue("@vlPrecoUltCompra", item.vlUnitario);
                        comando4.Parameters.AddWithValue("@vlprecocusto", vlPrecoCusto != 0 ? vlPrecoCusto : item.vlUnitario);
                        comando4.ExecuteNonQuery();
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


        public bool VerificaDuplicidade(string modNota, string serieNota, string nrNota, int idFornecedor)
        {
            try
            {
                AbrirConexao();
                var _where = string.Empty;

                _where = " WHERE tbCompras.modnota = '" + modNota + "'" + "AND tbCompras.serienota = '" + serieNota
                           + "'" + "AND tbCompras.nrnota =" + nrNota + "AND tbCompras.idfornecedor =" + idFornecedor;

                SqlQuery = new SqlCommand("SELECT * FROM tbCompras" + _where, con);
                reader = SqlQuery.ExecuteReader();

                if (reader.Read())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            finally
            {
                FecharConexao();
            }
        }

        public decimal CalcValorUnitario(int qtProduto, decimal vlUnitario, decimal vlFrete, decimal vlSeguro, decimal vlDespesas, decimal vlTotalProduto, decimal vlTotal)
        {
            var somaDespesas = vlFrete + vlSeguro + vlDespesas;
            if (vlFrete == 0 && vlSeguro == 0 && vlDespesas == 0)
            {
                return vlUnitario;
            }
            else
            {
                var vlUnitarioAtualizado = (((vlTotalProduto / vlTotal) * somaDespesas) / qtProduto) + vlUnitario;

                return vlUnitarioAtualizado;
            }
        }

        public decimal CalcPrecoCustoProduto(int idProduto, int qtEntrada, decimal custoEntrada)
        {

            var daoProduto = new DAOProduto();
            Produto produto = daoProduto.GetProdutosByID(idProduto);
            if (produto.vlPrecoCusto == 0)
            {
                return produto.vlPrecoCusto ?? 0;
            }
            else
            {
                var custo = ((produto.nrEstoque * produto.vlPrecoCusto) + (qtEntrada * custoEntrada)) / (produto.nrEstoque + qtEntrada);

                return custo ?? 0;
            }
        }

        public decimal CalcDiferencaCusto(int qtProduto, decimal vlUnitario, decimal vlFrete, decimal vlSeguro, decimal vlDespesas, decimal vlTotalProduto, decimal vlTotal)
        {
            var somaDespesas = vlFrete + vlSeguro + vlDespesas;
            if (vlFrete == 0 && vlSeguro == 0 && vlDespesas == 0)
            {
                return vlUnitario;
            }
            else
            {
                var vlUnitarioAtualizado = (((vlTotalProduto / vlTotal) * somaDespesas) / qtProduto);

                return vlUnitarioAtualizado;
            }
        }

        public List<Compra> GetCompras()
        {
            try
            {
                AbrirConexao();
                SqlQuery = new SqlCommand("SELECT * FROM tbcompras INNER JOIN tbFornecedores on tbcompras.idfornecedor = tbFornecedores.idfornecedor", con);
                reader = SqlQuery.ExecuteReader();

                var lista = new List<Compra>();

                while (reader.Read())
                {
                    var compra = new Compra
                    {
                        modNota = Convert.ToString(reader["modnota"]),
                        serieNota = Convert.ToString(reader["serieNota"]),
                        nrNota = Convert.ToInt32(reader["nrnota"]),
                        dtEmissao = Convert.ToDateTime(reader["dtemissao"]),
                        Fornecedor = new Fornecedor
                        {
                            idPessoa = Convert.ToInt32(reader["idFornecedor"]),
                            nmPessoa = Convert.ToString(reader["nmRazaoSocial"]),
                        },
                        vlTotal = Convert.ToDecimal(reader["vltotal"]),
                        dtCadastro = Convert.ToDateTime(reader["dtcadastro"]),
                        dtAtualizacao = Convert.ToDateTime(reader["dtAtualizacao"]),
                    };

                    lista.Add(compra);
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

        public Compra GetComprasByID(string modNota, string serieNota, int nrNota, int idFornecedor)
        {
            try
            {
                AbrirConexao();
                var _where = string.Empty;
                _where = " WHERE modnota = '" + modNota + "' AND A.serienota  = '" + serieNota + "' AND A.nrnota  = " + nrNota + " AND A.idfornecedor =" + idFornecedor;
                SqlQuery = new SqlCommand("SELECT * FROM tbcompras A INNER JOIN tbFornecedores B on A.idfornecedor = B.idfornecedor" + _where, con);
                reader = SqlQuery.ExecuteReader();
                var objCompra = new Compra();
                while (reader.Read())
                {
                    objCompra = new Compra()
                    {
                        modNota = Convert.ToString(reader["modnota"]),
                        serieNota = Convert.ToString(reader["serienota"]),
                        nrNota = Convert.ToInt32(reader["nrnota"]),
                        dtEmissao = Convert.ToDateTime(reader["dtemissao"]),
                        dtEntrega = Convert.ToDateTime(reader["dtentrega"] != DBNull.Value ? reader["dtentrega"] : null),
                        idCondPagamento = Convert.ToInt32(reader["idcondpagamento"]),
                        idFornecedor = Convert.ToInt32(reader["idfornecedor"]),
                        vlDespesas = Convert.ToDecimal(reader["vldespesas"] != DBNull.Value ? reader["vldespesas"] : null),
                        vlFrete = Convert.ToDecimal(reader["vlfrete"] != DBNull.Value ? reader["vlfrete"] : null),
                        vlSeguro = Convert.ToDecimal(reader["vlseguro"] != DBNull.Value ? reader["vlseguro"] : null),
                        vlTotal = Convert.ToDecimal(reader["vltotal"]),
                        Fornecedor = new Fornecedor
                        {
                            idPessoa = Convert.ToInt32(reader["idFornecedor"]),
                            nmPessoa = Convert.ToString(reader["nmrazaosocial"]),
                        },
                        dtCadastro = Convert.ToDateTime(reader["dtcadastro"]),
                        dtAtualizacao = Convert.ToDateTime(reader["dtatualizacao"]),
                        ItensCompra = GetItensByID(modNota, serieNota, nrNota, idFornecedor),
                        ContasPagar = GetParcelasByID(modNota, serieNota, nrNota, idFornecedor),
                    };
                }
                return objCompra;
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

        public List<ItemCompra> GetItensByID(string modNota, string serieNota, int nrNota, int idFornecedor)
        {
            try
            {
                AbrirConexao();
                var _where = string.Empty;
                _where = " WHERE A.modnota = '" + modNota + "' AND A.serienota  = '" + serieNota + "' AND A.nrnota  = " + nrNota + " AND A.idfornecedor =" + idFornecedor;
                SqlQuery = new SqlCommand("SELECT * FROM tbItemCompra A INNER JOIN tbProdutos B on A.idproduto = B.idproduto" + _where, con);
                reader = SqlQuery.ExecuteReader();
                List<ItemCompra> parcelas = new List<ItemCompra>();
                while (reader.Read())
                {
                    var obj = new ItemCompra()
                    {
                        modNota = Convert.ToString(reader["modnota"]),
                        serieNota = Convert.ToString(reader["serienota"]),
                        nrNota = Convert.ToInt32(reader["nrnota"]),
                        idFornecedor = Convert.ToInt32(reader["idfornecedor"]),
                        idProduto = Convert.ToInt32(reader["idproduto"]),
                        nmProduto = Convert.ToString(reader["nmproduto"]),
                        qtProduto = Convert.ToInt32(reader["qtitem"]),
                        vlUnitario = Convert.ToDecimal(reader["vlunitario"]),
                        vlTotalProduto = Convert.ToInt32(reader["qtitem"]) * Convert.ToDecimal(reader["vlunitario"]),
                        dtCadastro = Convert.ToDateTime(reader["dtcadastro"]),
                        dtAtualizacao = Convert.ToDateTime(reader["dtatualizacao"]),
                        flUnidade = Convert.ToString(reader["flunidade"]),
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

        public List<ContasPagar> GetParcelasByID(string modNota, string serieNota, int nrNota, int idFornecedor)
        {
            try
            {
                AbrirConexao();
                var _where = string.Empty;
                _where = " WHERE A.modnota = '" + modNota + "' AND A.serienota  = '" + serieNota + "' AND A.nrnota  = " + nrNota + " AND A.idfornecedor =" + idFornecedor;
                SqlQuery = new SqlCommand("SELECT * FROM tbContasPagar A INNER JOIN tbFormaPagamentos B on A.idformapagamento = B.idformapagamento" + _where, con);
                reader = SqlQuery.ExecuteReader();
                List<ContasPagar> parcelas = new List<ContasPagar>();
                while (reader.Read())
                {
                    var obj = new ContasPagar()
                    {
                        modNota = Convert.ToString(reader["modnota"]),
                        serieNota = Convert.ToString(reader["serienota"]),
                        nrNota = Convert.ToInt32(reader["nrnota"]),
                        idFornecedor = Convert.ToInt32(reader["idfornecedor"]),
                        nrParcela = Convert.ToInt32(reader["nrparcela"]),
                        dtVencimento = Convert.ToDateTime(reader["dtvencimento"]),
                        flSituacao = Convert.ToString(reader["flsituacao"]),
                        vlDesconto = Convert.ToDecimal(reader["vldesconto"] != DBNull.Value ? reader["vldesconto"] : null),
                        vlJuros = Convert.ToDecimal(reader["vljuros"] != DBNull.Value ? reader["vljuros"] : null),
                        vlMulta = Convert.ToDecimal(reader["vlmulta"] != DBNull.Value ? reader["vlmulta"] : null),
                        vlPago = Convert.ToDecimal(reader["vlpago"] != DBNull.Value ? reader["vlpago"] : null),
                        dtPagamento = Convert.ToDateTime(reader["dtpagamento"] != DBNull.Value ? reader["dtpagamento"] : null),
                        vlParcela = Convert.ToDecimal(reader["vlparcela"]),
                        dtCadastro = Convert.ToDateTime(reader["dtcadastro"]),
                        dtAtualizacao = Convert.ToDateTime(reader["dtatualizacao"]),
                        idFormaPagamento = Convert.ToInt32(reader["idformapagamento"]),
                        nmFormaPagamento = Convert.ToString(reader["nmformapagamento"]),
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
    }
}