namespace Markdown
{
    public struct Range
    {
        public Range(int index, int length)
        {
            Index = index;
            Length = length;
        }

        public int Index { get; }
        public int Length { get; }
    }
}