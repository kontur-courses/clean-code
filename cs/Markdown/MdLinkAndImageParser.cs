namespace Markdown
{
    public class MdLinkAndImageParser : IParser
    {
        public static readonly MdLinkAndImageParser Instance = new MdLinkAndImageParser();
        private MdLinkAndImageParser(){}
        public ParsingResult Parse(StringWithShielding mdText, int startBoundary, int endBoundary)
        {
            if (startBoundary + 4 > endBoundary)
                return ParsingResult.Fail(Status.NotFound);
            var type = TextType.Link;
            var index = startBoundary;
            if (mdText[startBoundary] == '!')
            {
                type = TextType.Image;
                index++;
            }
            var text = TryReadBrackets(mdText, ref index, endBoundary, '[', ']');
            if (text is null)
                return ParsingResult.Fail(Status.NotFound);
            index++;
            var source = TryReadBrackets(mdText, ref index, endBoundary, '(', ')');
            return source is null ? ParsingResult.Fail(Status.NotFound) : 
                ParsingResult.Success(new HyperTextElement<LinkInfo>(type, 
                    new LinkInfo(text, source)), startBoundary, index);
        }

        private static string TryReadBrackets(StringWithShielding mdText, ref int index, int endBoundary, char openBracket,
            char closedBracket)
        {
            if (mdText[index] != openBracket)
                return null;
            index++;
            var textStart = index;
            for (; index <= endBoundary; index++)
            {
                if (mdText[index] == closedBracket)
                    return mdText.ShieldedSubstring(textStart, index - 1);
            }
            return null;
        }
    }
}