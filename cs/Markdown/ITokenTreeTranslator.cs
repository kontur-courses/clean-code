namespace Markdown
{
    public interface ITokenTreeTranslator
    {
        string Translate(TokenTreeNode tokenTree);
    }
}