#region copyright
// <copyright file="LogManager.cs" company="Pentechs.com">
//     Pentechs.com
// </copyright>
// <author>Pentechs Architect</author>
#endregion

namespace CommonApplicationFramework.Logging
{
	#region namespaces
	using CommonApplicationFramework.Common;
	using CommonApplicationFramework.ConfigurationHandling;
	using CommonApplicationFramework.DataHandling;
	using Newtonsoft.Json;
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Diagnostics;
	using System.IO;
	#endregion

	/// <summary>
	/// abstract class for log the exception
	/// </summary>
	public abstract class LogBase
	{
		public abstract void Log(string message, string Code);
		public abstract void Log(Exception ex, string Code);
		public abstract void Log(string message, string Code, Exception ex);
		public abstract void Log(LogObject logObject);

		public DateTime ISTDateTime()
		{
			TimeZoneInfo TZ = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
			DateTime utc = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);
			return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TZ);
		}
	}

	public enum LogTarget
	{
		File, Database, EventLog
	}

	public class LogObject
	{
		public int Id { get; set; }
		public int ObjectId { get; set; }
		public string ObjectType { get; set; }
		public string Component { get; set; }
		public string MethodName { get; set; }
		public string RequestDetails { get; set; }
		public string ErrorCode { get; set; }
		public string ErrorMessage { get; set; }
		public string ErrorDescription { get; set; }
		public DateTimeOffset CreatedOn { get; set; }

	}

	/// <summary>
	/// Log Exception message into file
	/// </summary>
	public class FileLogger : LogBase
	{
		private readonly string filePath = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("LogFilePath")).Value.ToString();

		public override void Log(string message, string Code)
		{
			try
			{
				bool EnableErrorLogger = Convert.ToBoolean(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EnableFileErrorLogger")).Value.ToString());
				if (EnableErrorLogger)
				{
					string date = DateTime.Now.ToString("dd/MM/yyyy");
					using (StreamWriter streamWriter = new StreamWriter(filePath, true))
					{
						streamWriter.WriteLine("{0}: {1} :", Code, ISTDateTime());
						streamWriter.WriteLine("______________________________________________________");
						streamWriter.WriteLine("{0}", message);
						streamWriter.WriteLine("______________________________________________________");
						streamWriter.Close();
					}
				}
			}
			catch (Exception Ex)
			{
				throw Ex;
			}
		}

		public override void Log(Exception ex, string Code)
		{
			try
			{
				bool EnableErrorLogger = Convert.ToBoolean(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EnableFileErrorLogger")).Value.ToString());
				if (EnableErrorLogger)
				{
					using (StreamWriter streamWriter = new StreamWriter(filePath, true))
					{
						streamWriter.WriteLine("{0}: {1} :", Code, ISTDateTime());
						streamWriter.WriteLine("______________________________________________________");
						streamWriter.WriteLine(ex.Message);
						streamWriter.WriteLine(ex.StackTrace.ToString());
						streamWriter.WriteLine("______________________________________________________");
						streamWriter.Close();
					}
				}
			}
			catch (Exception Ex)
			{
				throw Ex;
			}
		}

		public override void Log(string message, string Code, Exception ex)
		{
			try
			{
				bool EnableErrorLogger = Convert.ToBoolean(Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("EnableFileErrorLogger")).Value.ToString());
				if (EnableErrorLogger)
				{
					using (StreamWriter streamWriter = new StreamWriter(filePath, true))
					{
						//streamWriter.WriteLine("{0}: {1} {2}:", Code, ISTDateTime(), DateTime.Now.ToShortTimeString());
						streamWriter.WriteLine("{0}: {1} :", Code, ISTDateTime());
						streamWriter.WriteLine("______________________________________________________");
						streamWriter.WriteLine(message);
						streamWriter.WriteLine(ex.StackTrace.ToString());
						streamWriter.WriteLine("______________________________________________________");
						streamWriter.Close();
					}
				}
			}
			catch (Exception Ex)
			{
				throw Ex;
			}
		}

		public override void Log(LogObject logObject)
		{
			try
			{
				using (StreamWriter streamWriter = new StreamWriter(filePath, true))
				{
					streamWriter.WriteLine("{0}: {1} :", ISTDateTime());
					streamWriter.WriteLine("______________________________________________________");					 
					streamWriter.WriteLine(logObject.ErrorDescription);
					streamWriter.WriteLine("______________________________________________________");
					streamWriter.Close();
				}
			}
			catch (Exception Ex)
			{
				throw Ex;
			}
		}

		
	}

	/// <summary>
	/// Log event
	/// </summary>
	public class EventLogger : LogBase
	{
		public override void Log(string message, string Code)
		{
			EventLog eventLog = new EventLog("");
			eventLog.Source = "STEventLog";
			eventLog.WriteEntry(message);
		}

		public override void Log(Exception ex, string Code)
		{
			EventLog eventLog = new EventLog("");
			eventLog.Source = "STEventLog";
			eventLog.WriteEntry(ex.StackTrace.ToString());
		}

		public override void Log(string message, string Code, Exception ex)
		{
			EventLog eventLog = new EventLog("");
			eventLog.Source = "STEventLog";
			eventLog.WriteEntry(message);
		}
		public override void Log(LogObject logObject)
		{ 
			EventLog eventLog = new EventLog("");
			eventLog.Source = "STEventLog";
			eventLog.WriteEntry(logObject.ErrorMessage);			 
		}
	}

	/// <summary>
	/// Log database event
	/// </summary>
	public class DBLogger : LogBase
	{
		string connectionString = string.Empty;
		DBManager dbManager = null;
		private readonly string filePath = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("LogFilePath")).Value.ToString();

		public override void Log(string message, string Code)
		{
			try
			{
				string date = DateTime.Now.ToString("dd/MM/yyyy");
				using (StreamWriter streamWriter = new StreamWriter(filePath, true))
				{
					streamWriter.WriteLine("{0}: {1} :", Code, ISTDateTime());
					streamWriter.WriteLine("______________________________________________________");
					streamWriter.WriteLine("{0}", message);
					streamWriter.WriteLine("______________________________________________________");
					streamWriter.Close();
				}
			}
			catch (Exception Ex)
			{
				throw Ex;
			}
		}
		public override void Log(Exception ex, string Code)
		{
			throw new NotImplementedException();
		}

		public override void Log(string message, string Code, Exception ex)
		{
			throw new NotImplementedException();
		}
		public override void Log(LogObject logObject)
		{
			try
			{
				 
				using (dbManager = new DBManager())
				{
					dbManager.ConnectionString = dbManager.GetControlDBConnectionString();
					dbManager.Open();
					string errorCode = logObject.ErrorCode;
					if (!string.IsNullOrEmpty(errorCode))
					{
						if (errorCode.Length >= 40)
							errorCode = errorCode.Substring(0, 45);
					}
                    string query = ""; // QueryConfig.LogsManagerQuerySettings["PostErrorLog"].ToString();
					dbManager.CreateParameters(9);
					dbManager.AddParameters(0, "@ObjectId", logObject.ObjectId);
					dbManager.AddParameters(1, "@ObjectType", logObject.ObjectType);
					dbManager.AddParameters(2, "@Component", logObject.Component);
					dbManager.AddParameters(3, "@MethodName", logObject.MethodName);
					dbManager.AddParameters(4, "@RequestDetails", logObject.RequestDetails);
					dbManager.AddParameters(5, "@ErrorCode", errorCode);
					dbManager.AddParameters(6, "@ErrorMessage", logObject.ErrorMessage);
					dbManager.AddParameters(7, "@ErrorDescription", logObject.ErrorDescription);
					dbManager.AddParameters(8, "@CreatedOn", logObject.CreatedOn);

					int result = dbManager.ExecuteNonQuery(CommandType.Text, query);


				}
			}
			catch (Exception ex)
			{
				this.Log(ex.StackTrace.ToString(),"Logged On");
				this.Log("ObjectId:" + logObject.ObjectId + "\n" +
								"ObjectType:" + logObject.ObjectType + "\n" +
								"Component:" + logObject.Component + "\n" +
								"MethodName:" + logObject.MethodName + "\n" +
								"RequestDetails:" + logObject.RequestDetails + "\n" +
								"ErrorCode:" + logObject.ErrorCode + "\n" +
								"ErrorMessage:" + logObject.ErrorDescription + ex.StackTrace + "\n" +
								"ErrorDescription:" + logObject.ErrorDescription + "\n" +
								"CreatedOn:" + logObject.CreatedOn + "\n", "Logged On");
			}
		}
	}

	/// <summary>
	/// Log Exception
	/// </summary>
	public static class LogManager
	{
		private static LogBase logger = null;

		public static void Log(string message, string Code = "Logged On", LogTarget target = LogTarget.File)
		{
			logger = GetLogTarget(target);
			logger.Log(message, Code);
		}

		public static void Log(Exception ex, string Code = "Logged On", LogTarget target = LogTarget.File)
		{
			logger = GetLogTarget(target);
			logger.Log(ex, Code);
		}
		public static void Log(string message, Exception ex, string Code = "Logged On", LogTarget target = LogTarget.File)
		{
			logger = GetLogTarget(target);
			logger.Log(message, Code, ex);
		}

		public static void Log(LogObject logObject, LogTarget target = LogTarget.Database)
		{
			logger = GetLogTarget(target);
			logger.Log(logObject);
		}

		private static LogBase GetLogTarget(LogTarget target)
		{
			LogBase logger = null;
			switch (target)
			{
				case LogTarget.Database:
					logger = new DBLogger();
					break;

				case LogTarget.EventLog:
					logger = new EventLogger();
					break;

				default:
					logger = new FileLogger();
					break;
			}
			return logger;
		}
	}

	#region User Logs

	//public class UserLogModel : BaseModel
	//{
	//    public int Id { get; set; }

	//    public Item User { get; set; }

	//    public Item Designation { get; set; }

	//    public string UserImage { get; set; }

	//    public Item ActivityType { get; set; }

	//    public string IPAddress { get; set; }
	//}

	//public static class UserLogs
	//{
	//    public static bool AddUsersLog(string code, DBManager dbManager, UserLogModel log)
	//    {
	//        try
	//        {
	//            dbManager.CreateParameters(4);
	//            dbManager.AddParameters(0, "@User", log.User.Id);
	//            dbManager.AddParameters(1, "@ActivityType", log.ActivityType.Value);
	//            dbManager.AddParameters(2, "@IPAddress", log.IPAddress);
	//            dbManager.AddParameters(3, "@CreatedBy", log.CreatedBy);
	//            dbManager.ExecuteNonQuery(CommandType.Text, QueryConfig.UserQuerySettings["AddUsersLog"].ToString());
	//            return true;
	//        }
	//        catch (Exception ex)
	//        {
	//            LogManager.Log(ex);
	//            throw ex;
	//        }
	//    }
	//    public static bool AddUserActivityLog(DBManager dbManager, UserLogModel log)
	//    {
	//        try
	//        {
	//            dbManager.CreateParameters(4);
	//            dbManager.AddParameters(0, "@User", log.User.Id);
	//            dbManager.AddParameters(1, "@ActivityType", log.ActivityType.Value);
	//            dbManager.AddParameters(2, "@IPAddress", log.IPAddress);
	//            dbManager.AddParameters(3, "@CreatedBy", log.CreatedBy);
	//            dbManager.ExecuteNonQuery(CommandType.Text, QueryConfig.UserQuerySettings["AddUsersLog"].ToString());
	//            return true;
	//        }
	//        catch (Exception ex)
	//        {
	//            LogManager.Log(ex);
	//            throw ex;
	//        }
	//    }

	//    public static List<UserLogModel> GetUsersLog(string code, DBManager dbManager, int userId, string activityType)
	//    {
	//        try
	//        {
	//            dbManager.CreateParameters(2);
	//            dbManager.AddParameters(0, "@User", userId);
	//            dbManager.AddParameters(1, "@ActivityType", activityType);
	//            IDataReader dr = dbManager.ExecuteReader(CommandType.Text, QueryConfig.UserQuerySettings["GetUsersLog"].ToString());
	//            List<UserLogModel> logs = new List<UserLogModel>();
	//            string filePath = Environments.Configurations.Settings.Find(x => x.Key.ToString().Equals("FileServerURL")).Value.ToString() + code + "/" + "USERIMAGE" + "/";
	//            while (dr.Read())
	//            {
	//                logs.Add(new UserLogModel
	//                {
	//                    Id = Convert.ToInt32(dr["Id"]),
	//                    User = new Item { Id = Convert.ToInt32(dr["UserId"]), Value = dr["FullName"].ToString() },
	//                    Designation = !string.IsNullOrEmpty(dr["DId"].ToString()) ? new Item { Id = Convert.ToInt32(dr["DId"]), Value = dr["Designation"].ToString() } : null,
	//                    UserImage = string.IsNullOrEmpty(dr["Image"].ToString()) ? null : filePath + dr["Image"].ToString(),
	//                    ActivityType = new Item { Id = Convert.ToInt32(dr["UATId"]), Value = dr["UATName"].ToString() },
	//                    IPAddress = dr["IPAddress"].ToString(),
	//                    CreatedOn = DateTimeOffset.Parse(dr["CreatedOn"].ToString()),
	//                    Creator = dr["Creator"].ToString()
	//                });
	//            }
	//            dr.Close();
	//            return logs;
	//        }
	//        catch (Exception ex)
	//        {
	//            LogManager.Log(ex);
	//            throw ex;
	//        }
	//    }
	//}

	#endregion
}
