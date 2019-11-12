namespace Markdown
{
    public class SpaceToken : MdToken
    {
        public SpaceToken(int position) : base(position, 1, " ", MdTokenType.Space)
        {
        }
    }
}