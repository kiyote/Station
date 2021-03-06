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
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

namespace Station.Client {
	public static class ExtensionMethods {
		public static string GetParameter( this NavigationManager uriHelper, string name ) {
			string queryString = uriHelper.Uri.Substring( uriHelper.Uri.IndexOf( '?' ) );
			string value = QueryHelpers
				.ParseQuery( queryString )
				.TryGetValue( name, out StringValues values ) ? values.First() : string.Empty;

			return value;
		}
	}
}
