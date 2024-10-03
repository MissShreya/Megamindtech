using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace MEGAMINDTECH
{
    internal class DALBase
    {
        private string strCon = "";
        private System.Data.SqlClient.SqlConnection objConnection;
        private System.Data.DataSet dsResultSet;

        public DALBase()
        {
            this.strCon = System.Configuration.ConfigurationManager.AppSettings["DBcon"].ToString();
        }

        public void Create_Connection()
        {
            this.objConnection = null;
            try
            {
                this.objConnection = new System.Data.SqlClient.SqlConnection(this.strCon);
                if (this.objConnection.State == System.Data.ConnectionState.Closed || this.objConnection == null)
                {
                    this.objConnection.Open();
                }
            }
            catch
            {
            }
        }

        public void Close_Connection()
        {
            try
            {
                if (this.objConnection.State == System.Data.ConnectionState.Open || this.objConnection != null)
                {
                    this.objConnection.Close();
                    this.objConnection = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public System.Data.DataSet ExecuteProcedure(string SPName, string[] pName, string[] pValue)
        {
            this.dsResultSet = null;
            try
            {
                this.dsResultSet = new System.Data.DataSet();
                this.Create_Connection();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = this.objConnection;
                sqlCommand.CommandText = SPName;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < pName.Length; i++)
                {
                    sqlCommand.Parameters.AddWithValue(pName[i], string.IsNullOrEmpty(pValue[i]) ? null : pValue[i]);
                }
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(this.dsResultSet);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            finally
            {
                this.Close_Connection();
            }
            return this.dsResultSet;
        }


    }
}