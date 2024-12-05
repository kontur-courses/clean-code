using Markdown.Tags;
using Markdown.Tags.TagSpecification;

namespace Markdown.MarkupSpecification;

public interface ISpecificationProvider
{
    public IEnumerable<BaseTag> GetMarkupSpecification();
}