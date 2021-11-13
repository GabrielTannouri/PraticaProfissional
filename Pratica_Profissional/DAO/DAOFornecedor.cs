using Pratica_Profissional.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Pratica_Profissional.DAO
{
    public class DAOFornecedor : DAO
    {

        public bool Create(Fornecedor fornecedor)
        {
            try
            {
                AbrirConexao();
                SqlQuery = new SqlCommand("INSERT INTO tbFornecedores (nmRazaoSocial, nmApelidoFantasia, cpf_cnpj, rg_inscestadual, cep, endereco, bairro, nrendereco, complemento," +
                    "idcidade, email, nrtel, nrcel, site, dsObservacao, dtcadastro, dtatualizacao, fltipo, idCondicaoPagamento, limiteCredito) VALUES (@nmRazaoSocial, @nmApelidoFantasia, @cpf_cnpj, @rg_inscestadual," +
                    "@cep, @endereco, @bairro, @nrendereco, @complemento, @idcidade, @email, @nrtel, @nrcel, @site, @dsObservacao, @dtcadastro, @dtatualizacao, @fltipo, @idCondicaoPagamento, @limiteCredito)", con);

                SqlQuery.Parameters.AddWithValue("@nmRazaoSocial", fornecedor.nmPessoa);
                SqlQuery.Parameters.AddWithValue("@nmApelidoFantasia", fornecedor.nmApelido ?? (object)DBNull.Value);
                SqlQuery.Parameters.AddWithValue("@cpf_cnpj", fornecedor.documento);
                SqlQuery.Parameters.AddWithValue("@rg_inscestadual", fornecedor.rg ?? (object)DBNull.Value);
                SqlQuery.Parameters.AddWithValue("@cep", fornecedor.cep);
                SqlQuery.Parameters.AddWithValue("@endereco", fornecedor.endereco);
                SqlQuery.Parameters.AddWithValue("@bairro", fornecedor.bairro);
                SqlQuery.Parameters.AddWithValue("@nrendereco", fornecedor.nrEndereco);
                SqlQuery.Parameters.AddWithValue("@complemento", fornecedor.complemento ?? (object)DBNull.Value);
                SqlQuery.Parameters.AddWithValue("@idcidade", fornecedor.idCidade);
                SqlQuery.Parameters.AddWithValue("@email", fornecedor.email ?? (object)DBNull.Value);
                SqlQuery.Parameters.AddWithValue("@nrtel", fornecedor.nrTel);
                SqlQuery.Parameters.AddWithValue("@nrcel", fornecedor.nrCel);
                SqlQuery.Parameters.AddWithValue("@site", fornecedor.site ?? (object)DBNull.Value);
                SqlQuery.Parameters.AddWithValue("@dsObservacao", fornecedor.dsObservacao ?? (object)DBNull.Value);
                SqlQuery.Parameters.AddWithValue("@dtcadastro", fornecedor.dtCadastro);
                SqlQuery.Parameters.AddWithValue("@dtatualizacao", fornecedor.dtAtualizacao);
                SqlQuery.Parameters.AddWithValue("@fltipo", fornecedor.flTipo);
                SqlQuery.Parameters.AddWithValue("@idCondicaoPagamento", fornecedor.idCondicaoPagamento);
                SqlQuery.Parameters.AddWithValue("@limiteCredito", (object)fornecedor.limiteCredito ?? DBNull.Value);

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

        public List<Fornecedor> GetFornecedores()
        {
            try
            {
                AbrirConexao();
                SqlQuery = new SqlCommand("SELECT * FROM tbFornecedores INNER JOIN tbCidades on tbFornecedores.idcidade = tbCidades.idcidade " +
                            "INNER JOIN tbCondicaoPagamentos on tbFornecedores.idcondicaopagamento = tbCondicaoPagamentos.idcondicaopagamento", con);
                reader = SqlQuery.ExecuteReader();

                var lista = new List<Fornecedor>();

                while (reader.Read())
                {
                    var fornecedor = new Fornecedor
                    {
                        idPessoa = Convert.ToInt32(reader["idFornecedor"]),
                        nmPessoa = Convert.ToString(reader["nmRazaoSocial"]),
                        nmApelido = Convert.ToString(reader["nmApelidoFantasia"]),
                        documento = Convert.ToString(reader["cpf_cnpj"]),
                        rg = Convert.ToString(reader["rg_inscestadual"]),
                        cep = Convert.ToString(reader["cep"]),
                        endereco = Convert.ToString(reader["endereco"]),
                        bairro = Convert.ToString(reader["bairro"]),
                        nrEndereco = Convert.ToString(reader["nrendereco"]),
                        complemento = Convert.ToString(reader["complemento"]),
                        cidade = new Cidade
                        {
                            idCidade = Convert.ToInt32(reader["idcidade"]),
                            nmCidade = Convert.ToString(reader["nmcidade"]),
                        },
                        email = Convert.ToString(reader["email"]),
                        nrTel = Convert.ToString(reader["nrtel"]),
                        nrCel = Convert.ToString(reader["nrcel"]),
                        site = Convert.ToString(reader["site"]),
                        dsObservacao = Convert.ToString(reader["dsObservacao"]),
                        dtCadastro = Convert.ToDateTime(reader["dtcadastro"]),
                        dtAtualizacao = Convert.ToDateTime(reader["dtatualizacao"]),
                        flTipo = Convert.ToString(reader["fltipo"]),
                        condicaoPagamento = new CondicaoPagamento
                        {
                            idCondicaoPagamento = Convert.ToInt32(reader["idcondicaopagamento"]),
                            nmCondicaoPagamento = Convert.ToString(reader["nmcondicaopagamento"]),
                        },
                        limiteCredito = Convert.ToDecimal(reader["limiteCredito"]),
                    };

                    lista.Add(fornecedor);
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

        public Fornecedor GetFornecedoresByID(int? idFornecedor)
        {
            try
            {
                AbrirConexao();
                var _where = string.Empty;
                _where = " WHERE idfornecedor = " + idFornecedor;
                SqlQuery = new SqlCommand("SELECT * FROM tbFornecedores INNER JOIN tbCidades on tbFornecedores.idcidade = tbCidades.idcidade " +
                            "INNER JOIN tbCondicaoPagamentos on tbFornecedores.idcondicaopagamento = tbCondicaoPagamentos.idcondicaopagamento" + _where, con);
                reader = SqlQuery.ExecuteReader();
                var objFornecedor = new Fornecedor();
                while (reader.Read())
                {
                    objFornecedor = new Fornecedor
                    {
                        idPessoa = Convert.ToInt32(reader["idFornecedor"]),
                        nmPessoa = Convert.ToString(reader["nmRazaoSocial"]),
                        nmApelido = Convert.ToString(reader["nmApelidoFantasia"]),
                        documento = Convert.ToString(reader["cpf_cnpj"]),
                        rg = Convert.ToString(reader["rg_inscestadual"]),
                        cep = Convert.ToString(reader["cep"]),
                        endereco = Convert.ToString(reader["endereco"]),
                        bairro = Convert.ToString(reader["bairro"]),
                        nrEndereco = Convert.ToString(reader["nrendereco"]),
                        complemento = Convert.ToString(reader["complemento"]),
                        cidade = new Cidade
                        {
                            idCidade = Convert.ToInt32(reader["idcidade"]),
                            nmCidade = Convert.ToString(reader["nmcidade"]),
                        },
                        email = Convert.ToString(reader["email"]),
                        nrTel = Convert.ToString(reader["nrtel"]),
                        nrCel = Convert.ToString(reader["nrcel"]),
                        site = Convert.ToString(reader["site"]),
                        dsObservacao = Convert.ToString(reader["dsObservacao"]),
                        dtCadastro = Convert.ToDateTime(reader["dtcadastro"]),
                        dtAtualizacao = Convert.ToDateTime(reader["dtatualizacao"]),
                        flTipo = Convert.ToString(reader["fltipo"]),
                        condicaoPagamento = new CondicaoPagamento
                        {
                            idCondicaoPagamento = Convert.ToInt32(reader["idcondicaopagamento"]),
                            nmCondicaoPagamento = Convert.ToString(reader["nmcondicaopagamento"]),
                        },
                        limiteCredito = Convert.ToDecimal(reader["limiteCredito"]),
                    };
                }
                return objFornecedor;
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

        public bool Edit(Fornecedor fornecedor)
        {
            try
            {
                AbrirConexao();
                SqlQuery = new SqlCommand("UPDATE tbFornecedores SET nmRazaoSocial=@nmRazaoSocial, nmApelidoFantasia=@nmApelidoFantasia, cpf_cnpj=@cpf_cnpj, rg_inscestadual=@rg_inscestadual, " +
                    "cep=@cep, endereco=@endereco, bairro=@bairro, nrendereco=@nrendereco, complemento=@complemento, idcidade=@idcidade, email=@email, nrtel=@nrtel, nrcel=@nrcel, " +
                    "site=@site, dsObservacao=@dsObservacao, dtatualizacao=@dtatualizacao, fltipo=@fltipo, idCondicaoPagamento=@idCondicaoPagamento, limiteCredito=@limiteCredito WHERE idfornecedor=@idfornecedor", con);

                SqlQuery.Parameters.AddWithValue("@idfornecedor", fornecedor.idPessoa);
                SqlQuery.Parameters.AddWithValue("@nmRazaoSocial", fornecedor.nmPessoa);
                SqlQuery.Parameters.AddWithValue("@nmApelidoFantasia", fornecedor.nmApelido ?? (object)DBNull.Value);
                SqlQuery.Parameters.AddWithValue("@cpf_cnpj", fornecedor.documento);
                SqlQuery.Parameters.AddWithValue("@rg_inscestadual", fornecedor.rg ?? (object)DBNull.Value);
                SqlQuery.Parameters.AddWithValue("@cep", fornecedor.cep);
                SqlQuery.Parameters.AddWithValue("@endereco", fornecedor.endereco);
                SqlQuery.Parameters.AddWithValue("@bairro", fornecedor.bairro);
                SqlQuery.Parameters.AddWithValue("@nrendereco", fornecedor.nrEndereco);
                SqlQuery.Parameters.AddWithValue("@complemento", fornecedor.complemento ?? (object)DBNull.Value);
                SqlQuery.Parameters.AddWithValue("@idcidade", fornecedor.idCidade);
                SqlQuery.Parameters.AddWithValue("@email", fornecedor.email ?? (object)DBNull.Value);
                SqlQuery.Parameters.AddWithValue("@nrtel", fornecedor.nrTel);
                SqlQuery.Parameters.AddWithValue("@nrcel", fornecedor.nrCel);
                SqlQuery.Parameters.AddWithValue("@site", fornecedor.site ?? (object)DBNull.Value);
                SqlQuery.Parameters.AddWithValue("@dsObservacao", fornecedor.dsObservacao ?? (object)DBNull.Value);
                SqlQuery.Parameters.AddWithValue("@dtatualizacao", fornecedor.dtAtualizacao);
                SqlQuery.Parameters.AddWithValue("@fltipo", fornecedor.flTipo);
                SqlQuery.Parameters.AddWithValue("@idCondicaoPagamento", fornecedor.idCondicaoPagamento);
                SqlQuery.Parameters.AddWithValue("@limiteCredito", (object)fornecedor.limiteCredito ?? DBNull.Value);

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
                SqlQuery = new SqlCommand("DELETE FROM tbFornecedores WHERE idfornecedor=@idfornecedor", con);

                SqlQuery.Parameters.AddWithValue("@idfornecedor", id);
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
