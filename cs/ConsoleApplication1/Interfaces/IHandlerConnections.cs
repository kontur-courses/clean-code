using System.Collections.Generic;

namespace ConsoleApplication1.Interfaces
{
    public interface IHandlerConnections<in TConnection, out TResult>
    {
        IEnumerable<TResult> TranslateConnections(IEnumerable<TConnection> connection, IEnumerable<int> remainingStrengths);
    }
}
