using System;
using System.Collections.Generic;

namespace Markdown.Md.TagHandlers
{
    public class EmphasisHandler : TagHandler
    {
        public override TokenNode Handle(string str, int position, IReadOnlyCollection<ITokenNode> openingTokenNodes)
        {
            var result = Recognize(str, position);

            if (result == null)
            {
                if (Successor == null)
                {
                    throw new InvalidOperationException(
                        "Can't transfer control to the next chain element because it was null");
                }

                return Successor.Handle(str, position, openingTokenNodes);
            }

            result.Value = MdSpecification.Tags[result.Type];

            return result;
        }

        private static TokenNode Recognize(string str, int position)
        {
            if (MdSpecification.IsEscape(str, position))
            {
                return null;
            }

            if (IsEmphasis(str, position)
                && IsClosedEmphasis(str, position)
                && (position - 1 < 0 || str[position - 1] != '_'))
            {
                return new TokenNode(MdSpecification.Emphasis, "", TokenPairType.Close);
            }

            if (IsEmphasis(str, position)
                && IsOpenedEmphasis(str, position))
            {
                return new TokenNode(MdSpecification.Emphasis, "", TokenPairType.Open);
            }

            return null;
        }

        private static bool IsOpenedEmphasis(string str, int position)
        {
            return
                position + 1 < str.Length
                && !char.IsWhiteSpace(str[position + 1])
                && !char.IsNumber(str[position + 1])
                && (position - 1 < 0 || !char.IsLetter(str[position - 1]));
        }

        private static bool IsClosedEmphasis(string str, int position)
        {
            return
                position - 1 >= 0
                && !char.IsWhiteSpace(str[position - 1])
                && !char.IsNumber(str[position - 1])
                && (position + 1 >= str.Length || !char.IsLetter(str[position + 1]));
        }

        private static bool IsEmphasis(string str, int position)
        {
            return str[position] == '_';
        }
    }
}