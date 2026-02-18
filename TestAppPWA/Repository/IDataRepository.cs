using TestAppPWA.Models;

namespace TestAppPWA.Repository
{
    public interface IDataRepository
    {
        public Task<UserSettings> GetUserSettingsAsync();
        public Task SetUserSettingsAsync(UserSettings userSettings);

        public Task<List<Race>> GetRacesAsync();

        public Task<Race?> GetRaceAsync(string id);
        public Task SetRaceAsync(Race race);
    }
}
