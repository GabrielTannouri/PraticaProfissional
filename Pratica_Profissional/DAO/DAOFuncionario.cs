using Pratica_Profissional.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Pratica_Profissional.DAO
{
    public class DAOFuncionario : DAO
    {
        public bool Create(Funcionario funcionario)
        {
            try
            {
                AbrirConexao();
                SqlQuery = new SqlCommand("INSERT INTO tbFuncionarios (idcidade, nmfuncionario, nmApelido, rg, cpf, genero, email, endereco, bairro, nrendereco, complemento," +
                    "cep, dtnascimento, nrcel, cargo, dsobservacao, salario, dtAdmissao, dtcadastro,dtatualizacao) VALUES (@idcidade, @nmfuncionario, @nmApelido, " +
                    "@rg, @cpf, @genero, @email, @endereco, @bairro, @nrendereco, @complemento, @cep, @dtnascimento, @nrcel, @cargo, @dsobservacao, @salario, @dtAdmissao, " +
                    "@dtcadastro, @dtatualizacao)", con);

                SqlQuery.Parameters.AddWithValue("@idcidade", funcionario.idCidade);
                SqlQuery.Parameters.AddWithValue("@nmfuncionario", funcionario.nmPessoa);
                SqlQuery.Parameters.AddWithValue("@nmApelido", funcionario.nmApelido ?? (object)DBNull.Value);
                SqlQuery.Parameters.AddWithValue("@rg", funcionario.rg ?? (object)DBNull.Value);
                SqlQuery.Parameters.AddWithValue("@cpf", funcionario.documento);
                SqlQuery.Parameters.AddWithValue("@genero", funcionario.genero);
                SqlQuery.Parameters.AddWithValue("@email", funcionario.email ?? (object)DBNull.Value);
                SqlQuery.Parameters.AddWithValue("@endereco", funcionario.endereco);
                SqlQuery.Parameters.AddWithValue("@bairro", funcionario.bairro);
                SqlQuery.Parameters.AddWithValue("@nrendereco", funcionario.nrEndereco);
                SqlQuery.Parameters.AddWithValue("@complemento", funcionario.complemento ?? (object)DBNull.Value);
                SqlQuery.Parameters.AddWithValue("@cep", funcionario.cep);
                SqlQuery.Parameters.AddWithValue("@dtnascimento", funcionario.dtNascimento);
                SqlQuery.Parameters.AddWithValue("@nrcel", funcionario.nrCel);
                SqlQuery.Parameters.AddWithValue("@cargo", funcionario.cargo);
                SqlQuery.Parameters.AddWithValue("@dsobservacao", funcionario.dsObservacao ?? (object)DBNull.Value);
                SqlQuery.Parameters.AddWithValue("@salario", funcionario.salario);
                SqlQuery.Parameters.AddWithValue("@dtAdmissao", funcionario.dtAdmissao);
                SqlQuery.Parameters.AddWithValue("@dtcadastro", funcionario.dtCadastro);
                SqlQuery.Parameters.AddWithValue("@dtatualizacao", funcionario.dtAtualizacao);

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

        public List<Funcionario> GetFuncionarios()
        {
            try
            {
                AbrirConexao();
                SqlQuery = new SqlCommand("SELECT * FROM tbFuncionarios INNER JOIN tbCidades on tbFuncionarios.idcidade = tbCidades.idcidade ", con);
                reader = SqlQuery.ExecuteReader();

                var lista = new List<Funcionario>();

                while (reader.Read())
                {
                    var funcionario = new Funcionario
                    {
                        idPessoa = Convert.ToInt32(reader["idfuncionario"]),
                        cidade = new Cidade
                        {
                            idCidade = Convert.ToInt32(reader["idcidade"]),
                            nmCidade = Convert.ToString(reader["nmcidade"]),
                        },
                        nmPessoa = Convert.ToString(reader["nmfuncionario"]),
                        nmApelido = Convert.ToString(reader["nmApelido"]),
                        rg = Convert.ToString(reader["rg"]),
                        documento = Convert.ToString(reader["cpf"]),
                        genero = Convert.ToString(reader["genero"]),
                        email = Convert.ToString(reader["email"]),
                        endereco = Convert.ToString(reader["endereco"]),
                        bairro = Convert.ToString(reader["bairro"]),
                        nrEndereco = Convert.ToString(reader["nrendereco"]),
                        complemento = Convert.ToString(reader["complemento"]),
                        cep = Convert.ToString(reader["cep"]),
                        dtNascimento = Convert.ToString(reader["dtnascimento"]),
                        nrCel = Convert.ToString(reader["nrcel"]),
                        cargo = Convert.ToString(reader["cargo"]),
                        dsObservacao = Convert.ToString(reader["dsobservacao"]),
                        salario = Convert.ToDecimal(reader["salario"]),
                        dtAdmissao = Convert.ToString(reader["dtAdmissao"]),
                        dtCadastro = Convert.ToDateTime(reader["dtcadastro"]),
                        dtAtualizacao = Convert.ToDateTime(reader["dtatualizacao"]),
                    };

                    lista.Add(funcionario);
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

        public Funcionario GetFuncionariosByID(int? idFuncionario)
        {
            try
            {
                AbrirConexao();
                var _where = string.Empty;
                _where = " WHERE idfuncionario = " + idFuncionario;
                SqlQuery = new SqlCommand("SELECT * FROM tbFuncionarios INNER JOIN tbCidades on tbFuncionarios.idcidade = tbCidades.idcidade " + _where, con);
                reader = SqlQuery.ExecuteReader();
                var objFuncionario = new Funcionario();
                while (reader.Read())
                {
                    objFuncionario = new Funcionario()
                    {
                        idPessoa = Convert.ToInt32(reader["idfuncionario"]),
                        cidade = new Cidade
                        {
                            idCidade = Convert.ToInt32(reader["idcidade"]),
                            nmCidade = Convert.ToString(reader["nmcidade"]),
                        },
                        nmPessoa = Convert.ToString(reader["nmfuncionario"]),
                        nmApelido = Convert.ToString(reader["nmApelido"]),
                        rg = Convert.ToString(reader["rg"]),
                        documento = Convert.ToString(reader["cpf"]),
                        genero = Convert.ToString(reader["genero"]),
                        email = Convert.ToString(reader["email"]),
                        endereco = Convert.ToString(reader["endereco"]),
                        bairro = Convert.ToString(reader["bairro"]),
                        nrEndereco = Convert.ToString(reader["nrendereco"]),
                        complemento = Convert.ToString(reader["complemento"]),
                        cep = Convert.ToString(reader["cep"]),
                        dtNascimento = Convert.ToString(reader["dtnascimento"]),
                        nrCel = Convert.ToString(reader["nrcel"]),
                        cargo = Convert.ToString(reader["cargo"]),
                        dsObservacao = Convert.ToString(reader["dsobservacao"]),
                        salario = Convert.ToDecimal(reader["salario"]),
                        dtAdmissao = Convert.ToString(reader["dtAdmissao"]),
                        dtDemissao = Convert.ToString(reader["dtDemissao"]),
                        dtCadastro = Convert.ToDateTime(reader["dtcadastro"]),
                        dtAtualizacao = Convert.ToDateTime(reader["dtatualizacao"]),
                    };
                }
                return objFuncionario;
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

        public bool Edit(Funcionario funcionario)
        {
            try
            {
                AbrirConexao();
                SqlQuery = new SqlCommand("UPDATE tbFuncionarios SET idcidade=@idcidade, nmfuncionario=@nmfuncionario, nmApelido=@nmApelido, rg=@rg, cpf=@cpf, genero=@genero, email=@email, endereco=@endereco, " +
                    "bairro=@bairro, nrendereco=@nrendereco, complemento=@complemento, cep=@cep, dtnascimento=@dtnascimento, nrcel=@nrcel, cargo=@cargo, " +
                    "dsobservacao=@dsobservacao, salario=@salario, dtDemissao=@dtDemissao, dtatualizacao=@dtatualizacao WHERE idfuncionario=@idfuncionario", con);

                SqlQuery.Parameters.AddWithValue("@idfuncionario", funcionario.idPessoa);

                SqlQuery.Parameters.AddWithValue("@idcidade", funcionario.idCidade);
                SqlQuery.Parameters.AddWithValue("@nmfuncionario", funcionario.nmPessoa);
                SqlQuery.Parameters.AddWithValue("@nmApelido", funcionario.nmApelido ?? (object)DBNull.Value);
                SqlQuery.Parameters.AddWithValue("@rg", funcionario.rg ?? (object)DBNull.Value);
                SqlQuery.Parameters.AddWithValue("@cpf", funcionario.documento);
                SqlQuery.Parameters.AddWithValue("@genero", funcionario.genero);
                SqlQuery.Parameters.AddWithValue("@email", funcionario.email ?? (object)DBNull.Value);
                SqlQuery.Parameters.AddWithValue("@endereco", funcionario.endereco);
                SqlQuery.Parameters.AddWithValue("@bairro", funcionario.bairro);
                SqlQuery.Parameters.AddWithValue("@nrendereco", funcionario.nrEndereco);
                SqlQuery.Parameters.AddWithValue("@complemento", funcionario.complemento ?? (object)DBNull.Value);
                SqlQuery.Parameters.AddWithValue("@cep", funcionario.cep);
                SqlQuery.Parameters.AddWithValue("@dtnascimento", funcionario.dtNascimento);
                SqlQuery.Parameters.AddWithValue("@nrcel", funcionario.nrCel);
                SqlQuery.Parameters.AddWithValue("@cargo", funcionario.cargo);
                SqlQuery.Parameters.AddWithValue("@dsobservacao", funcionario.dsObservacao ?? (object)DBNull.Value);
                SqlQuery.Parameters.AddWithValue("@salario", funcionario.salario);
                SqlQuery.Parameters.AddWithValue("@dtDemissao", funcionario.dtDemissao ?? (object)DBNull.Value);
                SqlQuery.Parameters.AddWithValue("@dtatualizacao", funcionario.dtAtualizacao);

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
                SqlQuery = new SqlCommand("DELETE FROM tbFuncionarios WHERE idfuncionario=@idfuncionario", con);

                SqlQuery.Parameters.AddWithValue("@idfuncionario", id);
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
