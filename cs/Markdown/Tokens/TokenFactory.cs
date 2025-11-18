using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown;

internal class TokenFactory
{
    private readonly List<(string mdTag, Func<int, string, IToken> factory)> _tokenTemplates;
    private readonly string _sourceText;

    public TokenFactory(string sourceText, IEnumerable<ITag> tags)
    {
        _sourceText = sourceText;
        _tokenTemplates = new List<(string, Func<int, string, IToken>)>();
        
        _tokenTemplates.Add(("\\", (pos, text) => new EscapeToken()));
        _tokenTemplates.Add(("\n", (pos, text) => new NewlineToken()));
        
        var sortedTags = tags.OrderByDescending(tag => tag.MdTag.Length).ThenBy(tag => tag.MdTag);
        foreach (var tag in sortedTags)
        {
            _tokenTemplates.Add((tag.MdTag, (pos, text) => 
            {
                var leftChar = text.ElementAtOrDefault(pos - 1);
                var rightChar = text.ElementAtOrDefault(pos + tag.MdTag.Length);
                return new TagToken(tag, leftChar, rightChar);
            }));
        }
    }

    public bool TryCreateToken(int position, out IToken token)
    {
        token = null;
        
        foreach (var (mdTag, factory) in _tokenTemplates)
        {
            if (position + mdTag.Length > _sourceText.Length ||
                _sourceText.Substring(position, mdTag.Length) != mdTag) 
                continue;
            
            token = factory(position, _sourceText);
            return true;
        }

        return false;
    }
}