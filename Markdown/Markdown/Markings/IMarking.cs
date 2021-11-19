using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Markings
{
    public interface IMarking<out TToken> where TToken : IToken
    {
        public IEnumerable<IEnumerable<TToken>> TokensLines { get; }

        public string ToString();
    }
}