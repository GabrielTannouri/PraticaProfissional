using Pratica_Profissional.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Pratica_Profissional.DAO
{
    public class DAOServico : DAO
    {

        public bool Create(Servico servico)
        {
            try
            {
                this.VerificaDuplicidade(servico.nmServico, null);
                AbrirConexao();
                SqlQuery = new SqlCommand("INSERT INTO tbServicos (nmservico, vlservico, dtcadastro, dtatualizacao) VALUES (@nmservico, @vlservico, @dtCadastro, @dtAtualizacao)", con);

                SqlQuery.Parameters.AddWithValue("@nmservico", servico.nmServico);
                SqlQuery.Parameters.AddWithValue("@vlservico", servico.vlServico);
                SqlQuery.Parameters.AddWithValue("@dtCadastro", servico.dtCadastro);
                SqlQuery.Parameters.AddWithValue("@dtAtualizacao", servico.dtAtualizacao);

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

        public void VerificaDuplicidade(string nmServico, int? idServico)
        {
            try
            {
                AbrirConexao();
                var _where = string.Empty;
                if (idServico > 0)
                {
                    _where = " WHERE tbServicos.nmservico = '" + nmServico + "'" + "AND tbServicos.idservico <>" + idServico;
                }
                else
                {
                    _where = " WHERE tbServicos.nmservico = '" + nmServico + "'";
                }
                SqlQuery = new SqlCommand("SELECT * FROM tbServicos" + _where, con);
                reader = SqlQuery.ExecuteReader();
                var objServico = new Servico();

                if (reader.Read())
                {
                    objServico.nmServico = Convert.ToString(reader["nmservico"]);
                    throw new Exception("Já existe um serviço cadastrado com esse nome, verifique!");
                }
            }
            finally
            {
                FecharConexao();
            }
        }

        public List<Servico> GetServicos()
        {
            try
            {
                AbrirConexao();
                SqlQuery = new SqlCommand("SELECT * FROM tbServicos", con);
                reader = SqlQuery.ExecuteReader();

                var lista = new List<Servico>();

                while (reader.Read())
                {
                    var servico = new Servico
                    {
                        idServico = Convert.ToInt32(reader["idservico"]),
                        nmServico = Convert.ToString(reader["nmservico"]),
                        vlServico = Convert.ToDecimal(reader["vlservico"]),
                        dtCadastro = Convert.ToDateTime(reader["dtCadastro"]),
                        dtAtualizacao = Convert.ToDateTime(reader["dtAtualizacao"])
                    };

                    lista.Add(servico);
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

        public Servico GetServicosByID(int? idServico)
        {
            try
            {
                AbrirConexao();
                var _where = string.Empty;
                _where = " WHERE idservico = " + idServico;
                SqlQuery = new SqlCommand("SELECT * FROM tbServicos" + _where, con);
                reader = SqlQuery.ExecuteReader();
                var objServico = new Servico();
                while (reader.Read())
                {
                    objServico = new Servico()
                    {
                        idServico = Convert.ToInt32(reader["idservico"]),
                        nmServico = Convert.ToString(reader["nmservico"]),
                        vlServico = Convert.ToDecimal(reader["vlservico"]),
                        dtCadastro = Convert.ToDateTime(reader["dtCadastro"]),
                        dtAtualizacao = Convert.ToDateTime(reader["dtAtualizacao"])
                    };
                }
                return objServico;
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

        public bool Edit(Servico servico)
        {
            try
            {
                this.VerificaDuplicidade(servico.nmServico, servico.idServico);
                AbrirConexao();
                SqlQuery = new SqlCommand("UPDATE tbServicos SET nmservico=@nmservico, vlservico=@vlservico, dtatualizacao=@dtAtualizacao WHERE idservico=@idservico", con);
                SqlQuery.Parameters.AddWithValue("@idservico", servico.idServico);
                SqlQuery.Parameters.AddWithValue("@nmservico", servico.nmServico);
                SqlQuery.Parameters.AddWithValue("@vlservico", servico.vlServico);
                SqlQuery.Parameters.AddWithValue("@dtatualizacao", servico.dtAtualizacao);

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
                SqlQuery = new SqlCommand("DELETE FROM tbServicos WHERE idservico=@idservico", con);

                SqlQuery.Parameters.AddWithValue("@idservico", id);
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