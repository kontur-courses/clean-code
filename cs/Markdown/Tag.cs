using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Tag
    {
        public static List<Tag> AllTags
        {
            get
            {
                return new List<Tag>
                {
                    new Tag("__", "<strong>", true, TokenType.Strong),
                    new Tag("__", "</strong>", false, TokenType.Strong),
                    new Tag("# ", "<h1>", true, TokenType.Header),
                    new Tag("\n", "</h1>", false, TokenType.Header),
                    new Tag("_", "<em>", true, TokenType.Italic),
                    new Tag("_", "</em>", false, TokenType.Italic),
                    new Tag("", "", true, TokenType.Simple),
                    new Tag("", "", false, TokenType.Simple)
                };
            }
        }

        public bool IsOpening { get; }
        public string MdTag { get; }
        public string HtmlTag { get; }
        public TokenType TokenType { get; }

        public static Tag GetTagByTokenType(TokenType tokenType, bool isOpening)
        {
            return AllTags.FirstOrDefault(tag => tag.TokenType == tokenType && tag.IsOpening == isOpening);
        }

        public Tag(string mdTag, string htmlTag, bool isOpening, TokenType tokenType)
        {
            MdTag = mdTag;
            HtmlTag = htmlTag;
            TokenType = tokenType;
            IsOpening = isOpening;
        }

        public void ReplaceMdTagToHtmlTag(StringBuilder newLine, Token token)
        {
            if (token.Type != TokenType.Simple)
                newLine.Append(HtmlTag);
        }
    }
}
