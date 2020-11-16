using System.Collections.Generic;

namespace Markdown
{
    public class TextData
    {
        public List<TextToken> Tokens { get; private set;}
        public string Value { get; set; }

        public TextData(string value)
        {
            Value = value;
            Tokens = new List<TextToken>();
        }

        public void AddTokens(params TextToken[] nestedToken)
        {
            Tokens.AddRange(nestedToken);
        }
    }
}