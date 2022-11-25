using System.Collections.Generic;
using Markdown.Enums;
using Markdown.Rules;
using Markdown.Tags;

namespace Markdown
{
    public class Token
    {
        public TokenType Type;
        public ITag Tag;
        public string Content = "";
        public Token Parent;
        public List<Token> Childrens = new ();

        public Token(TokenType type, ITag tag, Token parent)
        {
            Type = type;
            Tag = tag;
            Parent = parent;
        }

        public bool TryAddToken(Token token)
        {
            if (token == null) return false;

            if (Tag.NeedTagIgnore(token.Tag)) return false;

            Childrens.Add(token); return true;
        }

        public Token ToTextToken(bool needClosingMarkup = false)
        {
            if (Type == TokenType.Text)
                return this;
            
            var markdownTag = MarkdownToHtml.Rules.TryGetKeyByValue(Tag);
            
            if (markdownTag != null)
                Content = markdownTag.Opening;
            
            foreach (var children in Childrens)
                Content += children.Content;

            if (needClosingMarkup && markdownTag != null)
                Content += markdownTag.Closing;

            Childrens.Clear();
            
            Type = TokenType.Text;
            Tag = Tags.Tag.Empty;
            
            return this;
        }
    }
}