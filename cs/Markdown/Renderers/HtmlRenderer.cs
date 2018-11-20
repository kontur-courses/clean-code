using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.Renderers
{
    public class HtmlRenderer : IRenderer
    {
        private readonly IDictionary<string, string> htmlRules;

        public HtmlRenderer(IDictionary<string, string> htmlRules)
        {
            this.htmlRules = htmlRules;
        }

        public string Render(ITokenNode tokenNode)
        {
            if (tokenNode == null)
            {
                throw new ArgumentException("Given tokenNode can't be null", nameof(tokenNode));
            }

            var result = new StringBuilder();

            foreach (var token in GetNextTokenNode(tokenNode.Children))
            {
                var type = htmlRules[token.Type];

                if (token.PairType == TokenPairType.Close)
                {
                    type = type.Insert(1, "/");
                }

                result.Append(type);

                if (token.PairType == TokenPairType.NotPair)
                {
                    result.Append(token.Value);
                }
            }

            return result.ToString();
        }

        public IEnumerable<ITokenNode> GetNextTokenNode(ICollection<ITokenNode> children)
        {
            foreach (var child in children)
            {
                yield return child;

                foreach (var subChild in GetNextTokenNode(child.Children))
                    yield return subChild;
            }
        }
    }
}