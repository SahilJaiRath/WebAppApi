using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Einv_EwayBill_WebApp.Models
{
    public sealed class DataLayerParameterCache
    {
        #region private methods, variables, and constructors

        //Since this class provides only "" methods, make the default constructor private to prevent 
        //instances from being created with "new OracleDataAccessParameterCache()".
        public DataLayerParameterCache() { }

        private static Hashtable paramCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// resolve at run time the appropriate set of OracleParameters for a stored procedure
        /// </summary>
        /// <param name="connectionString">a valid connection string for a OracleConnection</param>
        /// <param name="spName">the name of the stored procedure</param>
        /// <param name="includeReturnValueParameter">whether or not to include their return value parameter</param>
        /// <returns></returns>
        private static OracleParameter[] DiscoverSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
        {
            using (OracleConnection cn = new OracleConnection(connectionString))
            using (OracleCommand cmd = new OracleCommand(spName, cn))
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;

                OracleCommandBuilder.DeriveParameters(cmd);

                if (!includeReturnValueParameter)
                {
                    // cmd.Parameters.RemoveAt(0);
                }

                OracleParameter[] discoveredParameters = new OracleParameter[cmd.Parameters.Count]; ;

                cmd.Parameters.CopyTo(discoveredParameters, 0);

                return discoveredParameters;
            }
        }

        //deep copy of cached OracleParameter array
        private static OracleParameter[] CloneParameters(OracleParameter[] originalParameters)
        {
            OracleParameter[] clonedParameters = new OracleParameter[originalParameters.Length];

            for (int i = 0, j = originalParameters.Length; i < j; i++)
            {
                clonedParameters[i] = (OracleParameter)((ICloneable)originalParameters[i]).Clone();
            }

            return clonedParameters;
        }

        #endregion private methods, variables, and constructors

        #region caching functions

        /// <summary>
        /// add parameter array to the cache
        /// </summary>
        /// <param name="connectionString">a valid connection string for a OracleConnection</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters to be cached</param>
        public static void CacheParameterSet(string connectionString, string commandText, params OracleParameter[] commandParameters)
        {
            string hashKey = CreateHashKey(connectionString, commandText);

            paramCache[hashKey] = commandParameters;
        }

        /// <summary>
        /// retrieve a parameter array from the cache
        /// </summary>
        /// <param name="connectionString">a valid connection string for a OracleConnection</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <returns>an array of SqlParamters</returns>
        public static OracleParameter[] GetCachedParameterSet(string connectionString, string commandText)
        {
            string hashKey = CreateHashKey(connectionString, commandText);

            OracleParameter[] cachedParameters = (OracleParameter[])paramCache[hashKey];

            if (cachedParameters == null)
            {
                return null;
            }
            else
            {
                return CloneParameters(cachedParameters);
            }
        }

        #endregion caching functions

        #region Parameter Discovery Functions

        /// <summary>
        /// Retrieves the set of OracleParameters appropriate for the stored procedure
        /// </summary>
        /// <remarks>
        /// This method will query the database for this information, and then store it in a cache for future requests.
        /// </remarks>
        /// <param name="connectionString">a valid connection string for a OracleConnection</param>
        /// <param name="spName">the name of the stored procedure</param>
        /// <returns>an array of OracleParameters</returns>
        public static OracleParameter[] GetSpParameterSet(string connectionString, string spName)
        {
            return GetSpParameterSet(connectionString, spName, false);
        }

        /// <summary>
        /// Retrieves the set of OracleParameters appropriate for the stored procedure
        /// </summary>
        /// <remarks>
        /// This method will query the database for this information, and then store it in a cache for future requests.
        /// </remarks>
        /// <param name="connectionString">a valid connection string for a OracleConnection</param>
        /// <param name="spName">the name of the stored procedure</param>
        /// <param name="includeReturnValueParameter">a bool value indicating whether the return value parameter should be included in the results</param>
        /// <returns>an array of OracleParameters</returns>
        public static OracleParameter[] GetSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
        {
            string hashKey = CreateHashKey(connectionString, spName, includeReturnValueParameter);

            OracleParameter[] cachedParameters;

            cachedParameters = (OracleParameter[])paramCache[hashKey];

            if (cachedParameters == null)
            {
                cachedParameters = (OracleParameter[])(paramCache[hashKey] = DiscoverSpParameterSet(connectionString, spName, includeReturnValueParameter));
            }

            return CloneParameters(cachedParameters);
        }

        private static string CreateHashKey(string connectionString, string spName, bool includeReturnValueParameter)
        {
            string hashKey = connectionString + ":" + spName + (includeReturnValueParameter ? ":include ReturnValue Parameter" : "");
            return hashKey;

        }
        private static string CreateHashKey(string connectionString, string commandText)
        {
            string hashKey = connectionString + ":" + commandText;
            return hashKey;

        }

        #endregion Parameter Discovery Functions

    }
}
