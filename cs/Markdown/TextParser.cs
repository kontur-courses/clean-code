namespace Markdown
{
    using System.Collections.Generic;

    internal static class TextParser
    {
        public static TextProcessor For(string text) =>
            new TextProcessor(
                              text,
                              rules: new List<ITextProcessorRule>
                                         {
                                             new PairedTagRule('_', 1), new PairedTagRule('_', 2),
                                             new PairedTagRule('*', 1), new PairedTagRule('*', 2)
                                         });

        public static IEnumerable<Token> Parse(this TextProcessor textProcessor) =>
            textProcessor.GetDelimiterPositions()
                         .RemoveEscapedDelimiters()
                         .RemoveNonValidDelimiters()
                         .ValidatePairs()
                         .GetTokensFromDelimiters();
    }
}
