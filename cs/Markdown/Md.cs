using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public static class Md
    {
        private static readonly string ParagraphDelimiter = Environment.NewLine;
        private static readonly TextWorker MdTextWorker;
        private static readonly TagCollector<ItalicTag> ItalicTagCollector;
        private static readonly TagCollector<BoldTag> BoldTagCollector;
        private static readonly TagCollector<HeaderTag> ParagraphStyleTagCollector;
        private static readonly TagCollector<UnorderedListTag> UnorderedListTagCollector;

        static Md()
        {
            MdTextWorker = TagCollectorFactory.MdTextWorker;

            ItalicTagCollector = TagCollectorFactory.CreateCollectorFor<ItalicTag>();
            BoldTagCollector = TagCollectorFactory.CreateCollectorFor<BoldTag>();
            ParagraphStyleTagCollector = TagCollectorFactory.CreateCollectorFor<HeaderTag>();
            UnorderedListTagCollector = TagCollectorFactory.CreateCollectorFor<UnorderedListTag>();
        }

        public static string Render(string textInMarkdown)
        {
            var htmlTextBuilder = new StringBuilder();
            var paragraphs = textInMarkdown.Split(ParagraphDelimiter);

            htmlTextBuilder.AppendJoin(ParagraphDelimiter, paragraphs.Select(RenderParagraph));

            return MdTextWorker.DeleteEscapeCharFromLine(htmlTextBuilder.ToString());
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

            tags.AddRange(BoldTagCollector.CollectTags(line));
            tags.AddRange(ItalicTagCollector.CollectTags(
                ReplaceTagsWithPlaceholder(line, tags)));
            tags.AddRange(ParagraphStyleTagCollector.CollectTags(line));
            tags.AddRange(UnorderedListTagCollector.CollectTags(line));

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
                }
                else
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
            return MdTextWorker.ReplaceCharsAt(line, placeholderChar, indexes);
        }
    }
}