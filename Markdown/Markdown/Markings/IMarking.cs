using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Markings
{
    // Marking - разметка, что-то вроде контейнера для токенов, можно сказать, что документ
    public interface IMarking<out TToken> where TToken : IToken
    {
        public IEnumerable<IEnumerable<TToken>> TokensLines { get; }

        public string ToString();
    }
}