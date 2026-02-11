using Api.Extensions;
using Serilog;
using Modules.Users.Infrastructure;
using Modules.Notifications.Infrastructure;
using Common.Application;
using Common.Presentation;
using Common.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureSwagger();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddConfigurations(builder.Configuration, "Users", "Notifications");
builder.Services.AddCommonApplication(
    Modules.Users.Application.AssemblyRefrence.Assembly,
    Modules.Notifications.Application.AssemblyRefrence.Assembly);
builder.Services.AddCommonPresentation(
    Modules.Users.Presentation.AssemblyRefrence.Assembly,
    Modules.Notifications.Presentation.AssemblyRefrence.Assembly);
builder.Services.AddCommonInfrastructure(builder.Configuration);
builder.Services.AddUsersModule(builder.Configuration);
builder.Services.AddNotificationsModule(builder.Configuration);
builder.Services.AddIntegrationEvents();
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
    await app.AddMigrations();
}
app.AddMiddlewares();
app.UseHttpLogging();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
