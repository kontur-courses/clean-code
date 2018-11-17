namespace Markdown.Md.TagHandlers
{
    public class TextHandler : MdTagHandler
    {
        public override MdToken Handle(string str, int position)
        {
            if (IsText(str, position, out var result))
            {
                if (result == "")
                {
                    result += str[position];
                }

                return new MdToken(MdType.Text, result);
            }

            Successor?.Handle(str, position);

            return new MdToken(MdType.Text, "");
        }

        public static bool IsText(string str, int position, out string result)
        {
            result = string.Empty;

            for (var i = position; i < str.Length; i++)
            {
                if (MdSpecification.Tags
                    .ContainsValue(str[i]
                        .ToString()))
                {
                    break;
                }

                result += str[i];
            }

            return true;
        }
    }
}