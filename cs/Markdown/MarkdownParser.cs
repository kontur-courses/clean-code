namespace Markdown
{
    public class MarkdownParser : UnderscoreParser, IParser
    {
        public const char HashSymbol = '#';

        public MarkdownParser()
        {
            keySymbols.Add(HashSymbol);
        }

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
                    TagInfo.AddContent(new TagInfo(text:Markdown.Substring(PreviousIndex)));
                    PreviousIndex = Markdown.Length;
                }

                if (NestedTagInfos.Count != 0)
                    TagInfo = NestedTagInfos.Pop();
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
                    case LinkOpenSymbol when !ShouldEscaped(Markdown[index]):
                        SetLinkTag(index);
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
                SetNewTagInfo(new TagInfo(Tag.Heading));
                PreviousIndex = index + 1;
            }

            State = States.Pop();
        }

        private void CloseTags()
        {
            do
            {
                if (TagInfo.Tag == Tag.Link)
                {
                    TagInfo.ResetFormatting();
                    PreviousIndex = Markdown.Length;
                    State = States.Pop();
                }
                State(Markdown.Length - 1);
            } while (NestedTagInfos.Count != 0 || PreviousIndex < Markdown.Length);
        }
    }
}