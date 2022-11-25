using MrakdaunV1.Enums;

namespace MrakdaunV1.Interfaces
{
    public interface IHtmlRenderer
    {
        string Render(string text, CharState[] charStates);
    }
}