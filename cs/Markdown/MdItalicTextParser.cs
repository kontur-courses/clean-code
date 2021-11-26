namespace Markdown
{
    public class MdItalicTextParser : MdQuotedParserBase
    {
        public static readonly MdItalicTextParser Default = new();

        private MdItalicTextParser() : base(TextType.ItalicText, Md.ItalicQuotes.ToString())
        {
            
        }

        protected override Result<int> FindEndQuotes(StringWithShielding mdText, int startBoundary, int endBoundary,
            bool inSameWord)
        {
            for (var i = startBoundary + 1; i <= endBoundary; i++)
            {
                if (inSameWord && mdText[i] == ' ')
                    return Result<int>.Fail(Status.NotFound);
                if (mdText[i] != Md.ItalicQuotes) continue;
                if (i == endBoundary || mdText[i + 1] != Md.ItalicQuotes)
                    return mdText[i - 1] != ' ' ? Result<int>.Success(i) : Result<int>.Fail(Status.BadResult);
                i++;
            }
            return Result<int>.Fail(Status.NotFound);;
        }
    }
}