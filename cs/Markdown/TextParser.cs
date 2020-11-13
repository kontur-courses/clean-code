#nullable enable
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class TextParser : ITextParser
    {
        private Dictionary<int, bool> EscapingBackslashes { get; set; }

        public List<Token> GetTokens(string text)
        {
            var tokens = new List<Token>();
            tokens.AddRange(FindTokens(text));

            return tokens;
        }

        private static bool IsDigitOrWhiteSpace(char symbol)
        {
            return char.IsDigit(symbol) || char.IsWhiteSpace(symbol);
        }

        private bool IsAfterBackslash(int index)
        {
            return index != 0
                   && EscapingBackslashes.ContainsKey(index - 1)
                   && EscapingBackslashes[index - 1];
        }

        private bool IsEmphasizedStartTag(string text, int index)
        {
            return text[index] == '_'
                   && index + 1 < text.Length
                   && text[index + 1] != '_'
                   && !IsDigitOrWhiteSpace(text[index + 1])
                   && !IsAfterBackslash(index);
        }

        private bool IsEmphasizedEndTag(string text, int index)
        {
            return text[index] == '_'
                   && !IsDigitOrWhiteSpace(text[index - 1])
                   && text[index - 1] != '_'
                   && (index + 1 == text.Length || text[index + 1] != '_')
                   && !IsAfterBackslash(index);
        }

        private bool IsStrongStartTag(string text, int index)
        {
            return text[index] == '_'
                   && index + 2 < text.Length
                   && text[index + 1] == '_'
                   && !IsDigitOrWhiteSpace(text[index + 2])
                   && !IsAfterBackslash(index);
        }

        private bool IsStrongEndTag(string text, int index)
        {
            return text[index] == '_'
                   && !IsDigitOrWhiteSpace(text[index - 1])
                   && text[index - 1] != '_'
                   && index + 1 < text.Length && text[index + 1] == '_'
                   && !IsAfterBackslash(index);
        }

        public List<Token> FindTokens(string text)
        {
            EscapingBackslashes = FindEscapingBackslashes(text);
            var tokens = new List<Token>();
            var startIndex = 0;

            for (var i = 0; i < text.Length; ++i)
            {
                Token? token = null;

                if (IsEmphasizedStartTag(text, i))
                {
                    token = FindEmphasizedTag(i, text);
                }
                else if (IsStrongStartTag(text, i))
                {
                    token = FindStrongTag(i, text);

                    if (token.Value == "")
                        i++;
                }
                else if (text[i] == '#' && i == 0)
                {
                    token = FindHeading(i, text);
                }

                if (token != null && token.Value != "")
                {
                    var value = text.Substring(startIndex, i - startIndex);
                    tokens.Add(new Token(startIndex, value, TokenType.PlainText));

                    i = token.Position + token.Length;
                    startIndex = i;
                    tokens.Add(token);
                }

                if (i + 1 == text.Length)
                {
                    var value = text.Substring(startIndex, i - startIndex + 1);
                    tokens.Add(new Token(startIndex, value, TokenType.PlainText));
                }
            }

            return tokens.Where(x => x.Value != "").ToList();
        }

        public Token FindEmphasizedTag(int index, string text)
        {
            var value = "";
            Token? intersectedToken = null;

            for (var i = index + 1; i < text.Length; i++)
            {
                if (IsStrongStartTag(text, i))
                {
                    var token = FindEmphasizedTag(i, text);

                    if (token.Value != "")
                    {
                        intersectedToken = token;
                    }
                }

                if (!IsEmphasizedEndTag(text, i))
                    continue;

                if (intersectedToken != null && intersectedToken.Length > i)
                    break;

                value = text.Substring(index, i - index + 1);
                break;
            }

            return new Token(index, value, TokenType.Emphasized);
        }

        public Token FindStrongTag(int index, string text)
        {
            var value = "";
            Token? intersectedToken = null;

            for (var i = index + 2; i < text.Length; i++)
            {
                if (IsEmphasizedStartTag(text, i))
                {
                    var token = FindEmphasizedTag(i, text);

                    if (token.Value != "")
                    {
                        intersectedToken = token;
                    }
                }

                if (!IsStrongEndTag(text, i))
                    continue;

                if (intersectedToken != null && intersectedToken.Length > i)
                    break;

                value = text.Substring(index, i - index + 2);
                break;
            }

            return new Token(index, value, TokenType.Strong);
        }

        public Token FindHeading(int index, string text)
        {
            var value = text.Substring(index, text.Length);
            return new Token(index, value, TokenType.Heading);
        }

        public Dictionary<int, bool> FindEscapingBackslashes(string text)
        {
            var dictionary = new Dictionary<int, bool>();

            for (var i = 0; i < text.Length; ++i)
            {
                if (text[i] == '\\' && i + 1 < text.Length && text[i + 1] == '\\')
                {
                    dictionary[i + 1] = false;
                    dictionary[i] = false;
                    i++;
                }
                else if (text[i] == '\\' && i + 1 < text.Length && text[i + 1] != '\\')
                {
                    dictionary[i] = true;
                }
            }

            return dictionary;
        }
    }
}