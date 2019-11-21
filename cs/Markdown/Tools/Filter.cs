using System.Collections.Generic;
using System.Linq;
using Markdown.Parser.TagsParsing;

namespace Markdown.Tools
{
    public static class Filter
    {
        public static List<TagEvent> PairEvents(List<TagEvent> events)
        {
            var result = new List<TagEvent>();
            var stack = new Stack<TagEvent>();

            foreach (var @event in events.OrderBy(r => r.Index))
            {
                if (@event.Type == TagEventType.Start)
                {
                    stack.Push(@event);
                }
                else
                {
                    if (stack.Count == 0 || stack.Peek().Tag.GetType() != @event.Tag.GetType())
                        continue;

                    var start = stack.Pop();

                    result.Add(start);
                    result.Add(@event);
                }
            }

            return result.OrderBy(x => x.Index).ToList();
        }
    }
}