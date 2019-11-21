using System.Collections.Generic;
using Markdown.DTOs;
using Markdown.Tags;

namespace Markdown.Validators
{
    public abstract class Validator
    {
        protected readonly List<ITag> tags;
        protected readonly Dictionary<string, bool> isOpen;

        public abstract bool IsEscaped(int start, string input);

        public abstract bool TryGetCorrectTagContainer(int start, string input, out TagTypeContainer tagContainer,
            ITag tag);

        public abstract bool TryGetTag(int start, string input, out ITag tag);

        public Validator(Dictionary<string, bool> openChecker, List<ITag> tagsList)
        {
            isOpen = openChecker;
            tags = tagsList;
        }
    }
}