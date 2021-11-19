using Pratica_Profissional.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Pratica_Profissional.DAO
{
    public class DAOConta : DAO
    {

        public bool Create(ContaContabil conta)
        {
            try
            {
                this.VerificaDuplicidade(conta.nmConta, 0);
                AbrirConexao();
                SqlQuery = new SqlCommand("INSERT INTO tbContasContabeis (nmconta, vlSaldo, dtcadastro, dtatualizacao) VALUES (@nmconta, @vlSaldo, @dtCadastro, @dtAtualizacao)", con);

                SqlQuery.Parameters.AddWithValue("@nmconta", conta.nmConta);
                SqlQuery.Parameters.AddWithValue("@vlSaldo", conta.vlSaldo);
                SqlQuery.Parameters.AddWithValue("@dtCadastro", conta.dtCadastro);
                SqlQuery.Parameters.AddWithValue("@dtAtualizacao", conta.dtAtualizacao);

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

        public void VerificaDuplicidade(string nmConta, int? idConta)
        {
            try
            {
                AbrirConexao();
                var _where = string.Empty;
                if (idConta > 0)
                {
                    _where = " WHERE tbContasContabeis.nmconta = '" + nmConta + "'" + "AND tbContasContabeis.idconta <>" + idConta;
                } else
                {
                    _where = " WHERE tbContasContabeis.nmconta = '" + nmConta + "'";
                }

                SqlQuery = new SqlCommand("SELECT * FROM tbContasContabeis" + _where, con);
                reader = SqlQuery.ExecuteReader();
                var objConta = new ContaContabil();

                if (reader.Read())
                {
                    objConta = new ContaContabil();

                    objConta.nmConta = Convert.ToString(reader["nmconta"]);
                    throw new Exception("Já existe uma conta cadastrada com esse nome, verifique!");
                }
            }
            finally
            {
                FecharConexao();
            }
        }

        public List<ContaContabil> GetContas()
        {
            try
            {
                AbrirConexao();
                SqlQuery = new SqlCommand("SELECT * FROM tbContasContabeis", con);
                reader = SqlQuery.ExecuteReader();

                var lista = new List<ContaContabil>();

                while (reader.Read())
                {
                    var conta = new ContaContabil
                    {
                        idConta = Convert.ToInt32(reader["idconta"]),
                        nmConta = Convert.ToString(reader["nmconta"]),
                        vlSaldo = Convert.ToDecimal(reader["vlsaldo"]),
                        dtCadastro = Convert.ToDateTime(reader["dtCadastro"]),
                        dtAtualizacao = Convert.ToDateTime(reader["dtAtualizacao"])
                    };

                    lista.Add(conta);
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

        public ContaContabil GetContasByID(int? idConta)
        {
            try
            {
                AbrirConexao();
                var _where = string.Empty;
                _where = " WHERE idconta = " + idConta;
                SqlQuery = new SqlCommand("SELECT * FROM tbContasContabeis" + _where, con);
                reader = SqlQuery.ExecuteReader();
                var conta = new ContaContabil();
                while (reader.Read())
                {
                    conta = new ContaContabil()
                    {
                        idConta = Convert.ToInt32(reader["idconta"]),
                        nmConta = Convert.ToString(reader["nmconta"]),
                        vlSaldo = Convert.ToDecimal(reader["vlsaldo"]),
                        dtCadastro = Convert.ToDateTime(reader["dtCadastro"]),
                        dtAtualizacao = Convert.ToDateTime(reader["dtAtualizacao"])
                    };
                }
                return conta;
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

        public bool Edit(ContaContabil conta)
        {
            try
            {
                this.VerificaDuplicidade(conta.nmConta, conta.idConta);
                AbrirConexao();
                SqlQuery = new SqlCommand("UPDATE tbContasContabeis SET nmconta=@nmconta, vlsaldo=@vlsaldo, dtatualizacao=@dtAtualizacao WHERE idconta=@idconta", con);
                SqlQuery.Parameters.AddWithValue("@idconta", conta.idConta);
                SqlQuery.Parameters.AddWithValue("@nmconta", conta.nmConta);
                SqlQuery.Parameters.AddWithValue("@vlsaldo", conta.vlSaldo);
                SqlQuery.Parameters.AddWithValue("@dtatualizacao", conta.dtAtualizacao);

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
                SqlQuery = new SqlCommand("DELETE FROM tbContasContabeis WHERE idconta=@idconta", con);

                SqlQuery.Parameters.AddWithValue("@idconta", id);
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