using System;
using System.Collections.Generic;
using System.Text;


namespace TestAppPWA.Utils
{
    public enum MenuItemType
    {
        Races,
        Timing,
        Ranking,
        NewRace,
        Passings,
        Participants,
        NewParticipant,
        About,
        StartRace
    }

    public enum LangUI
    { 
        FR,
        EN,
        DE
    }

    public enum ERaceType
    {
        Multilaps = 0,
        ChronopointStart = 1,
        ChronopointFinish = 2,
        StartRace = 5,
        StartRaceAndChronopointFinish = 10
    }

    public enum TimingMode
    {
        Numpad,
        Selection
    }
}
