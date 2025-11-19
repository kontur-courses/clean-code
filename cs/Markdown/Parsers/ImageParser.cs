using System.Text;
using Markdown.Parsers.Interfaces;

namespace Markdown.Parsers;

public class ImageParser : ITokenParser, ICompletableParse
{   
    private readonly Stack<Token> markdownBrackets = new();
    private readonly Stack<Token> markdownParenthesis = new();
    private bool startImageParse;
    private bool isAltEnd;
    private bool isUrlEnd;
    private readonly StringBuilder urlBuilder = new();
    private readonly ParserContext context;
    private readonly Stack<Token> imageTokens = new();
    
    public ImageParser(ParserContext context)
    {
        this.context = context;
    }
    
    public void Parse()
    {   
        imageTokens.Push(context.Current);
        if (!startImageParse)
        {
            if (context.Next is not { Type: TokenType.LBracket })
            {
                Finish();
                return;
            }
            startImageParse = true;
            return;

        }
        var currentType = context.Current.Type;
        if (currentType is TokenType.LBracket or TokenType.RBracket)
            ParseAlt();
        else 
            ParseUrl();
    }
    
    public void Finish()
    {
        startImageParse = false;
        isAltEnd = false;
        isUrlEnd = false;
        while (imageTokens.Count > 0)
        {
            imageTokens.Pop().Type = TokenType.Text;
        }
    }
    
    private void ParseAlt()
    {
        if (isAltEnd)
        {
            Finish();
            return;
        }
        
        TrackBracket(markdownBrackets, TokenType.LBracket, TokenType.RBracket, out isAltEnd);
    }

    private void ParseUrl()
    {
        if (!isAltEnd || context.Current.Type != TokenType.LParenthesis)
        {
            Finish();
            return;
        }
        
        while (context.Current.Type is not (TokenType.Eof or TokenType.NewLine or TokenType.Space))
        {
            var current = context.Current;
            if (current.Type is TokenType.LParenthesis or TokenType.RParenthesis)
            {
                imageTokens.Push(context.Current);
                TrackBracket(markdownParenthesis, TokenType.LParenthesis, TokenType.RParenthesis, out isUrlEnd);
                if (isUrlEnd)
                {
                    var isValidUrl = IsValidUrl(urlBuilder.ToString());
                    if (isValidUrl)
                    {
                        imageTokens.Clear();
                    }
                    else
                    {
                        Finish();
                        return;
                    }
                }
                context.IncreasePosition(); 
            }
            else
            {
                urlBuilder.Append(current.Value);
                current.Type = TokenType.Text;
                context.IncreasePosition();    
            }

        }
        
        Finish();
    }
    
    private void TrackBracket(Stack<Token> brackets, TokenType left, TokenType right, out bool parseEnd)
    {   
        var current = context.Current;
        
        if (brackets.Count == 0 && current.Type == left)
        {
            brackets.Push(current);
            parseEnd = false;
            return;     
        }

        if (current.Type == left)
        {
            current.Type = TokenType.Text;
            brackets.Push(current);
            parseEnd = false;
            return;
        }
        
        if (current.Type == right)
        {
            if (brackets.Count > 1)
            {
                current.Type = TokenType.Text;
                brackets.Pop();
                parseEnd = false;
                return;
            }
            brackets.Pop();
            parseEnd = true;
            return;
        }

        parseEnd = false;
    }
    
    private bool IsValidUrl(string text)
    {
        var allowed = new HashSet<string>{ ".jpg", ".jpeg", ".png", ".gif", ".svg", ".webp" };
        var canCreateUrl = Uri.TryCreate(text, UriKind.Absolute, out var url);
        if (canCreateUrl && url != null)
        {
            var path = url.AbsolutePath;
            var ext = Path.GetExtension(path).ToLowerInvariant();
            if (allowed.Contains(ext))
                return true;
        }

        return false;
    }

}