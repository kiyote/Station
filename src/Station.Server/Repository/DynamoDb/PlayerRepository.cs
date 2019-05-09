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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Station.Server.Model;
using Station.Server.Repository.DynamoDb.Model;
using Station.Shared.Model;

namespace Station.Server.Repository.DynamoDb {
	internal sealed class PlayerRepository : IPlayerRepository {

		private readonly IDynamoDBContext _context;

		public PlayerRepository(
			IDynamoDBContext context
		) {
			_context = context;
		}

		async Task<Player> IPlayerRepository.GetByUserId(
			Id<User> userId
		) {
			AsyncSearch<PlayerRecord> query = _context.QueryAsync<PlayerRecord>( UserRecord.GetKey( userId.Value ),
				QueryOperator.BeginsWith,
				new List<object>() { PlayerRecord.ItemType } );

			List<PlayerRecord> results = await query.GetRemainingAsync();
			PlayerRecord playerRecord = results.FirstOrDefault();

			if ( playerRecord == default) {
				return default;
			}

			return ToPlayer( playerRecord );
		}

		async Task<Player> IPlayerRepository.Create(
			Id<Player> playerId,
			Id<User> userId,
			string name
		) {
			var playerRecord = new PlayerRecord {
				PlayerId = playerId.Value,
				UserId = userId.Value,
				Name = name
			};
				
			await _context.SaveAsync( playerRecord );

			return ToPlayer( playerRecord );
		}

		async Task<Player> IPlayerRepository.GetByPlayerId( Id<Player> playerId ) {
			AsyncSearch<PlayerRecord> query = _context.QueryAsync<PlayerRecord>( PlayerRecord.GetKey( playerId.Value ),
				QueryOperator.BeginsWith,
				new List<object>() { UserRecord.ItemType },
				new DynamoDBOperationConfig() {
					IndexName = "GSI"
				} );

			List<PlayerRecord> results = await query.GetRemainingAsync();
			PlayerRecord playerRecord = results.FirstOrDefault();

			return await Get( playerRecord.UserId, playerRecord.PlayerId );
		}

		private async Task<Player> Get(string userId, string playerId) {
			PlayerRecord playerRecord = await _context.LoadAsync<PlayerRecord>( UserRecord.GetKey( userId ), PlayerRecord.GetKey( playerId ) );

			return ToPlayer( playerRecord );
		}

		private static Player ToPlayer(PlayerRecord playerRecord ) {
			return new Player(
				new Id<Player>( playerRecord.PlayerId ),
				new Id<User>( playerRecord.UserId ),
				playerRecord.Name );
		}
	}
}
