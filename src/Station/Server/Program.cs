using Station.Server.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<GrpcService>();

builder.Services.AddGrpc();
builder.Services.AddCors( o => o.AddPolicy( "AllowAll", builder => {
	_ = builder.AllowAnyOrigin()
		   .AllowAnyMethod()
		   .AllowAnyHeader()
		   .WithExposedHeaders( "Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding" );
} ) );
builder.Services.AddControllers();
builder.Services.AddResponseCompression();
builder.WebHost.UseUrls(
	[
		"https://+:443",
	]
);

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	_ = app.UseHsts();
}

app.UseResponseCompression();
app.UseHttpsRedirection();
app.UseRouting();
app.UseGrpcWeb();
app.UseCors();
app.UseEndpoints( endpoints => {
	_ = app.MapControllers();
	_ = endpoints.MapGrpcService<GrpcService>()
		.EnableGrpcWeb()
		.RequireCors( "AllowAll" );
} );

app.Run();
