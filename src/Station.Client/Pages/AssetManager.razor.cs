using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Station.Client.Pages {
	public class AssetManagerBase : ComponentBase {

		[Parameter] public string TerrainSrc { get; set; }

		[Parameter] public RenderFragment ChildContent { get; set; }

		public ElementReference? Terrain { get; set; }

		[Inject] protected IJSRuntime JSRuntime { get; set; }

		private bool _firstRun;


		protected bool AssetsLoaded { get; set; }

		public AssetManagerBase() {
			_firstRun = true;
		}

		[JSInvokable]
		public Task TerrainImageLoaded( UIProgressEventArgs args ) {
			AssetsLoaded = true;
			return Task.CompletedTask;
		}

		protected override async Task OnAfterRenderAsync() {
			if (_firstRun) {
				_firstRun = false;
				await JSRuntime.InvokeAsync<object>( "asset.setImage", Terrain, TerrainSrc );
			}
		}
	}
}
