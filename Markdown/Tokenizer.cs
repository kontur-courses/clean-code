using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    internal sealed class Tokenizer
    {
        public List<Token> Tokenize(string s)
        {
            var result = new List<Token>();

            int i = 0;
            int len = s.Length;

            while (i < len)
            {
                char c = s[i];

                switch (c)
                {
                    case '_':
                        if (i + 1 < len && s[i + 1] == '_')
                        {
                            result.Add(new Token(TokenType.DoubleUnderscore, false, false, "__"));
                            i += 2;
                        }
                        else
                        {
                            result.Add(new Token(TokenType.Underscore, false, false, "_"));
                            i += 1;
                        }
                        break;

                    case '\\':
                        result.Add(new Token(TokenType.Backslash, false, false, "\\"));
                        i++;
                        break;

                    case '\n':
                        result.Add(new Token(TokenType.NewLine, false, false, "\n"));
                        i++;
                        break;

                    case '#':
                        result.Add(new Token(TokenType.Grid, false, false, "#"));
                        i++;
                        break;

                    default:
                        if (char.IsDigit(c))
                        {
                            result.Add(new Token(TokenType.Digit, false, false, c.ToString()));
                            i++;
                        }
                        else if (char.IsWhiteSpace(c))
                        { 
                            result.Add(new Token(TokenType.Whitespace, false, false, " "));
                            i++;
                        }
                        else
                        {
                            var sb = new StringBuilder();
                            while (i < len)
                            {
                                char ch = s[i];
                                if (ch == '_' || ch == '\\' || ch == '\n' || ch == '\r' || ch == '#' ||
                                    char.IsWhiteSpace(ch) || char.IsDigit(ch))
                                    break;
                                sb.Append(ch);
                                i++;
                            }
                            if (sb.Length > 0)
                                result.Add(new Token(TokenType.Text, false, false, sb.ToString()));
                        }
                        break;
                }
            }

            ComputeOpenCloseFlags(result);

            return result;
        }

        private void ComputeOpenCloseFlags(List<Token> tokens)
        {
            for (int i = 0; i < tokens.Count; i++)
            {
                var t = tokens[i];
                if (t.Type != TokenType.Underscore && t.Type != TokenType.DoubleUnderscore)
                    continue;

                bool nextIsWhitespace = (i + 1 < tokens.Count) && tokens[i + 1].Type == TokenType.Whitespace;
                bool prevIsWhitespace = (i - 1 >= 0) && tokens[i - 1].Type == TokenType.Whitespace;

                bool nextIsDigit = (i + 1 < tokens.Count) && tokens[i + 1].Type == TokenType.Digit;
                bool prevIsDigit = (i - 1 >= 0) && tokens[i - 1].Type == TokenType.Digit;

                t.CanOpen = (i + 1 < tokens.Count) && !nextIsWhitespace && !nextIsDigit;

                t.CanClose = (i - 1 >= 0) && !prevIsWhitespace && !prevIsDigit;
            }
        }
    }
}
