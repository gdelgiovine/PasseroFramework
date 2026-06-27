using SqlKata.Compilers;

namespace Passero.Framework.QueryEngine
{
    /// <summary>
    /// Compiler dedicato per DB2 / DB2i.
    /// Riusa la sintassi Oracle-like supportata da SQLKata 2.4 per paging e quoting.
    /// </summary>
    public sealed class Db2Compiler : OracleCompiler
    {
    }
}