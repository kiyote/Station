using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Station.Client.Services;
using Station.Client.State;
using Station.Shared.Model;

namespace Station.Client.Pages {
	public class CreatePlayerBase: ComponentBase {

		[Inject] protected IUserApiService UserService { get; set; }

		[Inject] protected IAppState State { get; set; }

		[Inject] protected IUriHelper UriHelper { get; set; }

		protected string PlayerName { get; set; }

		protected async Task CreatePlayerClicked() {
			if (!string.IsNullOrWhiteSpace(PlayerName)) {
				ClientPlayer player = await UserService.CreatePlayer( PlayerName );
				if (player != default) {
					await State.Update( State.Game, player );
					UriHelper.NavigateTo( PlayPageBase.Url );
				}
			}
		}
	}
}
