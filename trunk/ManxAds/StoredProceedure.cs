using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace ManxAds
{
    /// <summary>
    /// Is thrown when a reader cannot read because there are no rows, to
    /// avoid this, always check that the reader has read successfully
    /// before calling classes which require data to be read from the database.
    /// </summary>
    public class CannotReadException : Exception
    {
        internal CannotReadException() : base() { }
        internal CannotReadException(string message) : base(message) { }
    }

    public class StoredProceedure : IDisposable
    {
        private SqlConnection connection;
        private SqlCommand command;
        private SqlDataReader reader;

        public SqlDataReader Reader
        {
            get { return reader; }
            set { reader = value; }
        }

        public SqlConnection Connection
        {
            get { return connection; }
        }

        public SqlCommand Command
        {
            get { return command; }
        }

        private StoredProceedure() { }

        public StoredProceedure(string name)
            : this(name, 30, LocalSettings.ConnectionString) { }

        public StoredProceedure(string name, int timeout)
            : this(name, timeout, LocalSettings.ConnectionString) { }

        public StoredProceedure(string name, string connectionString)
            : this(name, 30, connectionString) { }

        public StoredProceedure(string name, int timeout, string connectionString)
        {
            this.connection = new SqlConnection(connectionString);
            this.command = new SqlCommand(name, connection);
            this.command.CommandType = CommandType.StoredProcedure;
            this.command.CommandTimeout = timeout;
        }

        public void AddParam(string parameterName, object value)
        {
            if (value == null)
            {
                command.Parameters.AddWithValue(parameterName, DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue(parameterName, value);
            }
        }

        public void AddOutParam(string parameterName, SqlDbType type, int size)
        {
            SqlParameter param = new SqlParameter(parameterName, type, size);
            param.Direction = ParameterDirection.Output;
            command.Parameters.Add(param);
        }

        public void AddOutParam(string parameterName, SqlDbType type)
        {
            SqlParameter param = new SqlParameter(parameterName, type);
            param.Direction = ParameterDirection.Output;
            command.Parameters.Add(param);
        }

        public T GetParamValue<T>(string parameterName)
        {
            object value = command.Parameters[parameterName].Value;

            if (value == null)
            {
                throw new NullReferenceException(
                    "Value of parameter '" + parameterName + "' was contained in " +
                    "parameter collection, but has a null reference instead of DBNull.");
            }

            if (value == DBNull.Value)
            {
                return default(T);
            }

            return (T)value;
        }

        public T GetReaderValue<T>(string column)
        {
            if (reader == null)
            {
                throw new CannotReadException("Reader has not been initialized.");
            }

            if (!reader.HasRows)
            {
                throw new CannotReadException("Reader has now rows.");
            }

            try
            {
                if (reader[column] == DBNull.Value)
                {
                    return default(T);
                }

                return (T)reader[column];
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new IndexOutOfRangeException("The column '" + column + "' was not found.", ex);
            }
        }

        public static StoredProceedure FromReader(SqlDataReader reader)
        {
            StoredProceedure sp = new StoredProceedure();
            sp.reader = reader;
            return sp;
        }

        public SqlDataReader ExecuteReader()
        {
            reader = command.ExecuteReader();
            return reader;
        }

        public void Dispose()
        {
            if (reader != null)
            {
                reader.Close();
                reader.Dispose();
            }

            if (connection != null)
            {
                connection.Close();
                connection.Dispose();
            }
        }
    }

    public enum NonQueryReturnType
    {
        RowsAffected,
        InsertId
    }
}
