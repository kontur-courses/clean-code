using System;
using System.Linq;
using Markdown.Interfaces;

namespace Markdown.PairTags
{
    public class ClosingHeader : IPairTag
    {
        public string ViewTag => Environment.NewLine;

        public Tag Tag => Tag.Header;

        public TagType TagType => TagType.Close;


        public bool CheckForCompliance(string textContext, int position)
        {
            if (position + ViewTag.Length > textContext.Length)
                return false;

            return !ViewTag.Where((t, i) => textContext[i + position] != t).Any();
        }
    }
}