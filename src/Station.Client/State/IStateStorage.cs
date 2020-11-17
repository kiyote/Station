using System;
using System.Threading.Tasks;

namespace Station.Client.State {
	public interface IStateStorage {
		Task<DateTime> GetAsDateTime( string name );

		Task<int> GetAsInt( string name );

		Task<string> GetAsString( string name );

		Task<T> Get<T>( string name );

		Task Set( string name, DateTime value );

		Task Set( string name, int value );

		Task Set( string name, string value );

		Task Set<T>( string name, T value );
	}
}
