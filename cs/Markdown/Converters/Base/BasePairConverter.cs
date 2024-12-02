using Markdown.Extensions;
using Markdown.Models;

namespace Markdown.Converters.Base;

public abstract class BasePairConverter : BaseConverter
{
    public override IEnumerable<TagPair> Convert(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            yield break;
        }

        var index = 0;
        var isInTag = false;
        var isBetweenWord = false;
        var tagPairBuilder = TagPair.CreateBuilder(GetType());
        while (index < text.Length)
        {
            if (!isInTag && IsTagStart(text, index))
            {
                isBetweenWord = HasBetweenWord(text, index);
                var token = new TagToken(StartOriginalValue, StartConvertedValue, index);
                tagPairBuilder.AddOpenToken(token);
                index += StartOriginalValue.Length;
                isInTag = true;
                continue;
            }

            if (isInTag && ShouldAbortTag(text, index, isBetweenWord))
            {
                isBetweenWord = false;
                isInTag = false;
                continue;
            }

            if (isInTag && IsTagEnd(text, index))
            {
                isBetweenWord = false;
                var token = new TagToken(EndOriginalValue, EndConvertedValue, index);
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
               && !text.HasFollowingSpace(nextIndex)
               && !text.HasAdjacentTag(nextIndex, StartOriginalValue)
               && !text.HasAdjacentTag(previousIndex, StartOriginalValue);
    }

    protected virtual bool IsTagEnd(string text, int index)
    {
        if (!text.IsInBounds(index, EndOriginalValue.Length))
        {
            return false;
        }

        var nextIndex = index + EndOriginalValue.Length;
        var previousIndex = index - EndOriginalValue.Length;
        return text.ContainsTagAtIndex(index, EndOriginalValue)
               && !text.HasAdjacentTag(nextIndex, EndOriginalValue)
               && !text.HasAdjacentTag(previousIndex, EndOriginalValue);
    }

    protected virtual bool HasBetweenWord(string text, int index)
    {
        if (index - 1 < 0 || index + StartOriginalValue.Length >= text.Length)
        {
            return false;
        }

        var hasNextIsWord = char.IsLetter(text, index + StartOriginalValue.Length);
        var hasBeforeIsWord = char.IsLetter(text, index - 1);
        return hasNextIsWord && hasBeforeIsWord;
    }

    protected virtual bool ShouldAbortTag(string text, int index , bool isStartBeforeWord = false)
    {
        if (isStartBeforeWord && char.IsWhiteSpace(text, index))
        {
            return true;
        }
        
        if (!text.IsInBounds(index, EndOriginalValue.Length))
        {
            return false;
        }

        var hasDigit = char.IsDigit(text[index]);
        var hasWhiteSpace = HasEndWithBeforeWhiteSpace(text, index);
        return hasDigit || hasWhiteSpace;
    }

    protected virtual bool HasEndWithBeforeWhiteSpace(string text, int endIndex)
    {
        if (endIndex - 1 < 0)
        {
            return false;
        }

        var isEndIndex = IsTagEnd(text, endIndex);
        var previousIndex = endIndex - 1;
        return isEndIndex && char.IsWhiteSpace(text, previousIndex);
    }
}