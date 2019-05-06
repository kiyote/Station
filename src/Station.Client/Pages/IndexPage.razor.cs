using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Station.Client.Pages {
	public class IndexPageBase: ComponentBase {

		public const string Url = "/";

		[Inject] protected IUriHelper UriHelper { get; set; }

		[Inject] protected IConfig Config { get; set; }

		public void GoToLogin() {
			UriHelper.NavigateTo( $"{Config.LogInUrl}&redirect_uri={Config.RedirectUrl}" );
		}

	}
}
