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
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Station.Server.Managers;
using Station.Server.Model;
using Station.Shared.Model;

namespace Station.Server.Controllers {
	[Route( "api/player" )]
	public class PlayerController : Controller {

		private readonly UserManager _userManager;

		public PlayerController(
			UserManager userManager
		) {
			_userManager = userManager;
		}

		[HttpGet( "{id}" )]
		public async Task<ActionResult<ClientPlayer>> GetPlayer( string playerId ) {
			ClientPlayer player = await _userManager.GetPlayer( new Id<Player>( playerId ));
			return Ok( player );
		}
	}
}
