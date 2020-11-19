using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public static class MdRenderer
    {
        public static string Render(string markdown)
        {
            var htmlCode = new StringBuilder();

            foreach (var line in markdown.Split('\n'))
            {
                var tagTokens = TagTokensParser.GetCorrectTagTokens(line);
                htmlCode.Append(RenderLine(line, tagTokens));
            }

            return htmlCode.ToString();
        }

        private static string RenderLine(string line, IEnumerable<TagToken> tokens)
        {
            var rendered = new StringBuilder();
            var replacements =
                new Queue<TagToHtmlReplacement>(GetTagToHtmlReplacements(tokens).OrderBy(x => x.Position));
            var index = 0;

            if (replacements.Count == 0)
                return line;

            while (replacements.Count > 0)
            {
                var processingReplacement = replacements.Dequeue();
                if (index < processingReplacement.Position)
                {
                    var s = line.Substring(index, processingReplacement.Position - index);
                    rendered.Append(s);
                    index += s.Length;
                }

                rendered.Append(processingReplacement.NewValue);
                index += processingReplacement.ReplacedValueLength;
            }

            if (index < line.Length)
                rendered.Append(line.Substring(index, line.Length - index));

            return rendered.ToString();
        }

        private static IEnumerable<TagToHtmlReplacement> GetTagToHtmlReplacements(IEnumerable<TagToken> tokens)
        {
            foreach (var token in tokens)
            {
                if (token.Type is TagType.NonTag)
                    throw new Exception("NonTag cannot be replaced");

                yield return new TagToHtmlReplacement(token, false);
                yield return new TagToHtmlReplacement(token, true);
            }
        }
    }
}