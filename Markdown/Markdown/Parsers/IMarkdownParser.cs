using Markdown.Markings;
using Markdown.Tokens;

namespace Markdown.Parsers
{
    public interface IParser<out TMarking> where TMarking : IMarkingTree<IToken>
    {
        public TMarking Parse(string markdown);
    }
}