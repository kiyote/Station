using Grpc.Core;
using Station.Shared;

namespace Station.Server.Services;

public class GrpcService : StationServer.StationServerBase {

	public override Task<ConnectResponse> Connect(
		ConnectRequest request,
		ServerCallContext context
	) {
		var response = new ConnectResponse {
			Pid = Guid.NewGuid().ToString( "N" )
		};
#pragma warning disable CA1303
		Console.WriteLine( "Connection." );
#pragma warning restore CA1303
		return Task.FromResult( response );
	}
}
