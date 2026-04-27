using Dapper;
using Dapper.ColumnMapper;
using Dapper.Contrib.Extensions;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Passero.Framework.Extensions;
using Wisej.Web;
//using Passero.Framework.Base;
#nullable enable

namespace Passero.Framework
{
    public partial class Repository<ModelClass>
        where ModelClass : class
    {
        public async Task<ExecutionResult> ExecuteInTransactionScope(
       Func<IDbTransaction, Task> operation,
       IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
       int retryCount = 3)
        {
            var er = new ExecutionResult($"{mClassName}.ExecuteInTransactionScope()");
            for (int i = 0; i < retryCount; i++)
            {
                try
                {
                    //ValidateConnection();
                    using (var transaction = DbConnection.BeginTransaction(isolationLevel))
                    {
                        await operation(transaction);
                        transaction.Commit();
                        return er;
                    }
                }
                catch (Exception ex) when (i < retryCount - 1)
                {
                    await Task.Delay(100 * (i + 1)); // Ritenta con un breve delay
                }
                catch (Exception ex)
                {
                    er.Exception = ex;
                    er.ResultMessage = ex.Message;
                    HandleException(er);
                    return er;
                }
            }
            return er;
        }

    }
}
