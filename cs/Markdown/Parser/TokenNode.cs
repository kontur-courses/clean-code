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

        public override bool Equals(object obj)
        {
            var token = obj as TokenNode;

            return token != null &&
                Value == token.Value &&
                PairType == token.PairType &&
                Type == token.Type;
        }

        public override int GetHashCode()
        {
            var hashCode = -1206528968;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Value);
            hashCode = hashCode * -1521134295 + PairType.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Type);

            return hashCode;
        }
    }
}