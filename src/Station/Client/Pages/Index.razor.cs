using Microsoft.AspNetCore.Components;
using Station.Shared;

namespace Station.Client.Pages;
public partial class Index {

	public Index() {
		Id = string.Empty;
	}

	public string Id { get; set; }

	[Inject]
	protected StationServer.StationServerClient? Client { get; set; }

	public async Task Connect() {
		ConnectRequest request = new ConnectRequest {
			Name = "Kiyote"
		};
		ConnectResponse connectResponse = await Client!.ConnectAsync( request );

		Id = connectResponse.Pid;
	}
}
