namespace Markdown
{
    public class MarkdownParser : UnderscoreParser, IParser
    {
        public const char HashSymbol = '#';

        public TagInfo Parse(string markdown)
        {
            Markdown = markdown;
            State = ParseSymbol;

            for (var i = 0; i < markdown.Length; i++)
            {
                if (markdown[i] == '\\')
                    BackslashCounter++;
                State(i);
                if (ShouldEscaped(markdown[i]))
                    BackslashCounter = 0;
            }

            TextEnded = true;
            CloseTags();

            return TagInfo;
        }

        private void ParseSymbol(int index)
        {
            if (TextEnded)
            {
                if (PreviousIndex != Markdown.Length)
                {
                    TagInfo.AddText(Markdown.Substring(PreviousIndex));
                    PreviousIndex = Markdown.Length;
                }

                if (NestedTextInfos.Count != 0)
                    TagInfo = NestedTextInfos.Pop();
            }
            else if (ShouldEscaped(Markdown[index]))
                BackslashCounter = 0;
            else
            {
                switch (Markdown[index])
                {
                    case UnderscoreSymbol:
                        UnderscoreCounter = 1;
                        SetNewState(ParseOpeningUnderscore);
                        break;
                    case HashSymbol when index == 0:
                        SetNewState(ParseHashSymbol);
                        break;
                    default:
                        PreviousIsSpace = char.IsWhiteSpace(Markdown[index]) || Markdown[index] == '\\' && BackslashCounter % 2 == 0;
                        break;
                }
            }
        }

        private void ParseHashSymbol(int index)
        {
            if (Markdown[index] == ' ')
            {
                SetNewTextInfo(new TagInfo(Tag.Heading));
                PreviousIndex = index + 1;
            }

            State = States.Pop();
        }

        private void CloseTags()
        {
            do State(Markdown.Length - 1);
            while (NestedTextInfos.Count != 0 || PreviousIndex < Markdown.Length);
        }
    }
}