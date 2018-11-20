using System.Collections.Generic;

namespace Markdown
{
    public interface ITokenNode
    {
        ICollection<ITokenNode> Children { get; set; }

        TokenPairType PairType { get; set; }

        string Type { get; set; }

        string Value { get; set; }
    }
}