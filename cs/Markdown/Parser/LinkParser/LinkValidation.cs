namespace Markdown;

public static class LinkValidation
{
    public static bool IsValidInlineHref(string href)
    {
        return !string.IsNullOrWhiteSpace(href) && href.All(ch => !char.IsWhiteSpace(ch));
    }

    public static bool IsValidAutoLinkHref(string href)
    {
        if (!Uri.TryCreate(href, UriKind.Absolute, out var uri))
            return false;

        return uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps;
    }
}

