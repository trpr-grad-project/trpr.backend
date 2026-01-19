using Api.Middleware;
using Api.Extensions;
using Serilog;
using Modules.Users.Infrastructure;
using Common.Application;
using Common.Presentation;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureSwagger();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCommonApplication(Modules.Users.Application.AssemblyRefrence.Assembly);
builder.Services.AddCommonPresentation(Modules.Users.Presentation.AssemblyRefrence.Assembly);
builder.Services.AddUsersModule(builder.Configuration);
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();
builder.Host.UseSerilog();
var app = builder.Build();
app.UseStaticFiles();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    // app.AddMigrations();
}
app.AddMiddlewares();
app.UseHttpLogging();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
