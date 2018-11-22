using System.Collections.Generic;
using Markdown.Md.TagHandlers;
using Markdown.Renderers;

namespace Markdown.Md
{
    public static class MdSpecification
    {
        public static string Emphasis = "emphasis";
        public static string StrongEmphasis = "strong";
        public static string Text = "text";

        public static Dictionary<string, string> Tags = new Dictionary<string, string>
        {
            {Emphasis, "_"},
            {StrongEmphasis, "__"},
            {Text, ""},
        };

        public static bool IsEscape(string str, int position)
        {
            return position - 1 >= 0 && str[position - 1] == '\\';
        }

        public static TagHandler GetTagHandlerChain()
        {
            return new StrongEmphasisHandler()
                .SetSuccessor(new EmphasisHandler()
                    .SetSuccessor(new TextHandler()));
        }

        public static HtmlTagHandler GetHtmlTagHandlerChain()
        {
            return new HtmlTagHandlers.StrongEmphasisHandler()
                .SetSuccessor(new HtmlTagHandlers.EmphasisHandler()
                    .SetSuccessor(new HtmlTagHandlers.TextHandler()));
        }
    }
}