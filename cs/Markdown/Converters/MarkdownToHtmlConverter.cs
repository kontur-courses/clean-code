using System.Text;

namespace Markdown.Converters;

public static class MarkdownToHtmlConverter
{
    private static readonly Dictionary<TagType, string> tagToString = new()
    {
        { TagType.Em, "em" },
        { TagType.Strong, "strong" },
        { TagType.Link, "a" },
        { TagType.H1, "h1" }
    };

    public static StringBuilder Convert(List<Tag> tokens)
    {
        var st = new StringBuilder();
        foreach (var tok in tokens)
        {
            if (tok.Type == TagType.Text)
                st.Append(tok.TagText);
            else
            {
                switch (tok)
                {
                    case { PairType: PairTokenType.Opening }:
                        st.Append($"<{tagToString[tok.Type]}>");
                        break; 
                    case { PairType: PairTokenType.Closing }:
                        st.Append($"</{tagToString[tok.Type]}>");
                        break;
                    case { PairType: PairTokenType.Completed }:
                        st.Append($"{tok.TagText}");
                        break;
                    case { PairType: PairTokenType.Single }:
                        st.Append(GetLink(tok));
                        break;
                }  
            }
        }
        return st;
    }

    private static StringBuilder GetLink(Tag link)
    {
        var st = new StringBuilder();
        var linkExp = link.TagText;
        var linkTextEnd = linkExp.IndexOf(']');
        var linkStart = linkTextEnd + 1;
        var linkEnd = linkExp.IndexOf(')', linkStart);
        var firstC = linkExp.IndexOf('\"');
        var second = linkExp.IndexOf('\"', firstC + 1);
        if (firstC != -1 && second != -1)
        {
            st.Append($"<a href=\"{linkExp.Substring(linkStart + 1, firstC - linkStart - 2)}\" ");
            st.Append($"title=\"{linkExp.Substring(firstC + 1, second - firstC - 1)}\"");
        }
        else
            st.Append($"<a href=\"{linkExp.Substring(linkStart + 1, linkEnd - linkStart - 1)}\"");
        st.Append($">{linkExp.Substring(1, linkTextEnd - 1)}</a>");
        return st;
    }
}