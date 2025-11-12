using Markdown.Data;

namespace Markdown;

public class TokenParser
{
    private List<Token> _tokens;
    private string _text;
    private ParserValidator _validator;
    private Stack<PositionedTag> _stackOfTags;

    public TokenParser(string text = "")
    {
        _text = text;
        _validator = new (text);
        _tokens = new ();
        _stackOfTags = new ();
    }
    
    public IEnumerable<Token> ParseTokens(string input, string outerTokenMark = "")
    {
        _text = input;
        _validator = new ParserValidator(input);
        _tokens = new List<Token>();
        _stackOfTags = new Stack<PositionedTag>();

        _tokens.AddRange(FindAllTokens());
        
        _tokens.Sort((x, y) => x.StartPosition.CompareTo(y.StartPosition));
        return _tokens;
    }

    private IEnumerable<Token> FindAllTokens()
    {
        for (int i = 0; i < _text.Length; i++)
        {
            if (_text[i] == '\n')
            {
                foreach (var token in BuildTokensFromStack(i))
                {
                    yield return token;
                }
                continue;
            }
            
            var findedTag = FindTag(i);
            if (findedTag == null)
                continue;
            
            if (_stackOfTags.Count > 0)
            {
                var lastTag = _stackOfTags.Pop();
                if (lastTag.Name == findedTag.Name)
                {
                    if (CheckOuterToken())
                        continue;
                    
                    if (lastTag.IsOpening && (!findedTag.IsOpening || findedTag.IsOpenClose))
                    {
                        var token = BuildTokenOrNull(lastTag, findedTag);
                        if (token is not null)
                            yield return token;
                    }
                    else
                    {
                        if (lastTag.Name == TagNames.Header || lastTag.Name == TagNames.List)
                            _stackOfTags.Push(lastTag);
                        PushIfOpened(findedTag);
                    }
                }
                else
                {
                    
                    _stackOfTags.Push(lastTag);
                    _stackOfTags.Push(findedTag);
                }
            }
            else
                PushIfOpened(findedTag);
        }
        foreach (var token in BuildTokensFromStack(_text.Length))
        {
            yield return token;
        }
    }

    private bool CheckOuterToken()
    {
        if (_stackOfTags.Count == 0)
            return false;
        var outerTag = _stackOfTags.Pop();
        _stackOfTags.Push(outerTag);
        if (outerTag.Name == TagNames.Em)
        {
            return true;
        }
        return false;
    }

    private IEnumerable<Token> BuildTokensFromStack(int lineEnd)
    {
        while (_stackOfTags.Count > 0)
        {
            var current = _stackOfTags.Pop();
            if (current.Mark == Marks.Header || current.Mark == Marks.List)
            {
                var token = BuildTokenOrNull(current, new PositionedTag(lineEnd, current.Mark, false));
                if (token is not null)
                    yield return token;
            }
        }
    }

    private Token? BuildTokenOrNull(PositionedTag startTag, PositionedTag endTag)
    {
        var mark = startTag.Mark;
        var start = startTag.Position;
        var end = endTag.Position;

        if (end - start < 2 || startTag.Name != endTag.Name)
            return null;
                
        var content = _text.Substring(start + mark.Length + Marks.AfterMarkSpace(mark), endTag.Position - start - mark.Length - Marks.AfterMarkSpace(mark));
        
        if (!_validator.IsContentAcceptable(content, mark) || _validator.IsSplittingWords(start, end + mark.Length))
        {
            return null;
        }
        
        var token = new Token(
            TagFactory.BuildTag(mark),
            start,
            end
        );
        return token;
    }
    
    private void PushIfOpened(PositionedTag findedTag)
    {
        if (findedTag.IsOpening)
            _stackOfTags.Push(findedTag);
    }

    private PositionedTag? FindTag(int index)
    {
        foreach (var mark in Marks.AllMarks)
        {
            var tag = GetPositionedTagOrNull(index, mark);
            if (tag is not null) 
                return tag;
        }

        return null;
    }
    
    #region GetPositionedTag
    
    private PositionedTag? GetPositionedTagOrNull(int index, string mark)
    {
        var isOpening = CheckByMark(index, mark, true);
        var isClosing = CheckByMark(index, mark, false);
        if (isOpening && isClosing)
        {
            return new PositionedTag(index, mark, isOpenClose:true);
        }
        if (isOpening)
            return new PositionedTag(index, mark, isOpening:isOpening);
        if (isClosing)
            return new PositionedTag(index, mark, isOpening:isOpening);
        
        return null;
    }

    private bool CheckByMark(int index, string mark, bool isOpening)
    {
        switch (mark)
        {
            case Marks.Bold:
                return CheckMarkForBold(index, isOpening);
            case Marks.Italic:
                return CheckMarkForItalic(index, isOpening);
            case Marks.Header:
                return CheckMarkForHeader(index, isOpening);
            case Marks.List:
                return CheckMarkForList(index, isOpening);
            default:
                return false;
        }
    }

    private bool CheckMarkForBold(int index, bool isOpening)
    {
        return index + 1 < _text.Length
               && _text[index] == '_'
               && _text[index + 1] == '_'
               && _validator.IsMarkCorrect(index, isOpening, Marks.Bold.Length);
    }
    
    private bool CheckMarkForItalic(int index, bool isOpening)
    {
        return index < _text.Length
               && _text[index] == '_'
               && !_validator.IsDoubleUnderscore(index)
               && _validator.IsMarkCorrect(index, isOpening, Marks.Italic.Length);
    }
    
    private bool CheckMarkForHeader(int index, bool isOpening)
    {
        return isOpening
               && index + 1 < _text.Length
               && _text[index].ToString() == Marks.Header
               && char.IsWhiteSpace(_text[index + 1])
               && !_validator.IsScreened(index);
    }
    
    private bool CheckMarkForList(int index, bool isOpening)
    {
        return isOpening 
               && index + 1 < _text.Length
               && _text[index].ToString() == Marks.List
               && char.IsWhiteSpace(_text[index+1])
               && !_validator.IsScreened(index);
    }
    
    #endregion
}