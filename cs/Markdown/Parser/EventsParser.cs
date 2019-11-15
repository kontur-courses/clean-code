using System;
using System.Collections.Generic;

namespace Markdown.Parser
{
    public static class EventsParser
    {
        private static readonly Dictionary<EventType, string> EventToBoard = new Dictionary<EventType, string>()
        {
            [EventType.BoldStart] = "__",
            [EventType.BoldEnd] = "__",
            [EventType.ItalicStart] = "_",
            [EventType.ItalicEnd] = "_"
        };
        private static readonly string Underline = "_";

        public static IEnumerable<Event> ParseEvents(string markdown)
        {
            var events = GetEvents(markdown, 0, markdown.Length);
            return events;
        }

        private static IEnumerable<Event> GetEvents(string markdown, int start, int end)
        {
            var events = new List<Event>();

            for (var i = start; i < end; i++)
            {
                if (ParseHelper.IsBoldStart(markdown, i))
                {
                    TryParseEvent(markdown, ref events, ref i, EventType.BoldStart);
                    continue;
                }

                if (ParseHelper.IsItalicStart(markdown, i))
                {
                    TryParseEvent(markdown, ref events, ref i, EventType.ItalicStart);
                    continue;
                }

                if (ParseHelper.IsUnderline(markdown[i]))
                {
                    events.Add(new Event(EventType.PlainText, Underline));
                    continue;
                }

                var value = ParseHelper.ReadToNextUnderline(markdown, i, out var underlineIndex);
                i = underlineIndex - 1;
                events.Add(new Event(EventType.PlainText, value));
            }

            return events;
        }

        private static void TryParseEvent(string markdown, ref List<Event> events, ref int i, EventType startType)
        {
            events.Add(new Event(startType));
            var endType = ConvertToEnd(startType);

            if (!TryFindEnd(markdown, i + 1, endType, out var eventEnd))
            {
                events.RemoveAt(events.Count - 1);

                var board = EventToBoard[startType];
                events.Add(new Event(EventType.PlainText, board));
                i += board.Length - 1;
                return;
            }

            i += EventToBoard[endType].Length;
            events.AddRange(GetEvents(markdown, i, eventEnd - EventToBoard[endType].Length + 1));
            events.Add(new Event(endType));


            i = eventEnd;
        }

        private static EventType ConvertToEnd(EventType type)
        {
            return type == EventType.ItalicStart ? EventType.ItalicEnd : EventType.BoldEnd;
        }

        private static bool TryFindEnd(string markdown, int start, EventType endType, out int eventEnd)
        {
            for (var j = start; j < markdown.Length; j++)
            {
                if (!IsEnd(markdown, j, endType)) continue;

                eventEnd = j;
                return true;
            }

            eventEnd = -1;
            return false;
        }

        private static bool IsEnd(string markdown, int i, EventType type)
        {
            switch (type)
            {
                case EventType.BoldEnd:
                    return ParseHelper.IsBoldEnd(markdown, i);
                case EventType.ItalicEnd:
                    return ParseHelper.IsItalicEnd(markdown, i);
                default:
                    throw new Exception("Unknown EventType");
            }
        }
    }
}