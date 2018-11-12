using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Md
    {
        private readonly List<Tag> tags;
        private int i;
        private Tag emptyTag = new Tag("", "", "", "");

        public Md()
        {
            tags = new List<Tag>
            {
                new Tag("_", "_", "<em>", @"<\em>"),
                new Tag("__", "__", "<strong>", @"<\strong>")
            };
        }

        public string Render(string markdownString)
        {
            i = 0;
            return ParseWithoutRegexp(markdownString);
        }

        private string ParseWithoutRegexp(string markdownString)
        {
            var builder = new StringBuilder();
            var mainSpan = new Span(emptyTag, 0, markdownString.Length - 1);
            var openedSpans = new List<Span>();

            for (; i < markdownString.Length; i++)
            {
                
                if (markdownString[i] == '\\')
                {
                    mainSpan.Spans.Add(new Span(emptyTag, i, i + 1));
                    i += 1;
                    continue;
                }

                if (openedSpans.Count != 0)
                {
                    var found = FindSpanEnd(markdownString, openedSpans);
                    if (found)
                    {
                        mainSpan.Spans.AddRange(openedSpans.Where(t => t.EndIndex != 0));
                        openedSpans = openedSpans.Where(t => t.EndIndex == 0).ToList();
                        continue;
                    }
                }

                var newToken = FindStartToken(markdownString);
                if (newToken != null)
                {
                    openedSpans.Add(newToken);
                }
            }
            
            return mainSpan.Assembly(markdownString);
        }

        private Span FindStartToken(string markdownString)
        {
            var possibleTags = tags.Where(tag => markdownString[i] == tag.MarkdownStart[0]).ToList();

            if (possibleTags.Count == 0)
                return null;
            
            for (var j = 1; possibleTags.Count > 1; j++)
            {
                var posTags = new List<Tag>();
                var shortTags = new List<Tag>();
                foreach (var tag in possibleTags)
                {
                    if (tag.MarkdownStart.Length <= j)
                        shortTags.Add(tag);
                    if (tag.MarkdownStart.Length > j && tag.MarkdownStart[j] == markdownString[i + j])
                    {
                        posTags.Add(tag);
                    }
                }

                if (posTags.Count == 0)
                    posTags = shortTags;

                possibleTags = posTags;
            }

            if (possibleTags.Count != 1)
                return null;

            var startIndex = i;
            i = i + possibleTags[0].MarkdownStart.Length - 1;
            return new Span(possibleTags[0], startIndex);
            
        }
        private bool FindSpanEnd(string markdownString, List<Span> tokens)
        {
            var possibleTokens = tokens.Where(token => markdownString[i] == token.Tag.MarkdownEnd[0]).ToList();

            if (possibleTokens.Count == 0)
                return false;

            for (var j = 1; possibleTokens.Count > 1; j++)
            {
                var posTokens = new List<Span>();
                var shortTokens = new List<Span>();
                foreach (var token in possibleTokens)
                {
                    if (token.Tag.MarkdownEnd.Length <= j)
                        shortTokens.Add(token);
                    if (token.Tag.MarkdownEnd.Length > j && token.Tag.MarkdownEnd[j] == markdownString[i + j])
                    {
                        posTokens.Add(token);
                    }
                }

                if (posTokens.Count == 0)
                    posTokens = shortTokens;

                possibleTokens = posTokens;
            }

            if (possibleTokens.Count != 1)
                return false;
            
            var possibleToken = possibleTokens[0];
            possibleToken.EndIndex = i;
            i = i + possibleToken.Tag.MarkdownEnd.Length - 1;
            return true;
        }
    }
}
