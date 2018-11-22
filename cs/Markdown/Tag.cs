using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class Tag
    {
        public readonly string HtmlRepresentation;
        public readonly List<Func<List<Tag>, bool>> TagDependencies;

        public Tag(string htmpRep, List<Func<List<Tag>, bool>> tagDep = null)
        {
            HtmlRepresentation = htmpRep;
            if (tagDep == null)
                TagDependencies = new List<Func<List<Tag>, bool>>();
            else
                TagDependencies = tagDep;
        }

        public bool IsValidTag(List<Tag> upperTags)
        {
            return !TagDependencies.Any(dependency => !dependency(upperTags));
        }

        public static bool IsOpenTag(LinkedListNode<Token> token)
        {
            return token.Next != null && !token.Next.Value.IsWhiteSpace;
        }

        public static bool IsCloseTag(LinkedListNode<Token> token)
        {
            return token.Previous != null && !token.Previous.Value.IsWhiteSpace;
        }
    }
}