using TestAppPWA.Models;
using TestAppPWA.Utils;

namespace TestAppPWA.Services
{
    public interface IApiService
    {
        Task<ReturnResult> GetParticipants(bool devServer, string apiAccessCode);
        Task<ReturnResult> SendPassings(Race race);
        Task<ReturnResult> SendStart(Race race, int? bibFrom, int? bibTo);
        Task<bool> CheckConnection(bool devServer);
    }

    public class ParticipantModel
    {
        public int Bib { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime Birthdate { get; set; }
        public string Race { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Team { get; set; }
        public IList<string> OtherRaces { get; set; } = new List<string>();
    }
}
