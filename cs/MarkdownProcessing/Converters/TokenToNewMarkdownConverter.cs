using System.Text;
using MarkdownProcessing.Markdowns;
using MarkdownProcessing.Tokens;

namespace MarkdownProcessing.Converters
{
    public class TokenToNewMarkdownConverter
    {
        private readonly Token mainToken;
        private readonly StringBuilder output;
        private readonly IResultMarkdown markdown;

        public TokenToNewMarkdownConverter(Token mainToken, IResultMarkdown markdown)
        {
            this.mainToken = mainToken;
            this.markdown = markdown;
            output = new StringBuilder();
        }

        public string ConvertToHtml()
        {
            ConvertToken(mainToken);
            return output.ToString();
        }

        private void ConvertToken(Token token)
        {
            switch (token)
            {
                case ComplicatedToken cToken:
                {
                    AddComplicatedTokenToOutput(cToken);
                    break;
                }
                case SimpleToken sToken:
                    AddSimpleTokenToOutput(sToken);
                    break;
            }
        }

        private void AddComplicatedTokenToOutput(ComplicatedToken cToken)
        {
            output.Append(markdown.OpeningTags[cToken.Type]);
            foreach (var specialToken in cToken.ChildTokens)
                ConvertToken(specialToken);
            output.Append(markdown.ClosingTags[cToken.Type]);
        }

        private void AddSimpleTokenToOutput(SimpleToken sToken)
        {
            output.Append(markdown.OpeningTags[sToken.Type]);
            output.Append(sToken.InnerText);
            output.Append(markdown.ClosingTags[sToken.Type]);
        }
    }
}