using System.Runtime.InteropServices.ComTypes;
using Markdown.Tags;
using Markdown.TokenParsers.MarkdownParsers;
using Markdown.Tokens;

namespace Markdown.TokenParsers;

public class MarkdownTokenParser : ITokenParser
{
	private readonly EscapeParser escapeParser;
	private readonly Dictionary<TokenType, MarkdownTag> markdownTags;

	private readonly Dictionary<TokenType, IMarkdownTagParser> parsers;

	public MarkdownTokenParser()
	{
		
		parsers = new Dictionary<TokenType, IMarkdownTagParser>
		{
			{ TokenType.Italic, new DoubleTagParser(new MarkdownTag("_", TokenType.Italic), "__") },
			{ TokenType.Bold, new DoubleTagParser(new MarkdownTag("__", TokenType.Bold)) },
			{ TokenType.Header, new HeaderParser(new MarkdownTag("# ", $"{Environment.NewLine}{Environment.NewLine}", TokenType.Header)) },
			{ TokenType.Link, new DoubleTagParser(new MarkdownTag("(", ")", TokenType.Link))}
		};

		markdownTags = parsers.ToDictionary(pair => pair.Key, pair => pair.Value.Tag);

		escapeParser = new EscapeParser(new MarkdownTag("/", null, TokenType.Escape), markdownTags.Values);

		markdownTags.Add(TokenType.Escape, escapeParser.Tag);
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
			if (window == ParagraphSplitter) result.Add(i + ParagraphSplitter.Length);
		}

