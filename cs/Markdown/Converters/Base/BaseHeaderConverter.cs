using Markdown.Extensions;
using Markdown.Models;

namespace Markdown.Converters.Base;

public abstract class BaseHeaderConverter : BaseConverter
{
    public override IEnumerable<TagPair> Convert(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            yield break;
        }

        var index = 0;
        var isInTag = false;
        var tagPairBuilder = TagPair.CreateBuilder(GetType());
        while (index < text.Length)
        {
            if (!isInTag && IsTagStart(text, index))
            {
                var token = new TagToken(StartOriginalValue, StartConvertedValue, index);
                tagPairBuilder.AddOpenToken(token);
                index += StartOriginalValue.Length;
                isInTag = true;
                continue;
            }

            if (isInTag && ShouldAbortTag(text, index))
            {
                isInTag = false;
                continue;
            }

            if (isInTag && IsTagEnd(text, index))
            {
                var token = new TagToken(EndOriginalValue, EndConvertedValue, index);
                if (index == text.Length - 1)
                {
                    token = GetEndListToken(token, true);
                }
                tagPairBuilder.AddCloseToken(token);
                yield return tagPairBuilder.Build();
                index += EndOriginalValue.Length;
                isInTag = false;
                continue;
            }

            index++;
        }
    }

    protected virtual bool IsTagStart(string text, int index)
    {
        if (!text.IsInBounds(index, StartOriginalValue.Length))
        {
            return false;
        }

        var nextIndex = index + StartOriginalValue.Length;
        var previousIndex = index - StartOriginalValue.Length;
        return text.ContainsTagAtIndex(index, StartOriginalValue)
               && !text.HasAdjacentTag(nextIndex, StartOriginalValue)
               && !text.HasAdjacentTag(previousIndex, StartOriginalValue);
    }

    protected virtual bool IsTagEnd(string text, int index)
    {
        if (index + 1 == text.Length)
        {
            return true;
        }

        if (index < 0 || index + EndOriginalValue.Length > text.Length)
        {
            return false;
        }

        return text.ContainsTagAtIndex(index, EndOriginalValue);
    }

    protected virtual bool ShouldAbortTag(string text, int index)
    {
        return !text.IsInBounds(index, EndOriginalValue.Length);
    }
    
    private TagToken GetEndListToken(TagToken tokenValue, bool isEndTextIndex)
    {
        var closeToken = tokenValue with
        {
            TagIndex = isEndTextIndex ? tokenValue.TagIndex + 1 : tokenValue.TagIndex
        };
        return closeToken;
    }
}