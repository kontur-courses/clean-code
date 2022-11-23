using MarkdownRenderer.Abstractions.Elements;

namespace MarkdownRenderer.Implementations.Elements;

public class LinkElement : StandardElement
{
    public string Destination { get; }
    public string Title { get; }

    public LinkElement(string destination, string? title = null)
    {
        Destination = destination;
        Title = title ?? destination;
    }

    public static bool IsUriCorrect(string destination)
    {
        if (!destination.Contains("://"))
            destination = "https://" + destination;
        return Uri.TryCreate(destination, UriKind.Absolute, out var uriResult) &&
               (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}