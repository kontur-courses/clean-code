using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarkDown.TagParsers;
using MarkDown.Tokens;

namespace MarkDown
{
    public static class TagTokenizer
    {
        public static IEnumerable<MdToken> DivideTokensByTag(IEnumerable<MdToken> tokens, Tag tag)
        {
            foreach (var token in tokens.ToArray())
            {
                if (token.TokenType == TokenTypes.String)
                {
                    foreach (var nestedToken in ParseLineOnMdTokens(token.Value, tag))
                    {
                        yield return nestedToken;
                    }
                }
                else
                    yield return token;
            }
        }

        private static IEnumerable<MdToken> ParseLineOnMdTokens(string line, Tag tag)
        {
            var reader = new MdStringReader(line);
            var builder = new StringBuilder();
            while (reader.HasNext())
            {
                var substr = new string(reader.HasNextChars(tag.MdTag.Length) ? reader.LookNextChars(tag.MdTag.Length) : null);
                if (substr.Equals(tag.MdTag) && NotContainEscapeSequence(reader))
                {
                    if (builder.Length != 0)
                    {
                        yield return new StringToken(builder.ToString(), builder.Length);
                        builder.Clear();
                    }
                    yield return new TagToken(tag);
                    reader.ShiftPointer(tag.MdTag.Length);
                }
                else
                {
                    builder.Append(reader.LookAhead());
                    reader.ShiftPointer();
                }
            }
            if (builder.Length > 0)
                yield return new StringToken(builder.ToString(), builder.Length);
        }

        private static bool NotContainEscapeSequence(IMdReader reader)
        {
            if (reader.HasPreviousChars(2))
            {
                var previous = reader.LookPreviousChars(2);
                return previous[1] != '\\' || previous[0] == '\\' && previous[1] == '\\';
            }
            return !reader.HasPrevious() || reader.LookBehind() != '\\';
        }
    }
}