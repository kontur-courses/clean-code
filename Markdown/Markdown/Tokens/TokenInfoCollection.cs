using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    internal class TokenInfoCollection
    {
        private readonly IEnumerable<TokenInfo> tokens;

        internal TokenInfoCollection(IEnumerable<TokenInfo> tokens)
        {
            MdExceptionHelper.ThrowArgumentNullExceptionIf(new ExceptionCheckObject(nameof(tokens), tokens));
            this.tokens = tokens;
        }
        
        public TokenInfoCollection SelectValid()
        {
            return new TokenInfoCollection(tokens.Where(x => x.Valid));
        }
        
        public IEnumerable<TokenSegment> ToTokenSegments(TagRules rules)
        {
            MdExceptionHelper.ThrowArgumentNullExceptionIf(new ExceptionCheckObject(nameof(rules), rules));
            return new Segmenter(tokens, rules).ToTokenSegments();
        }
    }
}