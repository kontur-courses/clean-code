namespace Markdown
{
    public class MarkdownParser : UnderscoreParser, IParser
    {
        public TextInfo Parse(string markdown)
        {
            Markdown = markdown;
            State = ParseSymbol;

            for (var i = 0; i < markdown.Length; i++)
            {
                if (markdown[i] == '\\')
                    BackslashCounter++;

                State(i);
            }

            TextEnded = true;
            CloseTags();

            return TextInfo;
        }

        private void ParseSymbol(int index)
        {
            if (TextEnded)
            {
                if (PreviousIndex != Markdown.Length)
                {
                    TextInfo.AddText(Markdown.Substring(PreviousIndex));
                    PreviousIndex = Markdown.Length;
                }
            }
            else if (ShouldEscaped(Markdown[index]))
            {
                BackslashCounter = 0;
                WordStartIndex = index - 1;
                SetNewState(ParseInsideWord);
            }
            else if (!SymbolIsKey(Markdown[index]) && !char.IsWhiteSpace(Markdown[index]))
            {
                WordStartIndex = index;
                SetNewState(ParseInsideWord);
            }
            else
            {
                switch (Markdown[index])
                {
                    case '_':
                        UnderscoreCounter = 1;
                        SetNewState(ParseOpeningUnderscore);
                        break;
                    case '#' when index == 0:
                        SetNewState(ParseHashSymbol);
                        break;
                }
            }
        }

        private void ParseHashSymbol(int index)
        {
            if (Markdown[index] == ' ')
            {
                SetNewTextInfo(new TextInfo(Tag.Heading));
                PreviousIndex = index + 1;
            }

            State = States.Pop();
        }

        private void CloseTags()
        {
            State(Markdown.Length - 1);
            while (NestedTextInfos.Count != 0)
            {
                if (TextInfo.Tag == Tag.Bold || TextInfo.Tag == Tag.Italic)
                    TextInfo.ToNoFormatting();
                TextInfo = NestedTextInfos.Pop();
            }
        }
    }
}