using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class Md
    {
        public string Render(string markdownText)
        {
            var escapeCharacter = '\\';
            var escapedCharacters = new List<char>();
            var textWithReplacedEscapedCharacters = ReplaceEscapedCharacters(markdownText, escapeCharacter, escapedCharacters);
            var boldFontTokenReader = new TokenReader("__", "__");
            boldFontTokenReader.AddRuleForTokenContent(IsCorrectTokenContent);
            var tokensSplittedByDoubleUnderscores = boldFontTokenReader.SplitToTokens(textWithReplacedEscapedCharacters);
            RenderTextInsideTokensSplittedByDoubleUnderscores(tokensSplittedByDoubleUnderscores, escapeCharacter);
            var textWithSurroundingTags = AddSurroundingTags(tokensSplittedByDoubleUnderscores, "<strong>", "</strong>");
            return RemoveEscapeCharacters(textWithSurroundingTags, escapeCharacter, escapedCharacters);
        }

        public void RenderTextInsideTokensSplittedByDoubleUnderscores(List<Token> tokensSplittedByDoubleUnderscores, char escapeCharacter)
        {
            var italicsFontTokenReader = new TokenReader("_", "_");
            italicsFontTokenReader.AddRuleForTokenContent(IsCorrectTokenContent);
            var textDifference = 0;
            for (var i = 0; i < tokensSplittedByDoubleUnderscores.Count; i++)
            {
                var token = tokensSplittedByDoubleUnderscores[i];
                var tokensSplittedByUnderscores = italicsFontTokenReader.SplitToTokens(token.Text);
                var newTokenText = AddSurroundingTags(tokensSplittedByUnderscores, "<em>", "</em>");
                textDifference += newTokenText.Length - token.Length;
                var newToken = new Token(token.Position + textDifference, newTokenText, token.IsInterior);
                tokensSplittedByDoubleUnderscores[i] = newToken;
            }
        }

        public string AddSurroundingTags(List<Token> tokens, string openingTag, string closingTag)
        {
            var builder = new StringBuilder();
            foreach (var token in tokens)
            {
                if (token.IsInterior)
                    builder.Append(openingTag);
                builder.Append(token.Text);
                if (token.IsInterior)
                    builder.Append(closingTag);
            }

            return builder.ToString();
        }

        private bool IsCorrectTokenContent(string tokenContent)
        {
            if (tokenContent.Length == 0)
                return false;
            if (string.IsNullOrWhiteSpace(tokenContent[0].ToString()) || string.IsNullOrWhiteSpace(tokenContent[tokenContent.Length - 1].ToString()))
                return false;
            return true;
        }

        public string RemoveEscapeCharacters(string text, char escapeCharacter, List<char> escapedCharacters)
        {
            var builder = new StringBuilder();
            var numberOfRemovedEscapeCharacters = 0;
            for (var i = 0; i < text.Length; i++)
            {
                if (text[i] == escapeCharacter)
                {
                    i++;
                    builder.Append(escapedCharacters[numberOfRemovedEscapeCharacters]);
                    numberOfRemovedEscapeCharacters++;
                }
                else
                    builder.Append(text[i]);
            }

            return builder.ToString();
        }

        public string ReplaceEscapedCharacters(string text, char escapeCharacter, List<char> escapedCharacters)
        {
            var charArray = text.ToCharArray();
            for (var i = 0; i < charArray.Length; i++)
            {
                if (charArray[i] == escapeCharacter)
                {
                    i++;
                    escapedCharacters.Add(charArray[i]);
                    charArray[i] = '\\';
                }
            }

            return new string(charArray);
        }
    }
}
