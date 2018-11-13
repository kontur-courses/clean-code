using System;
using System.Collections.Generic;


namespace Markdown
{
    public class Md
    {
        private TagKeeper[] tags;
        private Stack<SpecialSymbolMarker> specialSymbolsBuffer;

        public Md(IEnumerable<TagKeeper> tags)
        {
            throw new NotImplementedException();
        }

        public string Render(string paragraph)
        {
            return "";
        }

        private string ReplaceTags(string line)
        {
            throw new NotImplementedException();
        }

        private bool IsEscaped(string line, int position)
        {
            throw new NotImplementedException();
        }

        private bool IsCorrect(string line, int position, Tag tag)
        {
            throw new NotImplementedException();
        }
    }
}
