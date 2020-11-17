using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Station.Client.Interop;

namespace Station.Client.Services {
	public interface IMapRenderer {
		Task RenderTerrain(
			IRender terrainCanvas,
			ElementReference terrainImage );
	}
}
