using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using TestAppPWA.Utils;

namespace TestAppPWA.Models
{
    [Serializable]
    public class Race : BaseNotify
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public DateTime? StartTime { get; set; }
        public ObservableCollection<Passing> Passings { get; set; } = new ObservableCollection<Passing>();
        public List<Participant> Participants { get; set; }
        public ERaceType RaceType { get; set; }
        public string ApiAccessCode { get; set; }
        public bool DevServer { get; set; } = false;
        public DateTime? StartTimeSent { get; set; }
        public DateTime? PassingsSent { get; set; }
        public DateTime? RankingValidated { get; set; }
        public DateTime? MarkFinished { get; set; }

        [NotMapped]
        [JsonIgnore]
        public string FormattedName => (DevServer ? "[DEV] " : "") + Name;
        [NotMapped]
        [JsonIgnore]
        public List<Passing> PassingsOrdered => Passings.OrderByDescending(p => p.PassingTime).ToList();
        [NotMapped]
        [JsonIgnore]
        public List<Passing> PassingsOrderedWithoutRemoved => Passings.Where(p => !p.IsRemoved).OrderByDescending(p => p.PassingTime).ToList();
        [NotMapped]
        [JsonIgnore]
        public int NbPassings => PassingsOrderedWithoutRemoved?.Count() ?? 0;

        [NotMapped]
        [JsonIgnore]
        public List<Participant> ParticipantsOrdered => Participants.OrderBy(p => p.Bib).ToList();

        [NotMapped]
        [JsonIgnore]
        public List<Participant> ParticipantsOrderedWithoutPassed
        {
            get
            {
                switch (RaceType)
                {
                    case ERaceType.ChronopointStart:
                    case ERaceType.ChronopointFinish:
                    case ERaceType.StartRaceAndChronopointFinish:
                        return Participants.Where(p => !p.HasPassing).OrderBy(p => p.Bib).ToList();
                    default:
                        return Participants.OrderBy(p => p.Bib).ToList();
                }
            }
        }

        [NotMapped]
        [JsonIgnore]
        public int NbParticipants => Participants?.Count() ?? 0;

        [NotMapped]
        [JsonIgnore]
        public bool IsMultilaps => RaceType == ERaceType.Multilaps;

        [NotMapped]
        [JsonIgnore]
        public bool IsWithRaceTime => RaceType == ERaceType.Multilaps || RaceType == ERaceType.StartRaceAndChronopointFinish;

        public List<Ranking> CalculateRanking()
        {
            List<Ranking> res = new List<Ranking>();

            if (RaceType != ERaceType.Multilaps) return res;
            // TODO : Implement ranking calcul for ERaceType.StartRaceAndChronopointFinish

            if (StartTime.HasValue)
            {
                foreach (var grp in Passings.Where(p => !p.IsRemoved && p.Bib > 0).GroupBy(p => p.Bib))
                {
                    res.Add(new Ranking { Race = this, Bib = grp.Key, Participant = this.Participants.FirstOrDefault(p => p.Bib == grp.Key), Laps = grp.Select(p => p.PassingTime).ToList() });
                }
            }

            var ranked = res.OrderByDescending(r => r.NbLaps).ThenBy(r => r.FinalTime).ThenBy(r => r.Bib).ToList();

            int rank = 1;
            foreach (var ranking in ranked)
            {
                ranking.Rank = rank++;
            }

            return ranked;
        }

        public string GetStartState()
        {
            return "GetStartState";
        }

        public void ClearData()
        {
            Passings.Clear();
            Participants.Clear();
            StartTime = null;
            StartTimeSent = null;
            PassingsSent = null;
            RankingValidated = null;
            MarkFinished = null;
        }
    }
}
