namespace Markdown
{
    public class EOLToken : MdToken
    {
        public EOLToken(int position) : base(position, 1, "\n", MdTokenType.EOL)
        {
        }
    }
}