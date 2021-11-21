using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class Md
    {
        private readonly IMdSpecification _specification;

        public Md(IMdSpecification specification)
        {
            _specification = specification;
        }

        public string Render(string mdText)
        {
            var parser = new MarkdownParser(_specification);
            var tokens = parser.ParseToTokens(mdText);
            var relationTokens = SetRelations(tokens);
            //var renderedTokens = tokens.Select(t => t.Render(mdText));
            var renderedTokens = new List<string>();
            foreach (var t in relationTokens)
                renderedTokens.Add(t.Render(mdText));

            var escapeProcessedTokens = ProcessEscapes(renderedTokens);
            return string.Join("", escapeProcessedTokens);
        }

        private List<string> ProcessEscapes(List<string> renderedTokens)
        {
            return renderedTokens
                //.Select(t => t.Contains(@"\\") ? t.Replace(@"\\", @"\") : t)
                //.ToList()
                ;
        }

        private List<Token> SetRelations(List<Token> tokens)
        {
            var sortedTokens = tokens
                .Where(t => t.Length != 0)
                .OrderBy(t => t.Length)
                .ThenBy(t => t.Begin)
                .ToList();
            var childs = new List<Token>();
            for (int i = 0; i < sortedTokens.Count; i++)
                for (int j = i + 1; j < sortedTokens.Count; j++)
                {
                    var parent = sortedTokens[j];
                    var child = sortedTokens[i];
                    if (parent is TagToken && parent.Begin <= child.Begin
                        && parent.End >= child.End)
                    {
                        childs.Add(sortedTokens[i]);
                        if (parent.AllowInners)
                            Token.SetRelation(sortedTokens[j], sortedTokens[i]);
                    }
                }
            return sortedTokens
                .Except(childs)
                .OrderBy(t => t.Begin)
                .ToList();
        }
    }
}
