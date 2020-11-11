using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Markdown
{
    public class Md
    {
        public static string Render(string markdown)
        {
            var renderedMarkdown = new StringBuilder();
            foreach (var paragraph in GetParagraphs(markdown))
            {
                renderedMarkdown.Append(RenderParagraph(paragraph));
            }

            return renderedMarkdown.ToString();
        }

        private static string RenderParagraph(string paragraph)
        {
            var tags = GetTags(paragraph);
            var textWithouttTags = DeleteTags(paragraph, tags);
            return InsertHtmlTags(textWithouttTags, tags.ToArray());
        }

        private static IEnumerable<Tag> GetTags(string text)
        {
            var tags = GetTagsWithEscapedTagsFromText(text);
            tags = GetTagsWithoutEscapedTags(tags);

            return DeleteNotPairTags(tags);
        }

        private static List<Tag> GetTagsWithoutEscapedTags(List<Tag> tags)
        {
            var tagsWithoutEscapedTags = new List<Tag>();
            for (var i = 0; i < tags.Count; i++)
            {
                if (tags[i].TagName == "\\" && tags.Count > i + 1 && tags[i].Position + 1 == tags[i + 1].Position)
                {
                    tagsWithoutEscapedTags.Add(tags[i]);
                    i++;
                }
                else if (tags[i].TagName != "\\")
                    tagsWithoutEscapedTags.Add(tags[i]);
            }

            return tagsWithoutEscapedTags;
        }

        private static List<Tag> GetTagsWithEscapedTagsFromText(string text)
        {
            var openTags = new HashSet<string>();
            var tags = new List<Tag>();
            Tag tag;
            for (var i = 0; i < text.Length; i += tag?.TagLenght ?? 1)
            {
                tag = GetTag(text, i);
                if (tag == null) continue;
                if (IsWrongTag(text, tag, openTags, i)) continue;

                tags.Add(tag);
            }

            return tags;
        }

        private static bool IsWrongTag(string text, Tag tag, HashSet<string> openTags, int i)
        {
            if (tag.TagName == "\\" || tag.TagName == "#") return false;
            if (openTags.Contains("_") && tag.TagName == "__") return true;

            if (openTags.Contains(tag.TagName))
            {
                if (text[i - 1] == ' ')
                    return true;
                openTags.Remove(tag.TagName);
            }
            else
            {
                if (text.Length > i + 1 && text[i + 1] == ' ')
                    return true;
                openTags.Add(tag.TagName);
            }

            return false;
        }

        private static IEnumerable<Tag> DeleteNotPairTags(List<Tag> tags)
        {
            var tagCount = new Dictionary<string, int>();
            foreach (var tag in tags.Where(tag => tag.IsPairTag && tag.TagName != "\\"))
            {
                if (tagCount.ContainsKey(tag.TagName))
                    tagCount[tag.TagName]++;
                else
                    tagCount.Add(tag.TagName, 1);
            }

            foreach (var index in tagCount.Where(x => x.Value % 2 != 0)
                .Select(x => tags.FindLastIndex((tag => tag.TagName == x.Key))))
            {
                tags.RemoveAt(index);
            }

            return tags;
        }

        private static Tag GetTag(string text, int position)
        {
            if (text[position] == '#') return new Tag("#", position, false);
            if (text[position] == '\\') return new Tag("\\", position);
            if (text[position] != '_') return null;
            var tagLength = 1;
            if (text.Length > position + 1 && text[position + 1] == '_') tagLength++;

            if (position != 0 && char.IsDigit(text[position - 1])) return null;
            if (text.Length > position + tagLength && char.IsDigit(text[position + tagLength])) return null;

            if (position != 0
                && text.Length > position + tagLength
                && text[position - 1] == ' '
                && text[position + tagLength] == ' ')
                return null;


            return tagLength == 2 ? new Tag("__", position) : new Tag("_", position);
        }

        private static string DeleteTags(string text, IEnumerable<Tag> tags)
        {
            var textWithoutTags = new StringBuilder(text);
            var tagOfset = 0;
            foreach (var tag in tags)
            {
                textWithoutTags.Remove(tag.Position - tagOfset, tag.TagLenght);
                tagOfset += tag.TagLenght;
            }

            return textWithoutTags.ToString();
        }

        private static IEnumerable<string> GetParagraphs(string text)
        {
            var paragraph = new StringBuilder();
            for (var i = 0; i < text.Length; i++)
            {
                if (paragraph.Length >= 2
                    && text[i] != '\n'
                    && text[i - 1] == '\n'
                    && text[i - 2] == '\n')
                {
                    yield return paragraph.ToString();
                    paragraph.Clear();
                    paragraph.Append(text[i]);
                }
                else
                {
                    paragraph.Append(text[i]);
                }
            }

            if (paragraph.Length != 0)
                yield return paragraph.ToString();
        }

        private static string InsertHtmlTags(string text, Tag[] tags)
        {
            var textWithHtmlTags = new StringBuilder();
            var openTags = new HashSet<string>();
            var tagIndex = 0;
            var ofset = 0;
            for (var i = 0; i < text.Length; i++)
            {
                if (tags.Length > tagIndex && tags[tagIndex].Position == i + ofset)
                {
                    var tagName = tags[tagIndex].TagName;
                    var isOpenTag = !openTags.Contains(tagName);
                    if (!openTags.Remove(tagName))
                    {
                        openTags.Add(tagName);
                    }

                    if (tagName == "#")
                        isOpenTag = true;
                    textWithHtmlTags.Append(tags[tagIndex].BuildHtmlTag(isOpenTag));
                    ofset += tags[tagIndex].TagLenght;
                    tagIndex++;
                }

                textWithHtmlTags.Append(text[i]);
            }

            if (tags.Length > tagIndex)
                textWithHtmlTags.Append(tags[tagIndex].BuildHtmlTag(false));

            if (openTags.Contains("#"))
                textWithHtmlTags.Append(new Tag("#", 0).BuildHtmlTag(false));

            return textWithHtmlTags.ToString();
        }
    }
}