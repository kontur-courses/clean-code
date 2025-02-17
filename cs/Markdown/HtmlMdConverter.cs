namespace Markdown;

public class HtmlMdConverter : TokenMdConverter
{
    protected override string ItalicOpen() => "<em>";

    protected override string ItalicClose() => "</em>";

    protected override string BoldOpen() => "<strong>";

    protected override string BoldClose() => "</strong>";

    protected override string HeaderOpen() => "<h1>";

    protected override string HeaderClose() => "</h1>";

    protected override string MarkerRangeOpen() => "<ul>";

    protected override string MarkerRangeClose() => "</ul>";

    protected override string MarkerOpen() => "<li>";

    protected override string MarkerClose() => "</li>";
}