		return result;
	}

	private IToken ParseParagraphs(string text, IReadOnlyList<int> paragraphsStartPositions)
	{
		IToken root = new MdToken(text, 0, 0, TokenType.PlainText);
		var currentToken = root;

		for (var i = 1; i < paragraphsStartPositions.Count; i++)
		{
			var paragraphStart = paragraphsStartPositions[i - 1];
			var paragraphEnd = paragraphsStartPositions[i] - ParagraphSplitter.Length;
			currentToken = ParseParagraph(currentToken, text, paragraphStart, paragraphEnd);
		}

		ParseParagraph(currentToken, text, paragraphsStartPositions.Last(), text.Length);

		return root.nextToken;
	}

	private IToken ParseParagraph(IToken currentToken, string text, int paragraphStart, int paragraphEnd)
	{
		var paragraphTokens = ParseParagraphTokens(text, paragraphStart, paragraphEnd);

		currentToken.nextToken = MergeTokens(paragraphTokens, text, paragraphStart, paragraphEnd);
		currentToken = currentToken.nextToken;

		if (paragraphEnd == text.Length) return currentToken;

		currentToken.nextToken =
			new MdToken(ParagraphSplitter, 0, ParagraphSplitter.Length, TokenType.PlainText);

		return currentToken.nextToken;
	}


	private Dictionary<TokenType, List<MdToken>> ParseParagraphTokens(string text, int paragraphStart, int paragraphEnd)
	{
		var escapeTokens = escapeParser.ParseParagraph(text, paragraphStart, paragraphEnd);
		var paragraphTokens = new Dictionary<TokenType, List<MdToken>> { { TokenType.Escape, escapeTokens } };

		foreach (var (tokenType, parser) in parsers)
		{
			var tokens = parser.ParseParagraph(
				text,
				paragraphStart,
				paragraphEnd,
				escapeTokens);

			if (tokens.Count == 0) continue;

			paragraphTokens.Add(tokenType, tokens);
		}

		return paragraphTokens;
	}

	private IToken MergeTokens(Dictionary<TokenType, List<MdToken>> tokens, string text, int paragraphStartPosition,
		int paragraphEndPosition)
	{
		var result = new MdToken(text, paragraphStartPosition, paragraphEndPosition, TokenType.PlainText);

		if (tokens.ContainsKey(TokenType.Header))
		{
			result = tokens[TokenType.Header].First();
			tokens.Remove(TokenType.Header);

			if (tokens.Count <= 0) return result;

			result.nestingTokens = MergeTokens(
				tokens,
				text,
				paragraphStartPosition + markdownTags[TokenType.Header].Open.Length,
				paragraphEndPosition);

			return result;
		}

		var allTokens = new List<MdToken>();

		foreach (var (tokenType, parsedTokens) in tokens) allTokens.AddRange(parsedTokens);

		var orderedTokens = allTokens.OrderBy(t => t.Start).ToList();
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
			(prevToken, currentToken) = FindPlace(prevToken, currentToken, token);
			
			if (currentToken is null)
			{
				if(prevToken is not null)
					prevToken.nextToken = token;
				currentToken = token;
				continue;
			}

			if (IntersectWithOtherToken(currentToken, token))
			{
				ResolveIntersection(prevToken, currentToken);
				continue;
			}

			currentToken = currentToken.Type == TokenType.PlainText 
				? InsertTokenInText(token, currentToken, prevToken) 
				: AddNestingToken(token, currentToken);
			
			if (prevToken is null) result = currentToken;
			prevToken = currentToken;
		}
	}

	private (MdToken? prevToken, MdToken? currentToken) FindPlace(MdToken? prevToken, MdToken? currentToken, MdToken token)
	{
		while (currentToken is not null)
		{
			if (currentToken.Start <= token.Start && currentToken.End > token.Start) return (prevToken, currentToken);
			prevToken = currentToken;
			currentToken = currentToken.nextToken as MdToken;
		}

		return (prevToken, currentToken);
	}

	private bool IntersectWithOtherToken(MdToken currentToken, MdToken token)
	{
		return currentToken.End < token.End;
	}

	private void ResolveIntersection(MdToken? prevToken, MdToken currentToken)
	{
		var tag = markdownTags[currentToken.Type];
		var start = prevToken?.End ?? 0;
		var end = currentToken.End + tag.Close?.Length ?? 0;
		var nextToken = currentToken.nextToken;

		currentToken = new MdToken(currentToken.SourceText, start, end, TokenType.PlainText)
		{
			nextToken = nextToken
		};

		if(prevToken is not null) prevToken.nextToken = currentToken;
	}

	private MdToken InsertTokenInText(MdToken splitter, MdToken currentToken, MdToken? prevToken)
	{
		if (currentToken.Type is not TokenType.PlainText) throw new ArgumentException();
		return splitter.Type is not TokenType.Escape 
			? InsertToken(splitter, currentToken, prevToken) 
			: InsertEscapeToken(splitter, currentToken, prevToken);
	}

	private MdToken InsertEscapeToken(MdToken splitter, MdToken currentToken, MdToken? prevToken)
	{
		if (splitter.Start == 0)
		{
			splitter.nextToken = new MdToken(currentToken.SourceText, currentToken.Start + splitter.End,
				currentToken.End, currentToken.Type);

			return splitter;
		}

		if (splitter.End != currentToken.SourceText.Length - 1) return InsertToken(splitter, currentToken, prevToken);

		currentToken = new MdToken(currentToken.SourceText, currentToken.Start, splitter.Start,
			currentToken.Type)
		{
			nextToken = splitter
		};

		if (prevToken != null) prevToken.nextToken = currentToken;

		return currentToken;

	}

	private MdToken InsertToken(MdToken splitter, MdToken currentToken, MdToken? prevToken)
	{
		var start = currentToken.Start;
		var end = currentToken.End;

		var left = new MdToken(
			currentToken.SourceText, 
			start,
			splitter.Start - markdownTags[splitter.Type].Open.Length,
			currentToken.Type);

		var right = new MdToken(
			currentToken.SourceText, 
			splitter.End + markdownTags[splitter.Type].Close?.Length ?? 0,
			end,
			currentToken.Type)
		{
			nextToken = currentToken.nextToken
		};

		splitter.nextToken = right;
		left.nextToken = splitter;

		if (prevToken != null) prevToken.nextToken = left;

		return left;
	}

	private MdToken AddNestingToken(MdToken token, MdToken main)
	{
		switch (main.Type)
		{
			case TokenType.Link:
				return main;
			case TokenType.Escape:
				return main;
		}

		switch (token.Type)
		{
			case TokenType.Header:
				throw new ArgumentException();
			case TokenType.Bold when main.Type == TokenType.Italic:
				return main;
		}

		if (main.nestingTokens == null)
		{
			var nestingToken = new MdToken(main.SourceText, main.Start, main.End, TokenType.PlainText);
			nestingToken = InsertTokenInText(token, nestingToken, null);
			main.nestingTokens = nestingToken;
			return main;
		}

		var (prevToken, place) = FindPlace(
			main.nextToken as MdToken,
			main.nestingTokens.nextToken as MdToken,
			token);

		return place.Type is TokenType.PlainText 
			? InsertTokenInText(token, place, prevToken) 
			: InsertToken(token, place, prevToken);
	}
}