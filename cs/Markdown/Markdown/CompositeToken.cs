using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class CompositeToken : Token
    {
        private readonly List<Token> childTokens = new List<Token>();

        public CompositeToken(string value)
            : base(value, 0, new MdWrapSetting("", MdTagType.Block))
        {
        }

        public void AddToken(Token childToken)
        {
            childTokens.Add(childToken);
        }

        public override string Render()
        {
            var sb = new StringBuilder(Value);
            var correct = 0;
            foreach (var child in childTokens.OrderBy(t => t.Position))
            {
                sb.Remove(child.Position + WrapSetting.MdTag.Length, child.Value.Length);
                sb.Insert(child.Position + WrapSetting.MdTag.Length, child.Render());
                correct += child.WrapSetting.HtmlOpenTag.Length - child.WrapSetting.MdTag.Length;
            }

            var value = sb.ToString();
            var text = WrapSetting.TagType == MdTagType.Block
                ? value.Remove(0, WrapSetting.MdTag.Length)
                : value.Remove(value.Length - WrapSetting.MdTag.Length).Remove(0, WrapSetting.MdTag.Length);
            return WrapSetting.HtmlOpenTag + text + WrapSetting.HtmlCloseTag;
            ;
        }
    }
}