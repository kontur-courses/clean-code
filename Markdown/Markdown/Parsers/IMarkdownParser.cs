using Markdown.Markings;
using Markdown.Tokens;

namespace Markdown.Parsers
{
    public interface IParser<out TMarking> where TMarking : IMarking<IToken>
    {
        public TMarking Parse(string markdown);
    }
}