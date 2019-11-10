using System;
using System.Collections.Generic;

namespace Markdown
{
    class Md
    {
        public string Render(string sourceText)
        {
            throw new NotImplementedException();
        }

        private Queue<Tuple<Tag, int>> GetTagsSequences(string sourceString, List<TagSpecification> currentSpecifications)
        {
            throw new NotImplementedException();
        }

        private List<TagsPair> GetTagsPair(Queue<Tuple<Tag, int>> characterSequences)
        {
            throw new NotImplementedException();
        }

        private bool CanBeOpenTag(string symbol, int position, string text)
        {
            throw new NotImplementedException();
        }

        private bool CanBeEndTag(string symbol, int position, string text)
        {
            throw new NotImplementedException();
        }

        private string ReplaceTags(string sourceStrings, List<TagSpecification> currentSpecifications,
            List<TagSpecification> newSpecifications, List<TagsPair> tagsPair)
        {
            throw new NotImplementedException();
        }
    }
}
