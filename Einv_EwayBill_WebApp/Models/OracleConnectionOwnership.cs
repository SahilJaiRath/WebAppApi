using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Einv_EwayBill_WebApp.Models
{
    internal enum OracleConnectionOwnership
    {
        /// <summary>Connection is owned and managed by SqlHelper</summary>
        Internal,
        /// <summary>Connection is owned and managed by the caller</summary>
        External
    }
}
