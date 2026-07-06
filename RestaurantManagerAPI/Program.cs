using RestaurantManagerAPI.Data;
using RestaurantManagerAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<DapperContext>();

builder.Services.AddScoped<IRestoranRepository, RestoranRepository>();
builder.Services.AddScoped<IYemekRepository, YemekRepository>();
builder.Services.AddScoped<ISiparisRepository, SiparisRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMvc", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowMvc");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
