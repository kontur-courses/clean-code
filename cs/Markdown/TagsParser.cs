namespace Markdown;

public class TagsParser(string paragraph)
{
    private int currentPos = 0;
    private List<Tag> tags = new();

    private readonly Dictionary<TagType, bool>  isTagOpened = new()
    { 
        { TagType.H1, false },
        { TagType.Strong, false },
        { TagType.Em, false },
        { TagType.Escaping, true },
    };

    public List<Tag> BuildTags()
    {
        GetAllTags();
        ProcessCharsInsideWords();
        ProcessTagsBorders(TagType.Em, TagType.Strong);
        CleanEmptyTagsInLine();
        return tags;
    }

    private void ProcessCharsInsideWords()
    {
        var tagInformationByType = CreateTagInWordInfoDict([TagType.Strong, TagType.Em]);
        foreach (var tagGroup in tagInformationByType.GroupBy(tag => tag.Tag.Type))
        {
            var tagsList = tagGroup.ToList();
            for (var tagIndex = 0; tagIndex < tagsList.Count; tagIndex++)
            {
                var currentTag = tagsList[tagIndex];
                var nextTag = (tagIndex + 1 < tagsList.Count)
                    ? tagsList[tagIndex + 1]
                    : null;
                if (IsEmTags(currentTag, nextTag))
                    tagIndex++;
            }
        }
    }
           
    private List<TagInWordInformation> CreateTagInWordInfoDict(List<TagType> tagTypes)
    {
        var tagInformationByType = new List<TagInWordInformation>();

        var wordIndex = 0;
        for (var i = 0; i < tags.Count; i++)
        {
            var currentTag = tags[i];
            if (IsSpaceChar(currentTag))
                wordIndex++;
            if (!tagTypes.Contains(currentTag.Type))
                continue;
            var tag = GetTagInWordInformation(wordIndex, i);
            tagInformationByType.Add(tag);
        }

        return tagInformationByType;
    }

    private TagInWordInformation? GetTagInWordInformation(int wordIndex, int pos) =>
        IsTagInListBounds(pos)
        ? new TagInWordInformation(wordIndex, IsTagPartOfWord(tags[pos - 1], tags[pos + 1]), tags[pos])
        : new TagInWordInformation(wordIndex, false, tags[pos]);

    private void CleanEmptyTagsInLine() 
    {
        var openTags = tags
            .Select((tag, ind) => (tag, ind))
            .Where(x => x.tag.PairType == PairTokenType.Opening)
            .ToList();
        var closeTags = tags
            .Select((tag, ind) => (tag, ind))
            .Where(x => x.tag.PairType == PairTokenType.Closing)
            .ToList();

        for (var i = 0; i < openTags.Count; i++)
        {
            var stringBetweenTags = tags[openTags[i].ind..closeTags[i].ind];
            if (IsTextInString(stringBetweenTags)) 
                continue;
            ConvertTagToTextTag(openTags[i].tag);
            ConvertTagToTextTag(closeTags[i].tag);
        }
    }

    private void GetAllTags()
    {
        while (currentPos < paragraph.Length)
        {
            var tag = GetTagFromChar();
            GetNextPos(tag);
            tags.Add(tag);
        }
    }

    private void GetNextPos(Tag tag)
    {
        currentPos += tag.TagText.Length;
        if (tag.Type == TagType.Escaping) 
            currentPos++;
    }

    private bool IsNextCharIs(params char[] chars)
    {
        var nextChar = paragraph[currentPos + 1];
        return chars.Contains(nextChar);
    }

    private Tag GetTagFromChar() => paragraph[currentPos] switch
    {
        '#' => ParseH1Tag(),
        '_' => ParseEmOrStrongTag(),
        '\\' => ParseEscapingTag(),
        '[' => ParseLinkTag(),
        _ => CreateCurrentPosTextTag(),
    };

    private Tag ParseH1Tag() =>
        (currentPos == 0 && paragraph[currentPos + 1] == ' ')
        ? Tag.Create(TagType.H1, PairTokenType.Opening, paragraph[currentPos] + " ")
        : CreateCurrentPosTextTag();


    private Tag ParseEmOrStrongTag() =>
        (IsEndOfParagraph() && IsNextCharIs('_'))
        ? Tag.Create(TagType.Strong, PairTokenType.Opening, paragraph.Substring(currentPos, 2))
        : Tag.Create(TagType.Em, PairTokenType.Opening, paragraph[currentPos]);

