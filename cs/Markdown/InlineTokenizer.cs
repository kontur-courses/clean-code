using Markdown.Tokens;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Markdown
{
    public class InlineTokenizer
    {
        public static Token[] Tokenize(string text)
        {
            if (string.IsNullOrEmpty(text))
                return Array.Empty<Token>();

            var tokens = new List<Token>();
            var textStart = -1;

            var i = 0;
            var n = text.Length;

            void FlushText(int endText)
            {
                if (textStart < 0) return;
                tokens.Add(new TextToken(text.Substring(textStart, endText - textStart)));
                textStart = -1;
            }

            while (i < n)
            {
                var c = text[i];
                if (c == '\\' && i + 1 < n && IsEscapable(text[i + 1]))
                {
                    FlushText(i);
                    tokens.Add(new EscapedToken(text[i + 1]));
                    i += 2;
                }
                else if (c == '_')
                {
                    var j = i;
                    while (j < n && text[j] == '_') j++;
                    var runLength = j - i;

                    if (runLength > 2)
                    {
                        if (textStart == -1) textStart = i;
                        i = j;
                        continue;
                    }

                    char? left = i > 0 ? text[i - 1] : (char?)null;
                    char? right = j < n ? text[j] : (char?)null;

                    var leftEscaped = i > 1 && text[i - 2] == '\\' && IsEscapable(text[i - 1]);
                    var rightEscaped = j < n - 1 && text[j] == '\\' && IsEscapable(text[j + 1]);

                    var leftIsSpace = !left.HasValue || char.IsWhiteSpace(left.Value);
                    var rightIsSpace = !right.HasValue || char.IsWhiteSpace(right.Value);
                    var bothNonSpace = !leftIsSpace && !rightIsSpace;
                    var hasDigit = (left.HasValue && char.IsDigit(left.Value))
                                 || (right.HasValue && char.IsDigit(right.Value));

                    if (bothNonSpace && hasDigit)
                    {
                        if (textStart == -1) textStart = i;
                        i = j;
                        continue;
                    }

                    FlushText(i);
                    tokens.Add(new UnderscoreToken(runLength, left, right, leftEscaped, rightEscaped));
                    i = j;
                }
                else if (c == '[')
                {
                    FlushText(i);
                    tokens.Add(new BracketOpenToken());
                    i++;
                }
                else if (c == ']' && i + 1 < n && text[i + 1] == '(')
                {
                    FlushText(i);
                    var j = i + 2;
                    while (j < n && text[j] != ')') j++;

                    if (j < n)
                    {
                        tokens.Add(new LinkTailToken(text.Substring(i + 2, j - i - 2), true));
                        i = j + 1;
                    }
                    else
                    {
                        tokens.Add(new LinkTailToken(text.Substring(i + 2), false));
                        i = n;
                    }
                }
                else
                {
                    if (textStart == -1) textStart = i;
                    i++;
                }
            }

            FlushText(n);
            return tokens.ToArray();
        }

        private static bool IsEscapable(char c)
            => c == '_' || c == '\\' || c == '[' || c == ']' || c == '(' || c == ')';
    }
}
