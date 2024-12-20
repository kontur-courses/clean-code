namespace Markdown.MdTags;

internal enum TagType
{
    /// <summary>
    /// Одиночный тег.
    /// </summary>
    Normal = 0,

    /// <summary>
    /// Область тега ограничена открывающим и закрывающим тегами.
    /// </summary>
    Pair = 1,

    /// <summary>
    /// Область тега начинается с начала строки и закрывается переносом строки.
    /// </summary>
    FullLine = 2,

    /// <summary>
    /// Игнорируемая, неизменяющаяся область.
    /// </summary>
    Ignore = 3,
}