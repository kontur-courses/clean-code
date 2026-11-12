using Markdown.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Entity
{
    public interface INode;

    public record TextNode(string Text) : INode;

    public record EmNode(IReadOnlyList<INode> Children) : INode;

    public record StrongNode(IReadOnlyList<INode> Children) : INode;

    public record LinkNode(string Url, IReadOnlyList<INode> Children) : INode;
}
