using System;
using Microsoft.JSInterop;

namespace Station.Client.Services {
	internal sealed class JSRuntimeProvider : IJSRuntimeProvider {

		private readonly IServiceProvider _serviceProvider;

		public JSRuntimeProvider(
			IServiceProvider serviceProvider
		) {
			_serviceProvider = serviceProvider;
		}

		public bool TryGet( out IJSRuntime jsRuntime ) {
			jsRuntime = (IJSRuntime)_serviceProvider.GetService( typeof( IJSRuntime ) );
			return ( jsRuntime != default );
		}

		public IJSRuntime Get() {
			IJSRuntime jsRuntime;
			if( !TryGet( out jsRuntime ) ) {
				throw new InvalidOperationException();
			}

			return jsRuntime;
		}
	}
}
