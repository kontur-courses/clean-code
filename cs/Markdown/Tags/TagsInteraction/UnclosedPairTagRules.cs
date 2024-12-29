namespace Markdown.Tags.TagsInteraction
{
    public class UnclosedPairTagsRules
    {
        public List<Tag> Apply(List<Tag> unclosedTags)
        {
            var closedTags = new List<Tag>();
            var current = unclosedTags.Count - unclosedTags.Count % 4;
            if (current < unclosedTags.Count)
            {
                var tag = unclosedTags[current];
                if (unclosedTags.Count - current == 2)
                {
                    if (tag is Italic)
                        closedTags.Add(new Italic(tag.MarkdownText, tag.TagStart));
                    else
                        closedTags.Add(new Italic(tag.MarkdownText, tag.TagStart + 1));
                }
                else if (unclosedTags.Count - current == 3)
                {
                    if (tag is Italic)
                    {
                        closedTags.Add(new Italic(tag.MarkdownText, tag.TagStart));
                        closedTags.Add(new Italic(tag.MarkdownText, tag.TagStart + 1));
                    }
                    else
                    {
                        closedTags.Add(new Bold(tag.MarkdownText, tag.TagStart));
                    }
                }
            }
            return closedTags;
        }
    }
}
