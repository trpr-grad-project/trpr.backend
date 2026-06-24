using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Modules.Payments.Application.Abstractions
{
    public interface IDbConnectionFactory
    {
        public Task<DbConnection> CreateSqlConnection();
    }
}