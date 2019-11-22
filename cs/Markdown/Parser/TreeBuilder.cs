using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Parser.Tags;
using Markdown.Parser.TagsParsing;
using Markdown.Tools;
using Markdown.Tree;

namespace Markdown.Parser
{
    public class TreeBuilder
    {
        private readonly List<MarkdownTag> tags;
        private readonly CharClassifier classifier;

        public TreeBuilder()
        {
            tags = new List<MarkdownTag> {new BoldTag(), new ItalicTag()};
            classifier = new CharClassifier(tags.SelectMany(t => t.String));
        }

        public RootNode ParseMarkdown(string markdown)
        {
            var tagsReader = new TagsReader(markdown, tags, classifier);
            var events = tagsReader.GetEvents();

            var pairEvents = Filter.PairEvents(events);
            var root = BuildTree(pairEvents, markdown);

            return root;
        }

        private RootNode BuildTree(List<TagEvent> events, string markdown)
        {
            var root = new RootNode();
            Node currentNode = root;

            //чтобы учесть текст до первого и после последнего ивента
            events.Insert(0, new TagEvent(0, TagEventType.Start, EmptyTag.Instance));
            events.Add(new TagEvent(markdown.Length, TagEventType.End, EmptyTag.Instance));

            var count = events.Count;

            for (var i = 1; i < count; i++)
            {
                var current = events[i];
                var previous = events[i - 1];
                var start = previous.Index + previous.Tag.String.Length;
                var end = current.Index;
                var len = end - start;

                if (len > 0)
                {
                    var plainText = RemovedEscape(markdown.Substring(start, len));
                    currentNode.AddNode(new PlainTextNode(plainText));
                }

                if (i == count - 1)
                    continue;

                if (current.Type != TagEventType.End)
                {
                    var node = current.Tag.CreateNode();
                    currentNode.AddNode(node);
                    currentNode = node;
                    continue;
                }

                currentNode = currentNode.Parent;
            }

            return root;
        }


        private string RemovedEscape(string str)
        {
            var result = new StringBuilder();
            var length = str.Length;

            for (var i = 0; i < length - 1; i++)
            {
                if (classifier.GetType(str[i]) != CharType.Escape
                    || classifier.GetType(str[i + 1]) != CharType.TagSymbol)
                {
                    result.Append(str[i]);
                }
            }

            result.Append(str[length - 1]);

            return result.ToString();
        }
    }
}