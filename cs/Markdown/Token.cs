using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Token
    {
        public Deque<Tuple<int, int>> StringBlocks = new Deque<Tuple<int, int>>();
        public Deque<Token> Value = new Deque<Token>();
        public int StartPosition;
        public int Length;
        public string Tag;
        public int ActualEnd;
        public int ActualStart;

        public string ConvertToHTMLTag(string tokensString)
        {
            if (!StringBlocks.Any())
                return string.IsNullOrWhiteSpace(Tag)
                    ? tokensString.Substring(StartPosition, Length)
                    : $"<{Tag}>{tokensString.Substring(StartPosition, Length)}</{Tag}>";

            var htmlString = new StringBuilder();
            while (StringBlocks.Any())
            {
                var (start, length) = StringBlocks.RemoveFirst();
                htmlString.Append(tokensString.Substring(start, length));
                if (!Value.Any()) continue;
                var innerToken = Value.RemoveFirst();
                htmlString.Append(innerToken.ConvertToHTMLTag(tokensString));
            }

            return string.IsNullOrWhiteSpace(Tag)
                ? htmlString.ToString()
                : $"<{Tag}>{htmlString}</{Tag}>";
        }
    }
}