namespace Markdown
{
    class StringPart
    {
        public string Value { get; }
        public ActionType ActionType { get; }
        public TagType TagType { get; }

        public StringPart(string value, ActionType actionType = ActionType.NotAnAction, TagType tagType = TagType.String)
        {
            Value = value;
            ActionType = actionType;
            TagType = tagType;
        }
    }
}
