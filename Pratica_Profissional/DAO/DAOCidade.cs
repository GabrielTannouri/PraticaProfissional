using Pratica_Profissional.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Pratica_Profissional.DAO
{
    public class DAOCidade : DAO
    {

        public bool Create(Cidade cidade)
        {
            try
            {
                this.VerificaDuplicidade(cidade.nmCidade, 0);
                AbrirConexao();
                SqlQuery = new SqlCommand("INSERT INTO tbCidades (nmcidade, ddd, dtcadastro, dtatualizacao, idestado) VALUES (@nmcidade, @ddd, @dtCadastro, @dtAtualizacao, @idestado)", con);

                SqlQuery.Parameters.AddWithValue("@nmcidade", cidade.nmCidade);
                SqlQuery.Parameters.AddWithValue("@ddd", cidade.ddd);
                SqlQuery.Parameters.AddWithValue("@dtCadastro", cidade.dtCadastro);
                SqlQuery.Parameters.AddWithValue("@dtAtualizacao", cidade.dtAtualizacao);
                SqlQuery.Parameters.AddWithValue("@idestado", cidade.idEstado);

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

        public void VerificaDuplicidade(string nmCidade, int? idCidade)
        {
            try
            {
                AbrirConexao();
                var _where = string.Empty;
                if (idCidade > 0)
                {
                    _where = " WHERE tbCidades.nmcidade = '" + nmCidade + "'" + "AND tbCidades.idcidade <>" + idCidade;
                }
                else
                {
                    _where = " WHERE tbCidades.nmcidade = '" + nmCidade + "'";
                }
                SqlQuery = new SqlCommand("SELECT * FROM tbCidades" + _where, con);
                reader = SqlQuery.ExecuteReader();
                var objPais = new Cidade();

                if (reader.Read())
                {
                    objPais = new Cidade();

                    objPais.nmCidade = Convert.ToString(reader["nmcidade"]);
                    throw new Exception("Já existe uma cidade cadastrada com esse nome, verifique!");
                }
            }
            finally
            {
                FecharConexao();
            }
        }

        public List<Cidade> GetCidades()
        {
            try
            {
                AbrirConexao();
                SqlQuery = new SqlCommand("SELECT * FROM tbCidades INNER JOIN tbEstados on tbCidades.idEstado = tbEstados.idEstado", con);
                reader = SqlQuery.ExecuteReader();

                var lista = new List<Cidade>();

                while (reader.Read())
                {
                    var cidade = new Cidade
                    {
                        idCidade = Convert.ToInt32(reader["idcidade"]),
                        nmCidade = Convert.ToString(reader["nmcidade"]),
                        ddd = Convert.ToString(reader["ddd"]),
                        dtCadastro = Convert.ToDateTime(reader["dtcadastro"]),
                        dtAtualizacao = Convert.ToDateTime(reader["dtAtualizacao"]),
                        Estado = new Estado
                        {
                            idEstado = Convert.ToInt32(reader["idestado"]),
                            nmEstado = Convert.ToString(reader["nmestado"]),
                        }
                    };

                    lista.Add(cidade);
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

        public Cidade GetCidadesByID(int? idCidade)
        {
            try
            {
                AbrirConexao();
                var _where = string.Empty;
                _where = " WHERE idcidade = " + idCidade;
                SqlQuery = new SqlCommand("SELECT * FROM tbCidades INNER JOIN tbEstados on tbCidades.idEstado = tbEstados.idEstado" + _where, con);
                reader = SqlQuery.ExecuteReader();
                var objCidade = new Cidade();
                while (reader.Read())
                {
                    objCidade = new Cidade()
                    {
                        idCidade = Convert.ToInt32(reader["idcidade"]),
                        nmCidade = Convert.ToString(reader["nmcidade"]),
                        ddd = Convert.ToString(reader["ddd"]),
                        dtCadastro = Convert.ToDateTime(reader["dtCadastro"]),
                        dtAtualizacao = Convert.ToDateTime(reader["dtAtualizacao"]),
                        Estado = new Estado
                        {
                            idEstado = Convert.ToInt32(reader["idestado"]),
                            nmEstado = Convert.ToString(reader["nmestado"]),
                        }
                    };
                }
                return objCidade;
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

        public bool Edit(Cidade cidade)
        {
            try
            {
                this.VerificaDuplicidade(cidade.nmCidade, cidade.idCidade);
                AbrirConexao();
                SqlQuery = new SqlCommand("UPDATE tbCidades SET nmcidade=@nmCidade, ddd=@ddd, idestado=@idEstado, dtatualizacao=@dtAtualizacao WHERE idcidade=@idCidade", con);
                SqlQuery.Parameters.AddWithValue("@idCidade", cidade.idCidade);
                SqlQuery.Parameters.AddWithValue("@nmcidade", cidade.nmCidade);
                SqlQuery.Parameters.AddWithValue("@ddd", cidade.ddd);
                SqlQuery.Parameters.AddWithValue("@idestado", cidade.idEstado);
                SqlQuery.Parameters.AddWithValue("@dtatualizacao", cidade.dtAtualizacao);

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
                SqlQuery = new SqlCommand("DELETE FROM tbCidades WHERE idcidade=@idCidade", con);

                SqlQuery.Parameters.AddWithValue("@idCidade", id);
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
