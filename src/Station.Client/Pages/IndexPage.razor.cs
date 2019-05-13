/*
 * Copyright 2018-2019 Todd Lang
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Station.Client.Services;
using Station.Client.State;
using Station.Shared.Model;

namespace Station.Client.Pages {
	public class IndexPageBase: ComponentBase {

		public const string Url = "/";

		[Inject] protected IUriHelper UriHelper { get; set; }

		[Inject] protected IUserApiService UserService { get; set; }

		[Inject] protected IAppState State { get; set; }

		protected override async Task OnInitAsync() {
			if (State.Authentication.IsAuthenticated) {
				ClientPlayer player = await UserService.GetPlayer();
				if (player != default) {
					await State.Update( State.Game, player );
				}
			}
		}

		public void Play() {
			UriHelper.NavigateTo( PlayPageBase.Url );
		}
	}
}
