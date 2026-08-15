using Calculator.Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorWasm", policy =>
    {
        policy
            .WithOrigins("https://localhost:7244")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithExposedHeaders(
                "Grpc-Status",
                "Grpc-Message",
                "Grpc-Encoding",
                "Grpc-Accept-Encoding",
                "Grpc-Status-Details-Bin");
    });
});

var app = builder.Build();

app.UseRouting();
app.UseGrpcWeb();
app.UseCors("BlazorWasm");
app.MapGrpcService<CalculatorGrpcService>()
    .EnableGrpcWeb()
    .RequireCors("BlazorWasm");

if (app.Environment.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

app.MapGet("/", () => "gRPC server is running.");

app.Run();