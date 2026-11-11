namespace Markdown.Blocks;

/// <param name="Content">
///     Для <see cref="BlockKind.Header" /> текст заголовка без маркера "# ".
/// </param>
public record Block(BlockKind Kind, string Content);