using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Parsers.Tags;

namespace Markdown.Parsers
{
    public class MarkdownParser
    {
        private readonly List<ITag> textTags = new List<ITag>();
        private readonly Queue<ITag> opendTag = new Queue<ITag>();

        private int position;
        private readonly NewLineTag newLineTag = new NewLineTag();
        public MarkdownParser(string markdownText)
        {
            var lines = markdownText.Split(newLineTag.ToString());
            foreach (var line in lines)
            {
                var lineTags = TransformToTags(line);
                textTags.AddRange(lineTags);
            }
        }

        private List<ITag> TransformToTags(string line)
        {
            var tags = new List<ITag>();
            position = 0;

            do
            {
                tags.Add(GetNextTagOf(line));
            } 
            while (position++ < line.Length);

            DeleteNotValidTagsIn(tags);

            tags.Add(newLineTag); // TODO: как не добавить лишний таг в конце, но не испортить код?

            return tags;
        }

        private ITag GetNextTagOf(string line)
        {
            return IsServiceSymbol(line[position]) ? 
                GetServiceTag(line) : 
                GetTextTag(line);
        }

        private bool IsServiceSymbol(char symbol) =>
            throw new NotImplementedException();

        private ITag GetServiceTag(string line) =>
            throw new NotImplementedException();

        private ITag GetTextTag(string line) =>
            throw new NotImplementedException();

        private void DeleteNotValidTagsIn(List<ITag> tags)
        {
            for (int i = 0; i < tags.Count; i++)
                if (opendTag.Contains(tags[i]))
                    tags[i] = tags[i].ToText();

            opendTag.Clear();
        }

        public List<ITag> GetTags() => textTags;
    }
}
