using System.Text;
using Markdown.Extensions;
using Markdown.Tokens;

namespace Markdown.TokenParser.TokenParsingIteratorState
{
    public class TextParsingIteratorState : TokenParsingIteratorState
    {
        public TextParsingIteratorState(TokenParsingIterator iterator) : base(iterator)
        {
        }

        public override TokenNode Parse()
        {
            var sb = new StringBuilder();
            var text = Iterator.Current.Value;
            sb.Append(text);
            while (Iterator.TryMoveNext(out var next))
                if (next.Type == TokenType.Text)
                {
                    sb.Append(next.Value);
                }
                else
                {
                    Iterator.PushToBuffer(next);
                    break;
                }

            return Token.Text(sb.ToString()).ToNode();
        }
    }
}