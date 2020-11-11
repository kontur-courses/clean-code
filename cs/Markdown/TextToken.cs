using System.Collections.Generic;

namespace Markdown
{
    public class TextToken
    {
        public FormattingState TokenState { get; private set; }
        public string Value { get; private set; }
        
        private List<TextToken> subTokens = new List<TextToken>();

        public TextToken(string value)
        {
            Value = value;
        }
    }
}