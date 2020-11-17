using System.Collections.Generic;
using Markdown.Builder;

namespace Markdown
{
    public class TextData
    {
        public List<TextToken> Tokens { get; private set;}
        public Dictionary<int, ReplacingData> ToRemove { get; private set;}
        public string Value { get; set; }

        public TextData(string value)
        {
            Value = value;
            Tokens = new List<TextToken>();
            ToRemove = new Dictionary<int, ReplacingData>();
        }

        public void AddTokens(params TextToken[] nestedToken)
        {
            Tokens.AddRange(nestedToken);
        }
        
        public void AddDataToRemove(string dataToRemove, int position)
        {
            ToRemove[position] = new ReplacingData(dataToRemove, "");
        }
    }
}