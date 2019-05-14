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
using Microsoft.AspNetCore.Components;
using Station.Client.Services;
using Station.Client.State;
using Station.Shared.Message;

namespace Station.Client.Pages {
	public class ChatBase: ComponentBase, IDisposable {
		[Inject] protected ISignalService Signal { get; set; }

		[Inject] protected IAppState State { get; set; }

		public List<string> ChatHistory { get; set; }

		public string ComposedText { get; set; }

		private IDisposable _chatNotificationHandler;
		private IDisposable _chatMessageHandler;

		public void Dispose() {
			_chatNotificationHandler.Dispose();
			_chatMessageHandler.Dispose();
		}

		protected override async Task OnInitAsync() {
			ChatHistory = new List<string>();
			_chatNotificationHandler = Signal.Register<string>( "ChatNotification", HandleChatNotification );
			_chatMessageHandler = Signal.Register<ChatMessage>( "ChatMessage", HandleChatMessage );
			await Signal.Connect();
		}

		protected async Task SendTextClicked() {
			Console.WriteLine( "ComposedText: " + ComposedText );
			if (!string.IsNullOrWhiteSpace(ComposedText)) {
				ChatMessage message = new ChatMessage( State.Authentication.User.Name, ComposedText );
				await Signal.Invoke( "ChatMessage", message );
				ComposedText = "";
			}
		}

		private void HandleChatNotification( string message ) {
			AddChatMessage( message );
		}

		private void HandleChatMessage( ChatMessage message ) {
			AddChatMessage( $"{message.Name}: {message.Text}" );
		}

		private void AddChatMessage( string message ) {
			if (ChatHistory.Count() + 1 >= 100) {
				ChatHistory = ChatHistory.Take( 100 ).ToList();
			}
			ChatHistory.Insert( 0, message );
			StateHasChanged();
		}
	}
}
