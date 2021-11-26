using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public static class Translator
    {
        private static Dictionary<string, HtmlTag> HtmlTags = new()
        {
            { "_", new HtmlTag("em") },
            { "__", new HtmlTag("strong") },
            { "#", new HtmlTag("h1") }
        };

        public static string Translate(IEnumerable<AnalyzedToken> tokens)
        {
            var result = new StringBuilder();
            var sentenceMods = new Stack<string>();

            foreach (var token in tokens)
            {
                var value = token.Value;

                if (token.IsTag)
                {
                    if (token.IsSentenceModifer)
                    {
                        result.Append(HtmlTags[value].Opened);
                        sentenceMods.Push(value);
                        continue;
                    }
                    
                    result.Append(token.IsOpener
                        ? HtmlTags[value].Opened
                        : HtmlTags[value].Closing);
                    continue;
                }

                result.Append(value);
            }

            while (sentenceMods.Count != 0)
            {
                var tag = sentenceMods.Pop();
                result.Append(HtmlTags[tag].Closing);
            }

            return result.ToString();
        }
    }
}