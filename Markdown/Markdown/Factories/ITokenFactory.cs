using Markdown.Tokens;

namespace Markdown.Factories
{
    public interface ITokenFactory<out TToken> where TToken : IToken
    {
        public TToken NewToken(string value);
    }
}