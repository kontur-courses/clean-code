namespace Markdown.Serializers
{
    public interface ITokenSerializer
    {
        string Serialize(MdDoc token);
    }
}