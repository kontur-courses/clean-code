namespace Markdown.Parser
{
    public class Event
    {
        public Event(EventType type)
        {
            Type = type;
        }

        public Event(EventType type, string value)
        {
            Type = type;
            Value = value;
        }

        public EventType Type { get; }
        public string Value { get; }
    }
}