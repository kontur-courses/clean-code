using static MarkdownTask.TagInfo;

namespace MarkdownTask
{
    public class ItalicTagParser : ITagParser
    {
        private static string tag = "_";
        public ICollection<Token> Parse(string text)
        {
            var tokens = new List<Token>();
            var opened = new Stack<Token>();

            if (text.Length <= 2)
                return tokens;

            int tagIndex = 0;

            do
            {
                tagIndex = text.IndexOf(tag, tagIndex);

                if (tagIndex < 0)
                {
                    break;
                }

                if (tagIndex >= 0 && !Utils.IsEscaped(text, tagIndex))
                {
                    if (opened.Any() && Utils.IsAfterNonSpace(text, tagIndex))
                    {
                        var openTag = opened.Pop();


                        if (Utils.CanSelect(text, openTag.Position, tagIndex) && tagIndex - openTag.Position > 1)
                        {
                            tokens.Add(openTag);
                            tokens.Add(new Token(TagType.Italic, tagIndex, Tag.Close, 1));
                        }
                    }
                    else if (Utils.IsBeforeNonSpace(text, tagIndex))
                    {
                        opened.Push(new Token(TagType.Italic, tagIndex, Tag.Open, 1));
                    }
                }
                tagIndex++;
            }
            while (tagIndex < text.Length - 1);

            return tokens;
        }
    }
}