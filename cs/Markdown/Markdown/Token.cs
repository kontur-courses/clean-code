using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Extensions;

namespace Markdown
{
    public class Token
    {
        private readonly List<Token> childTokens = new List<Token>();
        private int position;
        private bool isIgnore;

        public string Value { get; }
        public string Text => IsIgnore ? Value : Value.RemoveMdTags(WrapSetting);

        public int Position
        {
            get => Parent == null 
                ? position :
                Parent.IsIgnore ? position + Parent.WrapSetting.MdTag.Length : position;
            private set => position = value;
        }

        public MdWrapSetting WrapSetting { get; }
        public Token Parent { get; private set; }

        public IEnumerable<Token> AllDescendants =>
            new[] {this}.Concat(childTokens.SelectMany(token => token.AllDescendants));

        public Token GetRoot => Parent ?? this;

        public bool IsIgnore
        {
            get => isIgnore;
            set => isIgnore = isIgnore || value;
        }

        
        public Token(string value)
            : this(value, 0, new MdWrapSetting("", MdTagType.Root))
        {
        }

        public Token(string value, int position, MdWrapSetting wrapSetting)
        {
            Value = value;
            Position = position;
            WrapSetting = wrapSetting;
        }

        public void AddToken(Token token)
        {
            foreach (var child in childTokens.Where(child => child.IsChild(token)))
            {
                child.AddToken(token);
                token.Position -= child.Position + child.WrapSetting.MdTag.Length;
                token.Parent = child;
                return;
            }

            childTokens.Add(token);
            token.Parent = this;
        }

        public string Render()
        {
            if (IsIgnore)
                return Text;
            
            var render = new StringBuilder(Text);
            foreach (var childToken in childTokens.OrderByDescending(token => token.Position))
            {
                render.Remove(childToken.Position, childToken.Value.Length);
                render.Insert(childToken.Position, childToken.Render());
            }
            return IsIgnore ? render.ToString() : render.ToString().InsertHtmlTags(WrapSetting);
        }

        private bool IsChild(Token token)
        {
            return token.Position >= Position &&
                   token.Position < Position + Value.Length &&
                   token.Position + token.Value.Length <= Position + Value.Length;
        }
    }
}