namespace Markdown.Tokens
{
    public abstract class NestedToken : MdToken
    {
        private List<MdToken> mdTokens = new List<MdToken>();
    }
}