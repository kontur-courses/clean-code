using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.TokenInfo;

namespace Markdown
{
    public static class Converter
    {
        private static readonly Dictionary<TagType, ITokenInfo> _tokenInfos = new Dictionary<TagType, ITokenInfo>
        {
            {TagType.Bold, new BoldTokenInfo()},
            {TagType.Heading, new HeadingTokenInfo()},
            {TagType.Italics, new ItalicsTokenInfo()},
            {TagType.Text, new TextTokenInfo()},
            {TagType.EntireText, new EntireTextTokenInfo()}
        };

        private static readonly HtmlTagCreator _htmlTagCreator = new HtmlTagCreator();

        public static string TokensToHtml(string text, IEnumerable<Token> tokens)
        {
            var sb = new StringBuilder();
            foreach (var token in tokens)
            {
                sb.Append(GetTokenValue(token, text));
            }

            return sb.ToString();
        }

        private static string GetTokenValue(Token token, string text)
        {
            var sb = new StringBuilder();
            var tokenInfo = _tokenInfos[token.TagType];
            foreach (var i in token.NestedTokens)
                sb.Append(GetTokenValue(i, text));
            sb.Append(CreateHtmlTag(tokenInfo.TagType,
                text[tokenInfo.GetValueStartIndex(token.Start)..tokenInfo.GetValueFinishIndex(token.Finish)]));
            return sb.ToString();
        }

        private static string CreateHtmlTag(TagType type, string text)
        {
            return type switch
            {
                TagType.Heading => _htmlTagCreator.CreateHeading(text.Trim('\n', '\r')),
                TagType.Bold => _htmlTagCreator.CreateBold(text),
                TagType.Italics => _htmlTagCreator.CreateItalics(text),
                TagType.Text => text,
                TagType.EntireText => "",
                _ => throw new NotImplementedException()
            };
        }
    }
}
