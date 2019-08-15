using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Station.Client {
	internal class NullJSRuntime : IJSRuntime {

		public static IJSRuntime Instance = new NullJSRuntime();

		Task<TValue> IJSRuntime.InvokeAsync<TValue>( string identifier, params object[] args ) {
			throw new NotImplementedException();
		}

		Task<TValue> IJSRuntime.InvokeAsync<TValue>( string identifier, IEnumerable<object> args, CancellationToken cancellationToken ) {
			throw new NotImplementedException();
		}
	}
}
