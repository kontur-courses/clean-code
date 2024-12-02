using System.Text;
using Markdown.Converters.Base;
using Markdown.Extensions;

namespace Markdown.Converters;

public class HeaderConverter : BaseHeaderConverter
{
    protected override string StartOriginalValue => "# ";
    protected override string EndOriginalValue => "\n";
    protected override string StartConvertedValue => "<h1>";
    protected override string EndConvertedValue => "</h1>";
}