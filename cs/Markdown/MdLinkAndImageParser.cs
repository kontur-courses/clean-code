namespace Markdown
{
    public class MdLinkAndImageParser : IParser
    {
        public static readonly MdLinkAndImageParser Instance = new MdLinkAndImageParser();
        private MdLinkAndImageParser(){}
        public ParsingResult Parse(StringWithShielding mdText, int startBoundary, int endBoundary)
        {
            var type = TextType.Link;
            var index = startBoundary;
            if (mdText[startBoundary] == '!')
            {
                type = TextType.Image;
                index++;
            }
            var text = TryReadTextPart(mdText, ref index, endBoundary);
            if (text is null)
                return ParsingResult.Fail(Status.NotFound);
            index++;
            var source = TryReadSourcePart(mdText, ref index, endBoundary);
            return source is null ? ParsingResult.Fail(Status.NotFound) : 
                ParsingResult.Success(new HyperTextElement<LinkInfo>(type, 
                    new LinkInfo(text, source)), startBoundary, index);
        }

        private string TryReadTextPart(StringWithShielding mdText, ref int index, int endBoundary)
        {
            if (mdText[index] != '[')
                return null;
            index++;
            var textStart = index;
            for (; index <= endBoundary; index++)
            {
                if (mdText[index] == ']')
                    return mdText.ShieldedSubstring(textStart, index - 1);
            }
            return null;
        }
        private string TryReadSourcePart(StringWithShielding mdText, ref int index, int endBoundary)
        {
            if (mdText[index] != '(')
                return null;
            index++;
            var textStart = index;
            for (; index <= endBoundary; index++)
            {
                if (mdText[index] == ')')
                    return mdText.ShieldedSubstring(textStart, index - 1);
            }
            return null;
        }
    }
}