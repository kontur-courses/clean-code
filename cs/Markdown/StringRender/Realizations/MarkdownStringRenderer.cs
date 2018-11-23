using Markdown.Realizations;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class MarkdownStringRenderer : StringRenderer
    {
        public MarkdownStringRenderer() : base(new MarkdownTokenSelector())
        {
            var emTag = new Tag("em");
            var strongTag = new Tag("strong");
            strongTag.TagDependencies.Add(upperTags => !upperTags.Contains(emTag));
            Tags.Add("__", strongTag);
            Tags.Add("_", emTag);
        }
        
        public override void HandleTags(IEnumerable<Token> tags, List<Tag> upperTags)
        {
            var OpenTag = tags.FirstOrDefault(token => token.IsOpen);
            if (OpenTag == null)
                return;

            var CloseTag = tags.FirstOrDefault(token =>
                token.PosibleTag == OpenTag.PosibleTag && token.IsClose && token != OpenTag);
            if (CloseTag == null)
            {
                HandleTags(tags.SkipWhile(token => token != OpenTag).Skip(1), upperTags);
            }
            else
            {
                upperTags.Add(OpenTag.PosibleTag);
                HandleTags(
                    tags.SkipWhile(t => t != OpenTag).Skip(1).Reverse().SkipWhile(t => t != CloseTag).Skip(1).Reverse(),
                    upperTags);
                upperTags.RemoveRange(upperTags.Count - 1, 1);
                HandleTags(tags.SkipWhile(t => t != CloseTag).Skip(1), upperTags);
                if (OpenTag.PosibleTag.IsValidTag(upperTags))
                {
                    OpenTag.Value = "<" + OpenTag.PosibleTag.HtmlRepresentation + ">";
                    CloseTag.Value = "</" + CloseTag.PosibleTag.HtmlRepresentation + ">";
                }
            }
        }
    }
}