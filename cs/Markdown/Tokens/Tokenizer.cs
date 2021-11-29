using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Tokens.Parsers;

namespace Markdown.Tokens
{
    public class Tokenizer
    {
        private List<ITokenParser> parsers = new()
        {
            new EscapedParser(),
            new BoldParser(),
            new ItalicParser(),
            new Header1Parser(),
            new SpaceTokenParser(),
            new EndLineParser(),
            new SquareBracketsParser(),
            new RoundBracketParser(),
            new CharParser()
        };

        public List<IToken> GetTokens(string rawString)
        {
            if (rawString is null)
                throw new ArgumentNullException($"{nameof(rawString)} can not be null");
            
            var tokens = new List<IToken>();
            var position = 0;
            while (position < rawString.Length)
            {
                AddNextToken(tokens, rawString, ref position);
            }

            tokens = ConcatNeighbourCharTokens(tokens);
            tokens = AddParagraphEndTokens(tokens);
            return tokens;
        }

        private void AddNextToken(List<IToken> tokens, string rawString, ref int position)
        {
            foreach (var parser in parsers)
            {
                if (parser.TryFindToken(rawString, ref position, out var token))
                {
                    tokens.Add(token);
                    return;
                }
            }

            throw new Exception($"No parser was found for {rawString} in {position}");
        }
        
        private List<IToken> ConcatNeighbourCharTokens(List<IToken> tokens)
        {
            var charTokens = new List<CharToken>();
            var converted = new List<IToken>();
            foreach (var token in tokens)
            {
                if (token is CharToken charToken)
                {
                    charTokens.Add(charToken);
                }
                else
                {
                    if (charTokens.Any())
                    {
                        converted.Add(new WordToken(charTokens));
                        charTokens.Clear();
                    }
                    converted.Add(token);
                }
            }
            if (charTokens.Any())
            {
                converted.Add(new WordToken(charTokens));
            }

            return converted;

        }

        private List<IToken> AddParagraphEndTokens(List<IToken> tokens)
        {
            var result = new List<IToken>();
            for (var tokenPosition = 0; tokenPosition < tokens.Count; tokenPosition++)
            {
                var newToken = tokens[tokenPosition];
                if (newToken is EndLineToken)
                {
                    var emptyLinesCount = GetEmptyLinesCount(tokens, ref tokenPosition);

                    if (emptyLinesCount != 0)
                    {
                        result.Add(new ParagraphEndToken());
                        continue;
                    }
                }

                result.Add(newToken);
            }

            if (result.Count == 0 || result.Last() is not ParagraphEndToken)
            {
                result.Add(new ParagraphEndToken());
            }

            return result;
        }

        private int GetEmptyLinesCount(List<IToken> tokens, ref int position)
        {
            var emptyLinesCount = 0;
            while (tokens.InBorders(position + 1))
            {
                var nextLineId = tokens.FindIndex(position + 1, x => x is EndLineToken);
                if (nextLineId > 0 && LineIsEmpty(tokens, position + 1, nextLineId))
                {
                    emptyLinesCount++;
                    position = nextLineId;
                    continue;
                }
                break;
            }

            return emptyLinesCount;
        }

        private bool LineIsEmpty(List<IToken> tokens, int startPosition, int endPosition)
        {
            for (var position = startPosition; position < endPosition; position++)
            {
                if (tokens[position] is not SpaceToken and not EndLineToken)
                    return false;
            }

            return true;
        }
    }
}