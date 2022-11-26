using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Parsers.Tokens;
using Markdown.Parsers.Tokens.Tags;
using Markdown.Parsers.Tokens.Tags.Enum;
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
                var lineTokens = TransformToTokens();
                document.TextBlocks.Add(new ParsedTextBlock(lineTokens));
            }
        }

        private List<IToken> TransformToTokens()
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
            return mdTags.IsTagStart(currentSymbol) ? 
                GetServiceTag() : 
                GetTextToken();
        }

        //TODO: переделать
        private IToken GetServiceTag()
        {
            var text = ReadWithCheck(t => !mdTags.IsTag(t));
            if (!mdTags.IsTag(text))
                return new TextToken(text);
            var lastOpeningTag = openedTokens.LastOrDefault(el=>el.ToString() == text) as MdPairedTag;
            var tag = mdTags.CreateTagFor(text,
                lastOpeningTag is null ? TagPosition.Start : TagPosition.End);

            if(tag is MdPairedTag)
            {
                (tag as MdPairedTag).IntoWord = char.IsSymbol(currentLine[currentPosition - text.Length]);
            }

            var isThisTagCommented = tokens.LastOrDefault() is MdCommentTag;
            if (isThisTagCommented || !tag.IsValidTag(currentLine, currentPosition))
            {
                if (isThisTagCommented)
                    tokens.Remove(tokens.Last());
                return tag.ToText();
            }
            else if(tag is MdHeaderTag)
                currentPosition++;
            else if(tag is MdPairedTag)
            {
                if(lastOpeningTag != null && lastOpeningTag.IntoWord)
                {

                }

                if (lastOpeningTag is null)
                    openedTokens.Add(tag); //взять верхний подходящий
                else
                    openedTokens.Remove(lastOpeningTag);
                
            }
            return tag;
        }

        private IToken GetTextToken()
        {
            var text = ReadWithCheck(symbol => mdTags.IsTagStart(symbol));
            return new TextToken(text);
        }

        private string ReadWithCheck(Func<string, bool> isSomeText)
        {
            var startPosition = currentPosition;
            string text = "";
            while (!nextCharOutsideLine &&
                   !isSomeText(currentLine.Substring(startPosition, currentPosition - startPosition + 1)))
                currentPosition++;

            return currentLine.Substring(startPosition, currentPosition - startPosition);
        }
        
        private string ReadWithCheck(Func<char, bool> isEnd)
        {
            var startPosition = currentPosition;
            while (!nextCharOutsideLine && !isEnd(currentSymbol)) 
                currentPosition++;
            return currentLine.Substring(startPosition, currentPosition - startPosition);
        }

        private void DeleteNotValidTags()
        {
            for (int i = 0; i < tokens.Count; i++)
                if (openedTokens.Contains(tokens[i]))
                    tokens[i] = tokens[i].ToText();

            openedTokens.Clear();
        }

        public ParsedDocument GetParsedDocument() => document;
    }
}
