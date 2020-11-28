namespace Markdown.TokenInfo
{
    public interface ITokenInfo
    {
        public TagType[] NestedTypes { get; }
        public TagType TagType { get; }
        public bool TryReadToken(int startIndex, int finishIndex, string text, out Token token);

        public int GetValueStartIndex(int startIndex);
        public int GetValueFinishIndex(int tagFinishIndex);
        public string GetValue(int startIndex, int finishIndex, string text)
        {
            return text[(GetValueStartIndex(startIndex))..(GetValueFinishIndex(finishIndex))];
        }

        public TagType GetType();

    }
}
