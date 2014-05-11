using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data.OleDb;

namespace DAL
{
    public class DataBaseAccess
    {
        private string _conn = string.Empty;

        private string _providerName = string.Empty;

        private DbProviderFactory _factory = null;

        public DataBaseAccess(string conn, string providerName)
        {
            _conn = conn;
            _providerName = providerName;
            _factory = DbProviderFactories.GetFactory(providerName);
        }

        private DbConnection CreateConnection()
        {
            DbConnection conn = _factory.CreateConnection();
            conn.ConnectionString = _conn;
            return conn;
        }

        public SqlConnection CreateSqlConnection()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = _conn;
            return conn;
        }

        public DbParameter CreateParameter()
        {
            return _factory.CreateParameter();
        }

        public DbDataAdapter CreateDataAdapter()
        {
            return _factory.CreateDataAdapter();
        }

        public DbCommand CreateCommand()
        {
            DbCommand cmd = _factory.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = CreateConnection();
            cmd.CommandTimeout = 0;
            return cmd;
        }

        public SqlCommand CreateSqlCommand()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = CreateSqlConnection();
            cmd.CommandTimeout = 0;
            return cmd;
        }

        public DbCommandBuilder CreateCommandBuilder(DbDataAdapter da)
        {
            DbCommandBuilder cmdBuilder = _factory.CreateCommandBuilder();
            cmdBuilder.DataAdapter = da;
            return cmdBuilder;
        }

        public int ExecuteCommand(DbCommand cmd)
        {
            int count = 0;

            try
            {
                cmd.Connection.Open();

                count = cmd.ExecuteNonQuery();                
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
            }

            return count;
        }

        public int ExecuteCommand(SqlCommand cmd)
        {
            int count = 0;

            try
            {
                cmd.Connection.Open();

                count = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
            }

            return count;
        }

        public int UpdateDataBase(DbDataAdapter da, DataTable dt)
        {
            int updateCount = 0;

            try
            {
                DataTable dtChanges = dt.GetChanges();

                if (dtChanges != null && dtChanges.Rows.Count >= 0)
                {
                    updateCount = da.Update(dtChanges);
                    dt.AcceptChanges();
                }
            }
            catch (Exception)
            {
                dt.RejectChanges();
                throw;
            }
            return updateCount;
        }

        public DataSet GetDataSet(DbDataAdapter da, bool isFillSchema)
        {
            DataSet ds = new DataSet();
            try
            { 
                da.Fill(ds);

                if (isFillSchema)
                {
                    da.FillSchema(ds, SchemaType.Mapped);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return ds;
        }

        /// <summary>
        /// 返回表
        /// </summary>
        /// <param name="da"></param>
        /// <param name="blFillSchema">是否加载框架</param>
        /// <returns></returns>
        public DataTable GetDataTable(DbDataAdapter da, bool blFillSchema)
        {
            DataTable dt = null;

            try
            {
                dt = new DataTable();

                da.Fill(dt);

                if (blFillSchema)
                {
                    da.FillSchema(dt, SchemaType.Mapped);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return dt;
        }

        public DataTable GetTableSchema(DbDataAdapter da)
        {
            DataTable dt = null;

            try
            {
                dt = new DataTable();

                da.FillSchema(dt, SchemaType.Mapped);
            }
            catch (Exception)
            {
                throw;
            }

            return dt;
        }

        public DataSet GetDataSeteSchema(DbDataAdapter da)
        {
            DataSet ds = null;

            try
            {
                ds = new DataSet();

                da.FillSchema(ds, SchemaType.Mapped);
            }
            catch (Exception)
            {
                throw;
            }

            return ds;
        }

        public DataTable GetDataTable(DbCommand cmd)
        {
            DataTable dt;

            DbDataReader dataReader = null;

            try
            {
                dt = new DataTable();

                cmd.Connection.Open();

                dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);                

                dt.Load(dataReader);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Dispose();
                }
            }

            return dt;
        }

        public DataTable GetDataTable(DbCommand cmd, string tableName)
        {
            DataTable dt;

            DbDataReader dataReader = null;

            try
            {
                cmd.Connection.Open();

                dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                dt = new DataTable(tableName);

                dt.Load(dataReader);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Dispose();
                }
            }

            return dt;
        }


        public DataTable GetTableFromExcel(string filePath, string sheetname)
        {
            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=Excel 8.0; ";
            OleDbConnection conn = new OleDbConnection(strConn);
            OleDbDataAdapter da = new OleDbDataAdapter("select * from [" + sheetname + "$]", strConn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public bool DataCopy(DataTable sourceTable, string descTableName, List<SqlBulkCopyColumnMapping> listcolMap, int batchSize)
        {
            bool isComplete;

            using (SqlConnection conn = CreateSqlConnection())
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, trans);

                sqlBulkCopy.BatchSize = batchSize;
                sqlBulkCopy.DestinationTableName = descTableName;

                foreach (SqlBulkCopyColumnMapping colMap in listcolMap)
                {
                    sqlBulkCopy.ColumnMappings.Add(colMap);
                }

                try
                {
                    sqlBulkCopy.WriteToServer(sourceTable);
                    trans.Commit();
                    isComplete = true;
                }
                catch (Exception)
                {
                    trans.Rollback();
                    isComplete = false;
                    throw;
                }
                finally
                {
                    sqlBulkCopy.Close();
                }

            }

            return isComplete;
        }

        public bool DataCopyNotTrans(DataTable sourceTable, string descTableName)
        {
            bool isComplete;

            using (SqlConnection conn = CreateSqlConnection())
            {
                conn.Open();          
                SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(conn);
              
                sqlBulkCopy.DestinationTableName = descTableName;            

                try
                {
                    sqlBulkCopy.WriteToServer(sourceTable);                  
                    isComplete = true;
                }
                catch (Exception)
                {                   
                    isComplete = false;
                    throw;
                }
                finally
                {
                    sqlBulkCopy.Close();
                }

            }

            return isComplete;
        }
    }
}
