using Markdown.Converters.Base;
using Markdown.Extensions;

namespace Markdown.Converters;

public class ItalicConverter : BasePairConverter
{
    protected override string StartOriginalValue => "_";
    protected override string EndOriginalValue => "_";
    protected override string StartConvertedValue => "<i>";
    protected override string EndConvertedValue => "</i>";
}