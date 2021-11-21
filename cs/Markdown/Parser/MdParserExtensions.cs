using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Parser
{
    public static class MdParserExtensions
    {
        public static void Visit(this MdParser parser, HeaderToken token)
        {
            if (token.OpenIndex != 0 && parser.TextToParse[token.OpenIndex - 1] != '\n')
                return;

            var closeIndex = parser.ParserContext.TextToParse.IndexOf('\n', token.OpenIndex);

            if (closeIndex == -1)
                closeIndex = parser.TextToParse.Length;

            token.Close(closeIndex);
            parser.ParserContext.Result.Add(token);
        }

        public static void Visit(this MdParser parser, BoldToken token)
        {
            token.ValidatePlacedCorrectly(parser.ParserContext.TextToParse);

            ValidateBoldTokenInteractions(parser.ParserContext.Tokens, token);

            parser.ParserContext.Tokens.Remove(token.GetSeparator());

            if (token.IsCorrect)
                parser.ParserContext.Result.Add(token);
        }

        public static void Visit(this MdParser parser, ItalicToken token)
        {
            token.ValidatePlacedCorrectly(parser.ParserContext.TextToParse);

            ValidateItalicTokenInteractions(parser.ParserContext.Tokens, token);

            parser.ParserContext.Tokens.Remove(token.GetSeparator());

            if (token.IsCorrect)
                parser.ParserContext.Result.Add(token);
        }

        public static void Visit(this MdParser parser, ScreeningToken token)
        {
            token.Close(token.OpenIndex);
            parser.ParserContext.Tokens.Add(token.GetSeparator(), token);
        }

        public static void Visit(this MdParser parser, ImageToken token)
        {
            var text = parser.ParserContext.TextToParse;
            var endOfAltText = text.IndexOf(']', token.OpenIndex);
            var startOfSource = text.IndexOf('(', token.OpenIndex);
            var endOfSource = text.IndexOf(')', token.OpenIndex);
            var endOfParagraph = text.IndexOf('\n') > 0 ? text.IndexOf('\n') : text.Length - 1;

            if (endOfAltText == -1 || startOfSource == -1 || endOfSource == -1)
                return;

            if (endOfAltText > endOfParagraph || startOfSource > endOfParagraph || endOfSource > endOfParagraph)
                return;

            var altText = text.Substring(token.OpenIndex + token.GetSeparator().Length, endOfAltText - token.OpenIndex - token.GetSeparator().Length);
            token.AltText = altText;
            var source = text.Substring(startOfSource + 1, endOfSource - startOfSource - 1);
            token.Source = source;
            var screeningToken = new ScreeningToken(token.OpenIndex, endOfSource);
            parser.ParserContext.Tokens[ScreeningToken.Separator] = screeningToken;

            token.Close(endOfSource);
            parser.ParserContext.Result.Add(token);
        }

        private static void ValidateItalicTokenInteractions(IReadOnlyDictionary<string, Token> tokens, ItalicToken token)
        {
            if (!tokens.TryGetValue(BoldToken.Separator, out var boldToken)) return;
            if (!token.IsIntersectWith(boldToken)) return;

            boldToken.IsCorrect = false;
            token.IsCorrect = false;
        }

        private static void ValidateBoldTokenInteractions(IReadOnlyDictionary<string, Token> tokens, BoldToken token)
        {
            if (!token.IsCorrect || !tokens.TryGetValue(ItalicToken.Separator, out var italicToken)) return;

            if (token.IsIntersectWith(italicToken))
            {
                italicToken.IsCorrect = false;
                token.IsCorrect = false;
            }

            if (italicToken.OpenIndex < token.OpenIndex && italicToken.IsOpened)
                token.IsCorrect = false;
        }
    }
}