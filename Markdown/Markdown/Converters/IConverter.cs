using Markdown.Markings;
using Markdown.Tokens;

namespace Markdown.Converters
{
    public interface IConverter<in TIn, out TOut>
    {
        public TOut Convert(TIn convertingValue);
    }
}