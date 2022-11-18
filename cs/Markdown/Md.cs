using Markdown.Contracts;
using Markdown.HtmlParsers;
using Markdown.States;
using Markdown.Tokens;

namespace Markdown;

public class Md
{
    private readonly ITokenParser documentParser;

    public Md(ITokenParser documentParser)
    {
        this.documentParser = documentParser;
    }

    public static Md Html()
    {
        var parsers = new Dictionary<TokenType, ITokenParser>
        {
            { TokenType.Italic, new HtmlItalicParser() },
            { TokenType.Plain, new HtmlTextParser() }
        };
        parsers.Add(TokenType.Bold, new HtmlBoldTextParser(parsers));
        parsers.Add(TokenType.Header, new HtmlHeaderParser(parsers));
        parsers.Add(TokenType.Paragraph, new HtmlParagraphParser(parsers));
        var documentParser = new DocumentParser(parsers);
        return new(documentParser);
    }

    public string Render(string markdown)
    {
        var transitions = new List<Transition>
        {
            new ReadItalicTextErrorPossibleBoldTextCausedTransition(),
            new ReadItalicTextErrorAlfaNumericTransition(),
            new ReadItalicTextErrorInSeparateWordsTransition(),
            new ReadItalicTextErrorEndCausedTransition(),
            new ReadBoldTextErrorEmptyCausedTransition(),
            new ReadBoldTextErrorAlfaNumericTransition(),
            new ReadBoldTextErrorInSeparateWordsTransition(),
            new ReadBoldTextErrorEndCausedTransition(),
            new EndReadItalicTextTransition(),
            new EndReadPlainTextTransition(),
            new EndReadBoldTextTransition(),
            new EndReadHeaderTransition(),
            new EndReadParagraphTransition(),
            new EndReadDocumentTransition(),
            new ReadHeaderTransition(),
            new ReadParagraphTransition(),
            new ReadBoldTextTransition(),
            new ReadItalicTextTransition(),
            new ReadPlainTextTransition(),
            new ReadDocumentTransition()
        };
        var state = new State(markdown);

        while (state.Process != ProcessState.EndReadDocument)
        {
            Console.WriteLine(state);
            DoTransition(transitions, state);
            Console.WriteLine(state);
        }

        Console.WriteLine("End");
        Console.WriteLine(state);
        Console.WriteLine();

        var html = documentParser.Parse(state.Document);

        Console.WriteLine(html);

        return html;
    }

    private static void DoTransition(IEnumerable<Transition> transitions, State state)
    {
        var transition =
            transitions.FirstOrDefault(x => state.IgnoredTransitions.All(t => t.transition != x) && x.When(state));
        if (transition is null)
            throw new ApplicationException(
                $"Cannot parse markdown, transition not found for state {state}");
        Console.WriteLine($"Transition: {transition}");
        transition.Do(state);
    }
}