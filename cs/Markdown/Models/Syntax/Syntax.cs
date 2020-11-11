using System;
using System.Collections.Generic;
using Markdown.Models.Tags;

namespace Markdown.Models.Syntax
{
    internal class Syntax : ISyntax
    {
        private readonly Dictionary<string, Type> tagsToTagTypes =
            new Dictionary<string, Type>()
            {

            };

        private readonly Dictionary<Type, IEnumerable<Type>> tagTypesToAllowableInners =
            new Dictionary<Type, IEnumerable<Type>>()
            {

            };

        private readonly List<string> allTags = new List<string>()
        {

        };

        public bool TryGetTag(string text, int position, out Tag tag)
        {
            throw new NotImplementedException();
        }

        public bool IsValidAsOpening(Tag tag, string text)
        {
            throw new System.NotImplementedException();
        }

        public bool IsValidAsClosing(Tag tag, string text)
        {
            throw new System.NotImplementedException();
        }
    }
}
