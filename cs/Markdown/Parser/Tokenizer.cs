using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Parser.Tags;
using Markdown.Parser.TagsParsing;
using Markdown.Tools;

namespace Markdown.Parser
{
    public class Tokenizer
    {
        private readonly string text;
        private readonly List<TagEvent> events;
        private readonly CharClassifier classifier;

        public Tokenizer(string text, List<TagEvent> events, CharClassifier classifier)
        {
            this.text = text;
            this.events = events;
            this.classifier = classifier;
        }

        public IEnumerable<Token> GetTokens()
        {
            var tokens = new List<Token>();
            var pairEvents = GetPairEvents(events);

            //чтобы учесть текст до первого и после последнего ивента
            pairEvents.Insert(0, new TagEvent(0, TagEventType.Start, EmptyTag.Instance));
            pairEvents.Add(new TagEvent(text.Length, TagEventType.End, EmptyTag.Instance));

            var count = pairEvents.Count;

            for (var i = 1; i < count; i++)
            {
                var current = pairEvents[i];
                var previous = pairEvents[i - 1];
                var start = previous.Index + previous.Tag.String.Length;
                var end = current.Index;
                var len = end - start;

                if (len > 0)
                {
                    var plainText = RemovedEscape(text.Substring(start, len));
                    tokens.Add(new Token(TokenType.PlainText, plainText));
                }

                if (i == count - 1)
                    continue;

                var tokenType = current.Tag.GetTokenTypeByEventType(current.Type);

                tokens.Add(new Token(tokenType));
            }

            return tokens;
        }

        private static List<TagEvent> GetPairEvents(List<TagEvent> tagEvents)
        {
            var result = new List<TagEvent>();
            var stack = new Stack<TagEvent>();

            foreach (var @event in tagEvents.OrderBy(r => r.Index))
            {
                if (@event.Type == TagEventType.Start)
                {
                    stack.Push(@event);
                }
                else
                {
                    if (stack.Count == 0 || stack.Peek().Tag != @event.Tag)
                        continue;

                    var start = stack.Pop();

                    result.Add(start);
                    result.Add(@event);
                }
            }

            return result.OrderBy(x => x.Index).ToList();
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