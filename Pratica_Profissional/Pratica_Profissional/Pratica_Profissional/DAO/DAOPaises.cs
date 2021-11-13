using Pratica_Profissional.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Pratica_Profissional.DAO
{
    public class DAOPaises : Conexao
    {

        public bool Create(Paises paises)
        {
            try
            {
                AbrirConexao();
                SqlQuery = new SqlCommand("INSERT INTO tbpaises (nmpais, sigla, ddi, dtcadastro) VALUES (@nmpais, @sigla, @ddi, @dtCadastro)", con);

                SqlQuery.Parameters.AddWithValue("@nmpais", paises.nmPais);
                SqlQuery.Parameters.AddWithValue("@sigla", paises.sigla);
                SqlQuery.Parameters.AddWithValue("@ddi", paises.ddi);
                SqlQuery.Parameters.AddWithValue("@dtCadastro", paises.dtCadastro);

                // Tratamento para saber se alguma linha foi alterada no Banco de Dados
                int i = SqlQuery.ExecuteNonQuery();

                if (i >= 1)
                {
                    return true;
                } else {
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

        public List<Paises> GetPaises()
        {
            try
            {
                AbrirConexao();
                SqlQuery = new SqlCommand("SELECT * FROM tbpaises", con);
                reader = SqlQuery.ExecuteReader();

                var lista = new List<Paises>();

                while (reader.Read())
                {
                    var pais = new Paises
                    {
                        idPais = Convert.ToInt32(reader["idpais"]),
                        nmPais = Convert.ToString(reader["nmPais"]),
                        sigla = Convert.ToString(reader["sigla"]),
                        ddi = Convert.ToString(reader["ddi"]),
                        dtCadastro = Convert.ToDateTime(reader["dtCadastro"]),
                        //dtAtualizacao = Convert.ToDateTime(reader["dtAtualizacao"])
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

        public bool Edit(Paises paises)
        {
            try
            {
                AbrirConexao();
                SqlQuery = new SqlCommand("UPDATE tbpaises SET nmpais=@nmPais, sigla=@sigla, ddi=@ddi WHERE idpais=@idPais", con);

                SqlQuery.Parameters.AddWithValue("@idPais", paises.idPais);
                SqlQuery.Parameters.AddWithValue("@nmpais", paises.nmPais);
                SqlQuery.Parameters.AddWithValue("@sigla", paises.sigla);
                SqlQuery.Parameters.AddWithValue("@ddi", paises.ddi);

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