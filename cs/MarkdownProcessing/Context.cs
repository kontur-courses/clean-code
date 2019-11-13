using System;
using System.Collections.Generic;
using System.Text;

namespace MarkdownProcessing
{
    public class Context
    {
        private readonly Dictionary<TokenType, string> HtmlOpeningTagsDictionary = new Dictionary<TokenType, string>
        {
            {TokenType.Bold, "<strong>"},
            {TokenType.PlainText, ""},
            {TokenType.Parent, "<p>"},
            {TokenType.Italic, "<em>"},
            {TokenType.Header1, "<h1>"}
        };

        private readonly Dictionary<TokenType, string> HtmlClosingTagsDictionary = new Dictionary<TokenType, string>
        {
            {TokenType.Bold, "</strong>"},
            {TokenType.PlainText, ""},
            {TokenType.Parent, "</p>"},
            {TokenType.Italic, "</em>"},
            {TokenType.Header1, "</h1>"}
        };

        private readonly string input;
        public readonly StringBuilder output = new StringBuilder();
        private readonly ComplicatedToken mainToken;
        private StringBuilder currentPossibleTag;
        private StringBuilder currentPossiblePhrase;
        private Stack<TokenType> allTokens;

        public Context(string input)
        {
            this.input = input;
            mainToken = new ComplicatedToken(TokenType.Parent);
            currentPossibleTag = new StringBuilder();
            currentPossiblePhrase = new StringBuilder();
            allTokens = new Stack<TokenType>();
        }

        public string ParseInputIntoTokens()
        {
            foreach (var symbol in input)
            {
            }

            return MakeHtmlFromMarkdown();
        }

        private string MakeHtmlFromMarkdown()
        {
            ConvertToken(mainToken);
            return output.ToString();
        }

        public void ConvertToken(Token token)
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
            output.Append(HtmlOpeningTagsDictionary[cToken.Type]);
            foreach (var specialToken in cToken.ChildTokens)
                ConvertToken(specialToken);
            output.Append(HtmlClosingTagsDictionary[cToken.Type]);
        }

        private void AddSimpleTokenToOutput(SimpleToken sToken)
        {
            output.Append(HtmlOpeningTagsDictionary[sToken.Type]);
            output.Append(sToken.innerText);
            output.Append(HtmlClosingTagsDictionary[sToken.Type]);
        }
    }
}