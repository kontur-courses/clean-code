namespace NewMarkdown.Lexer;


public class TextReader
{
	private string text;
	private int position;

	public TextReader(string text)
	{
		this.text = text;
	}

	public bool IsEnd => position >= text.Length;
	public void MoveNext(int step = 1) => position += step;
	public char Current => text[position];
}