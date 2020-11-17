using System.Text.Json;

namespace Station.Client.Services {
	internal sealed class JsonConverter : IJsonConverter {

		private readonly JsonSerializerOptions _options;

		public JsonConverter() {
			_options = new JsonSerializerOptions {
				PropertyNameCaseInsensitive = true
			};
		}

		T IJsonConverter.Deserialize<T>( string value ) {
			return JsonSerializer.Deserialize<T>( value, _options );
		}

		string IJsonConverter.Serialize( object value ) {
			if( value is null ) {
				return "{}";
			}
			return JsonSerializer.Serialize( value, _options );
		}
	}
}
