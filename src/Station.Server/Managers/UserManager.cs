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
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using Station.Server.Model;
using Station.Server.Repository;
using Station.Server.Service;
using Station.Shared.Model;
using Image = Station.Server.Model.Image;
using LaborImage = SixLabors.ImageSharp.Image;

namespace Station.Server.Managers {
	public sealed class UserManager {

		private readonly IIdentificationService _identificationService;
		private readonly IImageService _imageService;
		private readonly IPlayerRepository _playerRepository;

		public UserManager(
			IIdentificationService identificationService,
			IImageService imageService,
			IPlayerRepository playerRepository
		) {
			_identificationService = identificationService;
			_imageService = imageService;
			_playerRepository = playerRepository;
		}

		public async Task<ClientUser> RecordLogin( string username ) {
			User user = await _identificationService.RecordLogin( username );

			return ToClientUser( user, await GetAvatarUrl( user ) );
		}

		public async Task<ClientUser> GetUser( Id<User> userId ) {
			User user = await _identificationService.GetUser( userId );

			return ToClientUser( user, await GetAvatarUrl( user ) );
		}

		public async Task<string> SetAvatar( Id<User> userId, string contentType, string content ) {
			using( var image = LaborImage.Load( Convert.FromBase64String( content ) ) ) {
				if( ( image.Width != 256 ) || ( image.Height != 256 ) ) {
					var options = new ResizeOptions() {
						Mode = ResizeMode.Max,
						Size = new Size( 256, 256 )
					};
					content = image.Clone( x => x.Resize( options ) ).ToBase64String( PngFormat.Instance ).Split( ',' )[ 1 ];
					contentType = "image/png";
				}
			}

			// Not a mistake, we're reusing the userId as the imageId for their avatar
			var id = new Id<Image>( userId.Value );
			Image avatar = await _imageService.Update( id, contentType, content );
			if (avatar == default) {
				throw new InvalidOperationException();
			}
			await _identificationService.SetAvatarStatus( userId, true );

			return avatar.Url;
		}

		public async Task<ClientPlayer> GetPlayer( Id<Player> playerId ) {
			Player player = await _playerRepository.GetByPlayerId( playerId );
			return ToClientPlayer( player );
		}

		public async Task<ClientPlayer> GetPlayerByUserId( Id<User> userId ) {
			Player player = await _playerRepository.GetByUserId( userId );
			return ToClientPlayer( player );
		}

		public async Task<ClientPlayer> CreatePlayer( Id<User> userId, string name ) {
			Id<Player> playerId = new Id<Player>();
			Player player = await _playerRepository.Create( playerId, userId, name );

			return ToClientPlayer( player );
		}

		private static ClientPlayer ToClientPlayer( Player player ) {
			if (player == default) {
				return default;
			}

			return new ClientPlayer(
				new Id<ClientPlayer>( player.Id.Value ),
				player.Name );
		}

		private static ClientUser ToClientUser( User user, string avatarUrl ) {
			return new ClientUser(
				new Id<ClientUser>(user.Id.Value),
				user.Username,
				avatarUrl,
				user.LastLogin,
				user.PreviousLogin,
				user.Name
			);
		}

		private async Task<string> GetAvatarUrl( User user ) {
			if( user.HasAvatar ) {
				// Not a mistake, we're reusing the userId as the imageId for their avatar
				return ( await _imageService.Get( new Id<Image>( user.Id.Value ) ) )?.Url;
			}

			return default;
		}
	}
}
