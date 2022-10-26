using Microsoft.AspNetCore.Components;
using Station.Shared;

namespace Station.Client.Pages;
public partial class Index {

	[Inject]
	protected StationServer.StationServerClient? Client { get; set; }

	public async Task Connect() {
		ConnectRequest request = new ConnectRequest {
			Name = "Kiyote"
		};
		ConnectResponse connectResponse = await Client!.ConnectAsync( request );
	}
}
