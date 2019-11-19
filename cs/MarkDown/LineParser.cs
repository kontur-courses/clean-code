using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarkDown.TokenParsers;

namespace MarkDown
{
    public class LineParser
    {
        private ParserGetter Parsers;
        private List<TokenParser> tokenParsers;
        private Dictionary<int, Tag> openingTags = new Dictionary<int, Tag>();
        private Dictionary<TokenType, Queue<Tag>> closingTags = new Dictionary<TokenType, Queue<Tag>>();
        private Queue<Token> Tokens = new Queue<Token>();

        public LineParser(ParserGetter parserGetter)
        {
            Parsers = parserGetter;
            tokenParsers = Parsers.GetTokenParsers();
            tokenParsers
                .Select(p => p.Type)
                .ToList()
                .ForEach(t => closingTags.Add(t, new Queue<Tag>()));
        }

        public string GetParsedLineFrom(string line)
        {
            //Дальше подровняю решение
            //разобью нормально на методы
            //а то тесты были реализации не было
            var parsedLine = new StringBuilder();

            //тэги всегда будут по порядку, это нужно как-то подчеркнуть в коде? добавить sort чтобы была уверенность
            for (var i = 0; i < line.Length; i++)
            {
                if (Parsers.FirstTokenChars.Contains(line[i]))
                {
                    var startIndex = i;
                    AddPossibleTags(line, startIndex);
                }
            }

            //токены как и тэги по порядку
            Token currentToken;
            foreach (var openTag in openingTags)
            {
                if (TryToGetTokenFromTags(openTag.Value, out currentToken))
                    Tokens.Enqueue(currentToken);
            }

            var indexNextToLastToken = 0;
            foreach (var token in Tokens)
            {
                parsedLine.Append(line.Substring(indexNextToLastToken, token.StartIndex - indexNextToLastToken));
                parsedLine.Append(Parsers.GetParserFromType(token.Type).GetTokenParsed(line, token));
                indexNextToLastToken = token.indexNextToToken;
            }

            parsedLine.Append(line.Substring(indexNextToLastToken, line.Length- indexNextToLastToken));


            return parsedLine.ToString();
        }

        private void AddPossibleTags(string line, int startIndex)
        {
            tokenParsers
                .Where(p => p.IsTag(line, startIndex, true)).ToList()
                .ForEach(p => openingTags
                    .Add(startIndex, new Tag(startIndex, p.OpeningTags.@from.Length, p.Type)));
            tokenParsers
                .Where(p => p.IsTag(line, startIndex, false)).ToList()
                .ForEach(p => closingTags[p.Type]
                    .Enqueue(new Tag(startIndex, p.ClosingTags.@from.Length, p.Type)));
        }

        private bool TryToGetTokenFromTags(Tag openTag, out Token token)
        {
            token = null;
            if (closingTags[openTag.Type].Count == 0)
                return false;

            var closestTag = closingTags[openTag.Type].Dequeue();

            while (openTag.indexNextToTag > closestTag.StartIndex)
            {
                if (closingTags[openTag.Type].Count == 0)
                    return false;

                closestTag = closingTags[openTag.Type].Dequeue();
            }

            openingTags.Remove(openTag.StartIndex);
            token = new Token(openTag, closestTag);
            return true;
        }
    }
}