using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passero.Framework
{
    public class TransactionScope : IDisposable
    {
        public IDbTransaction Transaction { get; } // Proprietà pubblica per accedere alla transazione
        private bool _committed;

        public TransactionScope(IDbConnection connection, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            Transaction = connection.BeginTransaction(isolationLevel);
        }

        public void Commit()
        {
            Transaction.Commit();
            _committed = true;
        }

        public void Dispose()
        {
            if (!_committed)
                Transaction.Rollback();
            Transaction.Dispose();
        }
    }
}
