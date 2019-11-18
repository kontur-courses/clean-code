namespace Markdown.Features
{
	internal class TokenStack
	{
		public TokenContainer MainToken { get; }
		public TokenContainer LastToken { get; private set; }

		public TokenStack(TokenInfo tokenInfo)
		{
			MainToken = new TokenContainer(tokenInfo);
			LastToken = MainToken;
		}

		public class TokenContainer
		{
			public TokenContainer Parent { get; }
			public TokenInfo TokenInfo { get; }

			public TokenContainer(TokenInfo tokenInfo, TokenContainer parent=null)
			{
				TokenInfo = tokenInfo;
				Parent = parent;
			}
		}

		public void Push(TokenInfo tokenInfo) => LastToken = new TokenContainer(tokenInfo, LastToken);

		public void Remove(TokenContainer currentToken) => LastToken = currentToken.Parent;
	}
}