using System.Collections.Generic;

namespace Markdown
{
    internal static class TextParser
    {
        public static TextProcessor For(string text) => new TextProcessor(text);

        public static IEnumerable<Token> Parse(this TextProcessor textProcessor) =>
            textProcessor.GetDelimiterPositions()
                         .RemoveEscapedDelimiters()
                         .RemoveNonValidDelimiters()
                         .ValidatePairs()
                         .GetTokensFromDelimiters();
    }
}
