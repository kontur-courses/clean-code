using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Models;

namespace Markdown
{
    public class MatchesMap
    {
        private readonly Dictionary<int, TokenMatch> matchStartAtPosition;
        private readonly Dictionary<int, TokenMatch> matchEndAtPosition;

        public MatchesMap(IReadOnlyCollection<TokenMatch> matches)
        {
            if (matches == null)
                throw new ArgumentNullException(nameof(matches));

            matchStartAtPosition = matches.ToDictionary(match => match.Start, match => match);
            matchEndAtPosition = matches.ToDictionary(
                match => match.Start + match.Length - match.Token.Pattern.EndTag.Length,
                match => match);
        }

        public bool TryGetTagReplacerAtPosition(int position, out TagReplacer replacer)
        {
            if (matchStartAtPosition.TryGetValue(position, out var matchAtStart))
            {
                replacer = matchAtStart.Token.TokenHtmlRepresentation.OpenTag;
                return true;
            }

            if (matchEndAtPosition.TryGetValue(position, out var matchAtEnd))
            {
                replacer = matchAtEnd.Token.TokenHtmlRepresentation.CloseTag;
                return true;
            }

            replacer = null;
            return false;
        }
    }
}