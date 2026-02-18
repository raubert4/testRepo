using TestAppPWA.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace TestAppPWA.Models
{
    [Serializable]
    public class Passing : BaseNotify
    {
        public string Id { get; set; }
        [JsonIgnore]
        public Race Race { get; set; }
        public int Bib { get; set; }
        public DateTime PassingTime { get; set; }
        public bool IsRemoved { get; set; }
        public bool IsValid { get; set; }

        [NotMapped]
        [JsonIgnore]
        public string RaceTimeFormatted
        {
            get
            {
                if (Race?.StartTime != null && Race.IsWithRaceTime)
                {
                    return Helpers.FormatTime(PassingTime - Race.StartTime.Value);
                }

                return Helpers.FormatHour(PassingTime);
            }
        }

        [NotMapped]
        [JsonIgnore]
        public string RaceTimeFormattedFull
        {
            get
            {
                if (Race?.StartTime != null && Race.IsWithRaceTime)
                {
                    return Helpers.FormatTimeFull(PassingTime - Race.StartTime.Value);
                }

                return Helpers.FormatHourFull(PassingTime);
            }
        }

        [NotMapped]
        [JsonIgnore]
        public TimeSpan RaceTime
        {
            get
            {
                if (Race?.StartTime != null && Race.IsWithRaceTime)
                {
                    return PassingTime - Race.StartTime.Value;
                }

                return PassingTime.TimeOfDay;
            }
            set
            {
                if (Race?.StartTime != null && Race.IsWithRaceTime)
                {
                    PassingTime = Race.StartTime.Value.Add(value);
                }
                else
                {
                    PassingTime = PassingTime.Date.Add(value);
                }
            }
        }

        [NotMapped]
        [JsonIgnore]
        public int LapNumber => Race?.Passings?.Count(p => p.Bib == Bib && !p.IsRemoved && p.PassingTime <= PassingTime) ?? 0;

        [NotMapped]
        [JsonIgnore]
        public Participant Participant => Race?.Participants?.FirstOrDefault(p => p.Bib == Bib);

        public Passing()
        {
            Id = Guid.NewGuid().ToString();
        }

        public Passing(Race race) : this()
        {
            Race = race;
        }
    }
}
