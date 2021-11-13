using Pratica_Profissional.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Pratica_Profissional.DAO
{
    public class DAOProduto : DAO
    {

        public bool Create(Produto produto)
        {
            try
            {
                AbrirConexao();
                SqlQuery = new SqlCommand("INSERT INTO tbProdutos (nmproduto, flunidade, idcategoria, nrestoque, vlprecocusto, vlprecovenda, vlprecoultcompra, " +
                    "dtCadastro, dtatualizacao, idfornecedor) VALUES (@nmProduto, @flUnidade, @idCategoria, @nrEstoque, @vlPrecoCusto, @vlPrecoVenda, @vlPrecoUltCompra, " +
                    "@dtCadastro, @dtAtualizacao, @idfornecedor)", con);

                SqlQuery.Parameters.AddWithValue("@nmproduto", produto.nmProduto);
                SqlQuery.Parameters.AddWithValue("@flunidade", produto.flUnidade);
                SqlQuery.Parameters.AddWithValue("@idCategoria", produto.idCategoria);
                SqlQuery.Parameters.AddWithValue("@nrEstoque", produto.nrEstoque);
                SqlQuery.Parameters.AddWithValue("@vlPrecoCusto", (object)produto.vlPrecoCusto ?? DBNull.Value);
                SqlQuery.Parameters.AddWithValue("@vlPrecoVenda", produto.vlPrecoVenda);
                SqlQuery.Parameters.AddWithValue("@vlPrecoUltCompra", (object)produto.vlPrecoUltCompra ?? DBNull.Value);
                SqlQuery.Parameters.AddWithValue("@dtCadastro", produto.dtCadastro);
                SqlQuery.Parameters.AddWithValue("@dtAtualizacao", produto.dtAtualizacao);
                SqlQuery.Parameters.AddWithValue("@idfornecedor", produto.idFornecedor);


                // Tratamento para saber se alguma linha foi alterada no Banco de Dados
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

        public List<Produto> GetProdutos()
        {
            try
            {
                AbrirConexao();
                SqlQuery = new SqlCommand("SELECT * FROM tbProdutos INNER JOIN tbCategorias on tbProdutos.idCategoria = tbCategorias.idCategoria " +
                    "INNER JOIN tbFornecedores on tbProdutos.idfornecedor = tbFornecedores.idfornecedor", con);
                reader = SqlQuery.ExecuteReader();

                var lista = new List<Produto>();

                while (reader.Read())
                {
                    var produto = new Produto
                    {
                        idProduto = Convert.ToInt32(reader["idproduto"]),
                        nmProduto = Convert.ToString(reader["nmproduto"]),
                        flUnidade = Convert.ToString(reader["flunidade"]),
                        nrEstoque = Convert.ToInt32(reader["nrestoque"]),
                        vlPrecoCusto = Convert.ToDecimal(reader["vlprecocusto"] != DBNull.Value ? reader["vlprecocusto"] : null),
                        vlPrecoVenda = Convert.ToDecimal(reader["vlprecovenda"]),
                        vlPrecoUltCompra = Convert.ToDecimal(reader["vlprecoultcompra"] != DBNull.Value ? reader["vlprecoultcompra"] : null),
                        dtCadastro = Convert.ToDateTime(reader["dtcadastro"]),
                        dtAtualizacao = Convert.ToDateTime(reader["dtatualizacao"]),
                        Categoria = new Categoria
                        {
                            idCategoria = Convert.ToInt32(reader["idcategoria"]),
                            nmCategoria = Convert.ToString(reader["nmcategoria"]),
                        },
                        Fornecedor = new Fornecedor
                        {
                            idPessoa = Convert.ToInt32(reader["idfornecedor"]),
                            nmPessoa = Convert.ToString(reader["nmrazaosocial"]),
                        }
                    };

                    lista.Add(produto);
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

        public Produto GetProdutosByID(int? idProduto)
        {
            try
            {
                AbrirConexao();
                var _where = string.Empty;
                _where = " WHERE idProduto = " + idProduto;
                SqlQuery = new SqlCommand("SELECT * FROM tbProdutos INNER JOIN tbCategorias on tbProdutos.idcategoria = tbCategorias.idcategoria " +
                     "INNER JOIN tbFornecedores on tbProdutos.idfornecedor = tbFornecedores.idfornecedor" + _where, con);
                reader = SqlQuery.ExecuteReader();
                var objProduto = new Produto();
                while (reader.Read())
                {
                    objProduto = new Produto()
                    {
                        idProduto = Convert.ToInt32(reader["idproduto"]),
                        nmProduto = Convert.ToString(reader["nmproduto"]),
                        flUnidade = Convert.ToString(reader["flunidade"]),
                        nrEstoque = Convert.ToInt32(reader["nrestoque"]),
                        vlPrecoCusto = Convert.ToDecimal(reader["vlprecocusto"] != DBNull.Value ? reader["vlprecocusto"] : null),
                        vlPrecoVenda = Convert.ToDecimal(reader["vlprecovenda"]),
                        vlPrecoUltCompra = Convert.ToDecimal(reader["vlprecoultcompra"] != DBNull.Value ? reader["vlprecoultcompra"] : null),
                        dtCadastro = Convert.ToDateTime(reader["dtcadastro"]),
                        dtAtualizacao = Convert.ToDateTime(reader["dtatualizacao"]),
                        Categoria = new Categoria
                        {
                            idCategoria = Convert.ToInt32(reader["idcategoria"]),
                            nmCategoria = Convert.ToString(reader["nmcategoria"]),
                        },
                        Fornecedor = new Fornecedor
                        {
                            idPessoa = Convert.ToInt32(reader["idfornecedor"]),
                            nmPessoa = Convert.ToString(reader["nmrazaosocial"]),
                        }
                    };
                }
                return objProduto;
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

        public bool Edit(Produto produto)
        {
            try
            {
                AbrirConexao();
                SqlQuery = new SqlCommand("UPDATE tbProdutos SET nmproduto=@nmProduto, flunidade=@flUnidade, nrestoque=@nrEstoque, vlprecocusto=@vlPrecoCusto," +
                    "vlprecovenda=@vlPrecoVenda, vlprecoultcompra=@vlPrecoUltCompra, idcategoria=@idCategoria, dtatualizacao=@dtAtualizacao, idfornecedor=@idfornecedor " +
                    "WHERE idproduto=@idProduto", con);

                SqlQuery.Parameters.AddWithValue("@idProduto", produto.idProduto);
                SqlQuery.Parameters.AddWithValue("@nmProduto", produto.nmProduto);
                SqlQuery.Parameters.AddWithValue("@flUnidade", produto.flUnidade);
                SqlQuery.Parameters.AddWithValue("@nrEstoque", produto.nrEstoque);
                SqlQuery.Parameters.AddWithValue("@vlPrecoCusto", (object)produto.vlPrecoCusto ?? DBNull.Value);
                SqlQuery.Parameters.AddWithValue("@vlPrecoVenda", produto.vlPrecoVenda);
                SqlQuery.Parameters.AddWithValue("@vlPrecoUltCompra", (object)produto.vlPrecoUltCompra ?? DBNull.Value);
                SqlQuery.Parameters.AddWithValue("@idCategoria", produto.idCategoria);
                SqlQuery.Parameters.AddWithValue("@dtatualizacao", produto.dtAtualizacao);
                SqlQuery.Parameters.AddWithValue("@idfornecedor", produto.idFornecedor);

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

        public bool Delete(int id)
        {
            try
            {
                AbrirConexao();
                SqlQuery = new SqlCommand("DELETE FROM tbProdutos WHERE idproduto=@idProduto", con);

                SqlQuery.Parameters.AddWithValue("@idProduto", id);
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
