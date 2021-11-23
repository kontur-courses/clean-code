using System.Collections.Generic;
using System.Linq;
using Markdown.Tags;

namespace Markdown
{
    public class MdSpecification : IMdSpecification
    {
        private readonly List<Tag> _tags;
        private readonly Dictionary<string, Tag> _stringToTag;
        private readonly List<string> _escapeSymbols;
        private readonly HashSet<string> _escapeSequences;

        public List<Tag> Tags => _tags.ToList();
        public Dictionary<string, Tag> TagByMdStringRepresentation => _stringToTag;
        public List<string> EscapeSymbols => _escapeSymbols.ToList();
        public List<string> EscapeSequences => _escapeSequences.ToList();
        public MdSpecification(List<Tag> tags = null, List<string> escapeSymbols = null)
        {
            _tags = tags ?? new List<Tag>
            {
                new StrongTag(),
                new EmTag(),
                new HeadingTag()
            };
            _escapeSymbols = escapeSymbols ?? new List<string> { @"\" };
            _stringToTag = _tags
                .ToDictionary(p => p.OpenMdTag, p => p)
                .Union(_tags.ToDictionary(p => p.CloseMdTag, p => p))
                .ToDictionary(p => p.Key, p => p.Value);
            _escapeSequences = _stringToTag
                .SelectMany(p => _escapeSymbols
                .Select(s => s + p.Key))
                .ToHashSet();
            _escapeSymbols
                .SelectMany(s => _escapeSymbols.Select(f => f + s))
                .ToList()
                .ForEach(seq => _escapeSequences.Add(seq));
        }
    }
}
