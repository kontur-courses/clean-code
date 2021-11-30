using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown.Common
{
    public class Token
    {
        private readonly List<Token> childTokens = new List<Token>();

        public string Value { get; }
        public string Text => WrapSetting.RemoveMdTags(Value);
        public int Position { get; private set; }
        public MdWrapSetting WrapSetting { get; }


        public Token(string value)
            : this(value, 0, new MdWrapSetting("", MdTagType.Block))
        {
        }

        public Token(int position, MdWrapSetting setting)
            : this(setting.MdTag, position, setting)
        {
            Value = setting.MdTag;
        }

        public Token(string value, int position, MdWrapSetting wrapSetting)
        {
            Value = value;
            Position = position;
            WrapSetting = wrapSetting;
        }


        public void AddToken(Token child)
        {
            var parent = childTokens.FirstOrDefault(token => token.IsChild(child));
            if (parent != null)
            {
                parent.AddToken(child);
                child.Position -= parent.Position + parent.WrapSetting.MdTag.Length;
            }
            else
                childTokens.Add(child);
        }

        public string Render()
        {
            var render = new StringBuilder(Text);
            foreach (var childToken in childTokens.OrderByDescending(token => token.Position))
            {
                render.Remove(childToken.Position, childToken.Value.Length);
                render.Insert(childToken.Position, childToken.Render());
            }

            return WrapSetting.InsertHtmlTags(render.ToString());
        }

        public bool IsChild(Token child)
        {
            return child.Position >= Position &&
                   child.Position < Position + Value.Length &&
                   child.Position + child.Value.Length <= Position + Value.Length;
        }
    }
}