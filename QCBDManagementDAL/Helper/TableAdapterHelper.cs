using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementDAL.Helper
{
    public class TableAdapterHelper
    {

        public static SqlTransaction BeginTransaction(object tableAdapter)
        {
            return BeginTransaction(tableAdapter, IsolationLevel.ReadUncommitted);
        }

        public static SqlTransaction BeginTransaction(object tableAdapter, IsolationLevel isolationLevel)
        {
            // get the table adapter's type
            Type type = tableAdapter.GetType();

            // get the connection on the adapter
            SqlConnection connection = GetConnection(tableAdapter);

            // make sure connection is open to start the transaction
            if (connection.State == ConnectionState.Closed)
                connection.Open();

            // start a transaction on the connection
            SqlTransaction transaction = connection.BeginTransaction(isolationLevel);

            // set the transaction on the table adapter
            SetTransaction(tableAdapter, transaction);

            return transaction;
        }

        /// <summary>
        /// Gets the connection from the specified table adapter.
        /// </summary>
        private static SqlConnection GetConnection(object tableAdapter)
        {
            Type type = tableAdapter.GetType();
            PropertyInfo connectionProperty = type.GetProperty("Connection", BindingFlags.NonPublic | BindingFlags.Instance);
            SqlConnection connection = (SqlConnection)connectionProperty.GetValue(tableAdapter, null);
            return connection;
        }

        /// <summary>
        /// Sets the connection on the specified table adapter.
        /// </summary>
        private static void SetConnection(object tableAdapter, SqlConnection connection)
        {
            Type type = tableAdapter.GetType();
            PropertyInfo connectionProperty = type.GetProperty("Connection", BindingFlags.NonPublic | BindingFlags.Instance);
            connectionProperty.SetValue(tableAdapter, connection, null);
        }

        /// <summary>
        /// Enlists the table adapter in a transaction.
        /// </summary>
        public static void SetTransaction(object tableAdapter, SqlTransaction transaction)
        {
            // get the table adapter's type
            Type type = tableAdapter.GetType();

            // set the transaction on each command in the adapter
            PropertyInfo commandsProperty = type.GetProperty("CommandCollection", BindingFlags.NonPublic | BindingFlags.Instance);
            SqlCommand[] commands = (SqlCommand[])commandsProperty.GetValue(tableAdapter, null);
            foreach (SqlCommand command in commands)
                command.Transaction = transaction;

            // set the connection on the table adapter
            SetConnection(tableAdapter, transaction.Connection);
        }
    }
}
