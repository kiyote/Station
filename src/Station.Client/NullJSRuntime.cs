using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Station.Client {
	internal class NullJSRuntime : IJSRuntime {

		public static IJSRuntime Instance = new NullJSRuntime();

		ValueTask<TValue> IJSRuntime.InvokeAsync<TValue>( string identifier, object[] args ) {
			throw new NotImplementedException();
		}

		ValueTask<TValue> IJSRuntime.InvokeAsync<TValue>( string identifier, CancellationToken cancellationToken, object[] args ) {
			throw new NotImplementedException();
		}
	}
}
