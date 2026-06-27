using Microsoft.Extensions.Options;
using Modules.Trips.Infrastructure.Options;

namespace Modules.Trips.Application.Services
{
    public class ThemeProvider : IThemeProvider
    {
        private readonly ThemeOptions _themeOptions;

        public ThemeProvider(IOptions<ThemeOptions> themeOptions)
        {
            _themeOptions = themeOptions.Value;
        }

        public ThemeDefinition Get(int themeId)
        {
            var theme = _themeOptions.Themes.FirstOrDefault(x => x.Id == themeId);

            if (theme == null)
                throw new ArgumentException($"Theme with ID {themeId} not found.");

            return theme;
        }
    }
}