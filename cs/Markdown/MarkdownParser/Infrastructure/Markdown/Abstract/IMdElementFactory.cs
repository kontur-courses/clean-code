using System;
using MarkdownParser.Infrastructure.Tokenization.Abstract;

namespace MarkdownParser.Infrastructure.Markdown.Abstract
{
    /// <summary>
    /// Basicly you don't need to inherit from this interface directly,
    /// look at <see cref="MdElementFactory{TElem,TToken}"/>
    /// </summary>
    public interface IMdElementFactory
    {
        Type TokenType { get; }
        bool CanCreate(Token token);
        MarkdownElement Create(Token token, Token[] nextTokens);
    }
}