using Markdown.Core;
using Markdown.Parsers;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class ParserOperator
    {
        public int Position { get; set; }
        private readonly Stack<string> stack;
        private readonly Dictionary<string, TokenParser> parsers;
        private bool isTokenOpen;
        private TokenParser currentParser;
        private List<Token> tokens;
        private string previousPart;
        private string nextPart;

        private bool skip;
        private List<TokenPart> partialValue = new List<TokenPart>();

        public ParserOperator()
        {
            stack = new Stack<string>();
            parsers = new Dictionary<string, TokenParser>();
            tokens = new List<Token>();
            InitParsers();
        }

        public void AddTokenPart((TokenPart Previous, TokenPart Current) bigram)
        {
            var part = bigram.Previous;
            nextPart = bigram.Current?.Value;
            if (part.Escaped)
                AddEscapedPart(part);
            else if (parsers.ContainsKey(part.Value) && !isTokenOpen)
                TokenOpen(part);
            else if (parsers.ContainsKey(part.Value) && isTokenOpen)
                TokenEnd(part);
            else if (isTokenOpen)
                partialValue.Add(part);
            else
                AddSimpleToken(part);
            previousPart = part.Value;
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

        private void TokenOpen(TokenPart part)
        {
            isTokenOpen = true;
            stack.Push(part.Value);
            currentParser = parsers[part.Value];
            currentParser.PartBeforeTokenStart = previousPart;
        }

        private void TokenEnd(TokenPart part)
        {
            if (stack.Peek() == part.Value && part.Value != "#")
                StartParse();
            else
                partialValue.Add(part);
        }

        private void AddSimpleToken(TokenPart text)
        {
            tokens.Add(new Token(Position, text.Value, TokenType.Simple));
        }

        public List<Token> GetTokens()
        {
            if (!IsClose())
                StartParse(false);
            else if (currentParser == null || partialValue.Count != 0)
                AddAllParts();
            return tokens;
        }

        private void StartParse(bool needClose = true)
        {
            if (needClose)
                stack.Pop();
            currentParser.IsTokenCorrupted = !IsClose();
            isTokenOpen = false;
            currentParser.PartAfterTokenEnd = nextPart;
            tokens.Add(currentParser.ParseToken(partialValue, Position));
            partialValue.Clear();
        }

        public bool TokenContainsFormattingStrings(string[] formattingStrings)
        {
            if (formattingStrings.Length == 0)
                return false;
            var strings = new HashSet<string>(formattingStrings);
            return stack.Any(s => strings.Contains(s));
        }
        public bool IsClose() => stack.Count == 0;
        public static bool IsCorrectStart(string text) => !text.StartsWith(" ");
        public static bool IsCorrectEnd(string text) => !text.EndsWith(" ");

        private void AddEscapedPart(TokenPart part)
        {
            if (isTokenOpen)
                partialValue.Add(part);
            else
                AddSimpleToken(part);
        }

        private void AddAllParts()
        {
            foreach (var part in partialValue)
            {
                AddSimpleToken(part);
            }
            partialValue.Clear();
        }
    }
}
