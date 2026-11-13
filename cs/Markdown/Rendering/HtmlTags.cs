namespace Markdown.Rendering;

/// <summary>
///     Имена HTML-тегов и сущностей, в которые рендерятся примитивы разметки.
///     Собраны в одном месте, чтобы литералы не дублировались по решению.
/// </summary>
public static class HtmlTags
{
    public const string Paragraph = "p";

    public const string Header = "h1";

    public const string Emphasis = "em";

    public const string Strong = "strong";

    public const string Link = "a";

    public const string TagStart = "<";

    public const string TagEnd = ">";

    /// <summary>Начало закрывающего тега: имя тега и <see cref="TagEnd" /> идут следом.</summary>
    public const string ClosingTagPrefix = TagStart + "/";

    /// <summary>Начало открывающего тега ссылки: url подставляется сразу после.</summary>
    public const string LinkOpen = TagStart + Link + " href=\"";

    /// <summary>Конец открывающего тега ссылки: закрывающая кавычка и угловая скобка.</summary>
    public const string LinkOpenEnd = "\">";

    public const string LinkClose = ClosingTagPrefix + Link + TagEnd;

    public const string Ampersand = "&amp;";

    public const string LessThan = "&lt;";

    public const string GreaterThan = "&gt;";

    public const string Quote = "&quot;";
}
