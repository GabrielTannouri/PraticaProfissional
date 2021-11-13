using Pratica_Profissional.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Pratica_Profissional.DAO
{
    public class DAOEstado : DAO
    {

        public bool Create(Estado estado)
        {
            try
            {
                this.VerificaDuplicidade(estado.nmEstado, null);
                AbrirConexao();
                SqlQuery = new SqlCommand("INSERT INTO tbEstados (nmestado, uf, dtcadastro, dtatualizacao, idpais) VALUES (@nmestado, @uf, @dtCadastro, @dtAtualizacao, @idpais)", con);

                SqlQuery.Parameters.AddWithValue("@nmestado", estado.nmEstado);
                SqlQuery.Parameters.AddWithValue("@uf", estado.uf);
                SqlQuery.Parameters.AddWithValue("@dtCadastro", estado.dtCadastro);
                SqlQuery.Parameters.AddWithValue("@dtAtualizacao", estado.dtAtualizacao);
                SqlQuery.Parameters.AddWithValue("@idpais", estado.idPais);

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

        public void VerificaDuplicidade(string nmEstado, int? idEstado)
        {
            try
            {
                AbrirConexao();
                var _where = string.Empty;
                if (idEstado > 0)
                {
                    _where = " WHERE tbEstados.nmestado = '" + nmEstado + "'" +"AND tbEstados.idestado <>" + idEstado;
                }
                else
                {
                    _where = " WHERE tbEstados.nmestado = '" + nmEstado + "'";
                }
               
                SqlQuery = new SqlCommand("SELECT * FROM tbEstados" + _where, con);
                reader = SqlQuery.ExecuteReader();
                var objPais = new Estado();

                if (reader.Read())
                {
                    objPais = new Estado();

                    objPais.nmEstado = Convert.ToString(reader["nmestado"]);
                    throw new Exception("Já existe um estado cadastrado com esse nome, verifique!");
                }
            }
            finally
            {
                FecharConexao();
            }
        }

        public List<Estado> GetEstados()
        {
            try
            {
                AbrirConexao();
                SqlQuery = new SqlCommand("SELECT * FROM tbestados INNER JOIN tbPaises on tbEstados.idPais = tbPaises.idPais", con);
                reader = SqlQuery.ExecuteReader();

                var lista = new List<Estado>();

                while (reader.Read())
                {
                    var estado = new Estado
                    {
                        idEstado = Convert.ToInt32(reader["idestado"]),
                        nmEstado = Convert.ToString(reader["nmestado"]),
                        uf = Convert.ToString(reader["uf"]),
                        dtCadastro = Convert.ToDateTime(reader["dtcadastro"]),
                        dtAtualizacao = Convert.ToDateTime(reader["dtAtualizacao"]),
                        Pais = new Pais
                        {
                            idPais = Convert.ToInt32(reader["idpais"]),
                            nmPais = Convert.ToString(reader["nmpais"]),
                        }
                    };

                    lista.Add(estado);
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

        public Estado GetEstadosByID(int? idEstado)
        {
            try
            {
                AbrirConexao();
                var _where = string.Empty;
                _where = " WHERE idEstado = " + idEstado;
                SqlQuery = new SqlCommand("SELECT * FROM tbestados INNER JOIN tbPaises on tbEstados.idPais = tbPaises.idPais" + _where, con);
                reader = SqlQuery.ExecuteReader();
                var objEstado = new Estado();
                while (reader.Read())
                {
                    objEstado = new Estado()
                    {
                        idEstado = Convert.ToInt32(reader["idestado"]),
                        nmEstado = Convert.ToString(reader["nmestado"]),
                        uf = Convert.ToString(reader["uf"]),
                        dtCadastro = Convert.ToDateTime(reader["dtCadastro"]),
                        dtAtualizacao = Convert.ToDateTime(reader["dtAtualizacao"]),
                        Pais = new Pais
                        {
                            idPais = Convert.ToInt32(reader["idpais"]),
                            nmPais = Convert.ToString(reader["nmpais"]),
                        }
                    };
                }
                return objEstado;
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

        public bool Edit(Estado estado)
        {
            try
            {
                this.VerificaDuplicidade(estado.nmEstado, estado.idEstado);
                AbrirConexao();
                SqlQuery = new SqlCommand("UPDATE tbestados SET nmestado=@nmEstado, uf=@uf, idpais=@idPais, dtatualizacao=@dtAtualizacao WHERE idestado=@idEstado", con);
                SqlQuery.Parameters.AddWithValue("@idEstado", estado.idEstado);
                SqlQuery.Parameters.AddWithValue("@nmEstado", estado.nmEstado);
                SqlQuery.Parameters.AddWithValue("@uf", estado.uf);
                SqlQuery.Parameters.AddWithValue("@idPais", estado.idPais);
                SqlQuery.Parameters.AddWithValue("@dtatualizacao", estado.dtAtualizacao);

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
                SqlQuery = new SqlCommand("DELETE FROM tbestados WHERE idestado=@idEstado", con);

                SqlQuery.Parameters.AddWithValue("@idEstado", id);
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
