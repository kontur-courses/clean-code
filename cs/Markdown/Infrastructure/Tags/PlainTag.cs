using System.Collections.Generic;
using Markdown.Infrastructure.Formatters;

namespace Markdown.Infrastructure.Tags
{
    public class PlainTag : ITag
    {
        private readonly string word;

        public PlainTag(string word)
        {
            this.word = word;
        }

        public IEnumerable<string> Format(TagFormatter _)
        {
            yield return word;
        }
    }
}