using Markdown.Tokenizer.Tags;

namespace Markdown.Tokenizer;

public class TagProcessor : ITagProcessor
{
	public void Process(List<Token> tags, Stack<Token> tagStack)
	{
		ProceedEscapedTags(tags);
		ProceedInWordsTags(tags);
		ProceedPairTags(tagStack);
	}

	private void ProceedInWordsTags(List<Token> tags)
	{
		for (var i = 0; i < tags.Count; i++)
		{
			var current = tags[i];
			if (current.TagStatus == TagStatus.InWord)
			{
				if (i - 2 >= 0)
				{
					if (tags[i - 1].TokenType == TokenType.String
						&& tags[i - 2].TagStatus == TagStatus.Open)
					{
						current.TagStatus = TagStatus.Closed;
					}
				}

				if (i + 2 >= tags.Count) continue;
				if (tags[i + 1].TokenType != TokenType.String) continue;
				if (tags[i + 2].TagStatus == TagStatus.Closed)
				{
					current.TagStatus = TagStatus.Open;
				}
				else if (tags[i + 2].TagStatus == TagStatus.InWord)
				{
					current.TagStatus = TagStatus.Open;
					tags[i + 2].TagStatus = TagStatus.Closed;
				}
			}
		}
	}

	private void ProceedEscapedTags(List<Token> tags)
	{
		for (var i = 0; i < tags.Count - 1; i++)
		{
			var current = tags[i];
			var next = tags[i + 1];
			if (current.TokenType is TokenType.Slash && current.TagStatus != TagStatus.Broken)
			{
				if (next is { TokenType: TokenType.Slash })
				{
					current.TagStatus = TagStatus.Escaped;
					next.TagStatus = TagStatus.Broken;
				}
				else if (next is { TagStatus: TagStatus.Open or TagStatus.Closed or TagStatus.Single })
				{
					next.TagStatus = TagStatus.Broken;
					current.TagStatus = TagStatus.Escaped;
				}
			}
		}
	}

	private void ProceedPairTags(Stack<Token> tagStack)
	{
		var tempStack = new Stack<Token>();

		while (tagStack.Count > 0)
		{
			var current = tagStack.Pop();

			if (current.TagStatus != TagStatus.Broken && current.TagStatus != TagStatus.Single)
			{
				if (tempStack.Count > 0)
				{
					var previousTag = tempStack.Peek();

					if (previousTag.TokenType == current.TokenType)
					{
						if (previousTag.TagStatus == TagStatus.Closed && current.TagStatus == TagStatus.Open)
						{
							tempStack.Pop();
						}
						else
						{
							tempStack.Push(current);
						}
					}
					else
					{
						if (current.TokenType == TokenType.Bold && previousTag.TokenType == TokenType.Italic)
						{
							current.TagStatus = TagStatus.Broken;
						}
						else
						{
							tempStack.Push(current);
						}
					}
				}
				else
				{
					tempStack.Push(current);
				}
			}
		}

		while (tempStack.Count > 0)
			tempStack.Pop().TagStatus = TagStatus.Broken;
	}
}