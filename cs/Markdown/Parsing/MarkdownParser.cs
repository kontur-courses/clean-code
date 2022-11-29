using System.Text;
using Markdown.MarkdownDocument;
using Markdown.Reading;
using TextReader = Markdown.Reading.TextReader;

namespace Markdown.Parsing;

public class MarkdownParser
{
    private readonly char _escapeSymbol = '\\';
    private readonly List<IMdTag> _tags;

    public IEnumerable<IMdTag> Tags => _tags;


    public MarkdownParser(List<IMdTag> tags)
    {
        _tags = tags;
    }

    public MarkdownParser()
    {
        _tags = new List<IMdTag>();
    }

    public MdParsedObjectModel ParseToDocument(string text)
    {
        return Parse(text).Document;
    }

    public List<Match> ParseToMatches(string text)
    {
        return Parse(text).Matches;
    }


    private (MdParsedObjectModel Document, List<Match> Matches) Parse(string text)
    {
        var matches = new List<Match>();
        var processMatches = new List<Match>();
        var reader = new TextReader(text);
        var document = new MdParsedObjectModel();

        var textSb = new StringBuilder();

        Token? currentToken = null;

        while (reader.ReadNextToken())
        {
            var previousToken = currentToken;
            currentToken = reader.Current;

            if (currentToken == null)
                break;

            //экранирование в самом начале тэга точно заработает
            if (previousToken != null && previousToken.Symbol == _escapeSymbol && currentToken.Symbol == _escapeSymbol)
                continue;

            // if (!currentToken.IsNull)


            foreach (var tagToCheck in _tags)
            {
                if (previousToken != null && previousToken.Symbol == _escapeSymbol)
                {
                    if (tagToCheck.NeedEscape(currentToken!))
                        break;
                }

                var mathPatterns = tagToCheck.CheckFirstCharMatch(currentToken!);
                if (mathPatterns.Any())
                {
                    mathPatterns.ForEach(pattern => { processMatches.Add(new Match(tagToCheck, currentToken.Position, pattern.CopyPatternTree())); });
                }
            }

            var fullMatchFound = CheckMatchesInProcess(matches, processMatches, currentToken, out var fullMatch);
            if (fullMatchFound)
            {
                matches.Add(fullMatch!);
                textSb.Remove(fullMatch!.StartPosition, fullMatch.Lenght);
                if (textSb.Length != 0)
                    document.AddNode(new PlaintextDocumentNode(textSb));

                if (fullMatch!.SourceGeneralMdTag.CanBeParsed)
                {
                    var parsedDoc = Parse(fullMatch.Text);
                    document.AddNode(new MatchedDocumentNode(fullMatch!.SourceGeneralMdTag.Name, parsedDoc.Document.Nodes));
                }
                else
                {
                    document.AddNode(new MatchedDocumentNode(fullMatch!.SourceGeneralMdTag.Name, new PlaintextDocumentNode(fullMatch.Text)));
                }

                textSb = new StringBuilder();
            }

            if (!currentToken.IsNull)
                textSb.Append(currentToken.Symbol);
        }

        if (textSb.Length != 0)
        {
            document.AddNode(new PlaintextDocumentNode(textSb));
        }

        if (processMatches.Any())
            processMatches.Clear();

        return (document, matches);
    }

    private bool CheckMatchesInProcess(List<Match> matchesFound, List<Match> processMatches, Token currentToken, out Match? match)
    {
        var matchesToRemove = new List<Match>();
        var matchFound = false;
        match = null;
        foreach (var matchToCheckState in processMatches)
        {
            if (matchFound)
            {
                matchesToRemove.Add(matchToCheckState);
                continue;
            }

            var checkResult = matchToCheckState.CheckToken(currentToken);

            if (checkResult == MatchState.TokenMatch)
                continue;

            if (checkResult == MatchState.FullMatch)
            {
                if (NotIntersectWithExist(matchesFound, matchToCheckState))
                {
                    match = matchToCheckState;
                    matchFound = true;
                }
            }

            matchesToRemove.Add(matchToCheckState);
        }

        if (matchesToRemove.Any())
        {
            matchesToRemove.ForEach(removeMatch => processMatches.Remove(removeMatch));
        }

        return matchFound;
    }

    //проверять что не пересекаются
    private static bool NotIntersectWithExist(List<Match> matches, Match matchToCheckState)
    {
        foreach (var match in matches)
        {
            if (match.StartPosition <= matchToCheckState.StartPosition &&
                match.StartPosition + match.Lenght >= matchToCheckState.StartPosition)
            {
                return false;
            }
        }

        return true;
    }
}