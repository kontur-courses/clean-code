namespace Markdown;

public class SimpleTagTokenizator : Tokenizator
{
    public HashSet<int> UsedIndexes { get; set; }
    private List<TagToken> result;
    public virtual string OpenTag => throw new Exception("MD tag not specified");
    public virtual string CloseTag => throw new Exception("MD tag not specified");
    public virtual Tag Tag => throw new Exception("Tag not specified");

    public SimpleTagTokenizator()
    {
        UsedIndexes = new();
        result = new();
    }

    public SimpleTagTokenizator(HashSet<int> usedIndexes)
    {
        UsedIndexes = usedIndexes;
        result = new();
    }

    public List<TagToken> Tokenize(string mdstring)
    {
        var words = mdstring.Split();
        var startIndex = 0;
        foreach (var word in words)
        {
            GetTokensFromWord(word, startIndex);
            startIndex += word.Length + 1;
        }

        GetAnotherTokens(mdstring);
        return result;
    }

    public void GetTokensFromWord(string word, int startIndex)
    {
        if (!(word.Contains(OpenTag) && word.Contains(CloseTag)))
            return;
        int i = 0, j;
        while (true)
        {
            i = word.IndexOf(OpenTag, i);
            while (UsedIndexes.Contains(startIndex + i))
            {
                i = word.IndexOf(OpenTag, i + 1);
                if (i == -1)
                    break;
            }

            if (i == -1)
                break;
            j = word.IndexOf(CloseTag, i + OpenTag.Length);
            while (UsedIndexes.Contains(startIndex + j))
            {
                j = word.IndexOf(CloseTag, j + 1);
                if (j == -1)
                    break;
            }

            if (j == -1)
                break;
            if (j - i == OpenTag.Length)
            {
                UpdateUsedIndexes(startIndex + i, startIndex + j);
                i = j + 1;
                continue;
            }

            result.Add(new TagToken(
                startIndex + i,
                startIndex + j + CloseTag.Length - 1,
                Tag));
            UpdateUsedIndexes(startIndex + i, startIndex + j);
            i = j + 1;
        }
    }

    public void GetAnotherTokens(string mdstring)
    {
        Stack<int> stack = new();
        for (int i = 0; i < mdstring.Length; i++)
        {
            if (i + OpenTag.Length < mdstring.Length
                && mdstring[i + OpenTag.Length] != ' '
                && (mdstring.Substring(i, OpenTag.Length) == OpenTag
                    && (i == 0 || mdstring[i - 1] == ' '))
                && !Contains(i, OpenTag.Length))
            {
                stack.Push(i);
            }

            if (i != 0
                && i + CloseTag.Length - 1 < mdstring.Length
                && mdstring[i - 1] != ' '
                && (mdstring.Substring(i, CloseTag.Length) == CloseTag
                    && (i == mdstring.Length - CloseTag.Length || mdstring[i + CloseTag.Length] == ' '))
                && !Contains(i, CloseTag.Length)
                && stack.Count > 0)
            {
                var j = stack.Pop();
                if (i - j == OpenTag.Length)
                {
                    UpdateUsedIndexes(j, i);
                    continue;
                }

                result.Add(new TagToken(
                    j,
                    i + CloseTag.Length - 1,
                    Tag));
                UpdateUsedIndexes(j, i);
            }
        }
    }

    public void UpdateUsedIndexes(int openTagIndex, int closeTagIndex)
    {
        for (int i = 0; i < OpenTag.Length; i++)
            UsedIndexes.Add(openTagIndex + i);
        for (int i = 0; i < CloseTag.Length; i++)
            UsedIndexes.Add(closeTagIndex + i);
    }

    public bool Contains(int i, int tagLength)
    {
        for (int j = 0; j < tagLength; j++)
        {
            if (UsedIndexes.Contains(i + j))
                return true;
        }

        return false;
    }
}