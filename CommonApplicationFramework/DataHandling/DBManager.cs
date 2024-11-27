// -----------------------------------------------------------------------------------------------------------
// <copyright file="DBManager.cs" company="Pentechs">Copyright (c) Pentechs . All rights reserved.</copyright>
// <author>Debabrata</author>
// <createdOn>16-11-2016</createdOn>
// <comment></comment>
// -----------------------------------------------------------------------------------------------------------

namespace CommonApplicationFramework.DataHandling
{
    #region Namespaces
    using CommonApplicationFramework.Common;
    using CommonApplicationFramework.ConfigurationHandling;
    using System;
    using System.Configuration;
    using System.Data;
    using CommonApplicationFramework.Caching;
    using Newtonsoft.Json;
    using CommonApplicationFramework.Logging;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.Configuration;
    using System.IO;
    using ConfigurationManager = System.Configuration.ConfigurationManager;
    #endregion

    /// -----------------------------------------------------------------
    ///   Namespace:    <UserManagementSystem>
    ///   Class:        <DBManager>
    ///   Description:  
    ///   Author:       <Debabrata>                  
    /// -----------------------------------------------------------------

    public class DBManager : IDBManager, IDisposable
    {
        private IDbConnection idbConnection;
        private IDataReader idataReader;
        private IDbCommand idbCommand;
        private DataProvider providerType;
        private IDbTransaction idbTransaction = null;
        private IDbDataParameter[] idbParameters = null;
        //private UserContext userContext;
        //private DBInstance dbInstance;
        private string strConnection;

        public DBManager()
        {
            string providername =  Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("ProviderType")).Value.ToString();
            this.providerType = (DataProvider)Enum.Parse(typeof(DataProvider), providername);
        }

        public DBManager(DataProvider providerType)
        {
            this.providerType = providerType;
        }

        public DBManager(DataProvider providerType, string connectionString)
        {
            this.providerType = providerType;
            this.strConnection = connectionString;
        }

