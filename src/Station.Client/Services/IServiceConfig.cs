namespace Station.Client.Services {
	public interface IServiceConfig {

		string Host { get; }

		string CognitoClientId { get; }

		string RedirectUrl { get; }

		string TokenUrl { get; }
	}
}
