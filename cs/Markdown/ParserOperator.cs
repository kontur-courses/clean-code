using Markdown.Parsers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class ParserOperator
    {
        public int Position { get; set; }
        private readonly Stack<string> stack;
        private List<string> partialTokenValue;
        private readonly Dictionary<string, ITokenParser> parsers;
        private bool isTokenOpen;
        private Func<IEnumerable<string>, int, Token> currentParser;
        private List<Token> tokens;

        public ParserOperator()
        {
            stack = new Stack<string>();
            partialTokenValue = new List<string>();
            parsers = new Dictionary<string, ITokenParser>();
            tokens = new List<Token>();
            InitParsers();
        }

        public void AddTokenPart(string part)
        {
            if (parsers.ContainsKey(part) && !isTokenOpen)
                TokenOpen(part);
            else if (parsers.ContainsKey(part) && isTokenOpen)
                TokenEnd(part);
            else if (isTokenOpen)
                partialTokenValue.Add(part);
        }

        private void InitParsers()
        {
            var italic = new ItalicParser();
            var bold = new BoldParser();
            var header = new HeaderParser();
            parsers["_"] = italic;
            parsers["__"] = bold;
            parsers["#"] = header;
        }

        private void TokenOpen(string part)
        {
            isTokenOpen = true;
            currentParser = parsers[part].ParseToken;
            stack.Push(part);
        }

        private void TokenEnd(string part)
        {
            if (stack.Peek() == part)
            {
                tokens.Add(currentParser(partialTokenValue, Position));
                isTokenOpen = false;
                stack.Pop();
            }
            else
                partialTokenValue.Add(part);
        }

        public List<Token> GetTokens()
        {
            if (stack.Count > 0)
                tokens.Add(currentParser(partialTokenValue, Position));
            return tokens;
        }

        public bool IsClose() => stack.Count == 0;
        public bool IsCorrectStart(string text) => !text.StartsWith(" ");
        public bool IsCorrectEnd(string text) => !text.EndsWith(" ");
    }
}
