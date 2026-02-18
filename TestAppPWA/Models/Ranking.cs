using TestAppPWA.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace TestAppPWA.Models
{
    public class Ranking : BaseNotify
    {
        public string Id { get; set; }
        public Race Race { get; set; }
        public int Rank { get; set; }
        public int Bib { get; set; }
        public Participant Participant { get; set; }
        public List<DateTime> Laps {get;set; }

        [NotMapped]
        [JsonIgnore]
        public int NbLaps => Laps?.Count ?? 0;
        [NotMapped]
        [JsonIgnore]
        public DateTime LastLap => Laps.Max();
        [NotMapped]
        [JsonIgnore]
        public TimeSpan? FinalTime => (Race.StartTime.HasValue ? LastLap - Race.StartTime : null);

        public Ranking()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
