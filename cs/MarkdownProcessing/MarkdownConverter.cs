using System.Collections.Generic;
using System.Text;

namespace MarkdownProcessing
{
    public class MarkdownConverter
    {
        private readonly Dictionary<TokenType, string> htmlOpeningTagsDictionary = new Dictionary<TokenType, string>
        {
            {TokenType.Bold, "<strong>"},
            {TokenType.PlainText, ""},
            {TokenType.Parent, "<p>"},
            {TokenType.Italic, "<em>"},
            {TokenType.Header1, "<h1>"}
        };

        private readonly Dictionary<TokenType, string> htmlClosingTagsDictionary = new Dictionary<TokenType, string>
        {
            {TokenType.Bold, "</strong>"},
            {TokenType.PlainText, ""},
            {TokenType.Parent, "</p>"},
            {TokenType.Italic, "</em>"},
            {TokenType.Header1, "</h1>"}
        };

        private readonly Token mainToken;
        private readonly StringBuilder output;

        public MarkdownConverter(Token mainToken)
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
            output.Append(htmlOpeningTagsDictionary[cToken.Type]);
            foreach (var specialToken in cToken.ChildTokens)
                ConvertToken(specialToken);
            output.Append(htmlClosingTagsDictionary[cToken.Type]);
        }

        private void AddSimpleTokenToOutput(SimpleToken sToken)
        {
            output.Append(htmlOpeningTagsDictionary[sToken.Type]);
            output.Append(sToken.innerText);
            output.Append(htmlClosingTagsDictionary[sToken.Type]);
        }
    }
}