    private Tag ParseEscapingTag()
    {
        if (!IsEndOfParagraph())
            return CreateCurrentPosTextTag();
        
        return IsNextCharIs('_', '#', '\\', '[', ']')
            ? Tag.Create(TagType.Escaping, PairTokenType.Completed, paragraph[currentPos + 1])
            : CreateCurrentPosTextTag();
    }

    private Tag CreateCurrentPosTextTag() =>
        Tag.Create(TagType.Text, PairTokenType.None, paragraph[currentPos]);

    private Tag ParseLinkTag()
    {
        var linkTextEnd = paragraph.IndexOf(']', currentPos);
        var linkStart = paragraph.IndexOf('(', currentPos);
        var linkEnd = paragraph.IndexOf(')', currentPos);
        var slash = paragraph.IndexOf('\\', currentPos);
        if (linkStart == linkTextEnd + 1 && linkEnd > linkStart
            && CheckSlashInLink(slash, linkTextEnd, linkStart, linkEnd)
            && linkEnd - linkStart > 1)
            return Tag.Create(TagType.Link, PairTokenType.Single, paragraph.Substring(currentPos, linkEnd - currentPos + 1));
        
        return CreateCurrentPosTextTag();
    }

    private void ProcessTagsBorders(TagType outsideTag, TagType insideTag)
    {
        var stack = new Stack<Tag>();
        for (var i = 0; i < tags.Count; i++)
        {
            var currentTag = tags[i];
            if (IsIncorrectTag(currentTag, i)) 
                continue;
            if (stack.Count > 0 && CheckCorrectPairTag(stack, currentTag, outsideTag, insideTag))
                continue; 
            stack.Push(currentTag);
            isTagOpened[currentTag.Type] = true;
        }
        CleanStackOfRemainingTags(stack);
    }

    private bool CheckCorrectPairTag(
        Stack<Tag> stack, 
        Tag currentTag, 
        TagType outsideTag, 
        TagType insideTag)
    {
        if (isTagOpened[currentTag.Type])
        {
            var lastTag = stack.Pop();
            if (IsPairTags(lastTag, currentTag))
                return true;
            ConvertTagToTextTag(currentTag);
            ProcessWrongTagsOnStack(stack);
        }
        else if (IsIncorrectTagsNesting(stack, currentTag, outsideTag, insideTag))
        {
            ConvertTagToTextTag(currentTag);
            return true;
        }
        return false;
    }

    private bool IsIncorrectTag(Tag currentTag, int pos) =>
        IsUnprocessedTag(currentTag)
        || IsTextTag(currentTag)
        || IsNotTag(pos);

    private void CleanStackOfRemainingTags(Stack<Tag> stack)
    {
        while (stack.Count > 0)
        {
            var currentTag = stack.Pop();
            if (currentTag.Type == TagType.H1)
                tags.Add(Tag.Create(TagType.H1, PairTokenType.Closing, "#"));
            else if (currentTag.PairType != PairTokenType.Single)
            {
                isTagOpened[currentTag.Type] = false;
                ConvertTagToTextTag(currentTag);
            }
        }
    }

    private bool IsPairTags(Tag lastTag, Tag currentTag)
    {
        if (lastTag.Type == currentTag.Type)
        {
            currentTag.PairType = PairTokenType.Closing;
            isTagOpened[currentTag.Type] = false;
            return true;
        }
        ConvertTagToTextTag(lastTag);
        isTagOpened[currentTag.Type] = false;
        return false;
    }

    private bool IsNotTag(int pos)
    {
        var currentTag = tags[pos];
        var prevPos = pos - 1;
        var nextPos = pos + 1;
        var prevTag = GetCorrectBoundaryTag(prevPos);
        var nextTag = GetCorrectBoundaryTag(nextPos);
        if (IsNotFirstOpenTag(currentTag, prevTag, nextTag)
            || IsNotCloseTagAfterOpenTag(currentTag, prevTag, nextTag)
            || IsTagNearNumber(prevTag, nextTag)
            || IsTagBetweenSpaces(prevTag, nextTag))
        {
            ConvertTagToTextTag(currentTag);
            return true;
        }
        return false;
    }

