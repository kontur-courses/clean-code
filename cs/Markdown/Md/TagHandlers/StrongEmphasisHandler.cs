using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.Md.TagHandlers
{
    public class StrongEmphasisHandler : TagHandler
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

            if (result.PairType == TokenPairType.Open
                && openingTokenNodes.Count != 0
                && openingTokenNodes.First()
                    .Type == MdSpecification.Emphasis)
            {
                result.Type = MdSpecification.Text;
                result.PairType = TokenPairType.NotPair;
            }

            return result;
        }

        private static TokenNode Recognize(string str, int position)
        {
            if (MdSpecification.IsEscape(str, position))
            {
                return null;
            }

            if (IsStrongEmphasis(str, position)
                && IsClosedEmphasis(str, position)
                && (position + 2 >= str.Length || str[position + 2] != '_'))
            {
                return new TokenNode(MdSpecification.StrongEmphasis, "", TokenPairType.Close);
            }

            if (IsStrongEmphasis(str, position)
                && IsOpenedEmphasis(str, position + 1)
                && (position - 1 < 0 || str[position - 1] == ' '))
            {
                return new TokenNode(MdSpecification.StrongEmphasis, "", TokenPairType.Open);
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

        private static bool IsStrongEmphasis(string str, int position)
        {
            return position + 1 < str.Length
                && str[position] == '_'
                && str[position + 1] == '_';
        }
    }
}