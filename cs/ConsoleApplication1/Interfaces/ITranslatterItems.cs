using System.Collections.Generic;
using ConsoleApplication1.UsefulEnums;

namespace ConsoleApplication1.Interfaces
{
    public interface ITranslatorDirectedItems<in TKey, out TResult>
    {
        void AddItem(TKey key, Direction direction);
        IEnumerable<TResult> ExtractConvertedItems();
    }
}
