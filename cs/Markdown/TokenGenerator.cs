using System.Text;
using Markdown.Parsers;
using Markdown.TagsType;

namespace Markdown;

public class TokenGenerator
{
    private BoldParser boldParser;
    private ItalicParser italicParser;
    private LinkParser linkParser;

    private readonly Stack<Token> tagsStack;
    private SlashParser slashParser;
    private readonly List<IParser> parsers;

    public TokenGenerator()
    {
        boldParser = new BoldParser();
        linkParser = new LinkParser();
        italicParser = new ItalicParser();
        var headingParser = new HeadingParser();
        slashParser = new SlashParser(boldParser);
        parsers = new List<IParser>
        {
            boldParser,
            headingParser,
            linkParser,
            slashParser,
            italicParser
        };

        tagsStack = GeneralParser.tagsStack;
    }

    public List<Token> SplitParagraphs(string text)
    {
        ListOfTokens<Token> tokens = new();

        var paragraphs = text.Split("\r\n");
        foreach (var line in paragraphs)
        {
            Tokenize(line, tokens);
            tagsStack.Clear();
        }

        tokens.Remove(tokens.Last());
        ConvertTokensWithOutPairToMarkdown(tokens);
        linkParser = new LinkParser();
        boldParser = new BoldParser();
        italicParser = new ItalicParser();
        slashParser = new SlashParser(boldParser);
        return tokens;
    }

    private void Tokenize(string line, ListOfTokens<Token> tokens)
    {
        var lastAddIndex = tokens.Count == 0 ? 0 : tokens.Count() + 1;
        var words = line.Split(' ');
        foreach (var word in words)
        {
            AddToken(word, tokens);
            if (tokens.Count != 0)
                tokens.Add(new Token(" ", new TextTag()));
        }

        tokens.Remove(tokens.Last());
        if (lastAddIndex > -1 && tokens[lastAddIndex].Context == new HeadingTag().GetHtmlOpenTag)
        {
            var headingTag = new HeadingTag();
            tokens.Add(new Token(headingTag.GetHtmlCloseTag, headingTag));
        }

        tokens.Add(new Token("\n", new TextTag()));
    }

    private void AddToken(string text, ListOfTokens<Token> tokens)
    {
        var countOfLastAddedTokens = tokens.Count();
        var isHaveDigitInside = false;
        if (linkParser.IsThisLink)
        {
            tokens.Add(linkParser.LinkParse(text));
            return;
        }

        for (int i = 0; i < text.Length; i++)
        {
            bool match = false;

            foreach (var parser in parsers)
            {
                if (parser.TryParse(text[i], text, i))
                {
                    match = true;
                    tokens.Add(parser.Parse(text, ref i));
                    break;
                }
            }

            if (!match)
            {
                if (Char.IsDigit(text[i]))
                    isHaveDigitInside = true;
                tokens.Add(new Token(text[i].ToString(), new TextTag()));
            }
        }

        countOfLastAddedTokens = tokens.Count - countOfLastAddedTokens;
        GetValidResult(isHaveDigitInside, tokens, countOfLastAddedTokens);
    }

    private void GetValidResult(bool isHaveDigitInside, ListOfTokens<Token> tokens, int countOfLastAddedTokens)
    {
        if (isHaveDigitInside)
            MarkLastTokensToMarkdown(tokens, countOfLastAddedTokens);
        if (italicParser.IsHaveInsideItalicTag && tagsStack.Count != 0)
        {
            tagsStack.Pop();
        }

        if (boldParser.IsHaveInsideBoldTag && tagsStack.Count != 0)
        {
            tagsStack.Pop();
        }
    }

    private void ConvertTokensWithOutPairToMarkdown(ListOfTokens<Token> tokens)
    {
        foreach (var token in tokens)
        {
            if (token.Type is not TextTag && !token.Type.HasPair)
            {
                token.Context = token.Type.MarkdownTag;
            }
        }
    }

    private void MarkLastTokensToMarkdown(ListOfTokens<Token> tokens, int countOfLastAddedTokens)
    {
        for (int i = tokens.Count - countOfLastAddedTokens; i < tokens.Count; i++)
        {
            if (tokens[i].Type is not TextTag)
            {
                tokens[i].Type!.HasPair = false;
            }
        }
    }
}