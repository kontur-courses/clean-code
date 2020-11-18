using Markdown.Extentions;
using System.Collections.Generic;

namespace Markdown
{
    public class TokenReader
    {
        private static HashSet<string> formattingCharacters = new HashSet<string> { "_", "__", "#", "\\" };

        public IEnumerable<Token> ReadTokens(string text)
        {
            var pareserOperator = new ParserOperator();
            var splittedText = text.SplitKeepSeparators(new[] { '_', '#', '\\' });
            foreach (var bigram in splittedText.GetBigrams())
                pareserOperator.AddTokenPart(bigram);
            return pareserOperator.GetTokens();
        }

        public static bool IsFormattingString(string c)
        {
            return formattingCharacters.Contains(c);
        }
    }
}
