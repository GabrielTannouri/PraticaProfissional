using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Pratica_Profissional.DAO
{
    public class Conexao
    {
        // variáveis de conexão, execução e resultSet
        protected SqlConnection con;
        protected SqlCommand SqlQuery;
        protected SqlDataReader reader;

        protected void AbrirConexao()
        {
            try
            {
                con = new SqlConnection(WebConfigurationManager.ConnectionStrings["DataBaseLindaPrata"].ConnectionString);
                con.Open();
            } catch(Exception error)
            {
                throw new Exception("Erro ao abrir a conexão: " + error.Message);
            }
        }

        protected void FecharConexao()
        {
            try
            {
                if (con != null)
                {
                    con.Close();
                }
            } catch (Exception error)
            {
                throw new Exception("Erro ao fechar a conexão: " + error.Message);
            }
        }
    }
}