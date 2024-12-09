using Markdown.Extensions;
using Markdown.Tags;
using Markdown.TokenParser.Interfaces;
using Markdown.Tokens;

namespace Markdown.TokenParser.TokenHandlers;

public class ItalicTokensHandler() : UnderscoreTokensHandler(TagType.Italic, "_")
{
}