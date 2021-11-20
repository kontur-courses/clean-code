using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Interfaces;


namespace Markdown
{
    public class TokenCreator : ITokenCreator
    {
        private string text;
        private int index;
        private bool isNewLine = true;
        private readonly HashSet<char> markupSymbols = new() { '\n', '_', '\\' };


        public IEnumerable<Token> Create(string text)
        {
            this.text = text;
            var tokens = new List<Token>();
            while (index < text.Length)
            {
                var item = text[index];
                var token = DefineToken(item);
                tokens.Add(token);
                isNewLine = tokens.Last().TokenType == TokenType.NewLine;
                index++;
            }

            return tokens;
        }


        private Token DefineToken(char current)
        {
            return current switch
            {
                '_' => ParseUnderScore(),
                '#' when isNewLine => new(TokenType.Header1, "#"),
                '\n' => new Token(TokenType.NewLine, "\n"),
                '\\' => new Token(TokenType.Escape, "\\"),
                _ => ParseText()
            };
        }

        private Token ParseUnderScore()
        {
            if (!NextSymbolIsTheSame('_')) 
                return new Token(TokenType.Italics, "_");
            index++;
            return new Token(TokenType.Strong, "__");

        }

        private Token ParseText()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(text[index]);
            var substringIndex = index + 1;
            while (substringIndex < text.Length && !markupSymbols.Contains(text[substringIndex]))
            {
                stringBuilder.Append(text[substringIndex]);
                substringIndex++;
            }

            index = substringIndex - 1;

            return new Token(TokenType.Text, stringBuilder.ToString());
        }
        
        private bool NextSymbolIsTheSame(char symbol) => index + 1 < text.Length && symbol == text[index + 1];
    }
}