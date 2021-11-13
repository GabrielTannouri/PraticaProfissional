using Pratica_Profissional.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Pratica_Profissional.DAO
{
    public class DAOCategoria : DAO
    {

        public bool Create(Categoria categoria)
        {
            try
            {
                this.VerificaDuplicidade(categoria.nmCategoria, null);
                AbrirConexao();
                SqlQuery = new SqlCommand("INSERT INTO tbCategorias (nmcategoria, dtcadastro, dtatualizacao) VALUES (@nmcategoria, @dtCadastro, @dtAtualizacao)", con);

                SqlQuery.Parameters.AddWithValue("@nmcategoria", categoria.nmCategoria);
                SqlQuery.Parameters.AddWithValue("@dtCadastro", categoria.dtCadastro);
                SqlQuery.Parameters.AddWithValue("@dtAtualizacao", categoria.dtAtualizacao);

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

        public void VerificaDuplicidade(string nmCategoria, int? idCategoria)
        {
            try
            {
                AbrirConexao();
                var _where = string.Empty;
                if (idCategoria > 0)
                {
                    _where = " WHERE tbCategorias.nmcategoria = '" + nmCategoria + "'" + "AND tbCategorias.idcategoria <>" + idCategoria;
                }
                else
                {
                    _where = " WHERE tbCategorias.nmcategoria = '" + nmCategoria + "'";
                }
                SqlQuery = new SqlCommand("SELECT * FROM tbCategorias" + _where, con);
                reader = SqlQuery.ExecuteReader();
                var objPais = new Categoria();

                if (reader.Read())
                {
                    objPais = new Categoria();

                    objPais.nmCategoria = Convert.ToString(reader["nmcategoria"]);
                    throw new Exception("Já existe uma categoria cadastrada com esse nome, verifique!");
                }
            }
            finally
            {
                FecharConexao();
            }
        }

        public List<Categoria> GetCategorias()
        {
            try
            {
                AbrirConexao();
                SqlQuery = new SqlCommand("SELECT * FROM tbCategorias", con);
                reader = SqlQuery.ExecuteReader();

                var lista = new List<Categoria>();

                while (reader.Read())
                {
                    var categoria = new Categoria
                    {
                        idCategoria = Convert.ToInt32(reader["idcategoria"]),
                        nmCategoria = Convert.ToString(reader["nmcategoria"]),
                        dtCadastro = Convert.ToDateTime(reader["dtCadastro"]),
                        dtAtualizacao = Convert.ToDateTime(reader["dtAtualizacao"])
                    };

                    lista.Add(categoria);
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

        public Categoria GetCategoriasByID(int? idCategoria)
        {
            try
            {
                AbrirConexao();
                var _where = string.Empty;
                _where = " WHERE idcategoria = " + idCategoria;
                SqlQuery = new SqlCommand("SELECT * FROM tbCategorias" + _where, con);
                reader = SqlQuery.ExecuteReader();
                var objCategoria = new Categoria();
                while (reader.Read())
                {
                    objCategoria = new Categoria()
                    {
                        idCategoria = Convert.ToInt32(reader["idcategoria"]),
                        nmCategoria = Convert.ToString(reader["nmcategoria"]),
                        dtCadastro = Convert.ToDateTime(reader["dtCadastro"]),
                        dtAtualizacao = Convert.ToDateTime(reader["dtAtualizacao"])
                    };
                }
                return objCategoria;
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

        public bool Edit(Categoria categoria)
        {
            try
            {
                this.VerificaDuplicidade(categoria.nmCategoria, categoria.idCategoria);
                AbrirConexao();
                SqlQuery = new SqlCommand("UPDATE tbCategorias SET nmcategoria=@nmCategoria, dtatualizacao=@dtAtualizacao WHERE idcategoria=@idCategoria", con);
                SqlQuery.Parameters.AddWithValue("@idCategoria", categoria.idCategoria);
                SqlQuery.Parameters.AddWithValue("@nmCategoria", categoria.nmCategoria);
                SqlQuery.Parameters.AddWithValue("@dtatualizacao", categoria.dtAtualizacao);

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
                SqlQuery = new SqlCommand("DELETE FROM tbCategorias WHERE idcategoria=@idCategoria", con);

                SqlQuery.Parameters.AddWithValue("@idCategoria", id);
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