using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Markdown
{
    public class Token
    {
        public readonly int StartIndex;
        public readonly int Length;
        public readonly string Value;
        public readonly TokenType Type;

        public Token(int length, int startIndex, TokenType type, string line)
        {
            this.StartIndex = startIndex;
            this.Length = length;
            this.Type = type;
            this.Value = CountValue(line);
        }

        private string CountValue(string line)
        {
            switch(Type)
            {
                case TokenType.Italic:
                    return CountItalicValue(line);
                case TokenType.Bold:
                    return CountBoldValue(line);
                case TokenType.Header:
                    return CountHeaderValue(line);
                
            }

            return "";

        }

        private string CountHeaderValue(string line)
        {
            var stringBuilder = new StringBuilder("<h1>");
            for (var i = 1; i < line.Length; i++)
            {
                var insertedToken = MarkdownParser.ReadBoldToken(line, i);
                insertedToken = (insertedToken.Length > 0) ? insertedToken :MarkdownParser.ReadItalicToken(line, i);
                if (insertedToken.Length > 0)
                {
                    stringBuilder.Append(insertedToken.Value);
                    i += insertedToken.Length;
                }
                else
                {
                    var count = MarkdownParser.SkipNotStyleWords(line, i);
                    stringBuilder.Append(line.Substring(i, count));
                    i += count;
                }
            }

            stringBuilder.Append("</h1>");

            return stringBuilder.ToString();
        }

        private string CountBoldValue(string line)
        {
            var stringBuilder = new StringBuilder("<strong>");
            for (var i = StartIndex + 2; i < StartIndex + Length - 2; i++)
            {
                var token = MarkdownParser.ReadItalicToken(line, i);
                if (token.Length == 0)
                {
                    var count = MarkdownParser.SkipNotStyleWords(line, i);
                    stringBuilder.Append(line.Substring(i, count));
                    i += count;
                    
                }
                else
                {
                    stringBuilder.Append(token.Value);
                    i += token.Length;
                }
            }

            stringBuilder.Append("</strong>");

            return stringBuilder.ToString();
        }

        private string CountItalicValue(string line)
        {
            return "<em>" + line.Substring(StartIndex + 1, Length - 2) + "</em>";
        }

        public static Token Empty()
        {
            return new Token(0, 0, TokenType.Bold, "");
        }
    }
    
    public enum TokenType{
        Italic,
        Bold,
        Header
    }
}