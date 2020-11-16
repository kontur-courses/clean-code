using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    internal class MdTokensFinder : ITokensFinder
    {
        private readonly Dictionary<Style, ITokensFinder> finders;
        private readonly HashSet<(Style external, Style inner)> shouldNotContain;
        private readonly HashSet<(Style first, Style second)> shouldNotIntersect;
        public readonly string Text;
        private readonly Dictionary<Style, HashSet<Token>> tokens;

        private IEnumerable<Token> tokensCache;

        internal MdTokensFinder(string text)
        {
            Text = text;
            tokens = new Dictionary<Style, HashSet<Token>>();
            finders = new Dictionary<Style, ITokensFinder>();
            shouldNotIntersect = new HashSet<(Style first, Style second)>();
            shouldNotContain = new HashSet<(Style external, Style inner)>();
        }

        public IEnumerable<Token> Find()
        {
            if (tokensCache != null)
                return tokensCache;
            foreach (var pair in finders) tokens[pair.Key] = pair.Value.Find().ToHashSet();
            DeleteIntersection();
            DeleteContaining();
            tokensCache = tokens.SelectMany(pair => pair.Value);
            return tokensCache;
        }

        public MdTokensFinder Using(Style style, ITokensFinder finder)
        {
            finders[style] = finder;
            return this;
        }

        public MdTokensFinder ExcludingIntersection(Style first, Style second)
        {
            shouldNotIntersect.Add((first, second));
            return this;
        }

        public MdTokensFinder ExcludingContaining(Style external, Style inner)
        {
            shouldNotContain.Add((external, inner));
            return this;
        }

        private void DeleteContaining()
        {
            foreach (var (external, inner) in shouldNotContain)
            {
                if (!tokens.ContainsKey(external) || !tokens.ContainsKey(inner))
                    continue;
                var contained = new HashSet<Token>();
                foreach (var externalToken in tokens[external])
                foreach (var innerToken in tokens[inner])
                    if (externalToken.Contains(innerToken))
                        contained.Add(innerToken);
                tokens[inner].ExceptWith(contained);
            }
        }

        private void DeleteIntersection()
        {
            foreach (var (first, second) in shouldNotIntersect)
            {
                if (!tokens.ContainsKey(first) || !tokens.ContainsKey(second))
                    continue;
                var intersectedFirst = new HashSet<Token>();
                var intersectedSecond = new HashSet<Token>();
                foreach (var firstToken in tokens[first])
                foreach (var secondToken in tokens[second])
                    if (firstToken.IntersectsWith(secondToken))
                    {
                        intersectedSecond.Add(secondToken);
                        intersectedFirst.Add(firstToken);
                    }

                tokens[first].ExceptWith(intersectedFirst);
                tokens[second].ExceptWith(intersectedSecond);
            }
        }
    }
}