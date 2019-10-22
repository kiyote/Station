using System;
using Microsoft.AspNetCore.Components;

namespace Station.Client {
	public class NullUriHelper : NavigationManager {

		public static NavigationManager Instance = new NullUriHelper();

		protected override void NavigateToCore( string uri, bool forceLoad ) {
			throw new NotImplementedException();
		}
	}
}
