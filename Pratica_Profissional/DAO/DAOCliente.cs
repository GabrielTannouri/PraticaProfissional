using Pratica_Profissional.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Pratica_Profissional.DAO
{
    public class DAOCliente : DAO
    {
        public bool Create(Cliente cliente)
        {
            try
            {
                AbrirConexao();
                SqlQuery = new SqlCommand("INSERT INTO tbClientes (idcidade, nmcliente, nmApelidoFantasia, rg_inscestadual, cpf_cnpj, genero, email, endereco, bairro, nrendereco, complemento, idcondicaopagamento," +
                    "cep, nrtel, limitecredito, dtnascimento, dtcadastro, dtatualizacao, fltipo, dsObservacao) VALUES (@idcidade, @nmcliente, @nmApelidoFantasia, @rg_inscestadual, @cpf_cnpj, @genero, @email, @endereco, @bairro, @nrendereco," +
                    "@complemento, @idcondicaopagamento, @cep, @nrtel, @limitecredito, @dtnascimento, @dtcadastro, @dtatualizacao, @fltipo, @dsObservacao)", con);

                SqlQuery.Parameters.AddWithValue("@idcidade", cliente.idCidade);
                SqlQuery.Parameters.AddWithValue("@nmcliente", cliente.nmPessoa);
                SqlQuery.Parameters.AddWithValue("@nmApelidoFantasia", cliente.nmApelido ?? (object)DBNull.Value);
                SqlQuery.Parameters.AddWithValue("@rg_inscestadual", cliente.rg ?? (object)DBNull.Value);
                SqlQuery.Parameters.AddWithValue("@cpf_cnpj", cliente.documento);
                SqlQuery.Parameters.AddWithValue("@genero", cliente.genero);
                SqlQuery.Parameters.AddWithValue("@email", cliente.email ?? (object)DBNull.Value);
                SqlQuery.Parameters.AddWithValue("@endereco", cliente.endereco);
                SqlQuery.Parameters.AddWithValue("@bairro", cliente.bairro);
                SqlQuery.Parameters.AddWithValue("@nrendereco", cliente.nrEndereco);
                SqlQuery.Parameters.AddWithValue("@complemento", cliente.complemento ?? (object)DBNull.Value);
                SqlQuery.Parameters.AddWithValue("@idcondicaopagamento", cliente.idCondicaoPagamento);
                SqlQuery.Parameters.AddWithValue("@cep", cliente.cep);
                SqlQuery.Parameters.AddWithValue("@nrtel", cliente.nrTel);
                SqlQuery.Parameters.AddWithValue("@limitecredito", (object)cliente.limiteCredito ?? DBNull.Value);
                SqlQuery.Parameters.AddWithValue("@dtnascimento", (object)cliente.dtNascimento ?? DBNull.Value);
                SqlQuery.Parameters.AddWithValue("@dtcadastro", cliente.dtCadastro);
                SqlQuery.Parameters.AddWithValue("@dtatualizacao", cliente.dtAtualizacao);
                SqlQuery.Parameters.AddWithValue("@fltipo", cliente.flTipo);
                SqlQuery.Parameters.AddWithValue("@dsObservacao", cliente.dsObservacao ?? (object)DBNull.Value);

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

        public List<Cliente> GetClientes()
        {
            try
            {
                AbrirConexao();
                SqlQuery = new SqlCommand("SELECT * FROM tbClientes INNER JOIN tbCidades on tbClientes.idcidade = tbCidades.idcidade " +
                                          "INNER JOIN tbCondicaoPagamentos on tbClientes.idcondicaopagamento = tbCondicaoPagamentos.idcondicaopagamento", con);
                reader = SqlQuery.ExecuteReader();

                var lista = new List<Cliente>();

                while (reader.Read())
                {
                    var cliente = new Cliente
                    {
                        idPessoa = Convert.ToInt32(reader["idcliente"]),
                        cidade = new Cidade
                        {
                            idCidade = Convert.ToInt32(reader["idcidade"]),
                            nmCidade = Convert.ToString(reader["nmcidade"]),
                        },
                        nmPessoa = Convert.ToString(reader["nmcliente"]),
                        nmApelido = Convert.ToString(reader["nmApelidoFantasia"]),
                        rg = Convert.ToString(reader["rg_inscestadual"]),
                        documento = Convert.ToString(reader["cpf_cnpj"]),
                        genero = Convert.ToString(reader["genero"]),
                        email = Convert.ToString(reader["email"]),
                        endereco = Convert.ToString(reader["endereco"]),
                        bairro = Convert.ToString(reader["bairro"]),
                        nrEndereco = Convert.ToString(reader["nrendereco"]),
                        complemento = Convert.ToString(reader["complemento"]),
                        condicaoPagamento = new CondicaoPagamento
                        {
                            idCondicaoPagamento = Convert.ToInt32(reader["idcondicaopagamento"]),
                            nmCondicaoPagamento = Convert.ToString(reader["nmcondicaopagamento"]),
                        },
                        cep = Convert.ToString(reader["cep"]),
                        nrTel = Convert.ToString(reader["nrtel"]),
                        limiteCredito = Convert.ToDecimal(reader["limitecredito"]),
                        dtNascimento = Convert.ToString(reader["dtnascimento"]),
                        dtCadastro = Convert.ToDateTime(reader["dtcadastro"]),
                        dtAtualizacao = Convert.ToDateTime(reader["dtatualizacao"]),
                        flTipo = Convert.ToString(reader["fltipo"]),
                        dsObservacao = Convert.ToString(reader["dsObservacao"]),
                    };

                    lista.Add(cliente);
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

        public Cliente GetClientesByID(int? idCliente)
        {
            try
            {
                AbrirConexao();
                var _where = string.Empty;
                _where = " WHERE idcliente = " + idCliente;
                SqlQuery = new SqlCommand("SELECT * FROM tbClientes INNER JOIN tbCidades on tbClientes.idcidade = tbCidades.idcidade " +
                                          "INNER JOIN tbCondicaoPagamentos on tbClientes.idcondicaopagamento = tbCondicaoPagamentos.idcondicaopagamento" + _where, con);
                reader = SqlQuery.ExecuteReader();
                var objCliente = new Cliente();
                while (reader.Read())
                {
                    objCliente = new Cliente()
                    {
                        idPessoa = Convert.ToInt32(reader["idcliente"]),
                        cidade = new Cidade
                        {
                            idCidade = Convert.ToInt32(reader["idcidade"]),
                            nmCidade = Convert.ToString(reader["nmcidade"]),
                        },
                        nmPessoa = Convert.ToString(reader["nmcliente"]),
                        nmApelido = Convert.ToString(reader["nmApelidoFantasia"]),
                        rg = Convert.ToString(reader["rg_inscestadual"]),
                        documento = Convert.ToString(reader["cpf_cnpj"]),
                        genero = Convert.ToString(reader["genero"]),
                        email = Convert.ToString(reader["email"]),
                        endereco = Convert.ToString(reader["endereco"]),
                        bairro = Convert.ToString(reader["bairro"]),
                        nrEndereco = Convert.ToString(reader["nrendereco"]),
                        complemento = Convert.ToString(reader["complemento"]),
                        condicaoPagamento = new CondicaoPagamento
                        {
                            idCondicaoPagamento = Convert.ToInt32(reader["idcondicaopagamento"]),
                            nmCondicaoPagamento = Convert.ToString(reader["nmcondicaopagamento"]),
                        },
                        cep = Convert.ToString(reader["cep"]),
                        nrTel = Convert.ToString(reader["nrtel"]),
                        limiteCredito = Convert.ToDecimal(reader["limitecredito"]),
                        dtNascimento = Convert.ToString(reader["dtnascimento"]),
                        dtCadastro = Convert.ToDateTime(reader["dtcadastro"]),
                        dtAtualizacao = Convert.ToDateTime(reader["dtatualizacao"]),
                        flTipo = Convert.ToString(reader["fltipo"]),
                        dsObservacao = Convert.ToString(reader["dsObservacao"]),
                    };
                }
                return objCliente;
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

        public bool Edit(Cliente cliente)
        {
            try
            {
                AbrirConexao();
                SqlQuery = new SqlCommand("UPDATE tbClientes SET idcidade=@idcidade, nmcliente=@nmcliente, nmApelidoFantasia=@nmApelidoFantasia, rg_inscestadual=@rg_inscestadual, " +
                    "cpf_cnpj=@cpf_cnpj, genero=@genero, email=@email, endereco=@endereco, bairro=@bairro, nrendereco=@nrendereco, complemento=@complemento, " +
                    "idcondicaopagamento=@idcondicaopagamento, cep=@cep, nrtel=@nrtel, limitecredito=@limitecredito, dtnascimento=@dtnascimento, " +
                    "dtatualizacao=@dtatualizacao, fltipo=@fltipo, dsObservacao=@dsObservacao WHERE idcliente=@idcliente", con);

                SqlQuery.Parameters.AddWithValue("@idcliente", cliente.idPessoa);
                SqlQuery.Parameters.AddWithValue("@idcidade", cliente.idCidade);
                SqlQuery.Parameters.AddWithValue("@nmcliente", cliente.nmPessoa);
                SqlQuery.Parameters.AddWithValue("@nmApelidoFantasia", cliente.nmApelido ?? (object)DBNull.Value);
                SqlQuery.Parameters.AddWithValue("@rg_inscestadual", cliente.rg ?? (object)DBNull.Value);
                SqlQuery.Parameters.AddWithValue("@cpf_cnpj", cliente.documento);
                SqlQuery.Parameters.AddWithValue("@genero", cliente.genero);
                SqlQuery.Parameters.AddWithValue("@email", cliente.email ?? (object)DBNull.Value);
                SqlQuery.Parameters.AddWithValue("@endereco", cliente.endereco);
                SqlQuery.Parameters.AddWithValue("@bairro", cliente.bairro);
                SqlQuery.Parameters.AddWithValue("@nrendereco", cliente.nrEndereco);
                SqlQuery.Parameters.AddWithValue("@complemento", cliente.complemento ?? (object)DBNull.Value);
                SqlQuery.Parameters.AddWithValue("@idcondicaopagamento", cliente.idCondicaoPagamento);
                SqlQuery.Parameters.AddWithValue("@cep", cliente.cep);
                SqlQuery.Parameters.AddWithValue("@nrtel", cliente.nrTel);
                SqlQuery.Parameters.AddWithValue("@limitecredito", (object)cliente.limiteCredito ?? DBNull.Value);
                SqlQuery.Parameters.AddWithValue("@dtnascimento", (object)cliente.dtNascimento ?? DBNull.Value);
                SqlQuery.Parameters.AddWithValue("@dtatualizacao", cliente.dtAtualizacao);
                SqlQuery.Parameters.AddWithValue("@fltipo", cliente.flTipo);
                SqlQuery.Parameters.AddWithValue("@dsObservacao", cliente.dsObservacao ?? (object)DBNull.Value);

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
                SqlQuery = new SqlCommand("DELETE FROM tbClientes WHERE idcliente=@idcliente", con);

                SqlQuery.Parameters.AddWithValue("@idcliente", id);
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
