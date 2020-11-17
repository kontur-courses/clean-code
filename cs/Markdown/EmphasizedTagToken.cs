namespace Markdown
{
    public class EmphasizedTagToken : ITagToken
    {
        public string Convert(Token token)
        {
            return $"<em>{token.GetValueWithoutTags()}</em>";
        }
    }
}