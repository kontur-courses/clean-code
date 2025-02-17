using System.Collections.Immutable;
using Markdown.treeVisitor;

namespace Markdown.Converter;

internal interface IConvertTree
{
    INode Convert(IImmutableList<Token> tokens);
}