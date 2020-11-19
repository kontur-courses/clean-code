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
            var replacements = GetTagToHtmlReplacements(tokens).OrderBy(x => x.Position);
            var index = 0;

            foreach (var replacement in replacements)
            {
                if (index < replacement.Position)
                {
                    var s = line.Substring(index, replacement.Position - index);
                    rendered.Append(s);
                    index += s.Length;
                }

                rendered.Append(replacement.NewValue);
                index += replacement.ReplacedValueLength;
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