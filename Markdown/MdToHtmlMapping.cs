namespace Markdown;

public class MdToHtmlMapping
{
	private static readonly Dictionary<MdKeySymbol, string> mapping = new()
	{
		[new(MdSymbolType.Underscore, MdSymbolRelativePosition.Open, 0)] = "<em>",
		[new(MdSymbolType.Underscore, MdSymbolRelativePosition.Close, 0)] = "</em>",
		[new(MdSymbolType.DoubleUnderscore, MdSymbolRelativePosition.Open, 0)] = "<strong>",
		[new(MdSymbolType.DoubleUnderscore, MdSymbolRelativePosition.Close, 0)] = "</strong>",
		[new(MdSymbolType.NumberSign, MdSymbolRelativePosition.Open, 0)] = "<h1>",
		[new(MdSymbolType.NumberSign, MdSymbolRelativePosition.Close, 0)] = "</h1>"
	};

	public static IReadOnlyDictionary<MdKeySymbol, string> Mapping => mapping;
}
