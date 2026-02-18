using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Radzen;
using Radzen.Blazor;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

namespace TestAppPWA.Utils
{
    public static class Helpers
    {
        public static void ShowNotification(NotificationService notificationSvc, bool success, RenderFragment content)
        {
            notificationSvc.Notify(new NotificationMessage
            {
                Severity = success ? NotificationSeverity.Success : NotificationSeverity.Error,
                Duration = success ? 2000 : 10000,
                SummaryContent = ns => content
            });
        }
        public async static Task<bool?> ShowDialogYesNo(DialogService dialogSvc, RenderFragment text)
        {
            return await dialogSvc.Confirm(text, "Confirmation", new ConfirmOptions
            {
                OkButtonText = "Oui",
                CancelButtonText = "Non",
                ShowClose = false                
            });
        }

        public static string FormatTime(TimeSpan time)
        {
            string res = string.Empty;

            if (time.Hours > 0)
            {
                res = $"{time.Hours.ToString("00")}:";
            }

            res += $"{time.Minutes.ToString("00")}:{time.Seconds.ToString("00")}";

            return res;
        }

        public static string FormatTimeFull(TimeSpan time, int millisecondsPrecision = 3)
        {
            string res = string.Empty;

            if (time.Hours > 0)
            {
                res = $"{time.Hours.ToString("00")}:";
            }

            res += $"{time.Minutes.ToString("00")}:{time.Seconds.ToString("00")}";

            if (millisecondsPrecision == 0)
            {
                // No millisecodns
            }
            else if (millisecondsPrecision == 1)
            {
                res += $".{(time.Milliseconds / 100).ToString("0")}";
            }
            else if (millisecondsPrecision == 2)
            {
                res += $".{(time.Milliseconds / 10).ToString("0")}";
            }
            else
            {
                res += $".{time.Milliseconds.ToString("000")}";
            }

            return res;
        }

        public static string FormatHour(DateTime time)
        {
            return time.ToString("HH:mm:ss");
        }

        public static string FormatHourFull(DateTime time)
        {
            return time.ToString("HH:mm:ss.fff");
        }

        public static string GetPageTitle(string pageUrl)
        //public static string GetPageTitle(IStringLocalizer<SharedResource> L, string pageUrl)
        {
            return pageUrl;
        //    switch (pageUrl.ToLower())
        //    {
        //        case "":
        //        case "/":
        //        case "index":
        //            return L["Menu_Home"];
        //        case "races":
        //            return L["Menu_Races"];
        //        case "createrace":
        //            return L["Menu_CreateRace"];
        //        case "participants":
        //            return L["Menu_Participants"];
        //        case "startrace":
        //            return L["Menu_StartRace"];
        //        case "timing":
        //            return L["Menu_Timing"];
        //        case "timingoverview":
        //            return L["Menu_TimingOverview"];
        //        case "passings":
        //            return L["Menu_Passings"];
        //        case "rankingscratch":
        //            return L["Menu_RankingScratch"];
        //        case "about":
        //            return L["Menu_About"];
        //        default:
        //            return string.Empty;
        //    }
        }

        public static string GetNbPassingForeground(int nbPassing)
        {
            switch (nbPassing % 9)
            {
                case 0:
                case 1:
                    return "#000";
                case 2:
                case 3:
                    return "#fff";
                case 4:
                case 5:
                case 6:
                    return "#000";
                case 7:
                case 8:
                    return "#fff";
            }

            return "#000";
        }

        public static string GetNbPassingBackground(int nbPassing)
        {
            switch (nbPassing % 9)
            {
                case 0:
                    return "#d2fbff";
                case 1:
                    return "#8cc5d1";
                case 2:
                    return "#4e95a9";
                case 3:
                    return "#20728b";
                case 4:
                    return "#d8ffe0";
                case 5:
                    return "#b7efc7";
                case 6:
                    return "#95deae";
                case 7:
                    return "#48b875";
                case 8:
                    return "#009440";
            }

            return "#ffffff";
        }
    }
}
