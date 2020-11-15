namespace Markdown.Tag.SpecificTags
{
    public class HeaderTagData : TagData
    {
        // очень не нравится этот конструктор
        // TODO: сделать менее громоздким
        public HeaderTagData(FormattingState state,
            TagBorder incomingBorder, TagBorder outgoingBorder,
            params FormattingState[] notAllowedNestedStates)
            : base(state, incomingBorder, outgoingBorder, notAllowedNestedStates)
        { }

        public override bool IsValid(string data, int startPos, int endPos)
        {
            return startPos == 0 || data[startPos - 1] == '\n';
        }
    }
}