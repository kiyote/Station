using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Station.Client.Pages {
	public partial class AssetManager {

		[Parameter] public string TerrainSrc { get; set; }

		[Parameter] public RenderFragment ChildContent { get; set; }

		public ElementReference? Terrain { get; set; }

		[Inject] protected IJSRuntime JSRuntime { get; set; }

		protected bool AssetsLoaded { get; set; }

		[JSInvokable]
		public Task TerrainImageLoaded( ProgressEventArgs args ) {
			AssetsLoaded = true;
			return Task.CompletedTask;
		}

		protected override async Task OnAfterRenderAsync( bool firstRender ) {
			if (firstRender) {
				await JSRuntime.InvokeAsync<object>( "asset.setImage", Terrain, TerrainSrc );
			}
		}
	}
}
