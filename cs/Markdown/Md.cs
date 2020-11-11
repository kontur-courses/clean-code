using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Markdown
{
    public class Md
    {
        private readonly ImmutableDictionary<TagType, string> Tags;

        public string Render(string markdown)
        {
            throw new NotImplementedException();
        }

        private List<TagToken> GetTokensFromLine(string line)
        {
            throw new NotImplementedException();
        }

        private void RemoveIncorrectIntersections(List<TagToken> tags)
        {
            throw new NotImplementedException();
        }

        private string RenderLine(string line, List<TagToken> tokens)
        {

            throw new NotImplementedException();
        }

        private bool IsPossibleToSetTag(string markdown, string tag, TagToken tagToken)
        {
            throw new NotImplementedException();
        }

        private void SetTag(string tag, TagToken token)
        {
            throw new NotImplementedException();
        }
    }
}