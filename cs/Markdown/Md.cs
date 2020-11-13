using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class Md
    {
        public string Render(string text)
        {
            var markdownParser = new TagParser(text);
            var allTags = markdownParser.GetTags();
            return MarkdownToHtmlConverter.Convert(text, GetCorrectTags(allTags, text));
        }

        private List<Tag> PairWordTags(IEnumerable<WordTag> wordTags, string text)
        {
            var openTags = new Dictionary<Type, WordTag>();
            var result = new List<Tag>();
            foreach (var wordTag in wordTags)
            {
                var tagType = wordTag.GetType();
                openTags.TryGetValue(tagType, out var openTag);
                if (openTag == null)
                {
                    if (wordTag.CanBeOpen(text))
                        openTags[tagType] = wordTag;
                }
                else
                {
                    if (openTag.TryPairCloseTag(wordTag, text))
                    {
                        result.Add(openTag);
                        result.Add(wordTag);
                        openTags[tagType] = null;
                    }
                    else if (openTag.InWord(text) && !openTag.InOneWordWith(wordTag, text)
                    || !openTag.InOneParagraphWith(wordTag, text))
                        openTags[tagType] = null;
                }
            }

            return result;
        }

        private IEnumerable<Tag> GetCorrectTags(IEnumerable<Tag> tags, string text)
        {
            var pairedWordTags = PairWordTags(tags.Where(x => x.GetType().BaseType == typeof(WordTag))
                .Select(x => (WordTag) x), text);
            var tagsWithoutIntersecting = RemoveIntersectingPairTags(pairedWordTags);
            var tagsWithoutNestingBold = RemoveBoldTagsInItalicTags(tagsWithoutIntersecting);
            var italicTags = tagsWithoutNestingBold.Where(x => x is ItalicTag);
            var boldTags = RemoveEmptyBoldTags(tagsWithoutNestingBold);
            var escapeTags = tags.Where(x => x is EscapeTag);
            var headerTags = tags.Where(x => x is HeaderTag);
            return italicTags.Concat(boldTags).Concat(escapeTags).Concat(headerTags);
        }
        
        private IEnumerable<Tag> RemoveIntersectingPairTags(IEnumerable<Tag> tags)
        {
            var openTags = new Stack<Tag>();
            var result = new List<Tag>();
            foreach (var tag in tags.OrderBy(x=>x.Position))
            {
                if (!openTags.TryPeek(out var openTag) || openTag.GetType()!= tag.GetType())
                {
                    openTags.Push(tag);
                }
                else
                {
                    result.Add(openTags.Pop());
                    result.Add(tag);
                }
            }

            return result;
        }

        private IEnumerable<Tag> RemoveBoldTagsInItalicTags(IEnumerable<Tag> tags)
        {
            var result = new List<Tag>();
            var inPair = false;
            foreach (var tag in tags.OrderBy(x=>x.Position))
            {
                if (tag is ItalicTag)
                {
                    inPair = !inPair;
                    result.Add(tag);
                }
                else if (!inPair)
                {
                    result.Add(tag);
                }
            }

            return result;
        }
        
        private IEnumerable<Tag> RemoveEmptyBoldTags(IEnumerable<Tag> tags)
        {
            var result = new List<Tag>();
            Tag openTag = null;
            foreach (var boldTag in tags.OrderBy(x=>x.Position))
            {
                if (openTag == null)
                {
                    openTag = boldTag;
                }
                else
                {
                    if (boldTag.Position - openTag.Position != openTag.MdTag.Length)
                    {
                        result.Add(openTag);
                        result.Add(boldTag);
                    }

                    openTag = null;
                }
            }

            return result;
        }
    }
}