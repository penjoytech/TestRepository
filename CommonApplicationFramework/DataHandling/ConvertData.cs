using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonApplicationFramework.DataHandling
{
	public static class ConvertData
	{
		public static int ToInt(object val)
		{
			return (val == null || val == DBNull.Value || string.IsNullOrEmpty(val+"")) ? 0 : int.Parse(val.ToString());
		}

		public static bool ToBoolean(object val)
		{
            return (val == null || val == DBNull.Value ? false :  Convert.ToBoolean(val));
		}

		public static DateTimeOffset? ToDate(object val)
		{
			return val == DBNull.Value ? (DateTimeOffset?)null : DateTimeOffset.Parse(val.ToString());
		}

		public static string ToString(object val)
		{
			return val == DBNull.Value ? string.Empty : val.ToString();
		}

		public static decimal ToDecimal(object val)
		{
			return val == DBNull.Value ? 0 : Convert.ToDecimal(val);
		}

		public static decimal Round(object val)
		{
			return val == DBNull.Value ? 0 : Math.Round(Convert.ToDecimal(val),2);
		}
        public static double ToDouble (object val)
        {
            return val == DBNull.Value ? 0 : Convert.ToDouble(val);
        }

    }
}
