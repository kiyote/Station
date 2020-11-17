using Microsoft.JSInterop;

namespace Station.Client.Services {
	public interface IJSRuntimeProvider {
		bool TryGet( out IJSRuntime jsRuntime );

		IJSRuntime Get();
	}
}
