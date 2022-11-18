using MarkdownRenderer.Abstractions.Elements;

namespace MarkdownRenderer.Implementations.Elements;

public class LinkElement : IElement
{
    string IElement.RawContent => null!;

    IEnumerable<IElement> IElement.NestedElements =>
        throw new InvalidOperationException($"{nameof(LinkElement)} cannot contain nested elements.");

    public string Destination { get; }
    public string Title { get; }

    public LinkElement(string destination, string? title = null)
    {
        Destination = destination;
        Title = title ?? destination;
    }

    public bool CanContainNested(Type nestedType) => false;

    public void AddNestedElement(IElement nested) =>
        throw new InvalidOperationException($"{nameof(LinkElement)} cannot contain nested elements.");
    
    public static bool IsUriCorrect(string destination)
    {
        if (!destination.Contains("://"))
            destination = "https://" + destination;
        return Uri.TryCreate(destination, UriKind.Absolute, out var uriResult) &&
               (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}