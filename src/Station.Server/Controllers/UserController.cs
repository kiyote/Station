﻿/*
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Station.Server.Managers;
using Station.Server.Model;
using Station.Shared.Message;
using Station.Shared.Model;

namespace Station.Server.Controllers {
	[ResponseCache( Location = ResponseCacheLocation.None, NoStore = true )]
	[Authorize]
	[Route( "api/user" )]
	public sealed class UserController : Controller {

		private readonly UserManager _userManager;
		private readonly IContextInformation _contextInformation;

		public UserController(
			UserManager userManager,
			IContextInformation contextInformation
		) {
			_userManager = userManager;
			_contextInformation = contextInformation;
		}

		[HttpGet( "login" )]
		public async Task<ActionResult> RecordLogin() {
			var user = await _userManager.RecordLogin( _contextInformation.Username );

			return Ok( user );
		}

		[HttpGet]
		public async Task<ActionResult<ClientUser>> GetUserInformation() {
			var result = await _userManager.GetUser( _contextInformation.UserId );

			if( result != default ) {
				return Ok( result );

			} else {
				return NotFound();
			}
		}

		[HttpGet("{userId}")]
		public async Task<ActionResult<ClientUser>> GetUserInformation( string userId ) {
			var result = await _userManager.GetUser( new Id<User>( userId ));

			if( result != default ) {
				return Ok( result );

			} else {
				return NotFound();
			}
		}

		[HttpGet("player")]
		public async Task<ActionResult<ClientPlayer>> GetUserPlayer() {
			ClientPlayer result = await _userManager.GetPlayerByUserId( _contextInformation.UserId );

			if (result == default) {
				return NotFound();
			}

			return Ok( result );
		}

		[HttpPost( "player" )]
		public async Task<ActionResult<ClientPlayer>> CreatePlayer( [FromBody] CreatePlayerRequest request ) {
			ClientPlayer result = await _userManager.CreatePlayer( _contextInformation.UserId, request.Name );

			return Ok( result );
		}

		[HttpPost( "avatar" )]
		public async Task<ActionResult<SetAvatarResponse>> SetAvatar( [FromBody] SetAvatarRequest request ) {

			var url = await _userManager.SetAvatar( _contextInformation.UserId, request.ContentType, request.Content );
			return Ok( new SetAvatarResponse( url ) );
		}
	}
}