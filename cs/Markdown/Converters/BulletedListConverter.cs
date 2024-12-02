using Markdown.Converters.Base;

namespace Markdown.Converters;

public class BulletedListConverter : BaseListConverter
{
    protected override string StartOriginalValue => "*";
    protected override string EndOriginalValue => "\n";
    protected override string StartConvertedValue => "<li>";
    protected override string EndConvertedValue => "</li>";
}