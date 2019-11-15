using System;
using System.Collections.Generic;
using Markdown.Tree;

namespace Markdown.Parser
{
    public static class TreeBuilder
    {
        public static RootNode ParseMarkdown(string markdown)
        {
            var events = EventsParser.ParseEvents(markdown);
            var root = BuildTree(events);

            return root;
        }

        private static RootNode BuildTree(IEnumerable<Event> events)
        {
            var root = new RootNode();
            Node current = root;

            foreach (var @event in events)
            {
                switch (@event.Type)
                {
                    case EventType.PlainText:
                        current.AddNode(new PlainTextNode(@event.Value));
                        break;

                    case EventType.BoldStart:
                        var bold = new BoldNode();
                        current.AddNode(bold);
                        current = bold;
                        break;


                    case EventType.ItalicStart:
                        var italic = new ItalicNode();
                        current.AddNode(italic);
                        current = italic;
                        break;

                    case EventType.ItalicEnd:
                    case EventType.BoldEnd:
                        current = current.Parent;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return root;
        }
    }
}