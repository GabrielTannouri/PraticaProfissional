using Pratica_Profissional.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Pratica_Profissional.DAO
{
    public class DAOPais : DAO
    {

        public bool Create(Pais paises)
        {
            try
            {
                this.VerificaDuplicidade(paises.nmPais, 0);
                AbrirConexao();
                SqlQuery = new SqlCommand("INSERT INTO tbpaises (nmpais, sigla, ddi, dtcadastro, dtatualizacao) VALUES (@nmpais, @sigla, @ddi, @dtCadastro, @dtAtualizacao)", con);

                SqlQuery.Parameters.AddWithValue("@nmpais", paises.nmPais);
                SqlQuery.Parameters.AddWithValue("@sigla", paises.sigla);
                SqlQuery.Parameters.AddWithValue("@ddi", paises.ddi);
                SqlQuery.Parameters.AddWithValue("@dtCadastro", paises.dtCadastro);
                SqlQuery.Parameters.AddWithValue("@dtAtualizacao", paises.dtAtualizacao);

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

        public void VerificaDuplicidade(string nmPais, int? idPais)
        {
            try
            {
                AbrirConexao();
                var _where = string.Empty;
                if (idPais > 0)
                {
                    _where = " WHERE tbpaises.nmpais = '" + nmPais + "'" + "AND tbpaises.idpais <>" + idPais;
                } else
                {
                    _where = " WHERE tbpaises.nmpais = '" + nmPais + "'";
                }

                SqlQuery = new SqlCommand("SELECT * FROM tbpaises" + _where, con);
                reader = SqlQuery.ExecuteReader();
                var objPais = new Pais();

                if (reader.Read())
                {
                    objPais = new Pais();

                    objPais.nmPais = Convert.ToString(reader["nmpais"]);
                    throw new Exception("Já existe um país cadastrado com esse nome, verifique!");
                }
            }
            finally
            {
                FecharConexao();
            }
        }

        public List<Pais> GetPaises()
        {
            try
            {
                AbrirConexao();
                SqlQuery = new SqlCommand("SELECT * FROM tbpaises", con);
                reader = SqlQuery.ExecuteReader();

                var lista = new List<Pais>();

                while (reader.Read())
                {
                    var pais = new Pais
                    {
                        idPais = Convert.ToInt32(reader["idpais"]),
                        nmPais = Convert.ToString(reader["nmPais"]),
                        sigla = Convert.ToString(reader["sigla"]),
                        ddi = Convert.ToString(reader["ddi"]),
                        dtCadastro = Convert.ToDateTime(reader["dtCadastro"]),
                        dtAtualizacao = Convert.ToDateTime(reader["dtAtualizacao"])
                    };

                    lista.Add(pais);
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

        public Pais GetPaisesByID(int? idPais)
        {
            try
            {
                AbrirConexao();
                var _where = string.Empty;
                _where = " WHERE idpais = " + idPais;
                SqlQuery = new SqlCommand("SELECT * FROM tbpaises" + _where, con);
                reader = SqlQuery.ExecuteReader();
                var objPais = new Pais();
                while (reader.Read())
                {
                    objPais = new Pais()
                    {
                        idPais = Convert.ToInt32(reader["idpais"]),
                        nmPais = Convert.ToString(reader["nmPais"]),
                        sigla = Convert.ToString(reader["sigla"]),
                        ddi = Convert.ToString(reader["ddi"]),
                        dtCadastro = Convert.ToDateTime(reader["dtCadastro"]),
                        dtAtualizacao = Convert.ToDateTime(reader["dtAtualizacao"])
                    };
                }
                return objPais;
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

        public bool Edit(Pais paises)
        {
            try
            {
                this.VerificaDuplicidade(paises.nmPais, paises.idPais);
                AbrirConexao();
                SqlQuery = new SqlCommand("UPDATE tbpaises SET nmpais=@nmPais, sigla=@sigla, ddi=@ddi, dtatualizacao=@dtAtualizacao WHERE idpais=@idPais", con);
                SqlQuery.Parameters.AddWithValue("@idPais", paises.idPais);
                SqlQuery.Parameters.AddWithValue("@nmpais", paises.nmPais);
                SqlQuery.Parameters.AddWithValue("@sigla", paises.sigla);
                SqlQuery.Parameters.AddWithValue("@ddi", paises.ddi);
                SqlQuery.Parameters.AddWithValue("@dtatualizacao", paises.dtAtualizacao);

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
                SqlQuery = new SqlCommand("DELETE FROM tbpaises WHERE idpais=@idPais", con);

                SqlQuery.Parameters.AddWithValue("@idPais", id);
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