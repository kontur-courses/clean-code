using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Markdown
{
    public class Md
    {
        private static readonly ImmutableHashSet<char> TagChars = new HashSet<char> { '_', '#'}.ToImmutableHashSet();

        public string Render(string markdown)
        {
            throw new NotImplementedException();
        }

        private string RenderLine(string line, List<TagToken> tokens)
        {
            throw new NotImplementedException();
        }

        private List<TagToken> ReadPairedTagsFromLine(string line)
        {
            throw new NotImplementedException();
        }

        private List<TagToken> ReadSingleTagsFromLine(string line)
        {
            throw new NotImplementedException();
        }

        private TagType GetTagType(string line, int index)
        {
            throw new NotImplementedException();
        }

        private void RemoveIncorrectIntersections(List<TagToken> tags)
        {
            throw new NotImplementedException();
        }

        private bool IsPossibleToSetTag(string markdown, string tag, TagToken tagToken)
        {
            throw new NotImplementedException();
        }
    }
}