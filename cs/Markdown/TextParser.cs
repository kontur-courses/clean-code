using System.Collections.Generic;

namespace Markdown
{
    internal static class TextParser
    {
        public static TextProcessor For(string text) =>
            new TextProcessor(text,
                              rules: new List<ITextProcessorRule>
                              {
                                  new PairedTagRule('_', 1), new PairedTagRule('_', 2), new PairedTagRule('_', 3),
                                  new PairedTagRule('*', 1), new PairedTagRule('*', 2), new PairedTagRule('*', 3)
                              });

        public static IEnumerable<Token> Parse(this TextProcessor textProcessor) =>
            textProcessor.GetDelimiterPositions()
                         .RemoveEscapedDelimiters()
                         .RemoveNonValidDelimiters()
                         .ValidatePairs()
                         .GetTokensFromDelimiters();
    }
}
