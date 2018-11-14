using System.Collections.Generic;
using ConsoleApplication1.ConnectionHandlers.Containers;
using ConsoleApplication1.UsefulEnums;

namespace ConsoleApplication1.Interfaces
{
    public interface IConnecter
    {
        void AddItem(int maxConnectionStrength, Direction direction);
        IEnumerable<Connection> GetSortedConneсtions();
        IEnumerable<int> GetResidualStrength();
    }
}
