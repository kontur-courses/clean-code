namespace Markdown.Tags.TagSpecification;

public class HeaderTag() : BasicSingleTag(new Tag("# ", "<h1>"), new Tag(Environment.NewLine, "</h1>"));