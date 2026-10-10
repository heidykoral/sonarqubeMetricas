using Application.UseCases;
using Infrastructure.Data;
using Infrastructure.Logging;

var builder = WebApplication.CreateBuilder(args);
var baseConnection = builder.Configuration.GetConnectionString("Sql");
var password = builder.Configuration["ConnectionStrings:SqlPassword"];

BadDb.ConnectionString = $"{baseConnection};Password={password}";


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




BadDb.ConnectionString = $"{baseConnection};Password={password}";

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Logging.ClearProviders();

builder.Services.AddCors(o => o.AddPolicy("bad", p => p.WithOrigins("https://localhost:63958").AllowAnyHeader().AllowAnyMethod()));

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

BadDb.ConnectionString = app.Configuration["ConnectionStrings:Sql"] ?? string.Empty;
app.UseCors("bad");

app.MapGet("/health", () =>
{
    Logger.Log("health ping");
    return Results.Ok("ok");
});

app.MapPost("/orders", async (HttpContext http) =>
{
    using var reader = new StreamReader(http.Request.Body);
    var body = await reader.ReadToEndAsync();
    var parts = (body ?? "").Split(',');
    var customer = parts.Length > 0 ? parts[0] : "anon";
    var product = parts.Length > 1 ? parts[1] : "unknown";
    var qty = parts.Length > 2 ? int.Parse(parts[2]) : 1;
    var price = parts.Length > 3 ? decimal.Parse(parts[3]) : 0.99m;

    var order = CreateOrderUseCase.Execute(customer, product, qty, price);

    return Results.Ok(order);
});

app.MapGet("/orders/last", () => Domain.Services.OrderService.LastOrders);

app.MapGet("/info", (IConfiguration cfg) => new
{
    sql = BadDb.ConnectionString,
    env = Environment.GetEnvironmentVariables(),
    version = "v0.0.1-unsecure"
});

await app.RunAsync();
