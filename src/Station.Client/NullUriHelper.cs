using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Station.Client {
	public class NullUriHelper : IUriHelper {

		public static IUriHelper Instance = new NullUriHelper();

		event EventHandler<LocationChangedEventArgs> IUriHelper.OnLocationChanged {
			add {
				throw new NotImplementedException();
			}

			remove {
				throw new NotImplementedException();
			}
		}

		string IUriHelper.GetAbsoluteUri() {
			throw new NotImplementedException();
		}

		string IUriHelper.GetBaseUri() {
			throw new NotImplementedException();
		}

		void IUriHelper.NavigateTo( string uri ) {
			throw new NotImplementedException();
		}

		void IUriHelper.NavigateTo( string uri, bool forceLoad ) {
			throw new NotImplementedException();
		}

		Uri IUriHelper.ToAbsoluteUri( string href ) {
			throw new NotImplementedException();
		}

		string IUriHelper.ToBaseRelativePath( string baseUri, string locationAbsolute ) {
			throw new NotImplementedException();
		}
	}
}
