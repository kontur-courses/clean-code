using System.Collections.Generic;
using Markdown.Markings;
using Markdown.Tokens;

namespace Markdown.Factories
{
    public interface IMarkingFactory<out TMarking> where TMarking : IMarking<IToken>
    {
        public TMarking NewMarking(IEnumerable<IEnumerable<IToken>> tokensLines);
    }
}