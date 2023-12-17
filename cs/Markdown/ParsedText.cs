using Markdown.Tags;

namespace Markdown
{
    public class ParsedText
    {
        public string paragraph { get; }
        public List<ITag> tags { get; }
        
        public ParsedText(string paragraph, List<ITag> tags)
        {
            if (paragraph is null || tags is null || tags.Any(x => x is null))
                throw new ArgumentException("Arguments and tags can not be null");
            if (tags.Any(x => x.Position > paragraph.Length))
                throw new ArgumentException("Tag position cannot be beyond paragraph", nameof(tags));
            for (var i = 1; i < tags.Count; i++)
                if (tags[i - 1].Position > tags[i].Position)
                    throw new ArgumentException("Tags positions should be ordered by ascending");
            this.paragraph = paragraph;
            this.tags = tags;
        }
    }
}
