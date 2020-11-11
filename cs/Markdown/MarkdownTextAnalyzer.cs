using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public static class MarkdownTextAnalyzer
    {
        public static readonly Dictionary<string, Styles> MarkdownStyles = new Dictionary<string, Styles>
        {
            {"_", Styles.Italic}, {"__", Styles.Bold}, {"#", Styles.Title}
        };

        public static IEnumerable<Token> GetTokens(string text)
        {
            var result = new List<Token>();
            if (IsItTitle(text))
                result.Add(new Token(text.Substring(1), Styles.Title, "#", 1, text.Length));
            result.AddRange(GetBoldAndItalicTokens(text));

            return result;
        }

        private static IEnumerable<Token> GetBoldAndItalicTokens(string text)
        {
            var separatorsWithPositions = new Stack<Separator>();
            var result = new List<Token>();
            for (var i = 0; i < text.Length; i++)
                if (text[i] == '_')
                {
                    var currentSeparator = i < text.Length - 1 && text[i + 1] == '_'
                        ? new Separator("__", i)
                        : new Separator("_", i);
                    var separatorLength = currentSeparator.Value.Length;
                    if (separatorsWithPositions.Any() && separatorsWithPositions.Peek().Value == currentSeparator.Value)
                    {
                        var lastSeparatorWithPosition = separatorsWithPositions.Pop();
                        var value = text.Substring(
                            lastSeparatorWithPosition.PositionInText + separatorLength,
                            i - lastSeparatorWithPosition.PositionInText - separatorLength);
                        var token = new Token(value, MarkdownStyles[currentSeparator.Value], currentSeparator.Value,
                            lastSeparatorWithPosition.PositionInText, i);
                        var boldIsInsideItalic = separatorsWithPositions.Any() && token.Separator == "__" &&
                                                 separatorsWithPositions.Peek().Value == "_";
                        if (IsItalicOrBoldTokenCorrect(token, lastSeparatorWithPosition, currentSeparator, text) &&
                            !boldIsInsideItalic)
                            result.Add(token);
                    }
                    else
                    {
                        separatorsWithPositions.Push(currentSeparator);
                    }

                    i += separatorLength - 1;
                }
                else if (text[i] == '\\')
                {
                    i++;
                }

            return result;
        }

        private static bool IsSeparatorInsideWord(Separator separator, string text)
        {
            return separator.PositionInText > 0 && char.IsLetter(text[separator.PositionInText - 1]) &&
                   separator.PositionInText + separator.Value.Length < text.Length &&
                   char.IsLetter(text[separator.PositionInText + separator.Value.Length]);
        }

        private static bool IsItalicOrBoldTokenCorrect(Token token, Separator startSeparator, Separator endSeparator, string text)
        {
            if (token.Value == "")
                return false;
            if ((IsSeparatorInsideWord(startSeparator, text) || IsSeparatorInsideWord(endSeparator, text)) &&
                token.Value.Contains(' '))
                return false;
            if (token.Value[0] == ' ' || token.Value.Last() == ' ')
                return false;
            var allSymbolsInTokenValueAreDigits = true;
            foreach (var symbol in token.Value)
                if (!char.IsDigit(symbol))
                    allSymbolsInTokenValueAreDigits = false;
            return !allSymbolsInTokenValueAreDigits;
        }

        private static bool IsItTitle(string paragraph)
        {
            return paragraph[0] == '#' && paragraph.Length > 1;
        }
    }
}