using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Parsers.Tokens;
using Markdown.Parsers.Tokens.Tags;
using Markdown.Parsers.Tokens.Tags.Markdown;


namespace Markdown.Parsers
{
    public class MarkdownParser
    {
        private readonly MdTags mdTags = MdTags.GetInstance();
        private readonly ParsedDocument document = new ParsedDocument();
        private readonly List<IToken> openedTokens = new List<IToken>();

        private int currentPosition;
        private readonly string currentLine;
        private char currentSymbol => currentLine[currentPosition];
        private bool nextCharOutsideLine => currentPosition == currentLine.Length;

        private List<IToken> tokens;
        //TODO: new line as \r\n
        private readonly NewLineToken newLineToken = new NewLineToken();
        public MarkdownParser(string markdownText)
        {
            var lines = markdownText.Split(newLineToken.ToString());
            foreach (var parsedLine in lines)
            {
                currentLine = parsedLine;
                var lineTokens = TransformCurrentLineToTokens();
                document.TextBlocks.Add(new ParsedTextBlock(lineTokens));
            }
        }

        private List<IToken> TransformCurrentLineToTokens()
        {
            tokens = new List<IToken>();
            currentPosition = 0;

            while (!nextCharOutsideLine)
                tokens.Add(GetNextToken());
            
            DeleteNotValidTags();

            return tokens;
        }

        private IToken GetNextToken()
        {
            return mdTags.IsTagStart(currentSymbol) 
                ? GetServiceTag() 
                : GetTextToken();
        }

        //TODO: переделать
        private IToken GetServiceTag()
        {
            var text = ReadWithCheck(startPosition => !mdTags.IsTag(
                currentLine.Substring(startPosition, currentPosition - startPosition + 1)));

            //TODO: delete
            if (!mdTags.IsTag(text))
                return new TextToken(text);

            var lastOpeningPairedTag = openedTokens.LastOrDefault(el=>el.ToString() == text) as MdPairedTag;

            var tag = mdTags.CreateTagFor(text, lastOpeningPairedTag);
            //text, currentLine, currentPosition, tokens, openedTokens
            return ServiceTag(tag);
        }

        private IToken ServiceTag(Tag tag)
        {
            if (!tag.IsValidTag(currentLine, currentPosition))
                return tag.ToText();

            if (tokens.LastOrDefault() is MdCommentTag)
            {
                tokens.Remove(tokens.Last());
                return tag.ToText();
            }

            if (tag is MdHeaderTag)
                currentPosition++;
            else if (tag is MdPairedTag pairedTag)
            {
                pairedTag.CheckInWord(currentLine, currentPosition);

                if (pairedTag.Pair is null)
                    openedTokens.Add(tag);
                else
                {
                    pairedTag.ProcessIntersections(tokens, openedTokens);

                    if(pairedTag.Pair is MdPairedTag { IntoWord: true } && !pairedTag.IsIntoWord(tokens))
                        return tag.ToText();

                    openedTokens.Remove(pairedTag.Pair);
                }
            }

            return tag;
        }

        private IToken GetTextToken()
        {
            var text = ReadWithCheck(symbol => mdTags.IsTagStart(currentSymbol));
            return new TextToken(text);
        }

        private string ReadWithCheck(Func<int, bool> stillAcceptableStartingFrom)
        {
            var startPosition = currentPosition;
            while (!nextCharOutsideLine && !stillAcceptableStartingFrom(startPosition))
                currentPosition++;
            return currentLine.Substring(startPosition, currentPosition - startPosition);
        }

        private void DeleteNotValidTags()
        {
            foreach (var idx in openedTokens.Select(token => tokens.FindIndex(el => el == token)))
                tokens[idx] = tokens[idx].ToText();

            openedTokens.Clear();
        }

        public ParsedDocument GetParsedDocument() => document;
    }
}
