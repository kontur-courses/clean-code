using System.Linq;
using System.Text;

namespace Markdown
{
    public static class TokenApplier
    {
        public static string ApplyTokensToString(string sourceText, IToken[] tokens)
        {
            var resultText = new StringBuilder(sourceText);
            var influence = 0;
            foreach (var token in tokens.OrderBy(token => token.StartIndex))
                if (token is AttributeToken attributeToken)
                {
                    var openTag = $"<{token.TagInfo.TagForConverting} href=\"{attributeToken.Attribute}\">";
                    ApplyAttributeToken(attributeToken, resultText, openTag, influence);
                    influence += openTag.Length - token.TagInfo.OpenTagInMd.Length;
                }
                else if (token is TokenWithSingleTag)
                {
                    ApplyTokenWithSingleTag(resultText, token, influence);
                    influence += token.TagInfo.TagForConverting.Length + 2 - token.TagInfo.OpenTagInMd.Length;
                }
                else
                {
                    ApplyEmphasizingToken(resultText, token, influence);
                    influence += token.TagInfo.TagForConverting.Length + 2 - token.TagInfo.OpenTagInMd.Length;
                }


            return resultText.ToString();
        }

        private static void ApplyEmphasizingToken(StringBuilder resultText, IToken token,
            int influence)
        {
            var tagInMd = token.TagInfo.OpenTagInMd;
            var tagInMdLength = tagInMd.Length;
            resultText = resultText.Replace(tagInMd, $"<{token.TagInfo.TagForConverting}>",
                token.StartIndex + influence, tagInMdLength);
            influence += token.TagInfo.TagForConverting.Length + 2 - tagInMdLength;
            resultText = resultText.Replace(token.TagInfo.OpenTagInMd, $"</{token.TagInfo.TagForConverting}>",
                token.EndTagIndex + influence,
                tagInMdLength);
        }

        private static void ApplyTokenWithSingleTag(StringBuilder resultText, IToken token,
            int influence)
        {
            var tagInMd = token.TagInfo.OpenTagInMd;
            var tagInMdLength = tagInMd.Length;
            resultText = resultText.Replace(tagInMd, $"<{token.TagInfo.TagForConverting}>",
                token.StartIndex + influence, tagInMdLength);
            influence += token.TagInfo.TagForConverting.Length + 2 - tagInMdLength;
            resultText = resultText.Insert(token.EndTagIndex + influence, $"</{token.TagInfo.TagForConverting}>");
        }

        private static void ApplyAttributeToken(AttributeToken attributeToken, StringBuilder resultText, string openTag,
            int influence)
        {
            var tagInMd = attributeToken.TagInfo.OpenTagInMd;
            var tagInMdLength = tagInMd.Length;
            resultText = resultText.Replace($"{tagInMd}",
                openTag, attributeToken.StartIndex + influence, tagInMdLength);
            influence += openTag.Length - tagInMdLength;
            resultText = resultText.Replace($"]({attributeToken.Attribute})",
                $"</{attributeToken.TagInfo.TagForConverting}>",
                attributeToken.StartIndex + openTag.Length + attributeToken.Title.Length,
                attributeToken.Attribute.Length + 3);
        }
    }
}