using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using PmeMemberApi;
using PmeMemberApi.Common.Swagger;
using PmeMemberApi.Core;
using PmeMemberApi.Core.Dao;
using PmeMemberApi.Core.IDao;
using PmeMemberApi.SecureAuth;


var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.

// Confifugure to use Sql using Entity Framework
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("Connection")));

//Added Identity Service
/*builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();*/
builder.Services.AddSwaggerService();
// Added Authentication Service
builder.Services.AddJWT(builder.Configuration);
builder.Services.AddAuth(builder.Configuration);

builder.Services.AddScoped<IAuthDao, AuthDao>();
builder.Services.AddScoped<IPmeMemberApiDao, PmeMemberApiDao>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();

// HTTP request pipeline Configuration.
/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}*/

app.UseSwaggerService();

app.UseHttpsRedirection();

// Authentication & Authorization middleware configuration
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();