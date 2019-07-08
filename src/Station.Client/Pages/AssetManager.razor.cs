using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Station.Client.Pages {
	public class AssetManagerBase : ComponentBase {

		public ElementRef? Terrain { get; set; }

		private bool _firstRun;

		[Inject] protected IJSRuntime JSRuntime { get; set; }

		[Parameter] protected string TerrainSrc { get; set; }

		[Parameter] protected RenderFragment ChildContent { get; set; }

		protected bool AssetsLoaded { get; set; }

		public AssetManagerBase() {
			_firstRun = true;
		}

		[JSInvokable]
		public Task TerrainImageLoaded( UIProgressEventArgs args ) {
			Console.WriteLine( $"Image loaded." );
			AssetsLoaded = true;
			return Task.CompletedTask;
		}

		protected override async Task OnAfterRenderAsync() {
			if (_firstRun) {
				_firstRun = false;
				Console.WriteLine( "OnAfterRenderAsync" );
				await JSRuntime.InvokeAsync<object>( "asset.setImage", Terrain, TerrainSrc );
			}
		}
	}
}
