using System;
using System.Collections.Generic;
using System.Linq;
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

            DeleteNotValidTagsIn(tokens);

            return tokens;
        }

        private IToken GetNextToken()
        {
            return mdTags.IsServiceSymbol(currentSymbol) ? 
                GetServiceTag() : 
                GetTextToken();
        }

        //TODO: переделать
        private IToken GetServiceTag()
        {
            var text = ReadWithCheck(symbol => !mdTags.IsServiceSymbol(symbol));
            if (!mdTags.IsTag(text))
                return new TextToken(text);
            var lastOpeningTag = openedTokens.LastOrDefault(el=>el.ToString() == text);
            var tag = mdTags.CreateTagFor(text,
                lastOpeningTag is null ? TagPosition.Start : TagPosition.End);

            var isCommentedTag = tag.IsCommentedTag(currentLine, currentPosition - text.Length);
            if (isCommentedTag || !tag.IsValidTag(currentLine, currentPosition))
            {
                if (isCommentedTag)
                    tokens.Remove(tokens.Last());
                return tag.ToText();
            }
            else if(tag is PairedTag)
            {
                if (lastOpeningTag is null)
                    openedTokens.Add(tag); //взять верхний подходящий
                else
                    openedTokens.Remove(lastOpeningTag);
                
            }
            return tag;
        }

        private IToken GetTextToken()
        {
            var text = ReadWithCheck(symbol => mdTags.IsServiceSymbol(symbol));
            return new TextToken(text);
        }

        private string ReadWithCheck(Func<char, bool> IsEnd)
        {
            var startPosition = currentPosition;
            while (!nextCharOutsideLine && !IsEnd(currentSymbol)) 
                currentPosition++;
            return currentLine.Substring(startPosition, currentPosition - startPosition);
        }

        private void DeleteNotValidTagsIn(List<IToken> tokens)
        {
            for (int i = 0; i < tokens.Count; i++)
                if (openedTokens.Contains(tokens[i]))
                    tokens[i] = tokens[i].ToText();

            openedTokens.Clear();
        }

        public ParsedDocument GetParsedDocument() => document;
    }
}
