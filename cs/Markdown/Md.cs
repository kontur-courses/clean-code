using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class Md
    {
        public string Render(string[] text)
        {
            throw new NotImplementedException();
        }
    }

    internal static class MarkdownParser
    {
        public static Token ReadItalicToken(string line, int startIndex)
        {
            if (!IsItalicTokenStarting(line, startIndex))
            {
                return Token.Empty();
            }
            for (var i = startIndex + 1; i < line.Length; i++)
            {
                if (line[i] == '_' && line[i - 1] != ' ')
                {
                    var lengthWithoutBorders = i - startIndex - 1;
                    return new Token(lengthWithoutBorders + 2, startIndex, TokenType.Italic,
                        line.Substring(startIndex + 1, lengthWithoutBorders));
                }
            }
            return Token.Empty();
        }

        private static bool IsItalicTokenStarting(string line, int startIndex)
        {
            return line[startIndex] == '_'
                   && startIndex != line.Length - 1
                   && line[startIndex + 1] != '_'
                   && !Char.IsDigit(line[startIndex + 1])
                   && line[startIndex + 1] != ' ';
        }
        
        public static Token ReadBoldToken(string line, int startIndex)
        {
            if (!IsBoldTokenStart(line, startIndex))
            {
                return Token.Empty();
            }

            var inserted = new List<Token>();

            for (var i = startIndex + 2; i < line.Length - 1; i++)
            {
                if (IsBoldTokenEnd(line, i + 1))
                {
                    return Token.CreateBoldToken(startIndex, i + 1, line, inserted);
                }

                if (line[i] == '_')
                {
                    var token = ReadItalicToken(line, i);
                    if (token.Length > 0)
                    {
                        inserted.Add(token);
                        i += token.Length;
                    }
                }
            }

            return Token.Empty();
        }

        public static Token ReadHeaderToken(string line)
        {
            if (line[0] != '#')
            {
                return Token.Empty();
            }
            var token = new Token(line.Length, 0, TokenType.Header, line.Substring(1));
            for (var i = 1; i < line.Length; i++)
            {
                var insertedToken = ReadBoldToken(line, i);
                insertedToken = (insertedToken.Length > 0) ? insertedToken :ReadItalicToken(line, i);
                if (insertedToken.Length > 0)
                {
                    token.AddInsertedToken(insertedToken);
                    i += insertedToken.Length;
                }
            }
            return token;
        }

        private static bool IsBoldTokenStart(string line, int startIndex)
        {
            return line[startIndex] == '_'
                   && startIndex <= line.Length - 4
                   && line[startIndex + 1] == '_'
                   && line[startIndex + 2] != ' ';
        }

        private static bool IsBoldTokenEnd(string line, int endIndex)
        {
            return line[endIndex - 1] == '_' && line[endIndex] == '_' && line[endIndex - 2] != ' ';
        }

        public static int SkipNotStyleWords(string line, int startIndex)
        {
            for (var i = startIndex; i < line.Length; i++)
            {
                if (line[i] == '_')
                {
                    return i - startIndex;
                }
            }
            return line.Length - startIndex;
        }
    } 
}