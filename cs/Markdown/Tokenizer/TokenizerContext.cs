namespace Markdown.Tokenizer;

public class TokenizerContext(string text)
{
	private int position = 0;
	public bool IsEnd => position >= text.Length;
	public char Current => text[position];
	public int Position => position;
	public int Length => text.Length;
	public void Advance() => position++;
	public char? Previous => position > 0 ? text[position - 1] : null;
	public char? Next => position < text.Length - 1 ? text[position + 1] : null;
	public char? NextNext => position < text.Length - 2 ? text[position + 2] : null;
}