using System.Collections.Generic;

namespace Markdown
{
    public class TextToken
    {
        public FormattingState TokenState { get; set; }
        public string Value { get; }
        
        private List<TextToken> subTokens = new List<TextToken>();

        public TextToken(string value)
        {
            TokenState = FormattingState.NoFormatting;
            Value = value;
        }
    }
}