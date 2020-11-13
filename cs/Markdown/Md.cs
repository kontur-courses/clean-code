using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public static class Md
    {
        private static readonly string paragraphDelimiter = Environment.NewLine;
        private static readonly TextWorker textWorker;
        private static readonly TextStyleTagCollector<ItalicTag> italicTagCollector;
        private static readonly TextStyleTagCollector<BoldTag> boldTagCollector;
        private static readonly LikeHeaderTagCollector<HeaderTag> LikeHeaderTagCollector;
        private static readonly LikeHeaderTagCollector<UnorderedListTag> unorderedListTagCollector;

        static Md()
        {
            var beginingOfMdTags = new[] {'_', '#', '*'};
            textWorker = new TextWorker(beginingOfMdTags);

            italicTagCollector = new TextStyleTagCollector<ItalicTag>(textWorker);
            boldTagCollector = new TextStyleTagCollector<BoldTag>(textWorker);
            LikeHeaderTagCollector = new LikeHeaderTagCollector<HeaderTag>(textWorker);
            unorderedListTagCollector = new LikeHeaderTagCollector<UnorderedListTag>(textWorker);
        }

        public static string Render(string lineInMarkdown)
        {
            var htmlLineBuilder = new StringBuilder();
            var paragraphs = lineInMarkdown.Split(paragraphDelimiter);

            htmlLineBuilder.AppendJoin(paragraphDelimiter, paragraphs.Select(RenderParagraph));

            return textWorker.DeleteEscapeCharFromLine(htmlLineBuilder.ToString());
        }

        private static string RenderParagraph(string paragraph)
        {
            if (string.IsNullOrEmpty(paragraph))
                return string.Empty;

            var tags = GetTags(paragraph);

            return MdToHtml(paragraph, tags);
        }

        private static List<Tag> GetTags(string line)
        {
            var tags = new List<Tag>();

            tags.AddRange(boldTagCollector.CollectTags(line));
            tags.AddRange(italicTagCollector.CollectTags(
                ReplaceTagsWithPlaceholder(line, tags)));
            tags.AddRange(LikeHeaderTagCollector.CollectTags(line));
            tags.AddRange(unorderedListTagCollector.CollectTags(line));

            return PrepareTags(tags);
        }

        private static string MdToHtml(string line, List<Tag> tags)
        {
            var renderLine = new StringBuilder();
            for (int i = 0; i < line.Length; i++)
            {
                var tagToReplace = tags.Find(tagToFind => 
                    tagToFind.StartOfOpeningTag == i || tagToFind.StartOfClosingTag == i);

                if (tagToReplace != null)
                {
                    renderLine.Append(i == tagToReplace.StartOfOpeningTag ?
                        tagToReplace.OpeningHtmlTag :
                        tagToReplace.ClosingHtmlTag);
                    i += tagToReplace.MdTag.Length - 1;
                    continue;
                }

                renderLine.Append(line[i]);
            }

            var headerLikeTag = tags.Find(tag => tag is HeaderTag || tag is UnorderedListTag);
            if (headerLikeTag != null)
                renderLine.Append(headerLikeTag.ClosingHtmlTag);

            return renderLine.ToString();
        }

        private static List<Tag> PrepareTags(List<Tag> tags)
        {
            tags.RemoveAll(tagToRemove =>
                tagToRemove is BoldTag && tags.Any(tag =>
                    tag is ItalicTag && tag.Contains(tagToRemove)));

            foreach (var italicTag in tags.OfType<ItalicTag>().ToArray())
            {
                var intersectingTag = tags.Find(otherTag =>
                    otherTag is BoldTag && otherTag.IntersectWith(italicTag));

                if (intersectingTag == null)
                    continue;

                tags.Remove(italicTag);
                tags.Remove(intersectingTag);
            }

            return tags;
        }

        private static string ReplaceTagsWithPlaceholder(
            string line, IEnumerable<Tag> tags, char placeholderChar = '/')
        {
            var indexes = tags.SelectMany(tag => tag.OccupiedIndexes).ToArray();
            return textWorker.ReplaceCharsAt(line, placeholderChar, indexes);
        }
    }
}