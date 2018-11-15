using System.Collections.Generic;
using ConsoleApplication1.ConnectionHandlers.Containers;
using ConsoleApplication1.UsefulEnums;

namespace ConsoleApplication1.Interfaces
{
    public interface ITranslatorDirectedItems<in TKey>
    {
        void AddItem(TKey key, Direction direction);
        IReadOnlyCollection<MdConvertedItem> ExtractConvertedItems();
    }
}
