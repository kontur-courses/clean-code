using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public static class MdParser
    {
        public static readonly IReadOnlyList<Tag> Tags = new List<Tag>()
        {
            new Tag("_", "em"),
            new Tag("__", "strong"),
            new Tag("*", "em"),
            new Tag("**", "strong"),
            new Tag("#", "h1", withClosure: false),
            new Tag("##", "h2", withClosure: false),
            new Tag("###", "h3", withClosure: false),
            new Tag("####", "h4", withClosure: false),
            new Tag("#####", "h5", withClosure: false),
            new Tag("######", "h6", withClosure: false)
        };

        public static List<Token> GetAllTokens(string mdParagraph)
        {
            var allTokens = new List<Token>();
            var position = 0;
            while (position < mdParagraph.Length)
            {
                var symbolRepeatCount = GeneralFunctions.SymbolInRowCount(
                    mdParagraph[position], mdParagraph, position);
                if (Tags.All(tag => symbolRepeatCount != tag.Md.Length))
                {
                    position += symbolRepeatCount;
                }
                else if (TryGetToken(mdParagraph, position, symbolRepeatCount, out var possibleToken))
                {
                    allTokens.Add(possibleToken);
                    position += possibleToken.Tag.Md.Length;
                }
                else
                    position++;
            }

            return allTokens;
        }

        public static List<Token> GetNotIntersectingEndsTokens(List<Token> tokens)
        {
            if (tokens.Count < 2)
                return tokens;

            var notIntersectingTokens = new List<Token>();
            for (var i = 1; i < tokens.Count; i++)
            {
                if (tokens[i - 1].EndPosition != tokens[i].EndPosition ||
                    notIntersectingTokens.All(token => token.EndPosition != tokens[i - 1].EndPosition))
                {
                    notIntersectingTokens.Add(tokens[i - 1]);
                }
            }

            if (notIntersectingTokens.Count == 0 ||
                tokens.Last().EndPosition != notIntersectingTokens.Last().EndPosition)
            {
                notIntersectingTokens.Add(tokens.Last());
            }

            return notIntersectingTokens;
        }

        public static List<Token> RemoveNotWorkingNestedTokens(List<Token> tokens)
        {
            if (tokens.Count < 2)
                return tokens;

            var removeList = new List<Token>();
            for (var i = 1; i < tokens.Count; i++)
            {
                if (IsNestedToken(tokens[i], tokens[i - 1]) &&
                    IsNotWorkingNestedToken(tokens[i], tokens[i - 1]))
                {
                    removeList.Add(tokens[i]);
                }
            }

            var lastNested = tokens.Last();
            var lastMain = tokens[tokens.Count - 2];
            if (IsNestedToken(lastNested, lastMain) && IsNotWorkingNestedToken(lastNested, lastMain))
                removeList.Add(lastNested);

            return tokens
                .Where(token => !removeList.Contains(token))
                .ToList();
        }

        private static bool TryGetToken(
            string line, int position, int symbolRepeatCount, out Token possibleToken)
        {
            possibleToken = default(Token);
            foreach (var tag in Tags.Where(tag => tag.Md.Length == symbolRepeatCount))
            {
                if (position + tag.Md.Length - 1 < line.Length &&
                    line.Substring(position, tag.Md.Length) == tag.Md)
                {
                    if (tag.WithClosure && IsValidOpenTag(line, position, tag))
                        return TryGetPairTagToken(tag, line, position, out possibleToken);
                    if (IsValidSingleTag(line, position, tag))
                        return TryGetSingleTagToken(tag, line, position, out possibleToken);
                }
            }

            return false;
        }

        private static bool TryGetPairTagToken(Tag tag, string line, int position, out Token possibleToken)
        {
            possibleToken = default(Token);
            var closeTagIndex = line.IndexOf(
                tag.Md, Math.Min(position + tag.Md.Length, line.Length - 1));
            while (closeTagIndex > -1)
            {
                var SymbolRepeatCount = GeneralFunctions.SymbolInRowCount(
                    line[closeTagIndex], line, closeTagIndex);
                if (SymbolRepeatCount != tag.Md.Length)
                {
                    closeTagIndex += SymbolRepeatCount;
                }
                else if (IsValidCloseTag(line, closeTagIndex, tag))
                {
                    possibleToken = new Token(tag, position + tag.Md.Length, closeTagIndex);
                    return true;
                }

                closeTagIndex = line.IndexOf(tag.Md,
                    Math.Min(closeTagIndex + tag.Md.Length, line.Length - 1));
            }

            return false;
        }

        private static bool TryGetSingleTagToken(
            Tag tag, string line, int position, out Token possibleToken)
        {
            possibleToken = default(Token);
            var singleTagEnd = line.IndexOf("\r");
            if (singleTagEnd >= 0)
            {
                possibleToken = new Token(
                    tag,
                    position + tag.Md.Length, singleTagEnd);
                return true;
            }

            return false;
        }

        private static bool IsValidOpenTag(string line, int position, Tag tag)
        {
            if (GeneralFunctions.WasOddCountEscaping(line, position))
                return false;

            return (position - 1 < 0 || !char.IsLetterOrDigit(line[position - 1])) &&
                   (position + tag.Md.Length < line.Length && line[position + tag.Md.Length] != ' ');
        }

        private static bool IsValidCloseTag(string line, int position, Tag tag)
        {
            if (GeneralFunctions.WasOddCountEscaping(line, position))
                return false;

            return (position - 1 >= 0 && line[position - 1] != ' ') &&
                   (position + tag.Md.Length >= line.Length ||
                    !char.IsLetterOrDigit(line[position + tag.Md.Length]));
        }

        private static bool IsValidSingleTag(string line, int position, Tag tag)
        {
            return position == 0 || line[position - 1] == '\n' || line[position - 1] == '\r';
        }

        private static bool IsNestedToken(Token nested, Token main)
        {
            return nested.StartPosition > main.StartPosition && nested.EndPosition < main.EndPosition;
        }

        private static bool IsNotWorkingNestedToken(Token nested, Token main)
        {
            return nested.Tag.Html == "strong" && main.Tag.Html == "em";
        }
    }
}
