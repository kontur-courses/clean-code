using System.Text;
using MarkdownProcessing.Tags;
using MarkdownProcessing.Tokens;

namespace MarkdownProcessing.Converters
{
    public class TokenToHtmlConverter
    {
        private readonly Token mainToken;
        private readonly StringBuilder output;

        public TokenToHtmlConverter(Token mainToken)
        {
            this.mainToken = mainToken;
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
            output.Append(HtmlTags.HtmlOpeningTagsDictionary[cToken.Type]);
            foreach (var specialToken in cToken.ChildTokens)
                ConvertToken(specialToken);
            output.Append(HtmlTags.HtmlClosingTagsDictionary[cToken.Type]);
        }

        private void AddSimpleTokenToOutput(SimpleToken sToken)
        {
            output.Append(HtmlTags.HtmlOpeningTagsDictionary[sToken.Type]);
            output.Append(sToken.InnerText);
            output.Append(HtmlTags.HtmlClosingTagsDictionary[sToken.Type]);
        }
    }
}