using System.Collections.Generic;
using Markdown.Markings;
using Markdown.Tokens;

namespace Markdown.Factories
{
    public interface IMarkingFactory<in TToken, out TMarking>
        where TToken : IToken
        where TMarking : IMarking<TToken>
    {
        public TMarking NewMarking(IEnumerable<IEnumerable<TToken>> tokensLines);
    }
}