    private Tag GetCorrectBoundaryTag(int pos) =>
        !IsTagInListBounds(pos)
        ? CreateDefaultTextTag()
        : tags[pos];

    private bool IsTagInListBounds(int pos) => pos - 1 > 0 && pos + 1 < tags.Count;

    private bool IsEndOfParagraph() => currentPos + 1 < paragraph.Length;

    private bool IsNotFirstOpenTag(Tag currentTag, Tag prevTag, Tag nextTag) =>
        IsOpenTag(prevTag, nextTag) && isTagOpened[currentTag.Type];

    private bool IsNotCloseTagAfterOpenTag(Tag currentTag, Tag prevTag, Tag nextTag) =>
        IsCloseTag(prevTag, nextTag) && !isTagOpened[currentTag.Type];

    private bool CheckSlashInLink(int slash, int linkTextEnd, int linkStart, int linkEnd) =>
        (slash == -1 ||
        (linkTextEnd - slash != 1 && currentPos - slash != 1
        && linkStart - slash != 1 && linkEnd - slash != 1));

    private static bool IsOpenTag(Tag prevTag, Tag nextTag) =>
        IsSpaceChar(prevTag) && !IsSpaceChar(nextTag);

    private static bool IsCloseTag(Tag prevTag, Tag nextTag) =>
        IsSpaceChar(nextTag) && !IsSpaceChar(prevTag);

    private static bool IsTagNearNumber(Tag prevTag, Tag nextTag) =>
        (char.IsDigit(nextTag.TagText[0]) && char.IsLetter(prevTag.TagText[0])) 
        || (char.IsDigit(prevTag.TagText[0]) && char.IsLetter(nextTag.TagText[0]));

    private static bool IsTagPartOfWord(Tag prevTag, Tag nextTag) =>
        char.IsLetter(nextTag.TagText[0]) && char.IsLetter(prevTag.TagText[0]);

    private static bool IsTagBetweenSpaces(Tag prevTag, Tag nextTag) =>
        IsSpaceChar(nextTag) && IsSpaceChar(prevTag);
    
    private static bool IsSpaceChar(Tag tag) => tag.TagText == " ";

    private static bool IsTagsInOneWord(TagInWordInformation currentTag, TagInWordInformation nextTag) =>
        currentTag.WordWithTagIndex != nextTag.WordWithTagIndex;

    private static bool IsUnprocessedTag(Tag tag) => tag.Type == TagType.Escaping || tag.Type == TagType.Link;
    
    private static bool IsTextTag(Tag tag) => tag.Type == TagType.Text;

    private static bool IsTextInString(List<Tag> stringBetweenTags) => stringBetweenTags.Any(x =>
        char.IsLetter(x.TagText[0]) || char.IsDigit(x.TagText[0])
                                    || x.TagText.Length > 2);

    private static bool IsIncorrectTagsNesting(
        Stack<Tag> stack, 
        Tag currentTag,  
        TagType outsideTag, 
        TagType insideTag
        ) => stack.Last().Type == outsideTag && currentTag.Type == insideTag;

    private static Tag CreateDefaultTextTag() =>
        Tag.Create(TagType.Text, PairTokenType.None, "/");

    private static void ConvertTagToTextTag(Tag tag)
    {
        tag.Type = TagType.Text;
        tag.PairType = PairTokenType.None;
    }

    private static void ProcessWrongTagsOnStack(Stack<Tag> stack)
    {
        while (stack.Count > 0)
        {
            var currentTag = stack.Pop();
            ConvertTagToTextTag(currentTag);
        }
    }

    private static bool IsEmTags(
        TagInWordInformation currentTag,
        TagInWordInformation? nextTag
        )
    {
        nextTag = nextTag == null
                    ? new TagInWordInformation(null, false, CreateDefaultTextTag())
                    : nextTag;
        if (IsTagsInOneWord(currentTag, nextTag))
        {
            if (currentTag.IsTagInWord)
                ConvertTagToTextTag(currentTag.Tag);
            if (nextTag.IsTagInWord)
                ConvertTagToTextTag(nextTag.Tag);
            return false;
        }
        return true;
    }
}


public class TagInWordInformation(int? wordWithTagIndex, bool isTagInWord, Tag tag)
{
    public int? WordWithTagIndex { get; set; } = wordWithTagIndex;
    public bool IsTagInWord { get; set; } = isTagInWord;
    public Tag Tag { get; set; } = tag;
}

