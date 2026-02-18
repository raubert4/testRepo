using TestAppPWA.Models;
using TestAppPWA.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TestAppPWA.Services
{
    public class ArApiService : IApiService
    {
        #region Variables

        private readonly IHttpClientFactory _clientFactory;

        #endregion

        #region Properties

        #endregion

        #region Constructor

        public ArApiService(IHttpClientFactory clientFactory)
		{
            _clientFactory = clientFactory;
		}

        #endregion

        #region Methods

        public async Task<ReturnResult> GetParticipants(bool devServer, string apiAccessCode)
        {
            if (string.IsNullOrEmpty(apiAccessCode))
            {
                return ReturnResult.KO("Missing API code !");
            }

            var isConOk = await CheckConnection(devServer);
            if (!isConOk)
            {
                return ReturnResult.KO("Cannot connect to AR2Timing server !");
            }

            try
            {
                string url = $"api/TimingApi/GetCompParticipants?apiAccessCode={apiAccessCode}";

                var client = GetClient(devServer);
                var res = await client.GetAsync(url);

                var json = await res.Content.ReadAsStringAsync();

                var resPDs = JsonSerializer.Deserialize<IEnumerable<ParticipantModel>>(json);

                return ReturnResult.OK(resPDs.ToList());
            }
            catch (Exception e)
            {
                return ReturnResult.KO("Error while getting participants. " + e.Message);
            }

        }

        public async Task<ReturnResult> SendPassings(Race race)
        {
            var isConOk = await CheckConnection(race.DevServer);
            if (!isConOk)
            {
                return ReturnResult.KO("Cannot connect to AR2Timing server !");
            }

            try
            {
                var data = new CompetitionTimes
                {
                    ApiAccessCode = race.ApiAccessCode,
                    BibTimes = new List<CompetitionTimes.BibTime>()
                };

                if (race.RaceType == ERaceType.ChronopointStart)
                {
                    data.ChronopointType = ChronopointTypeEnum.Start;
                }
                else if (race.RaceType == ERaceType.ChronopointFinish || race.RaceType == ERaceType.StartRaceAndChronopointFinish)
                {
                    data.ChronopointType = ChronopointTypeEnum.Finish;
                }
                else if (race.RaceType == ERaceType.Multilaps)
                {
                    data.ChronopointType = ChronopointTypeEnum.Lap;
                }

                foreach (var passing in race.Passings)
                {
                    if (passing.IsRemoved) continue;
                    if (passing.Bib <= 0) continue;

                    data.BibTimes.Add(new CompetitionTimes.BibTime { Bib = passing.Bib, Time = passing.PassingTime.TimeOfDay.ToString() });
                }

                var json = JsonSerializer.Serialize(data);
                HttpContent c = new StringContent(json, Encoding.UTF8, "application/json");

                var client = GetClient(race.DevServer);
                var res = await client.PostAsync($"api/TimingApi/AddCompetitionTimes", c);
                var resData = await res.Content.ReadAsStringAsync();
                if (res.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return ReturnResult.OK(resData);
                }
                else
                {
                    return ReturnResult.KO(resData);
                }
            }
            catch (Exception e)
            {
                // TODO : Manage exception
                return ReturnResult.KO($"Error while sending passings. Error : {e.Message}");
            }
        }

        public async Task<ReturnResult> SendStart(Race race, int? bibFrom, int? bibTo)
        {
            var isConOk = await CheckConnection(race.DevServer);
            if (!isConOk)
            {
                return ReturnResult.KO("Cannot connect to AR2Timing server !");
            }

            try
            {
                string parameters = $"apiAccessCode={race.ApiAccessCode}";
                parameters += $"&startTime={race.StartTime.Value.TimeOfDay.ToString()}";
                if (bibFrom.HasValue)
                {
                    parameters += $"&bibFrom={bibFrom.Value}";
                }
                if (bibTo.HasValue)
                {
                    parameters += $"&bibTo={bibTo.Value}";
                }

                var client = GetClient(race.DevServer);
                var res = await client.PostAsync($"api/TimingApi/SetStartTimes?{parameters}", null);
                var resData = await res.Content.ReadAsStringAsync();
                if (res.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return ReturnResult.OK(resData);
                }
                else
                {
                    return ReturnResult.KO(resData);
                }
            }
            catch (Exception e)
            {
                // TODO : Manage exception
                return ReturnResult.KO($"Error while sending passings. Error : {e.Message}");
            }
        }

        public async Task<bool> CheckConnection(bool devServer)
        {
            try
            {
                var client = GetClient(devServer);
                var readTask = await client.GetAsync($"api/TimingApi/Test");

                var resData = await readTask.Content.ReadAsStringAsync();

                if (resData == Constantes.CST_Api_TestResult)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                // TODO : Manage exception
            }

            return false;
        }

        public void Dispose()
        {
        }

        private HttpClient GetClient(bool devServer)
        {
            var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri(devServer ? Constantes.CST_ApiUrlDev : Constantes.CST_ApiUrl);
            return client;
        }

        #endregion

        #region Events

        #endregion

        private enum ChronopointTypeEnum
        {
            Start = 0,
            Intermediate = 10,
            Lap = 11,
            Finish = 99
        }

        private class CompetitionTimes
        {
            public string ApiAccessCode { get; set; }
            public ChronopointTypeEnum ChronopointType { get; set; }
            public List<BibTime> BibTimes { get; set; }

            public class BibTime
            {
                public int Bib { get; set; }
                public string Time { get; set; }
            }
        }
    }
}
