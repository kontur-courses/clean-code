using System.Collections.Generic;

namespace Markdown.Md
{
    public class TokenNode : ITokenNode
    {
        public string Value { get; set; }

        public TokenPairType PairType { get; set; }

        public string Type { get; set; }

        public ICollection<ITokenNode> Children { get; set; }

        public TokenNode(string type, string value, TokenPairType pairType)
        {
            Type = type;
            Value = value;
            Children = new List<ITokenNode>();
            PairType = pairType;
        }

        public TokenNode(string type, string value)
        {
            Type = type;
            Value = value;
            Children = new List<ITokenNode>();
            PairType = TokenPairType.NotPair;
        }
    }
}