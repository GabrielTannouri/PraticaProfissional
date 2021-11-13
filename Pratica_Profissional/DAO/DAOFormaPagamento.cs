using Pratica_Profissional.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Pratica_Profissional.DAO
{
    public class DAOFormaPagamento : DAO
    {

        public bool Create(FormaPagamento formaPagamento)
        {
            try
            {
                this.VerificaDuplicidade(formaPagamento.nmFormaPagamento, null);
                AbrirConexao();
                SqlQuery = new SqlCommand("INSERT INTO tbFormaPagamentos (nmformapagamento, dtcadastro, dtatualizacao) VALUES (@nmformapagamento, @dtCadastro, @dtAtualizacao)", con);

                SqlQuery.Parameters.AddWithValue("@nmformapagamento", formaPagamento.nmFormaPagamento);
                SqlQuery.Parameters.AddWithValue("@dtCadastro", formaPagamento.dtCadastro);
                SqlQuery.Parameters.AddWithValue("@dtAtualizacao", formaPagamento.dtAtualizacao);

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

        public void VerificaDuplicidade(string nmFormaPagamento, int? idFormaPagamento)
        {
            try
            {
                AbrirConexao();
                var _where = string.Empty;
                if (idFormaPagamento > 0)
                {
                    _where = " WHERE tbFormaPagamentos.nmformapagamento = '" + nmFormaPagamento + "'" + "AND tbFormaPagamentos.idformapagamento <>" + idFormaPagamento;
                }
                else
                {
                    _where = " WHERE tbFormaPagamentos.nmformapagamento = '" + nmFormaPagamento + "'";
                }

                SqlQuery = new SqlCommand("SELECT * FROM tbFormaPagamentos" + _where, con);
                reader = SqlQuery.ExecuteReader();
                var objPais = new FormaPagamento();

                if (reader.Read())
                {
                    objPais = new FormaPagamento();

                    objPais.nmFormaPagamento = Convert.ToString(reader["nmformapagamento"]);
                    throw new Exception("Já existe uma forma de pagamento cadastrada com esse nome, verifique!");
                }
            }
            finally
            {
                FecharConexao();
            }
        }

        public List<FormaPagamento> GetFormaPagamentos()
        {
            try
            {
                AbrirConexao();
                SqlQuery = new SqlCommand("SELECT * FROM tbFormaPagamentos", con);
                reader = SqlQuery.ExecuteReader();

                var lista = new List<FormaPagamento>();

                while (reader.Read())
                {
                    var formaPagamento = new FormaPagamento
                    {
                        idFormaPagamento = Convert.ToInt32(reader["idformapagamento"]),
                        nmFormaPagamento = Convert.ToString(reader["nmformapagamento"]),
                        dtCadastro = Convert.ToDateTime(reader["dtCadastro"]),
                        dtAtualizacao = Convert.ToDateTime(reader["dtAtualizacao"])
                    };

                    lista.Add(formaPagamento);
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

        public FormaPagamento GetFormaPagamentosByID(int? idFormaPagamento)
        {
            try
            {
                AbrirConexao();
                var _where = string.Empty;
                _where = " WHERE idformapagamento = " + idFormaPagamento;
                SqlQuery = new SqlCommand("SELECT * FROM tbFormaPagamentos" + _where, con);
                reader = SqlQuery.ExecuteReader();
                var objFormaPagamento = new FormaPagamento();
                while (reader.Read())
                {
                    objFormaPagamento = new FormaPagamento()
                    {
                        idFormaPagamento = Convert.ToInt32(reader["idformapagamento"]),
                        nmFormaPagamento = Convert.ToString(reader["nmformapagamento"]),
                        dtCadastro = Convert.ToDateTime(reader["dtCadastro"]),
                        dtAtualizacao = Convert.ToDateTime(reader["dtAtualizacao"])
                    };
                }
                return objFormaPagamento;
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

        public bool Edit(FormaPagamento formaPagamento)
        {
            try
            {
                this.VerificaDuplicidade(formaPagamento.nmFormaPagamento, formaPagamento.idFormaPagamento);
                AbrirConexao();
                SqlQuery = new SqlCommand("UPDATE tbFormaPagamentos SET nmformapagamento=@nmFormaPagamento, dtatualizacao=@dtAtualizacao WHERE idformapagamento=@idFormaPagamento", con);
                SqlQuery.Parameters.AddWithValue("@idformapagamento", formaPagamento.idFormaPagamento);
                SqlQuery.Parameters.AddWithValue("@nmFormaPagamento", formaPagamento.nmFormaPagamento);
                SqlQuery.Parameters.AddWithValue("@dtatualizacao", formaPagamento.dtAtualizacao);

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
                SqlQuery = new SqlCommand("DELETE FROM tbFormaPagamentos WHERE idformapagamento=@idFormaPagamento", con);

                SqlQuery.Parameters.AddWithValue("@idFormaPagamento", id);
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