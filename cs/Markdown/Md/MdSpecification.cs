using System.Collections.Generic;
using Markdown.Md.TagHandlers;
using Markdown.Md.TagHandlersFactory;

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

        public static readonly Dictionary<string, string> HtmlRules = new Dictionary<string, string>
        {
            {Text, ""},
            {Emphasis, "<ul>"},
            {StrongEmphasis, "<strong>"},
        };

        public static bool IsEscape(string str, int position)
        {
            return position - 1 >= 0 && str[position - 1] == '\\';
        }

        public static TagHandler GetTagHandlerChain()
        {
            var emphasisHandler = new EmphasisHandlerFactory().Create();
            var strongEmphasisHandler = new StrongEmphasisHandlerFactory().Create();
            var textHandler = new TextHandlerFactory().Create();

            return strongEmphasisHandler.SetSuccessor(emphasisHandler.SetSuccessor(textHandler));
        }
    }
}