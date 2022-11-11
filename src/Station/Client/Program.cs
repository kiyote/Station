using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Station.Client;
using Station.Shared;

var builder = WebAssemblyHostBuilder.CreateDefault( args );
builder.RootComponents.Add<App>( "#app" );
builder.RootComponents.Add<HeadOutlet>( "head::after" );

builder.Services.AddScoped( sp => new HttpClient { BaseAddress = new Uri( builder.HostEnvironment.BaseAddress ) } );

// Create the gRPC client
builder.Services.AddSingleton( services => {
	//string baseUri = "https://localhost:5001/";
	string baseUri = "http://localhost:5000/";
	var channel = GrpcChannel.ForAddress(
		baseUri,
		new GrpcChannelOptions {
			HttpHandler = new GrpcWebHandler( new HttpClientHandler() )
		} );
	return new StationServer.StationServerClient( channel );
} );

await builder.Build().RunAsync().ConfigureAwait( false );
