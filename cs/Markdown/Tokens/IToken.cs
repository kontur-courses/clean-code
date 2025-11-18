namespace Markdown.Tokens;

internal interface IToken
{
    string Value { get; }
    int Length { get; }
}