        public DBManager(string connectionString)
        {
            string providername = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("ProviderType")).Value.ToString();
            this.providerType = (DataProvider)Enum.Parse(typeof(DataProvider), providername);
            //this.providerType = providerType;
            this.strConnection = connectionString;
        }

        //public UserContext UserContext
        //{
        //    get
        //    {
        //        return this.userContext;
        //    }
        //    set
        //    {
        //        this.userContext = value;
        //    }
        //}

        //public DBInstance DBInstance { get { return this.dbInstance; } set { this.dbInstance = value; } }    

        public IDbConnection Connection
        {
            get
            {
                return this.idbConnection;
            }
        }

        public IDataReader DataReader
        {
            get
            {
                return this.idataReader;
            }

            set
            {
                this.idataReader = value;
            }
        }

        public DataProvider ProviderType
        {
            get
            {
                return this.providerType;
            }

            set
            {
                this.providerType = value;
            }
        }

        public string ConnectionString
        {
            get
            {
                return this.strConnection;
            }

            set
            {
                this.strConnection = value;
            }
        }

        public IDbCommand Command
        {
            get
            {
                return this.idbCommand;
            }
        }

        public IDbTransaction Transaction
        {
            get
            {
                return this.idbTransaction;
            }
            set
            {
                this.idbTransaction = value;
            }
        }

        public IDbDataParameter[] Parameters
        {
            get
            {
                return this.idbParameters;
            }
        }

        public void Open()
        {
            this.idbConnection =
            DBManagerFactory.GetConnection(this.providerType);
            this.idbConnection.ConnectionString = this.ConnectionString;
            if (this.idbConnection.State != ConnectionState.Open)
            {
                this.idbConnection.Open();
            }
            this.idbCommand = DBManagerFactory.GetCommand(this.ProviderType);
        }

        public void Close()
        {
            if (this.idbConnection != null)
            {
                if (this.idbConnection.State != ConnectionState.Closed)
              {
                    this.idbConnection.Close();
                }
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.Close();
            this.idbCommand = null;
            this.idbTransaction = null;
            this.idbConnection = null;
        }

        public void CreateParameters(int paramsCount)
        {
            this.idbParameters = new IDbDataParameter[paramsCount];
            this.idbParameters = DBManagerFactory.GetParameters(this.ProviderType, paramsCount);
        }

        public void AddParameters(int index, string paramName, object objValue)
        {
            if (index < this.idbParameters.Length)
            {
                this.idbParameters[index].ParameterName = paramName;
                if (objValue != null)
                {
                    this.idbParameters[index].Value = objValue;
                }
                else
                {
                    this.idbParameters[index].Value = DBNull.Value;
                }
            }
        }

        public void AddItemParameters(int index, string paramName, Item objValue)
        {
            if (index < this.idbParameters.Length)
            {
                this.idbParameters[index].ParameterName = paramName;
                if (objValue != null && objValue.Id > 0)
                    this.idbParameters[index].Value = objValue.Id;
                else
                    this.idbParameters[index].Value = DBNull.Value;
            }
        }

        public void BeginTransaction()
        {
            if (this.idbTransaction == null)
            {
                this.idbTransaction = DBManagerFactory.GetTransaction(this.ProviderType);
            }

            this.idbCommand.Transaction = this.idbTransaction;
        }

        public void CommitTransaction()
        {
            if (this.idbTransaction != null)
            {
                this.idbTransaction.Commit();
            }

            this.idbTransaction = null;
        }

        public IDataReader ExecuteReader(CommandType commandType, string commandText)
        {
            this.idbCommand = DBManagerFactory.GetCommand(this.ProviderType);
            this.idbCommand.Connection = this.Connection;
            this.PrepareCommand(this.idbCommand, this.Connection, this.Transaction, commandType, commandText, this.Parameters);
            this.DataReader = this.idbCommand.ExecuteReader();
            this.idbCommand.Parameters.Clear();
            return this.DataReader;
        }

        public void CloseReader()
        {
            if (this.DataReader != null)
            {
                this.DataReader.Close();
            }
        }

        private void AttachParameters(IDbCommand command, IDbDataParameter[] commandParameters)
        {
            foreach (IDbDataParameter idbParameter in commandParameters)
            {
                if ((idbParameter.Direction == ParameterDirection.InputOutput) && (idbParameter.Value == null))
                {
                    idbParameter.Value = DBNull.Value;
                }

                command.Parameters.Add(idbParameter);
            }
        }

        private void PrepareCommand(IDbCommand command, IDbConnection connection, IDbTransaction transaction, CommandType commandType, string commandText, IDbDataParameter[] commandParameters)
        {
            command.Connection = connection;
            command.CommandText = commandText;
            command.CommandType = commandType;

            if (transaction != null)
            {
                command.Transaction = transaction;
            }

            if (commandParameters != null)
            {
                this.AttachParameters(command, commandParameters);
            }
        }

        public void GetCacheConnection()
        {
            this.strConnection = System.Configuration.ConfigurationManager.ConnectionStrings["cacheConnection"].ToString();
            // return System.Configuration.ConfigurationManager.ConnectionStrings["cacheConnection"].ToString();
        }

        public string GetDBConnectionString(string code)
        {
            string dbConnectionString = string.Empty;
            string query = "SELECT Id,Code,Name,ConnectionString FROM Company where code=@code";
            GetCacheConnection();
            this.providerType = DataProvider.SqlServer;
            this.CreateParameters(1);
            this.AddParameters(0, "@code", code);
            this.Open();
            IDataReader dr = this.ExecuteReader(CommandType.Text, query);
            while (dr.Read())
            {
                dbConnectionString = dr["ConnectionString"].ToString();
                this.strConnection = dbConnectionString;
            }
            this.Close();
            return dbConnectionString;
        }

        public bool GetURLDetails(string key)
        {
            List<URLs> urls = new List<URLs>();
            URLs url = new URLs();
            string dbConnectionString = string.Empty;
            string query = "select Id,Code,url from CodeURLMapping";
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            string hostName = config.Build().GetSection("HostName").Value;

            this.ConnectionString = DBSServers.DBServerList.Find(x => x.Name.Equals(hostName)).ConnectionString;
            this.Open();
            IDataReader dr = this.ExecuteReader(CommandType.Text, query);
            DataTable dt = new DataTable();
            dt.Load(dr);
            List<string> codes = dt.AsEnumerable().Select(x => Convert.ToString(x[1])).Distinct().ToList();
            foreach (string code in codes)
            {
                url = new URLs();
                url.Code = code;
                url.Url = dt.AsEnumerable().Where(x => Convert.ToString(x[1]) == code).Select(x => Convert.ToString(x[2])).ToList();
                urls.Add(url);
            }
            bool isExist = GlobalCacheManager.Instance.Exists(key);
            if (isExist)
            {
                GlobalCacheManager.Instance.Remove(key);
            }
            GlobalCacheManager.Instance.Set(key, JsonConvert.SerializeObject(urls));
            if (urls.Count > 0)
                return true;
            else
                return false;
        }

        public string GetCodeFromURL(string url)
        {
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            string redisKey = config.Build().GetSection("HostName").Value + "_" + config.Build().GetSection("ProjectName").Value + "_" + "URL_CODE";
            bool isExist = GlobalCacheManager.Instance.Exists(redisKey);
            if (!isExist)
            {
                GetURLDetails(redisKey);
            }
            List<URLs> urlDetails = JsonConvert.DeserializeObject<List<URLs>>(CacheManager.Instance.Get(redisKey));
            foreach (URLs urlDeatil in urlDetails)
            {
                if (urlDeatil.Url.Contains(url))
                {
                    return urlDeatil.Code;
                }
            }
            return string.Empty;
        }

        public bool SynchronizeRedisConnections(string key)
        {
            string dbConnectionString = string.Empty;
            //string query = "SELECT [Id],[CompanyName],[Code],[DBServerName],[DBName],[DBUserName],[DBPassword] FROM [dbo].[Company]";
            //string query = "SELECT mo.[Id], mo.[Name], mo.[Code], comMod.[DBServerName], comMod.[DBName], comMod.[DBUserName], comMod.[DBPassword] FROM CompanyApps AS comMod JOIN Module AS mo ON comMod.ModuleId = mo.Id";
            string query = "SELECT mo.[Id], mo.[Id], mo.[Name], mo.[Code], comMod.[DBServerName], comMod.[DBName], comMod.[DBUserName], comMod.[DBPassword] FROM CompanyApps AS comMod JOIN Module AS mo ON comMod.ModuleId = mo.Id";
            //GetCacheConnection();
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            this.strConnection = DBSServers.DBServerList.Find(x => x.Name.Equals(config.Build().GetSection("HostName").Value)).ConnectionString;
            this.providerType = DataProvider.SqlServer;
            this.Open();
            IDataReader dr = this.ExecuteReader(CommandType.Text, query);
            DBInstance dbInstance = new DBInstance();
            DBInstance dbDetails = new DBInstance();
            List<DBInstance> dbInstances = new List<DBInstance>();
            while (dr.Read())
            {
                dbInstance = new DBInstance();
                dbInstance.Id =Convert.ToInt32( dr["Id"].ToString());
                dbInstance.Name = dr["Name"].ToString().Trim();
                dbInstance.Code = dr["Code"].ToString().Trim();
                dbInstance.DBServer = dr["DBServerName"].ToString().Trim();
                dbInstance.DBName = dr["DBName"].ToString().Trim();
                dbInstance.DBUserName = dr["DBUserName"].ToString().Trim();
                dbInstance.DBPassword = dr["DBPassword"].ToString().Trim();
                dbInstances.Add(dbInstance);
            }
            this.Close();
            bool isExist = GlobalCacheManager.Instance.Exists(key);
            List<DBInstance> dbInstList = new List<DBInstance>();
            if (isExist)
            {
                dbInstList = JsonConvert.DeserializeObject<List<DBInstance>>(GlobalCacheManager.instance.Get(key));
                foreach (DBInstance dbIns in dbInstances)
                {
                    dbDetails = dbInstList.Where(x => x.Code.Contains(dbIns.Code)).FirstOrDefault();
                    if (dbDetails == null)
                    {
                        dbInstList.Add(dbIns);
                    }
                    dbDetails = new DBInstance();
                }
                //GlobalCacheManager.Instance.Remove("ConnectionString");
            }
            else
            {
                dbInstList.AddRange(dbInstances);
            }
            GlobalCacheManager.Instance.Set(key, JsonConvert.SerializeObject(dbInstList));
            if (dbInstances.Count > 0)
                return true;
            else
                return false;
        }

        public bool SynchronizeWithRedisConnectionInfo(string key)
        {
            string dbConnectionString = string.Empty;
            string query = "SELECT commod.ConnectionId as ConnectionId, 0 as [Id], '' [Name], '' [Code], comMod.[DBServerName], comMod.[DBName], comMod.[DBUserName], comMod.[DBPassword] FROM CompanyApps AS comMod";
            //GetCacheConnection();
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            this.strConnection = DBSServers.DBServerList.Find(x => x.Name.Equals(config.Build().GetSection("HostName").Value)).ConnectionString;
            this.providerType = DataProvider.SqlServer;
            this.Open();
            IDataReader dr = this.ExecuteReader(CommandType.Text, query);
            DBInstance dbInstance = new DBInstance();
            DBInstance dbDetails = new DBInstance();
            List<DBInstance> dbInstances = new List<DBInstance>();
            while (dr.Read())
            {
                dbInstance = new DBInstance();
                dbInstance.ConnectionId = dr["ConnectionId"].ToString();
                dbInstance.Name = dr["Name"].ToString().Trim();
                dbInstance.Code = dr["Code"].ToString().Trim();
                dbInstance.DBServer = dr["DBServerName"].ToString().Trim();
                dbInstance.DBName = dr["DBName"].ToString().Trim();
                dbInstance.DBUserName = dr["DBUserName"].ToString().Trim();
                dbInstance.DBPassword = dr["DBPassword"].ToString().Trim();
                dbInstances.Add(dbInstance);
            }
            this.Close();
            bool isExist = GlobalCacheManager.Instance.Exists(key);
            List<DBInstance> dbInstList = new List<DBInstance>();
            if (isExist)
            {
                dbInstList = JsonConvert.DeserializeObject<List<DBInstance>>(GlobalCacheManager.instance.Get(key));
                if (dbInstList.Count== dbInstances.Count)
                {
                    foreach (DBInstance dbIns in dbInstances)
                    {
                        dbDetails = dbInstList.Where(x => x.Code.Contains(dbIns.Code)).FirstOrDefault();
                        if (dbDetails == null)
                        {
                            dbInstList.Add(dbIns);
                        }
                        dbDetails = new DBInstance();
                    } 
                }
                else
                {
                    dbInstList.AddRange(dbInstances);
                }
                //GlobalCacheManager.Instance.Remove("ConnectionString");
            }
            else
            {
                dbInstList.AddRange(dbInstances);
            }
            GlobalCacheManager.Instance.Set(key, JsonConvert.SerializeObject(dbInstList));
            if (dbInstances.Count > 0)
                return true;
            else
                return false;
        }


        public bool SynchRedisConnections(string key)  
        {
            string dbConnectionString = string.Empty;
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            string query = "SELECT [Id],[Name] ,[Code],[DBServer],[DBName],[DBUserName],[DBPassword] FROM [dbo].InstanceDetails";
            //GetCacheConnection();
            this.strConnection = DBSServers.DBServerList.Find(x => x.Name.Equals(config.Build().GetSection("HostName").Value)).ConnectionString;
            this.providerType = DataProvider.SqlServer;
            this.Open();
            IDataReader dr = this.ExecuteReader(CommandType.Text, query);
            DBInstance dbInstance = new DBInstance();
            DBInstance dbDetails = new DBInstance();
            List<DBInstance> dbInstances = new List<DBInstance>();
            while (dr.Read())
            {
                dbInstance = new DBInstance();
                dbInstance.Name = dr["Name"].ToString().Trim();
                dbInstance.Code = dr["Code"].ToString().Trim();
                dbInstance.DBServer = dr["DBServer"].ToString().Trim();
                dbInstance.DBName = dr["DBName"].ToString().Trim();
                dbInstance.DBUserName = dr["DBUserName"].ToString().Trim();
                dbInstance.DBPassword = dr["DBPassword"].ToString().Trim();
                dbInstances.Add(dbInstance);
            }
            this.Close();
            bool isExist = GlobalCacheManager.Instance.Exists(key);
            List<DBInstance> dbInstList = new List<DBInstance>();
            if (isExist)
            {
                dbInstList = JsonConvert.DeserializeObject<List<DBInstance>>(GlobalCacheManager.instance.Get(key));
                foreach (DBInstance dbIns in dbInstances)
                {
                    dbDetails = dbInstList.Where(x => x.Code.Contains(dbIns.Code)).FirstOrDefault();
                    if (dbDetails == null)
                    {
                        dbInstList.Add(dbIns);
                    }
                    dbDetails = new DBInstance();
                }
                //GlobalCacheManager.Instance.Remove("ConnectionString");
            }
            else
            {
                dbInstList.AddRange(dbInstances);
            }
            GlobalCacheManager.Instance.Set(key, JsonConvert.SerializeObject(dbInstList));
            if (dbInstances.Count > 0)
                return true;
            else
                return false;
        }


        public string GetConnectionString(string cacheKey)
        {
            string dbConnectionString = string.Empty;
            string context = GlobalCacheManager.Instance.Get("usercontext_" + cacheKey.ToString());
            UserContext userContext = JsonConvert.DeserializeObject<UserContext>(GlobalCacheManager.Instance.Get("usercontext_" + cacheKey.ToString()));
            DBInstance dbInstance = userContext.InstanceList.Find(x => x.Code.Trim().Equals(userContext.CurCompanyCode));
            dbConnectionString = "Server=" + dbInstance.DBServer + "; Database=" + dbInstance.DBName + "; User Id=" + dbInstance.DBUserName + "; Password=" + dbInstance.DBPassword;
            userContext = null;
            return dbConnectionString;
        }

        public string GetCompanyConnectionString(string code)
        {
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            string redisKey = config.Build().GetSection("HostName").Value+ "_" + config.Build().GetSection("ProjectName").Value + "_" + "ConnectionString";
            string dbConnectionString = string.Empty;
            List<DBInstance> dbInstances = new List<DBInstance>();
            bool isExist = GlobalCacheManager.Instance.Exists(redisKey);
            if (isExist)
            {
                dbInstances = JsonConvert.DeserializeObject<List<DBInstance>>(GlobalCacheManager.Instance.Get(redisKey));
                if (!dbInstances.Select(x => x.Code.Trim()).ToList().Contains(code))
                {
                    SynchronizeRedisConnections(redisKey);
                    dbInstances = new List<DBInstance>();
                    dbInstances = JsonConvert.DeserializeObject<List<DBInstance>>(GlobalCacheManager.Instance.Get(redisKey));
                }
            }
            else
            {
                SynchronizeRedisConnections(redisKey);
                dbInstances = JsonConvert.DeserializeObject<List<DBInstance>>(GlobalCacheManager.Instance.Get(redisKey));
            }
            DBInstance dbInstance = dbInstances.Where(x => x.Code.Trim() == code.Trim()).FirstOrDefault();
            dbConnectionString = "Server=" + dbInstance.DBServer + "; Database=" + dbInstance.DBName + "; User Id=" + dbInstance.DBUserName + "; password=" + dbInstance.DBPassword;
            this.providerType = DataProvider.SqlServer;
            this.strConnection = dbConnectionString;
            return dbConnectionString;
        }

        /*The Below method has implemented by joy. Please go through the CAF release note for more details*/
        public string GenerateConnectionString(string connectionId)
        {

            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            string redisKey = config.Build().GetSection("HostName").Value + "_" + config.Build().GetSection("ProjectName").Value + "_" + "ConnectionString";
            string dbConnectionString = string.Empty;
            List<DBInstance> dbInstances = new List<DBInstance>();
            bool isExist = GlobalCacheManager.Instance.Exists(redisKey);
            if (isExist)
            {
                dbInstances = JsonConvert.DeserializeObject<List<DBInstance>>(GlobalCacheManager.Instance.Get(redisKey));
                if (!dbInstances.Select(x => x.ConnectionId).ToList().Contains(connectionId))
                {
                    SynchronizeWithRedisConnectionInfo(redisKey);
                    dbInstances = new List<DBInstance>();
                    dbInstances = JsonConvert.DeserializeObject<List<DBInstance>>(GlobalCacheManager.Instance.Get(redisKey));
                }
            }
            else
            {
                SynchronizeWithRedisConnectionInfo(redisKey);
                dbInstances = JsonConvert.DeserializeObject<List<DBInstance>>(GlobalCacheManager.Instance.Get(redisKey));
            }
            DBInstance dbInstance = dbInstances.Where(x => x.ConnectionId== connectionId).FirstOrDefault();
            dbConnectionString = "Server=" + dbInstance.DBServer + "; Database=" + dbInstance.DBName + "; User Id=" + dbInstance.DBUserName + "; password=" + dbInstance.DBPassword;
            this.providerType = DataProvider.SqlServer;
            this.strConnection = dbConnectionString;
            return dbConnectionString;
        }

        public string GetDatabaseConnectionString(string code)      
        {
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            string redisKey = config.Build().GetSection("HostName").Value+ "_" + config.Build().GetSection("ProjectName").Value + "_" + "ConnectionString";
            string dbConnectionString = string.Empty;
            List<DBInstance> dbInstances = new List<DBInstance>();
            bool isExist = GlobalCacheManager.Instance.Exists(redisKey);
            if (isExist)
            {
                dbInstances = JsonConvert.DeserializeObject<List<DBInstance>>(GlobalCacheManager.Instance.Get(redisKey));
                if(!dbInstances.Select(x=>x.Code.Trim()).ToList().Contains(code))
                {
                    SynchRedisConnections(redisKey);
                    dbInstances = new List<DBInstance>();
                    dbInstances = JsonConvert.DeserializeObject<List<DBInstance>>(GlobalCacheManager.Instance.Get(redisKey));
                }
            }
            else
            {
                SynchRedisConnections(redisKey);
                dbInstances = JsonConvert.DeserializeObject<List<DBInstance>>(GlobalCacheManager.Instance.Get(redisKey));
            }
            DBInstance dbInstance = dbInstances.Where(x => x.Code.Trim() == code.Trim()).FirstOrDefault();
            dbConnectionString = "Server=" + dbInstance.DBServer + "; Database=" + dbInstance.DBName + "; User Id=" + dbInstance.DBUserName + "; password=" + dbInstance.DBPassword;
            this.providerType = DataProvider.SqlServer;
            this.strConnection = dbConnectionString;
            return dbConnectionString;
        }

        public string GetConnectionStringFromURL(string code, string url)
        {
            if (string.IsNullOrEmpty(code))
            {

            }
            return string.Empty;
        }

        public string GetControlDBConnectionString()
        {
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            string hostName = config.Build().GetSection("HostName").Value;
            string dbConnectionString = DBSServers.DBServerList.Find(x => x.Name.Equals(hostName)).ConnectionString;
            return dbConnectionString;
        }

        public int ExecuteNonQuery(CommandType commandType, string commandText)
        {
            this.idbCommand = DBManagerFactory.GetCommand(this.ProviderType);
            this.PrepareCommand(this.idbCommand, this.Connection, this.Transaction, commandType, commandText, this.Parameters);
            int returnValue = this.idbCommand.ExecuteNonQuery();
            this.idbCommand.Parameters.Clear();
            return returnValue;
        }

        public object ExecuteScalar(CommandType commandType, string commandText)
        {
            this.idbCommand = DBManagerFactory.GetCommand(this.ProviderType);
            this.PrepareCommand(this.idbCommand, this.Connection, this.Transaction, commandType, commandText, this.Parameters);
            object returnValue = this.idbCommand.ExecuteScalar();
            this.idbCommand.Parameters.Clear();
            return returnValue;
        }

        public DataSet ExecuteDataSet(CommandType commandType, string commandText)
        {
            this.idbCommand = DBManagerFactory.GetCommand(this.ProviderType);
            this.PrepareCommand(this.idbCommand, this.Connection, this.Transaction, commandType, commandText, this.Parameters);
            IDbDataAdapter dataAdapter = DBManagerFactory.GetDataAdapter(this.ProviderType);
            dataAdapter.SelectCommand = this.idbCommand;
            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            this.idbCommand.Parameters.Clear();
            return dataSet;
        }
    }
}
