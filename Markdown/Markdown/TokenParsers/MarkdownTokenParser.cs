using Markdown.Tags;
using Markdown.TokenParsers.MarkdownParsers;
using Markdown.Tokens;

namespace Markdown.TokenParsers;

public class MarkdownTokenParser : ITokenParser
{
	private readonly Dictionary<TokenType, MarkdownTag> markdownTags;

	private readonly Dictionary<TokenType, IMarkdownTagParser> parsers;
	private readonly Dictionary<MarkdownTag, TokenType> tokenTypes;


	public MarkdownTokenParser()
	{
		tokenTypes = new Dictionary<MarkdownTag, TokenType>
		{
			{ new MarkdownTag("__"), TokenType.Bold },
			{ new MarkdownTag("_"), TokenType.Italic },
			{ new MarkdownTag("# ", $"{Environment.NewLine}{Environment.NewLine}"), TokenType.Header }
		};

		markdownTags = tokenTypes.ToDictionary(pair => pair.Value, pair => pair.Key);

		parsers = new Dictionary<TokenType, IMarkdownTagParser>
		{
			{ TokenType.Italic, new ItalicParser() },
			{ TokenType.Bold, new BoldParser() },
			{ TokenType.Header, new HeaderParser() }
		};
	}

	private static string ParagraphSplitter => $"{Environment.NewLine}{Environment.NewLine}";

	public IToken Parse(string text)
	{
		var paragraphsStartPositions = GetParagraphsStartPositions(text);
		var result = ParseParagraphs(text, paragraphsStartPositions);
		return result;
	}

	private List<int> GetParagraphsStartPositions(string text)
	{
		var result = new List<int> { 0 };

		for (var i = 0; i < text.Length - ParagraphSplitter.Length - 1; i++)
		{
			var window = text.Substring(i, ParagraphSplitter.Length);
			if (window == ParagraphSplitter) result.Add(i + ParagraphSplitter.Length + 1);
		}

		return result;
	}

	private IToken ParseParagraphs(string text, IReadOnlyList<int> paragraphsStartPositions)
	{
		IToken result = new MdToken(text, 0, 0, TokenType.PlainText);
		var currentToken = result;

		Dictionary<TokenType, List<MdToken>> paragraphTokens;
		for (var i = 1; i < paragraphsStartPositions.Count; i++)
		{
			var paragraphStart = paragraphsStartPositions[i - 1];
			var paragraphEnd = paragraphsStartPositions[i] - ParagraphSplitter.Length;
			paragraphTokens = ParseParagraphTokens(text, paragraphStart, paragraphEnd);

			currentToken.nextToken = MergeTokens(paragraphTokens, text, paragraphStart, paragraphEnd);
			currentToken = currentToken.nextToken;
			currentToken.nextToken =
				new MdToken(ParagraphSplitter, 0, ParagraphSplitter.Length, TokenType.PlainText);
		}

		paragraphTokens = ParseParagraphTokens(
			text,
			paragraphsStartPositions.Last(),
			text.Length);

		if (paragraphTokens.Count == 0)
			return new MdToken(text, paragraphsStartPositions.Last(), text.Length, TokenType.PlainText);
		currentToken.nextToken =
			MergeTokens(paragraphTokens, text, paragraphsStartPositions.Last(), text.Length);

		return result;
	}

	private Dictionary<TokenType, List<MdToken>> ParseParagraphTokens(string text, int paragraphStartPosition,
		int paragraphEndPosition)
	{
		var paragraphTokens = new Dictionary<TokenType, List<MdToken>>();

		foreach (var (tokenType, parser) in parsers)
		{
			var tokens = parser.ParseParagraph(
				text,
				paragraphStartPosition,
				paragraphEndPosition);

			if (tokens.Count == 0) continue;

			paragraphTokens.Add(tokenType, tokens);
		}

		return paragraphTokens;
	}

