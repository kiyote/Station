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
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Station.Shared;

namespace Station.Server.Hubs {
	[Authorize]
	public sealed class SignalHub : Hub {

		public readonly static string Url = "/signalhub";
		private readonly ILogger<SignalHub> _logger;
		private string _username;

		public SignalHub(
			ILogger<SignalHub> logger
		) {
			_logger = logger;
		}

		public override async Task OnConnectedAsync() {
			SetContextInformation();

			await Clients.Others.SendAsync( "Send", $"{_username} connected" );
		}

		public override async Task OnDisconnectedAsync( Exception ex ) {
			await this.Clients.Others.SendAsync( "Send", $"{_username} left" );

			ClearContextInformation();
		}

		public Task GetTerrain( int chunkColumn, int chunkRow ) {
			var chunk = new TerrainChunk();
			chunk.ChunkColumn = chunkColumn;
			chunk.ChunkRow = chunkRow;
			return Clients.Caller.SendAsync( "TerrainChunk", chunk );
		}

		public Task Send( string message ) {
			return Clients.All.SendAsync( "Send", $"{_username}: {message}" );
		}

		public Task SendToOthers( string message ) {
			return Clients.Others.SendAsync( "Send", $"{_username}: {message}" );
		}

		public Task SendToConnection( string connectionId, string message ) {
			return Clients.Client( connectionId )
				.SendAsync( "Send", $"Private message from {_username}: {message}" );
		}

		public Task SendToGroup( string groupName, string message ) {
			return Clients.Group( groupName )
				.SendAsync( "Send", $"{_username}@{groupName}: {message}" );
		}

		public Task SendToOthersInGroup( string groupName, string message ) {
			return Clients.OthersInGroup( groupName )
				.SendAsync( "Send", $"{_username}@{groupName}: {message}" );
		}

		public async Task JoinGroup( string groupName ) {
			await Groups.AddToGroupAsync( this.Context.ConnectionId, groupName );

			await Clients.Group( groupName ).SendAsync( "Send", $"{_username} joined {groupName}" );
		}

		public async Task LeaveGroup( string groupName ) {
			await Clients.Group( groupName ).SendAsync( "Send", $"{_username} left {groupName}" );

			await Groups.RemoveFromGroupAsync( this.Context.ConnectionId, groupName );
		}

		public Task Echo( string message ) {
			return Clients.Caller.SendAsync( "Send", $"{_username}: {message}" );
		}

		private void SetContextInformation() {
			_username = default;

			ClaimsIdentity principal = Context.User?.Identities?.First();
			_username = principal.GetUsername();
		}

		private void ClearContextInformation() {
			_username = default;
		}
	}
}
