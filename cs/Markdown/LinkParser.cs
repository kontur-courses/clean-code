namespace Markdown
{
    public class LinkParser : Parser
    {
        public const char LinkOpenSymbol = '[';
        public const char LinkCloseSymbol = ']';
        public const char AttributeOpenSymbol = '(';
        public const char AttributeCloseSymbol = ')';

        protected LinkParser()
        {
            keySymbols.Add(LinkOpenSymbol);
            keySymbols.Add(LinkCloseSymbol);
            keySymbols.Add(AttributeOpenSymbol);
            keySymbols.Add(AttributeCloseSymbol);
        }
        
        protected void SetLinkTag(int index)
        {
            if (index - PreviousIndex > 0)
                TagInfo.AddContent(new TagInfo(text:Markdown.Substring(PreviousIndex, index - PreviousIndex)));
            PreviousIndex = index + 1;
            SetNewTagInfo(new TagInfo(Tag.Link));
            SetNewState(ParseLinkText);
        }

        private void ParseLinkText(int index)
        {
            if (Markdown[index] == LinkCloseSymbol && !ShouldEscaped(Markdown[index]))
            {
                TagInfo.AddContent(new TagInfo(text:Markdown.Substring(PreviousIndex, index - PreviousIndex)));
                State = ParseHref;
            }
            else if (Markdown[index] == LinkOpenSymbol && !ShouldEscaped(Markdown[index]))
            {
                TagInfo.AddContent(new TagInfo(text:Markdown.Substring(PreviousIndex, index - PreviousIndex)));
                TagInfo.ResetFormatting();
                TagInfo = NestedTagInfos.Pop();
                PreviousIndex = index + 1;
                SetNewTagInfo(new TagInfo(Tag.Link));
            }
        }

        private void ParseHref(int index)
        {
            if (Markdown[index] != AttributeOpenSymbol)
            {
                TagInfo.ResetFormatting(true);
                State = States.Pop();
            }

            PreviousIndex = index + 1;
            State = ParseHrefText;
        }

        private void ParseHrefText(int index)
        {
            if (Markdown[index] == '\"' && char.IsWhiteSpace(Markdown[index - 1]))
            {
                if (index - PreviousIndex - 1 > 0)
                {
                    var text = Markdown.Substring(PreviousIndex, index - PreviousIndex - 1);
                    TagInfo.AddAttribute(new TagAttribute(TagAttributeType.Href, text));
                }
                PreviousIndex = index + 1;
                SetNewState(ParseAltText);
            }
            else if (Markdown[index] == AttributeCloseSymbol && !ShouldEscaped(Markdown[index]))
            {
                if (index - PreviousIndex > 0)
                {
                    var text = Markdown.Substring(PreviousIndex, index - PreviousIndex);
                    TagInfo.AddAttribute(new TagAttribute(TagAttributeType.Href, text));
                }

                PreviousIndex = index + 1;
                TagInfo = NestedTagInfos.Pop();
                State = States.Pop();
            }
        }

        private void ParseAltText(int index)
        {
            if (Markdown[index] == '\"')
            {
                var text = Markdown.Substring(PreviousIndex, index - PreviousIndex);
                TagInfo.AddAttribute(new TagAttribute(TagAttributeType.Alt, text));
                PreviousIndex = index + 1;
                State = States.Pop();
            }
        }
    }
}