using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Models;

namespace Markdown.Entities.Parsers
{
    /// <summary>
    /// Tokenizer который работает с markdown текстом
    /// </summary>
    public class MarkdownTokenizer : ITokenizer
    {
        public List<Token> Tokenize(string text)
        {
            return TokenizeLines(TextToLines(text));
        }

        public List<string> TextToLines(string text)
        {
            return text.Split("\n").ToList();
        }

        public List<Token> TokenizeLines(IEnumerable<string> lines)
        {
            var tokens = new List<Token>();
            foreach (var line in lines)
            {
                tokens.AddRange(TokenizeLine(line));
            }
            return tokens;
        }

        public List<Token> TokenizeLine(string line)
        {
            throw new NotImplementedException();
        }
    }
}
