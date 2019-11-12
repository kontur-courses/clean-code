namespace Markdown
{
    public class TextToken : MdToken
    {
        public TextToken(int position, int length, string value) : base(position, length, value, MdTokenType.Text)
        {
        }
    }
}