using Modules.Trips.Infrastructure.Options;

namespace Modules.Trips.Application.Services
{
    public interface IThemeProvider
    {
        ThemeDefinition Get(int themeId);
    }
}