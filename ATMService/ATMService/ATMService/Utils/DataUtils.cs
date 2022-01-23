using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMService.Utils
{
    public static class DataUtils
    {

		/// <summary>
		/// Replaceable key in connection string
		/// </summary>
        public const string CONTENT_ROOT_PLACE_HOLDER = "%CONTENTROOTPATH%";

		/// <summary>
		/// Fixing connection string
		/// </summary>
		/// <param name="Configuration">Project configuration</param>
		/// <param name="contentRootPath">Reference path</param>
		/// <param name="connectionStringKey">Key of connection</param>
		/// <returns>Fixed connection string</returns>
		public static string ResolveDbConnectionString(IConfiguration Configuration, string contentRootPath, string connectionStringKey)
		{

			var connectionString = Configuration.GetConnectionString(connectionStringKey);

			if (connectionString.Contains(CONTENT_ROOT_PLACE_HOLDER))
			{
				connectionString = connectionString.Replace(CONTENT_ROOT_PLACE_HOLDER, contentRootPath);
			}
			return connectionString;
		}

	}
}
