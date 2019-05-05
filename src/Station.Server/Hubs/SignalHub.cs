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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Station.Server.Hubs {
	[Authorize( JwtBearerDefaults.AuthenticationScheme )]
	public sealed class SignalHub : Hub {

		public readonly static string Url = "/signalhub";

		public override async Task OnConnectedAsync() {
			object username = Context.GetHttpContext().Items["User"];
			await Clients.Others.SendAsync( "Send", $"{username} joined" );
		}

		public override async Task OnDisconnectedAsync( Exception ex ) {
			object username = Context.GetHttpContext().Items["User"];
			await Clients.Others.SendAsync( "Send", $"{username} left" );
		}

		public Task Send( string message ) {
			object username = Context.GetHttpContext().Items["User"];
			return Clients.All.SendAsync( "Send", $"{username}: {message}" );
		}

		public Task SendToOthers( string message ) {
			object username = Context.GetHttpContext().Items["User"];
			return Clients.Others.SendAsync( "Send", $"{username}: {message}" );
		}

		public Task SendToConnection( string connectionId, string message ) {
			return Clients.Client( connectionId )
				.SendAsync( "Send", $"Private message from {Context.ConnectionId}: {message}" );
		}

		public Task SendToGroup( string groupName, string message ) {
			return Clients.Group( groupName )
				.SendAsync( "Send", $"{Context.ConnectionId}@{groupName}: {message}" );
		}

		public Task SendToOthersInGroup( string groupName, string message ) {
			return Clients.OthersInGroup( groupName )
				.SendAsync( "Send", $"{Context.ConnectionId}@{groupName}: {message}" );
		}

		public async Task JoinGroup( string groupName ) {
			await Groups.AddToGroupAsync( Context.ConnectionId, groupName );

			await Clients.Group( groupName ).SendAsync( "Send", $"{this.Context.ConnectionId} joined {groupName}" );
		}

		public async Task LeaveGroup( string groupName ) {
			await Clients.Group( groupName ).SendAsync( "Send", $"{this.Context.ConnectionId} left {groupName}" );

			await Groups.RemoveFromGroupAsync( Context.ConnectionId, groupName );
		}

		public Task Echo( string message ) {
			return Clients.Caller.SendAsync( "Send", $"{Context.ConnectionId}: {message}" );
		}
	}
}
