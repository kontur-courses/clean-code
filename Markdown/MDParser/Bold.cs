using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace Markdown.MDParser
{
    public class Bold : ITokenizer
    {
        public Token? TryFindToken(string text, int idx)
        {
            var len = text.Length;
            if (idx + 2 == len)
                return new Token("__", TokenProperty.Normal);
            if (idx + 3 == len)
                return new Token(text[^3..], TokenProperty.Normal);
            if (idx + 4 == len)
                return new Token(text[^4..], TokenProperty.Normal);
            if (text[idx + 2] == ' ')
                return new Token("__", TokenProperty.Normal);
            var i = idx + 4;
            List<int> hiddens = [];
            while (i < len)
            {
                if (text[i] != '_')
                {
                    i++;
                    continue;
                }

                if (text[i - 1] != '_')
                {
                    i++;
                    continue;
                }

                if (text[i - 2] == ' ')
                {
                    i++;
                    continue;
                }

                if (text[i - 2] == '\\')
                {
                    hiddens.Add(i - 2);
                    i++;
                    continue;
                }
                return new Token(text[(idx + 2)..(i - 1)], TokenProperty.Bold);
            }
            var hiddenCount = hiddens.Count;
            if (hiddenCount != 0 && text[hiddens[hiddenCount - 1]] == '\\')
                return new Token(GetStringWithHiddens(text, hiddens), TokenProperty.Normal);
            return new Token(text[idx..i], TokenProperty.Normal);
        }

        private static string GetStringWithHiddens(string text, List<int> hiddens)
        {
            var result = new StringBuilder();
            var leftBorder = 0;
            foreach (var hidden in hiddens)
            {
                result.Append(text[leftBorder..hidden]);
                leftBorder = hidden + 1;
            }
            result.Append(text[(hiddens.Last() + 1)..]);
            return result.ToString();
        }
    }
}
