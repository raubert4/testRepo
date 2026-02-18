using TestAppPWA.Utils;

namespace TestAppPWA.Models
{
    public class UserSettings
    {
        public required string UniqueId { get; set; }
        public bool IsRegistred { get; set; }

        public string? CurrentRaceId { get; set; }
        public string? CurrentRaceName { get; set; }

        public LangUI OptionLang { get; set; } = LangUI.FR;
        public TimingMode OptionTimingMode { get; set; } = TimingMode.Numpad;
        public double OptionTimingSizeInBibSelection { get; set; } = 50; 
        public double OptionTimingSizeInNumpad { get; set; } = 50;
        public int OptionBibSelectionButtonSize { get; set; } = 80;
        public bool OptionBibSelectionHidePassed { get; set; } = false;
        public int OptionNumpadButtonHeight { get; set; } = 50;

    }
}
