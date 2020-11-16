using System.Linq;
using System.Text;

namespace Markdown
{
    public static class TokenApplier
    {
        public static string ApplyTokensToString(string sourceText, Token[] tokens)
        {
            var resultText = new StringBuilder(sourceText);
            var influence = 0;
            foreach (var token in tokens.OrderBy(token => token.StartIndex))
            {
                var tagInMdLength = token.TagInfo.TagInMd.Length;
                resultText = resultText.Replace(token.TagInfo.TagInMd, $"<{token.TagInfo.TagForConverting}>",
                    token.StartIndex + influence, tagInMdLength);
                influence += token.TagInfo.TagForConverting.Length + 2 - tagInMdLength;
                resultText = token.TagInfo.IsSingle
                    ? resultText.Insert(token.EndTagIndex + influence, $"</{token.TagInfo.TagForConverting}>")
                    : resultText.Replace(token.TagInfo.TagInMd, $"</{token.TagInfo.TagForConverting}>",
                        token.EndTagIndex + influence,
                        tagInMdLength);
            }


            return resultText.ToString();
        }
    }
}