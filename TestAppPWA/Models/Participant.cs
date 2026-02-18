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
    public class Participant : BaseNotify
    {
        public string Id { get; set; }
        [JsonIgnore]
        public Race Race { get; set; }
        public int Bib { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [NotMapped]
        [JsonIgnore]
        public string DisplayName => $"{LastName.ToUpper()} {FirstName}";

        [NotMapped]
        [JsonIgnore]
        public int NbPassing => Race.Passings.Count(p => p.Bib == Bib && !p.IsRemoved);

        [NotMapped]
        [JsonIgnore]
        public bool HasPassing => NbPassing > 0;

        [NotMapped]
        [JsonIgnore]
        public int PassingColorIndex
        {
            get
            {
                if (Race.RaceType == ERaceType.Multilaps)
                {
                    var nb = Race.Passings.Count(p => p.Bib == Bib && !p.IsRemoved);

                    return nb % 10;
                }
                else
                {
                    return HasPassing ? 9 : 0;
                }
            }
        }

        public Participant()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
