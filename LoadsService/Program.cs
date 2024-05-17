using System.Text;
using LoadsService;
using LoadsService.BD;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = Consts.Issuer,
            ValidateAudience = true,
            ValidAudience = Consts.Audience,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Consts.Key)),
            ValidateIssuerSigningKey = true,
        };
    });

// Add services to the container.

var connection = builder.Configuration.GetConnectionString("ConnectionStringBeg") + 
                 Environment.GetEnvironmentVariable("HOSTNAME") + 
                 builder.Configuration.GetConnectionString("DefaultConnection") + 
                 "Username=" + Environment.GetEnvironmentVariable("USERNAME") + 
                 ";Password="+
                 Environment.GetEnvironmentVariable("PASSWORD");
        
// добавляем контекст ApplicationContext в качестве сервиса в приложение
Console.WriteLine(connection);
builder.Services.AddDbContext<BDManage>(options => options.UseNpgsql(connection));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();