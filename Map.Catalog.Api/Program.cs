using Microsoft.AspNetCore.Mvc;
using Map.Catalog.Api.Services;
using Map.Catalog.Api.Logic;
using Map.Catalog.Api.MapDB;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);



// отображение из базы данных
/*
var MAP_SQL_CONN_STR = builder.Configuration["MAP_SQL_CONN_STR"] ?? throw new ArgumentNullException("MAP_SQL_CONN_STR");
builder.Services.AddDbContext<MapdbContext>(
              options => options.UseNpgsql(MAP_SQL_CONN_STR));
builder.Services.AddScoped<IMapRuleService, MapRuleServiceMapdb>();
*/
// отображение из статичких данных
builder.Services.AddScoped<IMapRuleService, MapRuleServiceTest>();
builder.Services.AddScoped<IMapper, Mapper>();
builder.Services.AddScoped<IMapService, MapService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapPost("yandex/wildberries",
(
    Map.Catalog.Api.Model.Yandex.YandexHeatingSystem source,
    [FromServices] IMapService mapService
) =>
{
    var result = mapService.Map(source, "yandex", "wildberries");
    return result;
})
.WithName("YandexToWildberries")
.WithOpenApi();

app.MapPost("wildberries/yandex",
(
    Map.Catalog.Api.Model.Wildberries.WildberriesHeatingSystem source,
    [FromServices] IMapService mapService
) =>
{
    var result = mapService.Map(source, "wildberries", "yandex");
    return result;
})
.WithName("WildberriesToYandex")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
