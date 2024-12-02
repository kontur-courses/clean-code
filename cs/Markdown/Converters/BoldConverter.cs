using System.Text;
using Markdown.Converters.Base;
using Markdown.Extensions;

namespace Markdown.Converters;

public class BoldConverter : BasePairConverter
{
    protected override string StartOriginalValue => "__";
    protected override string EndOriginalValue => "__";
    protected override string StartConvertedValue => "<b>";
    protected override string EndConvertedValue => "</b>";
}