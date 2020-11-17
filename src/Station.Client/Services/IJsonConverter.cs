namespace Station.Client.Services {
	public interface IJsonConverter {
		T Deserialize<T>( string value );

		string Serialize( object value );
	}
}
