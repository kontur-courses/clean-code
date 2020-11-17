namespace Markdown
{
    public class MdRawTextToken : MdToken
    {
        public MdRawTextToken(int startPosition, int length = 0) : base(startPosition, length) { }
    }
}