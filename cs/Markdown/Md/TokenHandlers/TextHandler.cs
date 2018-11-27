using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Markdown.Md.TagHandlers
{
    public class TextHandler : TokenHandler
    {
        public override Token Handle(string str, int position, ImmutableStack<Token> openingTokens)
        {
            if (IsText(str, position, out var result))
            {
                if (result == "")
                {
                    result += str[position];
                }

                return new Token(MdSpecification.Text, result);
            }

            Successor?.Handle(str, position, openingTokens);

            return new Token(MdSpecification.Text, "");
        }

        public static bool IsText(string str, int position, out string result)
        {
            result = string.Empty;
            var sb = new StringBuilder();

            for (var i = position; i < str.Length; i++)
            {
                if (MdSpecification.Tags
                    .ContainsValue(str[i]
                        .ToString()))
                {
                    break;
                }

                sb.Append(str[i]);
            }

            result = sb.ToString();

            return true;
        }
    }
}