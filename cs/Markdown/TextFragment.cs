using Markdown.Tags;

namespace Markdown;

public record TextFragment(TagReplacementSpecification OpeningTag, TagReplacementSpecification ClosingTag);