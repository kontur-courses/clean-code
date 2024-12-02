using Markdown.Models;

namespace Markdown.Converters.Base;

public abstract class BaseConverter : IConverter
{
    protected abstract string StartOriginalValue { get; }
    protected abstract string EndOriginalValue { get; }
    protected abstract string StartConvertedValue { get; }
    protected abstract string EndConvertedValue { get; }

    public abstract IEnumerable<TagPair> Convert(string text);
}