using System;
using System.Data;
using System.Threading.Tasks;
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
