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
            foreach (var token in tokens.OrderByDescending(token => token.StartIndex))
            {
                var tagInMdLength = token.TagInfo.TagInMd.Length;
                resultText = resultText.Replace(token.TagInfo.TagInMd, $"<{token.TagInfo.TagForConverting}>", token.StartIndex + influence, 1);
                influence += token.TagInfo.TagForConverting.Length + 2 - tagInMdLength;
                resultText = resultText.Replace(token.TagInfo.TagInMd, $"</{token.TagInfo.TagForConverting}>", token.StartIndex + token.Length + influence - 1,
                    1);
                influence += token.TagInfo.TagForConverting.Length + 3 - tagInMdLength;
            }

            return resultText.ToString();
        }
    }
}