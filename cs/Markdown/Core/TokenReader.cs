using System.Collections.Generic;

namespace Markdown
{
    public class TokenReader
    {
        private static HashSet<string> formattingCharacters = new HashSet<string> { "_", "__", "#", "\\" };

        public IEnumerable<Token> ReadTokens(string text)
        {
            var pareserOperator = new ParserOperator();
            foreach (var part in text.SplitKeepSeparators(new[] { '_', '#', '\\' }))
                pareserOperator.AddTokenPart(part);
            return pareserOperator.GetTokens();
        }

        public static bool IsFormattingString(string c)
        {
            return formattingCharacters.Contains(c);
        }
    }
}
