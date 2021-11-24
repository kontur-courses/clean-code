using System.Collections.Generic;
using System.Linq;
using Markdown.Extensions;
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

        public TokenContext(Token token, bool isInMiddleOfWord)
        {
            IsInMiddleOfWord = isInMiddleOfWord;
            Token = token;
        }

        public void AddChild(TagNode node)
        {
            ContainsWhiteSpace = ContainsWhiteSpace || node.Tag.Value.Contains(Characters.WhiteSpace);
            children.Add(node);
        }

        public string ToText()
        {
            if (children.Count == 0)
                return Token.Value;
            return Token.Value + StringUtils.Join(children.Select(x => x.ToText()));
        }
    }
}