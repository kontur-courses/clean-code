namespace NewMarkdown;

[TestFixture]
public class TokenizerTest
{
	[Test]
	public void Test()
	{
		var tokenizer = new Tokenizer();
		var result = tokenizer.Tokenize("This is a sample text.");

		foreach (var node in result)
		{
			Console.WriteLine($"{node.Type}: {node.Text}");
		}
	}
}