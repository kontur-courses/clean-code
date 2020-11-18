using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class MdRenderer
    {
        public string Render(string markdown)
        {
            var htmlCode = new StringBuilder();

            foreach (var line in markdown.Split('\n'))
            {
                var tagTokens = TagTokensParser.ReadTagsFromLine(line);
                TagTokensParser.RemoveIncorrectTokens(line, tagTokens);
                htmlCode.Append(RenderLine(line, tagTokens.OrderBy(token => token.StartPosition)));
            }

            return htmlCode.ToString();
        }

        private string RenderLine(string line, IEnumerable<TagToken> tokens)
        {
            var rendered = new StringBuilder(line);
            var replacements = GetTagToHtmlReplacements(tokens).OrderBy(x => x.Position);
            var shift = 0;

            foreach (var replacement in replacements)
            {
                if (replacement.Type is TagType.Shield)
                {
                    rendered.Remove(replacement.Position + shift, 1);
                    shift--;
                    continue;
                }

                if (replacement.Position + shift < rendered.Length)
                {
                    rendered.Remove(replacement.Position + shift, replacement.TagSignLength);
                    rendered.Insert(replacement.Position + shift, replacement.NewValue);
                }
                else
                    rendered.Append(replacement.NewValue);

                shift += replacement.NewValue.Length - replacement.TagSignLength;
            }

            return rendered.ToString();
        }

        private IEnumerable<TagToHtmlReplacement> GetTagToHtmlReplacements(IEnumerable<TagToken> tokens)
        {
            foreach (var token in tokens)
            {
                if (token.Type is TagType.NonTag)
                    throw new Exception("NonTag cannot be replaced");

                yield return new TagToHtmlReplacement(token.StartPosition, token.Type, false);
                if(token.Type != TagType.Shield)
                    yield return new TagToHtmlReplacement(token.EndPosition, token.Type, true);
            }
        }
    }
}