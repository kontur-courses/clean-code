using System;
using System.Collections.Generic;

namespace ConsoleApplication1.Interfaces
{
    public interface ITextConverter<in TData, in TAdditive>
    {
        String ConvertText(IReadOnlyCollection<TData> data, IReadOnlyCollection<TAdditive> additiveInformaion);
    }
}
