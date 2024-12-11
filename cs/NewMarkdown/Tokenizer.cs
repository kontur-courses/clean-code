using System.Text;
using TextReader = NewMarkdown.Lexer.TextReader;

namespace NewMarkdown;

public class Tokenizer
{
	private readonly List<Node> result = new List<Node>();
	public Tokenizer()
	{ }

	public List<Node> Tokenize(string text)
	{
		var reader = new TextReader(text);
		var buffer = new StringBuilder();
		while (!reader.IsEnd)
		{
			TryParseHeader(reader);
		}
		result.Add(new Node(NodeType.Text, buffer.ToString()));
		return result;
	}

	// Если попадается слэш, то проверяем экранирует ли он

	private void TryParseHeader(TextReader reader)
	{
		if(reader.Current == "#" && reader.)
	}
}