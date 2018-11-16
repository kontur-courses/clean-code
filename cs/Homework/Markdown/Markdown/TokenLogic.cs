using System.Collections.Generic;

namespace Markdown
{
    public static class TokenLogic
    {
        private static readonly List<Tag> tags = new List<Tag>()
        {
            new Tag("_", "em"),
            new Tag("__", "strong")
        };
        public static List<Tag> Tags => new List<Tag>(tags);

        public static List<Token> GetPossibleTokens(string mdParagraph)
        {
            var tokens = new List<Token>();
            var i = 0;

            while (i < mdParagraph.Length)
            {
                Token token = null;
                if (TryGetToken(mdParagraph, i, out token))
                {
                    tokens.Add(token);
                    i += token.Tag.MD.Length;
                }
                else
                    i++;
            }

            return tokens;
        }

        private static bool TryGetToken(string data, int position, out Token possibleToken)
        {
            possibleToken = default(Token);
            var dataPart = data.Substring(position);
            var wasToken = false;

            foreach (var tag in tags)
            {
                if (dataPart.StartsWith(tag.MD))
                {
                    if (IsValidOpenTag(data, position, tag))
                    {
                        possibleToken = new Token(tag, position);
                        wasToken = true;
                    }
                    else if (IsValidCloseTag(data, position, tag))
                    {
                        possibleToken = new Token(tag, position, false);
                        wasToken = true;
                    }
                }
            }

            return wasToken;
        }

        private static bool IsValidOpenTag(string data, int position, Tag tag)
        {
            if (WasEscaping(data, position))
                return false;
            var shift = tag.MD.Length;

            return (position - 1 < 0 || !char.IsLetterOrDigit(data[position - 1])) &&
                   (position + shift < data.Length && data[position + shift] != ' ');
        }

        private static bool IsValidCloseTag(string data, int position, Tag tag)
        {
            if (WasEscaping(data, position))
                return false;
            var shift = tag.MD.Length;

            return (position - 1 >= 0 && data[position - 1] != ' ') &&
                   (position + shift >= data.Length || !char.IsLetterOrDigit(data[position + shift]));
        }

        private static bool WasEscaping(string data, int position)
        {
            var escapesCount = 0;
            for (var i = position - 1; i >= 0; i++)
            {
                if (data[i] != '\\')
                    break;
                escapesCount++;
            }

            return escapesCount % 2 == 1;
        }
    }
}
