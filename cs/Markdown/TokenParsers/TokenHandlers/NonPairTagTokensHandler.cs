using Markdown.Tags.Enum;
using Markdown.Tokens;

namespace Markdown.TokenParsers.TokenHandlers;

public class NonPairTagTokensHandler : ITokenHandler
{
    public int Priority => 5;
    private readonly Dictionary<TagType, HashSet<TokenType>> notValidTokenTypesInTagContent =
        CreateNotValidTokensInTagContent();
    private static readonly TagType[] TagTypes = Enum.GetValues<TagType>();
    
    public IReadOnlyList<Token> Handle(IReadOnlyList<Token> tokens)
    {
        var handledTokens = tokens.ToArray();
        var openTags = CreateStartOpenTagsDictionary();
        
        for (var i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];

            if (token.Type is TokenType.Word or TokenType.Separator)
                continue;
            
            ClearOpenTagsIfTokenNotValid(token, handledTokens, openTags);
            
            if (TokenUtilities.TryGetTagByCloseTag(token, out var tag) && 
                tag!.IsCorrectCloseTag(tokens, i) && 
                openTags[tag.TagType].Count != 0)
            {
                var openTag = 0;
                while (openTags[tag.TagType].TryPeek(out openTag) && 
                       tag.IsHaveProblemWithTags(tokens, openTag, i) == TagContentProblem.OpenTag)
                {
                    ChangeLastOpenTagTokenToWordToken(handledTokens, openTags, tag.TagType);
                }
                
                var lastTagContentProblem = tag.IsHaveProblemWithTags(tokens, openTag, i);
                // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
                switch (lastTagContentProblem)
                {
                    case TagContentProblem.NoContent:
                        ChangeLastOpenTagTokenToWordToken(handledTokens, openTags, tag.TagType);
                        handledTokens[i] = TokenUtilities.CreateTextToken(token.Content);
                        continue;
                    case TagContentProblem.None:
                        openTags[tag.TagType].Pop();
                        continue;
                }
            }
            
            if (TokenUtilities.TryGetTagByOpenTag(token, out var newOpenTag) && newOpenTag!.IsCorrectOpenTag(tokens, i))
                openTags[newOpenTag.TagType].Push(i);
            else
                ChangeTagTokenToWordToken(handledTokens, i);
        }
        ChangeNotClosedTagsInWordToken(handledTokens, openTags);
        
        return handledTokens;
    }
    
    private static Dictionary<TagType,Stack<int>> CreateStartOpenTagsDictionary()
    {
        var openTags = new Dictionary<TagType, Stack<int>>();
        
        foreach (var tagType in TagTypes)
            openTags[tagType] = new Stack<int>();
        
        return openTags;
    }

    private static Dictionary<TagType, HashSet<TokenType>> CreateNotValidTokensInTagContent()
    {
        var notValidTokensInTagContent = new Dictionary<TagType, HashSet<TokenType>>();
        
        foreach (var tagType in TagTypes)
            notValidTokensInTagContent[tagType] = new HashSet<TokenType>();
        notValidTokensInTagContent[TagType.Italic].Add(TokenType.NewLine);
        notValidTokensInTagContent[TagType.Strong].Add(TokenType.NewLine);
        
        return notValidTokensInTagContent;
    }
    
    private void ClearOpenTagsIfTokenNotValid(Token token,
        Token[] handledTokens,
        Dictionary<TagType, Stack<int>> openTags)
    {
        foreach (var tagNotValidTokens in notValidTokenTypesInTagContent)
        {
            if (!tagNotValidTokens.Value.Contains(token.Type)) 
                continue;
            ChangeTagTokensToWordTokens(handledTokens, openTags[tagNotValidTokens.Key]);
            openTags[tagNotValidTokens.Key].Clear();
        }
    }

    private static void ChangeNotClosedTagsInWordToken(Token[] handledTokens,
        Dictionary<TagType, Stack<int>> openTags)
    {
        foreach (var tagPositions in openTags.SelectMany(x => x.Value))
            ChangeTagTokenToWordToken(handledTokens, tagPositions);
    }
    
    private static void ChangeLastOpenTagTokenToWordToken(Token[] handledTokens,
        Dictionary<TagType, Stack<int>> openTags,
        TagType tagType)
    {
        var tag = openTags[tagType].Pop();
        ChangeTagTokenToWordToken(handledTokens, tag);
    }

    private static void ChangeTagTokensToWordTokens(Token[] handledTokens, IEnumerable<int> tags)
    {
        foreach (var tag in tags)
            ChangeTagTokenToWordToken(handledTokens, tag);
    }

    private static void ChangeTagTokenToWordToken(Token[] handledTokens, int position)
    {
        var token = handledTokens[position];
        
        if (token.Type == TokenType.Tag)
            handledTokens[position] = TokenUtilities.CreateTextToken(token.Content);
    }
}