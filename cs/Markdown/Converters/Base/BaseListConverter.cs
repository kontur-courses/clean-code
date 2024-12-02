using Markdown.Constants;
using Markdown.Extensions;
using Markdown.Models;

namespace Markdown.Converters.Base;

public abstract class BaseListConverter : BaseConverter
{
    private string StartPrefixValue => "<ul>";
    private string EndPrefixValue => "</ul>";

    public override IEnumerable<TagPair> Convert(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            yield break;
        }

        var index = 0;
        var isInTag = false;
        var tagPairBuilder = TagPair.CreateBuilder(GetType());
        var tokenIndex = 0;
        TagPair? tagBuffer = null;
        while (index < text.Length)
        {
            if (!isInTag && IsTagStart(text, index, tokenIndex))
            {
                var token = new TagToken(StartOriginalValue, StartConvertedValue, index);
                token = tokenIndex == 0 ? GetStartListToken(token) : token;
                tagPairBuilder.AddOpenToken(token);
                index += StartOriginalValue.Length;
                isInTag = true;
                continue;
            }

            if (ShouldAbortTag(text, index, isInTag))
            {
                if (tagBuffer is not null)
                {
                    var endTagPair = tagBuffer with
                    {
                        CloseToken = GetEndListToken(tagBuffer.CloseToken, false)
                    };
                    yield return endTagPair;
                    tagBuffer = null;
                }
                tokenIndex = 0;
                isInTag = false;
            }

            if (isInTag && IsTagEnd(text, index))
            {
                tokenIndex++;
                if (tagBuffer is not null)
                {
                    yield return tagBuffer;
                }

                var token = new TagToken(EndOriginalValue, EndConvertedValue, index);
                tagPairBuilder.AddCloseToken(token);
                tagBuffer = tagPairBuilder.Build();
                index += EndOriginalValue.Length;
                isInTag = false;
                continue;
            }

            index++;
        }

        if (tagBuffer is null)
        {
            yield break;
        }

        var isEndTextIndex = tagBuffer.CloseToken.TagIndex == text.Length;
        var tagPair = tagBuffer with
        {
            CloseToken = GetEndListToken(tagBuffer.CloseToken, isEndTextIndex)
        };
        yield return tagPair;
    }

    private TagToken GetStartListToken(TagToken tokenValue)
    {
        var openToken = tokenValue with
        {
            ReplaceTag = $"{StartPrefixValue}{tokenValue.ReplaceTag}",
        };
        return openToken;
    }

    private TagToken GetEndListToken(TagToken tokenValue, bool isEndTextIndex)
    {
        var closeToken = tokenValue with
        {
            ReplaceTag = $"{tokenValue.ReplaceTag}{EndPrefixValue}",
            TagIndex = isEndTextIndex ? tokenValue.TagIndex + 1 : tokenValue.TagIndex
        };
        return closeToken;
    }

    protected virtual bool IsTagStart(string text, int index, int tokenIndex)
    {
        if (!text.IsInBounds(index, StartOriginalValue.Length))
        {
            return false;
        }

        var nextIndex = index + StartOriginalValue.Length;
        var previousIndex = index - StartOriginalValue.Length;
        return text.ContainsTagAtIndex(index, StartOriginalValue)
               && !text.HasAdjacentTag(nextIndex, StartOriginalValue)
               && !text.HasAdjacentTag(previousIndex, StartOriginalValue)
               && (IsStartList(text, previousIndex, tokenIndex) 
                   || tokenIndex > 0);
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

    protected virtual bool ShouldAbortTag(string text, int index, bool isInTag)
    {
        return isInTag && !text.IsInBounds(index, EndOriginalValue.Length)
            || !isInTag && text[index].Equals(ConverterConstants.NewLine) && !IsTagEnd(text, index + 1);
    }

    private bool IsStartList(string text, int previousIndex, int tokenIndex)
    {
        return tokenIndex == 0 && (text.HasAdjacentTag(previousIndex, "\n") || previousIndex < 0);
    }
}