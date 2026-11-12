namespace Markdown
{
    public class HtmlInlineRenderer
    {
        private const int EscapeSymbolLength = 1;
        private const int EscapeSequenceLength = 2;
        private const char TextBoundarySymbol = ' ';

        private readonly string text;
        private readonly List<string> htmlParts = [];
        private readonly Dictionary<TagType, Stack<OpenedTag>> openedTags = new ()
        {
            [TagType.Emphasis] = new (),
            [TagType.Strong] = new (),
        };

        private int position;
        private int plainTextStart;
        private int wordIndex;

        private HtmlInlineRenderer(string text)
        {
            this.text = text;
        }

        public static string Render(string text)
        {
            var renderer = new HtmlInlineRenderer(text);
            return renderer.RenderToHtml();
        }

        private string RenderToHtml()
        {
            while (position < text.Length)
            {
                if (IsWhitespaceAfterWord())
                    wordIndex++;

                if (IsEscapeSequence())
                    SkipEscapeSymbol();
                else if (text[position] == MarkdownSyntax.UnderscoreSymbol)
                    ProcessUnderscores();
                else
                    position++;
            }

            AddPlainText();
            return string.Concat(htmlParts);
        }

        private void SkipEscapeSymbol()
        {
            AddPlainText();
            plainTextStart = position + EscapeSymbolLength;
            position += EscapeSequenceLength;
        }

        private void ProcessUnderscores()
        {
            var underscoreCount = CountUnderscores();
            var isTagMarker = MarkdownSyntax.TryGetUnderscoreTag(underscoreCount, out var tagType)
                              && !IsInsideNumber(underscoreCount);
            if (isTagMarker)
                ProcessTagMarker(tagType, underscoreCount);

            position += underscoreCount;
        }

        private void ProcessTagMarker(TagType tagType, int markerLength)
        {
            if (TryFindTagToClose(tagType, markerLength, out var openedTag))
                CloseTag(openedTag, markerLength);
            else if (CanOpen(markerLength))
                OpenTag(tagType, markerLength);
        }

        private bool TryFindTagToClose(TagType tagType, int markerLength, out OpenedTag openedTag)
        {
            return openedTags[tagType].TryPeek(out openedTag)
                   && CanClose()
                   && !IsBreakingWord(openedTag, markerLength);
        }

        private void OpenTag(TagType tagType, int markerLength)
        {
            AddPlainText();
            var openedTag = new OpenedTag(tagType, htmlParts.Count, IsInsideWord(markerLength), wordIndex);
            openedTags[tagType].Push(openedTag);
            htmlParts.Add(MarkdownSyntax.GetMarker(tagType));
            plainTextStart = position + markerLength;
        }

        private void CloseTag(OpenedTag openedTag, int markerLength)
        {
            var isPlainText = HasTagsOpenedAfter(openedTag) || IsStrongInsideEmphasis(openedTag.Type);
            RemoveTagsOpenedSince(openedTag);
            if (isPlainText)
                return;

            AddPlainText();
            htmlParts[openedTag.HtmlPartIndex] = HtmlConverter.Open(openedTag.Type);
            htmlParts.Add(HtmlConverter.Close(openedTag.Type));
            plainTextStart = position + markerLength;
        }

        private bool HasTagsOpenedAfter(OpenedTag openedTag)
        {
            return openedTags.Values.Any(tags => tags.TryPeek(out var lastTag)
                                                 && lastTag.HtmlPartIndex > openedTag.HtmlPartIndex);
        }

        private void RemoveTagsOpenedSince(OpenedTag openedTag)
        {
            foreach (var tags in openedTags.Values)
            {
                while (tags.TryPeek(out var lastTag) && lastTag.HtmlPartIndex >= openedTag.HtmlPartIndex)
                {
                    tags.Pop();
                }
            }
        }

        private bool IsStrongInsideEmphasis(TagType tagType)
        {
            return tagType == TagType.Strong && openedTags[TagType.Emphasis].Count > 0;
        }

        private bool IsBreakingWord(OpenedTag openedTag, int markerLength)
        {
            var isInDifferentWords = openedTag.WordIndex != wordIndex;
            return isInDifferentWords && (openedTag.IsInsideWord || IsInsideWord(markerLength));
        }

        private void AddPlainText()
        {
            htmlParts.Add(text[plainTextStart..position]);
        }

        private bool IsWhitespaceAfterWord()
        {
            var previousSymbol = GetSymbol(position - 1);
            return char.IsWhiteSpace(text[position]) && !char.IsWhiteSpace(previousSymbol);
        }

        private bool IsEscapeSequence()
        {
            var nextSymbol = GetSymbol(position + EscapeSymbolLength);
            return text[position] == MarkdownSyntax.EscapeSymbol && MarkdownSyntax.CanBeEscaped(nextSymbol);
        }

        private int CountUnderscores()
        {
            var end = position;
            while (end < text.Length && text[end] == MarkdownSyntax.UnderscoreSymbol)
            {
                end++;
            }

            return end - position;
        }

        private bool IsInsideNumber(int markerLength)
        {
            return IsInsideWord(markerLength) && IsNextToDigit(markerLength);
        }

        private bool CanOpen(int markerLength)
        {
            var nextSymbol = GetSymbol(position + markerLength);
            return !char.IsWhiteSpace(nextSymbol);
        }

        private bool CanClose()
        {
            var previousSymbol = GetSymbol(position - 1);
            return !char.IsWhiteSpace(previousSymbol);
        }

        private bool IsInsideWord(int markerLength)
        {
            var previousSymbol = GetSymbol(position - 1);
            var nextSymbol = GetSymbol(position + markerLength);
            return char.IsLetterOrDigit(previousSymbol) && char.IsLetterOrDigit(nextSymbol);
        }

        private bool IsNextToDigit(int markerLength)
        {
            var previousSymbol = GetSymbol(position - 1);
            var nextSymbol = GetSymbol(position + markerLength);
            return char.IsDigit(previousSymbol) || char.IsDigit(nextSymbol);
        }

        private char GetSymbol(int index)
        {
            var isInsideText = index >= 0 && index < text.Length;
            return isInsideText ? text[index] : TextBoundarySymbol;
        }
    }
}
