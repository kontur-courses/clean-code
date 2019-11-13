using System;
using System.Collections.Generic;
using System.Text;

namespace MarkdownProcessing
{
    public class TokenParser
    {
        private readonly string input;
        private readonly ComplicatedToken mainToken;
        private StringBuilder currentPossibleTag;
        private StringBuilder currentPossiblePhrase;
        private Stack<Token> allTokens;

        public TokenParser(string input)
        {
            this.input = input;
            mainToken = new ComplicatedToken(TokenType.Parent);
            currentPossibleTag = new StringBuilder();
            currentPossiblePhrase = new StringBuilder();
            allTokens = new Stack<Token>();
        }

        public string ParseInputIntoTokens()
        {
            foreach (var symbol in input)
            {
                //...
            }

            return MakeHtmlFromMarkdown();
        }

        private void CheckForOpeningTag()
        {
        }

        private void CheckForClosingTag()
        {
        }

        private void RefreshTagAndPhraseBuffers()
        {
            currentPossibleTag = new StringBuilder();
            currentPossiblePhrase = new StringBuilder();
        }

        private string MakeHtmlFromMarkdown()
        {
            return new MarkdownConverter(mainToken).ConvertToHtml();
        }
    }
}