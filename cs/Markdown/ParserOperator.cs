using Markdown.Parsers;
using System;
using System.Collections.Generic;

namespace Markdown
{
    public class ParserOperator
    {
        public int Position { get; set; }
        private readonly Stack<string> stack;
        private List<string> partialTokenValue;
        private readonly Dictionary<string, TokenParser> parsers;
        private bool isTokenOpen;
        private bool isShielding;
        private TokenParser currentParser;
        private List<Token> tokens;
        private string previousPart;
        private string nextPart;

        public ParserOperator()
        {
            stack = new Stack<string>();
            partialTokenValue = new List<string>();
            parsers = new Dictionary<string, TokenParser>();
            tokens = new List<Token>();
            InitParsers();
        }

        public void AddTokenPart(Tuple<string, string> bigram)
        {
            var part = bigram.Item1;
            nextPart = bigram.Item2;
            if (parsers.ContainsKey(part) && !isTokenOpen)
                TokenOpen(part);
            else if (parsers.ContainsKey(part) && isTokenOpen)
                TokenEnd(part);
            else if (isShielding)
                TokenEnd("\\" + part);
            else if (part == "\\\\" && !isTokenOpen)
                AddSimpleToken("\\");
            else if (isTokenOpen)
                partialTokenValue.Add(part);
            else
                AddSimpleToken(part);
            previousPart = part;
        }

        private void InitParsers()
        {
            var italic = new ItalicParser();
            var bold = new BoldParser();
            var header = new HeaderParser();
            parsers["_"] = italic;
            parsers["__"] = bold;
            parsers["#"] = header;
            parsers["\\"] = header;
        }

        private void TokenOpen(string part)
        {
            isTokenOpen = true;
            if (part == "\\")
                isShielding = true;
            else
                stack.Push(part);
            currentParser = parsers[part];
            currentParser.PartBeforeTokenStart = previousPart;
        }

        private void TokenEnd(string part)
        {
            if (isShielding)
            {
                AddSimpleToken(part);
                isShielding = false;
                isTokenOpen = false;
            }
            else if (stack.Peek() == part && part != "#")
                StartParse();
            else
                partialTokenValue.Add(part);
        }

        private void AddSimpleToken(string text)
        {
            tokens.Add(new Token(Position, text, TokenType.Simple));
        }

        public List<Token> GetTokens()
        {
            if (!IsClose())
                StartParse(false);
            return tokens;
        }

        private void StartParse(bool needClose = true)
        {
            if (needClose)
                stack.Pop();
            currentParser.IsTokenCorrupted = !IsClose();
            isTokenOpen = false;
            currentParser.PartAfterTokenEnd = nextPart;
            tokens.Add(currentParser.ParseToken(partialTokenValue, Position));
            partialTokenValue.Clear();
        }

        public bool IsClose() => stack.Count == 0;
        public static bool IsCorrectStart(string text) => !text.StartsWith(" ");
        public static bool IsCorrectEnd(string text) => !text.EndsWith(" ");
    }
}
