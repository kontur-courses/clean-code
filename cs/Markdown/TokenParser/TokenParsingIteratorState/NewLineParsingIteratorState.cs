using Markdown.Extensions;
using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.TokenParser.TokenParsingIteratorState
{
    public class NewLineParsingIteratorState : TokenParsingIteratorState
    {
        public NewLineParsingIteratorState(TokenParsingIterator iterator) : base(iterator)
        {
        }

        public override TagNode Parse()
        {
            if (Iterator.TryFlushContextsUntil(out var context, TokenContext.IsHeader1))
            {
                var node = Iterator.ToNode(context);
                if (node.Tag.Type == TagType.Text)
                    return Token.Text(StringUtils.Join(node.Tag, Token.NewLine)).ToNode();

                Iterator.PushToBuffer(Token.NewLine);
                return new TagNode(node.Tag, node.Children);
            }

            return Token.NewLine.ToText().ToNode();
        }
    }
}