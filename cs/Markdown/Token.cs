using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Token
    {
        public Queue<Tuple<int, int>> StringBlocks = new Queue<Tuple<int, int>>();
        public Queue<Token> Value = new Queue<Token>();
        public int StartPosition;
        public int Length;
        public string Tag;
        
        public string ConvertToHTMLTag(string tokensString)
        {
            if (!StringBlocks.Any())
                return string.IsNullOrWhiteSpace(Tag) 
                    ? tokensString.Substring(StartPosition, Length) 
                    : $"<{Tag}>{tokensString.Substring(StartPosition, Length)}</{Tag}>";
            
            var htmlString = new StringBuilder();
            while (StringBlocks.Any())
            {
                var (start, length) = StringBlocks.Dequeue();
                htmlString.Append(tokensString.Substring(start, length));
                if (!Value.Any()) continue;
                var innerToken = Value.Dequeue();
                htmlString.Append(innerToken.ConvertToHTMLTag(tokensString));
            }

            return $"<{Tag}>{htmlString}</{Tag}>";
        }
    }
}