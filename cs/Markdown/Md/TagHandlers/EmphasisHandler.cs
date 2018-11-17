using System;

namespace Markdown.Md.TagHandlers
{
    public class EmphasisHandler : MdTagHandler
    {
        public override MdToken Handle(string str, int position)
        {
            var type = Recognize(str, position);

            if (type == MdType.Text)
            {
                if (Successor == null)
                {
                    throw new InvalidOperationException(
                        "Can't transfer control to the next chain element because it was null");
                }

                return Successor.Handle(str, position);
            }

            return new MdToken(type, MdSpecification.Tags[type]);
        }

        private static MdType Recognize(string str, int position)
        {
            if (MdSpecification.IsEscape(str, position))
            {
                return MdType.Text;
            }

            if (IsStrongEmphasis(str, position)
                && IsClosedEmphasis(str, position)
                && (position + 2 >= str.Length || str[position + 2] != '_'))
            {
                return MdType.CloseStrongEmphasis;
            }

            if (IsStrongEmphasis(str, position)
                && IsOpenedEmphasis(str, position + 1)
                && (position - 1 < 0 || str[position - 1] == ' '))
            {
                return MdType.OpenStrongEmphasis;
            }

            if (IsEmphasis(str, position)
                && IsClosedEmphasis(str, position)
                && (position - 1 < 0 || str[position - 1] != '_'))
            {
                return MdType.CloseEmphasis;
            }

            if (IsEmphasis(str, position)
                && IsOpenedEmphasis(str, position))
            {
                return MdType.OpenEmphasis;
            }

            return MdType.Text;
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

        private static bool IsStrongEmphasis(string str, int position)
        {
            return position + 1 < str.Length
                && str[position] == '_'
                && str[position + 1] == '_';
        }
    }
}