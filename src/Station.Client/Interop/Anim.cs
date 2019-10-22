using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Station.Client.Services;

namespace Station.Client.Interop {
	public class Anim : IAnim {

		private readonly IJSRuntimeProvider _jsRuntimeProvider;
		private IAnimCallback _callback;
		private int _context;
		private float _lastTimestamp;

		public Anim( IJSRuntimeProvider jsRuntimeProvider ) {
			_jsRuntimeProvider = jsRuntimeProvider;
			_context = -1;
			_lastTimestamp = 0.0f;
		}

		public async Task Start( IAnimCallback callback ) {
			_callback = callback;
			IJSRuntime jsRuntime = _jsRuntimeProvider.Get();
			_context = await jsRuntime.InvokeAsync<int>( "anim.start", DotNetObjectReference.Create( this ) );
		}

		public Task Stop() {
			IJSRuntime jsRuntime = _jsRuntimeProvider.Get();
			if( _context != -1 ) {
				( (IJSInProcessRuntime)jsRuntime ).Invoke<object>( "anim.stop", _context );
			}
			return Task.CompletedTask;
		}

		[JSInvokable]
		public Task SetCallbackContext( int context ) {
			_context = context;
			return Task.CompletedTask;
		}

		[JSInvokable]
		public async Task AnimCallback( float timestamp ) {
			float interval = timestamp - _lastTimestamp;
			_lastTimestamp = timestamp;
			await _callback.RenderFrame( interval );
		}
	}
}
