using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Extensions;
using Markdown.Parsers.Tokens;
using Markdown.Parsers.Tokens.Tags.Markdown;

namespace Markdown.Parsers
{
    public class MarkdownParsingLine
    {
        public List<IToken> OpenedTokens { get; private set; }
        public List<IToken> Tokens { get; private set; }

        public int CurrentPosition { get; set; }
        public string Line { get; }

        private readonly MdTags mdTags = MdTags.GetInstance();
        private char currentSymbol => Line[CurrentPosition];
        private bool nextCharOutsideLine => CurrentPosition == Line.Length;

        public MarkdownParsingLine(string line)
        {
            this.Line = line;
        }

        public List<IToken> Parse()
        {
            CurrentPosition = 0;
            Tokens = new List<IToken>();
            OpenedTokens = new List<IToken>();

            while (!nextCharOutsideLine)
                Tokens.Add(GetNextToken());

            DeleteNotValidTags();

            return Tokens;
        }
        
        private IToken GetNextToken()
        {
            return mdTags.IsTagStart(currentSymbol)
                ? GetServiceTag()
                : GetTextToken();
        }

        private IToken GetServiceTag()
        {
            var text = ReadWithCheck(startPosition => !mdTags.IsTag(
                Line[startPosition..(CurrentPosition+1)]));
            return mdTags.TryToCreateTagFor(text, this);
        }

        private IToken GetTextToken()
        {
            var text = ReadWithCheck(symbol => mdTags.IsTagStart(currentSymbol));
            return new TextToken(text);
        }

        private string ReadWithCheck(Func<int, bool> stillAcceptableStartingFrom)
        {
            var startPosition = CurrentPosition;
            while (!nextCharOutsideLine && !stillAcceptableStartingFrom(startPosition))
                CurrentPosition++;
            return Line[startPosition..CurrentPosition];
        }

        private void DeleteNotValidTags()
        {
            var openedTokenIndexes = OpenedTokens.Select(token => Tokens.FindIndex(el => el == token));
            foreach (var idx in openedTokenIndexes)
                Tokens.ToTextAt(idx);

            OpenedTokens.Clear();
        }
    }
}
