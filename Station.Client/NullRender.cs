using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Station.Client {
	internal class NullRender : IRender {

		public static IRender Instance = new NullRender();

		Task IRender.Clear() {
			throw new NotImplementedException();
		}

		Task IRender.Fill() {
			throw new NotImplementedException();
		}

		Task IRender.DrawText( string text, int x, int y ) {
			throw new NotImplementedException();
		}
	}
}
