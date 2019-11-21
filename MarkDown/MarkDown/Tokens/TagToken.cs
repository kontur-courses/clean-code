using MarkDown.TagParsers;
using System.Collections.Generic;

namespace MarkDown.Tokens
{
    public class TagToken : MdToken
    {
        public Tag Tag { get; }
        public override string Value => Tag.MdTag;
        public override int Length => Tag.MdTag.Length;
        public override TokenTypes TokenType { get; protected set; } = TokenTypes.Tag;

        public TagToken(Tag tag)
        {
            Tag = tag;
        }

        public void ToStringToken()
        {
            TokenType = TokenTypes.String;
        }

        public override bool Equals(object obj)
        {
            return obj is TagToken token &&
                   EqualityComparer<Tag>.Default.Equals(Tag, token.Tag);
        }

        public override int GetHashCode()
        {
            return Tag.GetHashCode();
        }
    }
}