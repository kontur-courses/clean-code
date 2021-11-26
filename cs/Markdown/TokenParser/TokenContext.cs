using System.Collections.Generic;
using System.Linq;
using Markdown.Extensions;
using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.TokenParser
{
    public class TokenContext
    {
        public bool ContainsWhiteSpace { get; private set; }
        public readonly Token Token;
        public readonly bool IsInMiddleOfWord;
        public IEnumerable<TagNode> Children => children;

        private readonly List<TagNode> children = new();

        public TokenContext(Token token, bool isInMiddleOfWord = false)
        {
            IsInMiddleOfWord = isInMiddleOfWord;
            Token = token;
        }

        public void AddChild(TagNode node)
        {
            ContainsWhiteSpace = ContainsWhiteSpace || node.Tag.GetText().Contains(Characters.WhiteSpace);
            children.Add(node);
        }

        public string ToText()
        {
            if (children.Count == 0)
                return Token.Value;
            return Token.Value + StringUtils.Join(children.Select(x => x.ToText()));
        }

        public static bool IsHeader1(TokenContext context) => context.Token.Type == TokenType.Header1;
        
        public static bool IsLink(TokenContext context) => context.Token.Type == TokenType.OpenSquareBracket;
    }
}