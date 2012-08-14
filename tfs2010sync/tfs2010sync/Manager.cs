using System;
using System.Data;
using System.Threading;
using Oracle.DataAccess.Client;
using System.Net.Sockets;

namespace tfs2010sync
{
    public class Manager
    {
        #region Fields

        public OracleConnection Connection { get; set; }
        public OracleTransaction Transaction { get; set; }
        public OracleCommand Command { get; set; }
        public bool InProcess { get; set; }
        private const int Timeout = 1000;

        #endregion

        public Manager()
        {
            Connection = new OracleConnection();
            InProcess = false;
        }

        #region Connection and Transaction Methods

        public void ConnectionOpen(Connection connection, string login, string password)
        {
            if (Connection.State == ConnectionState.Open) return;

            Connection = new OracleConnection(connection.GetConnectionString(login, password));

            try
            {
                foreach (var host in connection.Hosts)
                {
                    using (var tcpClient = new TcpClient())
                    {
                        tcpClient.Connect(host.IP, host.Port);
                    }
                }
            }
            catch
            {
                throw new Exception("DB is not available");
            }

            try
            {
                Connection.Open();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            //Connection.

            var ora = OracleGlobalization.GetClientInfo();

            //Connection.SetSessionInfo();
        }

        private OracleTransaction TransactionCreate()
        {
            return Transaction = Connection.BeginTransaction(IsolationLevel.ReadCommitted);
        }
        public void ConnectionClose()
        {
            while (InProcess) Thread.Sleep(10);
            Connection.Close();
            OracleConnection.ClearAllPools();
        }
        public void Commit()
        {
            Transaction.Commit();
        }
        public void Rollback()
        {
            Transaction.Rollback();
        }
        public void Rollback(string message)
        {
            Transaction.Rollback();
        }

        #endregion

        public void Call(string query)
        {
            #region Preparation

            Command = Connection.CreateCommand();
            Command.CommandText = query;
            Command.CommandType = CommandType.Text;
            Command.CommandTimeout = Timeout;
            Command.Transaction = TransactionCreate();

            #endregion

            #region Executing

            InProcess = true;
            try
            {
                Command.ExecuteNonQuery();
            }
            catch (OracleException e)
            {
                if (e.Number == 3114) throw new Exception(e.Message);
                if (e.Number == 1013)
                {
                    Rollback();
                    throw new Exception("Cancel");
                }
                Rollback(e.Procedure + ": " + e.Message);
                throw new Exception(e.Message);
            }
            finally
            {
                InProcess = false;
            }

            #endregion

            #region Post processing

            Commit();
            Command.Dispose();

            #endregion

        }
    }
}
