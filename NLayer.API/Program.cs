using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLayer.Data;
using NLayer.Data.Models;
using NLayer.Data.Models.JWTModels;
using NLayer.Data.Repositories;
using System.Text;
using StackExchange.Redis;
using NLayer.Service.CacheService;
using NLayer.Service.ProductServices;
using NLayer.Service.TokenServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

builder.Services.AddDbContext<AppDbContext>(options => {
  options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConn"), action =>  
  action.MigrationsAssembly("NLayer.Data"));
});

builder.Services.AddScoped<GenericRepository<Product>>();
builder.Services.AddScoped<GenericRepository<Category>>();
builder.Services.AddScoped<GenericRepository<ProductFeature>>();
builder.Services.AddScoped<GenericRepository<User>>();
builder.Services.AddScoped<IJWTManagerRepository, JWTManagerRepository>();
builder.Services.AddScoped<UnitOfWork>();

builder.Services.AddAuthentication(x =>
{
	x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
	var Key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]);
	o.SaveToken = true;
	o.TokenValidationParameters = new TokenValidationParameters {
		ValidateIssuer = false,
		ValidateAudience = false,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
		ValidAudience = builder.Configuration["JWT:ValidAudience"],
		IssuerSigningKey = new SymmetricSecurityKey(Key)
	};
});
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<RefreshTokenService>();
builder.Services.AddScoped<InMemoryCacheService>();
builder.Services.AddSingleton<IRedisCacheService, RedisCacheService>();

var multiplexer = ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis"));
builder.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseAuthentication(); // This need to be added	
app.UseAuthorization();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();