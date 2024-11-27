// ------------------------------------------------------------------------------------------------------------
// <copyright file="IDBManager.cs" company="Pentechs">Copyright (c) Pentechs . All rights reserved.</copyright>
// <author>Debabrata</author>
// <createdOn>16-11-2016</createdOn>
// <comment></comment>
// ------------------------------------------------------------------------------------------------------------

namespace CommonApplicationFramework.DataHandling
{
    #region Namespaces
    using System.Data;
    #endregion

    /// -----------------------------------------------------------------
    ///   Namespace:    <UserManagementSystem>
    ///   Class:        <IDBManager>
    ///   Description:    
    ///   Author:       <Debabrata>                    
    /// -----------------------------------------------------------------

    public enum DataProvider
    {
        SqlServer, OleDb, Odbc, Oracle
    }

    public interface IDBManager
    {
        DataProvider ProviderType
        {
            get;
            set;
        }

        string ConnectionString
        {
            get;
            set;
        }

        IDbConnection Connection
        {
            get;
        }

        IDbTransaction Transaction
        {
            get;
        }

        IDataReader DataReader
        {
            get;
        }

        IDbCommand Command
        {
            get;
        }

        IDbDataParameter[] Parameters
        {
            get;
        }

        void Open();

        void BeginTransaction();

        void CommitTransaction();

        void CreateParameters(int paramsCount);

        void AddParameters(int index, string paramName, object objValue);

        IDataReader ExecuteReader(CommandType commandType, string commandText);

        DataSet ExecuteDataSet(CommandType commandType, string commandText);

        object ExecuteScalar(CommandType commandType, string commandText);

        int ExecuteNonQuery(CommandType commandType, string commandText);

        void CloseReader();

        void Close();

        void Dispose();
    }
}
