using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class Md
    {
        public string Render(string[] text)
        {
            var stringAccumulator = new StringBuilder();
            foreach (var line in text)
            {
                var token = MarkdownParser.ReadHeaderToken(line);
                if (token.Length > 0)
                {
                    stringAccumulator.Append(token.Value);
                }
                else
                {
                    stringAccumulator.Append(MarkdownParser.GetHtmlValue(line));
                }
            }
            return stringAccumulator.ToString();
        }
    }

    internal static class MarkdownParser
    {
        public static string GetHtmlValue(string line)
        {
            var stringAccumulator = new StringBuilder();
            for (var i = 0; i < line.Length; i++)
            {
                var insertedToken = ReadBoldToken(line, i);
                insertedToken = (insertedToken.Length > 0) ? insertedToken :MarkdownParser.ReadItalicToken(line, i);
                if (insertedToken.Length > 0)
                {
                    stringAccumulator.Append(insertedToken.Value);
                    i += insertedToken.Length;
                }
                else
                {
                    var count = MarkdownParser.SkipNotStyleWords(line, i);
                    stringAccumulator.Append(line.Substring(i, count));
                    i += count;
                }
            }

            return stringAccumulator.ToString();
        }
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
                    return new Token(lengthWithoutBorders + 2, startIndex, TokenType.Italic, line);
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

            for (var i = startIndex + 2; i < line.Length - 1; i++)
            {
                if (IsBoldTokenEnd(line, i + 1))
                {
                    return new Token(i + 2 - startIndex, startIndex, TokenType.Bold, line);
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
            var token = new Token(line.Length, 0, TokenType.Header, line);
            
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