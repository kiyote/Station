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
using Amazon.DynamoDBv2.DataModel;

namespace Station.Server.Repository.DynamoDb.Model {
#if DEBUG
	[DynamoDBTable( "Station-Development" )]
#else
	[DynamoDBTable( "Station" )]
#endif
	internal sealed class PlayerRecord {

		public const string ItemType = "Player-";

		public PlayerRecord() {
			PlayerId = "";
			UserId = "";
			Name = "";
		}

		[DynamoDBHashKey( "PK" )]
		private string PK {
			get {
				return UserRecord.GetKey( UserId );
			}
			set {
				UserId = UserRecord.GetIdFromKey( value );
			}
		}

		[DynamoDBRangeKey( "SK" )]
		private string SK {
			get {
				return GetKey( PlayerId );
			}
			set {
				PlayerId = GetIdFromKey( value );
			}
		}

		[DynamoDBIgnore]
		public string UserId { get; set; }

		[DynamoDBIgnore]
		public string PlayerId { get; set; }

		[DynamoDBProperty( "Name" )]
		public string Name { get; set; }

		public static string GetKey( string playerId ) {
			return $"{ItemType}{playerId}";
		}

		public static string GetIdFromKey( string key ) {
			return key.Substring( ItemType.Length );
		}
	}
}
