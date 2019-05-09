using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Station.Shared.Message {
	public sealed class CreatePlayerRequest {

		[JsonConstructor]
		public CreatePlayerRequest(
			string name
		) {
			Name = name;
		}

		public string Name { get; }
	}
}
