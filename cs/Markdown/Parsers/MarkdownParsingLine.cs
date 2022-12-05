using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Extensions;
using Markdown.Parsers.Tokens;
using Markdown.Parsers.Tokens.Markdown;
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

            Tokens.ToTextThatContainedIn(OpenedTokens);
            OpenedTokens.Clear();
            return Tokens;
        }
        
        private IToken GetNextToken()
        {
            return mdTags.IsTagStart(currentSymbol)
                ? GetTag()
                : GetToken();
        }

        private IToken GetTag()
        {
            var text = ReadLineWhile(startPosition => !mdTags.IsTag(
                Line[startPosition..(CurrentPosition+1)]));

            return IsActiveComment() ? Comment(text) : mdTags.TryToCreateTagFor(text, this);
        }

        private IToken GetToken()
        {
            string text;
            if (MdCommentToken.IsStart(currentSymbol))
            {
                text = currentSymbol.ToString();
                CurrentPosition++;
                return IsActiveComment() ? Comment(text) : new MdCommentToken();
            }

            text = ReadLineWhile(symbol => 
                mdTags.IsTagStart(currentSymbol) || MdCommentToken.IsStart(currentSymbol));
            return new TextToken(text);
        }

        private bool IsActiveComment() => Tokens.LastOrDefault() is MdCommentToken;

        private IToken Comment(string text)
        {
            Tokens.Remove(Tokens.Last());
            return new TextToken(text);
        }

        private string ReadLineWhile(Func<int, bool> stillAcceptableStartingFrom)
        {
            var startPosition = CurrentPosition;
            while (!nextCharOutsideLine && !stillAcceptableStartingFrom(startPosition))
                CurrentPosition++;
            return Line[startPosition..CurrentPosition];
        }
    }
}
