namespace Markdown
{
    public class EmTagType : TagType
    {
        public EmTagType() : base("<em>", "</em>", "_", "_")
        {
        }

        public static bool IsOpenedTag(string text, int position)
        {
            if (text[position] != '_' )
                return false;
            if (position == 0)
                return char.IsLetter(text[position + 1]);
            return position + 1 < text.Length && char.IsLetter(text[position + 1]) && text[position - 1] == ' ';
        }

        public static bool IsClosedTag(string text, int position)
        {
            if (text[position] != '_' || position < 1)
                return false;
            if (position == text.Length - 1)
                return char.IsLetter(text[position - 1]);
            return text[position + 1] == ' ' && char.IsLetter(text[position - 1]);
        }
    }
}