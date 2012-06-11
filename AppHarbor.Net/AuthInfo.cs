namespace AppHarbor
{
	public class AuthInfo
	{
		public AuthInfo(string accessToken, string tokenType = null)
		{
			AccessToken = accessToken;
			TokenType = tokenType;
		}

		public string AccessToken
		{
			get;
			private set;
		}

		/// <summary>
		/// This is optional, currently this is not used it is added for future support
		/// </summary>
		public string TokenType
		{
			get;
			private set;
		}
	}
}