	private IToken MergeTokens(Dictionary<TokenType, List<MdToken>> tokens, string text, int paragraphStartPosition,
		int paragraphEndPosition)
	{
		if (tokens.Count == 0) throw new ArgumentException("Needed at least one token");
		var result = new MdToken(text, paragraphStartPosition, paragraphEndPosition, TokenType.PlainText);

		if (tokens.ContainsKey(TokenType.Header))
		{
			result = tokens[TokenType.Header].First();
			tokens.Remove(TokenType.Header);
			if (tokens.Count > 0)
				result.nestingTokens = MergeTokens(tokens, text, paragraphStartPosition + 2, paragraphEndPosition);

			return result;
		}

		var orderedTokens = new List<MdToken>();
		foreach (var (tokenType, parsedTokens) in tokens) orderedTokens.AddRange(parsedTokens);
		orderedTokens = orderedTokens.OrderBy(t => t.Start).ToList();
		AddTokens(orderedTokens, ref result);

		return result;
	}

	private void AddTokens(List<MdToken> tokens, ref MdToken result)
	{
		var currentToken = result;
		result = currentToken;
		MdToken? prevToken = null;
		foreach (var token in tokens)
		{
			FindPlace(ref prevToken, ref currentToken, token);
			if (IntersectWithOtherToken(currentToken, token))
			{
				ResolveIntersection(prevToken, currentToken, token);
				continue;
			}

			if (currentToken.Type == TokenType.PlainText)
				InsertTokenInText(token, ref currentToken, prevToken);
			else
				AddNestingToken(token, ref currentToken);

			if (prevToken is null) result = currentToken;
			prevToken = currentToken;
		}
	}

	private void FindPlace(ref MdToken prevToken, ref MdToken currentToken, MdToken token)
	{
		if (currentToken.Start <= token.Start && currentToken.End > token.Start) return;
		if (currentToken.nextToken is null) return;

		prevToken = currentToken;
		currentToken = currentToken.nextToken as MdToken;
	}

	private bool IntersectWithOtherToken(MdToken currentMdToken, MdToken mdToken)
	{
		return currentMdToken.End < mdToken.End;
	}

	private void ResolveIntersection(MdToken prevToken, MdToken currentToken, MdToken token)
	{
		var tag = markdownTags[currentToken.Type];
		var start = prevToken.End;
		var end = currentToken.End + tag.WithEnd.Length;
		var nextToken = currentToken.nextToken;

		currentToken = new MdToken(currentToken.SourceText, start, end, TokenType.PlainText);
		currentToken.nextToken = nextToken;
		prevToken.nextToken = currentToken;
	}

	private void InsertTokenInText(MdToken splitter, ref MdToken currentToken, MdToken? prevToken)
	{
		if (currentToken.nestingTokens is not null) throw new ArgumentException();
		if (currentToken.Type is not TokenType.PlainText) throw new ArgumentException();

		var start = currentToken.Start;
		var end = currentToken.End;
		var left = new MdToken(currentToken.SourceText, start,
			splitter.Start - markdownTags[splitter.Type].WithStart.Length,
			currentToken.Type);
		var right = new MdToken(currentToken.SourceText, splitter.End + markdownTags[splitter.Type].WithEnd.Length,
			end,
			currentToken.Type);

		right.nextToken = currentToken.nextToken;
		splitter.nextToken = right;
		left.nextToken = splitter;

		currentToken = left;

		if (prevToken != null) prevToken.nextToken = left;
	}

	public void AddNestingToken(MdToken token, ref MdToken main)
	{
		if (token.Type == TokenType.Header) throw new ArgumentException();
		if (token.Type == TokenType.Bold && main.Type == TokenType.Italic) return;

		if (main.nestingTokens == null)
		{
			var nestingToken = new MdToken(main.SourceText, main.Start, main.End, TokenType.PlainText);
			InsertTokenInText(token, ref nestingToken, null);
			main.nestingTokens = nestingToken;
			return;
		}

		var place = main.nestingTokens;
		while (place.nextToken != null) place = place.nextToken;

		place.nextToken = token;
	}
}