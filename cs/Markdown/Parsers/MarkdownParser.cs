using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Parsers.Tokens;
using Markdown.Parsers.Tokens.Tags;
using Markdown.Parsers.Tokens.Tags.Enum;
using Markdown.Parsers.Tokens.Tags.Html;
using Markdown.Parsers.Tokens.Tags.Markdown;


namespace Markdown.Parsers
{
    public class MarkdownParser
    {
        private readonly ParsedDocument document = new ParsedDocument();
        private readonly Queue<IToken> opendTokens = new Queue<IToken>();
        private readonly HashSet<char> serviceSymbols = new HashSet<char>();

        private int position;
        //TODO: new line as \r\n
        private readonly NewLineToken newLineToken = new NewLineToken();
        public MarkdownParser(string markdownText)
        {
            PrepareServiceSymbols();
            var lines = markdownText.Split(newLineToken.ToString());
            foreach (var line in lines)
            {
                var lineTokens = TransformToTokens(line);
                document.TextBlocks.Append(new ParsedTextBlock(lineTokens));
            }
        }

        private void PrepareServiceSymbols()
        {
            var possibleTags = new List<Tag>()
            {
                new MdBoldTag(TagPosition.Any),
                new MdItalicTag(TagPosition.Any),
                new MdHeaderTag()
            };
            foreach (var tag in possibleTags)
            {
                foreach (var symbol in tag.ToString())
                {
                    serviceSymbols.Add(symbol);
                }
            }
        }

        private List<IToken> TransformToTokens(string line)
        {
            var tags = new List<IToken>();
            position = 0;

            do
            {
                tags.Add(GetNextTagOf(line));
            } 
            while (position++ < line.Length);

            DeleteNotValidTagsIn(tags);

            tags.Add(newLineToken); // TODO: как не добавить лишний таг в конце, но не испортить код?

            return tags;
        }

        private IToken GetNextTagOf(string line)
        {
            return IsServiceSymbol(line[position]) ? 
                GetServiceTag(line) : 
                GetTextTag(line);
        }

        private bool IsServiceSymbol(char symbol) =>
            serviceSymbols.Contains(symbol);

        private IToken GetServiceTag(string line) =>
            throw new NotImplementedException();

        private IToken GetTextTag(string line) =>
            throw new NotImplementedException();

        private void DeleteNotValidTagsIn(List<IToken> tags)
        {
            for (int i = 0; i < tags.Count; i++)
                if (opendTokens.Contains(tags[i]))
                    tags[i] = tags[i].ToText();

            opendTokens.Clear();
        }

        public ParsedDocument GetParsedDocument() => document;
    }
}
