namespace MarkDown
{
    public class MdToken
    {
        public readonly string Value;
        public readonly int StartIndex;
        public readonly int Length;

        public MdToken(string value, int startIndex, int length)
        {
            Value = value;
            StartIndex = startIndex;
            Length = length;
        }
    }
}