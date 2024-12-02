using Markdown.Tokens;

namespace Markdown.HtmlConverting.HtmlTagConverters;

public class HeaderTagConverter() : OpenedTagConverter(TagType.Header, TokenType.NewLine);