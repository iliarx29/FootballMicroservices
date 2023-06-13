using Microsoft.IdentityModel.Logging;
using Teams.API;
using Teams.API.Middlewares;
using Teams.Domain;
using Teams.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddApi(builder.Configuration)
    .AddDomain()
    .AddInsfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    IdentityModelEventSource.ShowPII = true;
}

/*pp.UseHttpsRedirection();
*/
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
