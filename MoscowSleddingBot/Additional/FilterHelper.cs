using Telegram.Bot.Types.ReplyMarkups;

namespace MoscowSleddingBot.Additional;

public static class FilterHelper
{

    public static readonly List<string> lstWithFilterHeaders = new List<string>{
        "global_id","ObjectName","NameWinter","PhotoWinter","AdmArea","District","Address","Email","WebSite","HelpPhone",
        "HelpPhoneExtension","WorkingHoursWinter","ClarificationOfWorkingHoursWinter","HasEquipmentRental","EquipmentRentalComments",
        "HasTechService","TechServiceComments","HasDressingRoom","HasEatery","HasToilet","HasWifi","HasCashMachine","HasFirstAidPost",
        "HasMusic","UsagePeriodWinter","DimensionsWinter","Lighting","SurfaceTypeWinter","Seats","Paid","PaidComments","DisabilityFriendly",
        "ServicesWinter","geoData","geodata_center","geoarea"};

    public static InlineKeyboardMarkup CreateFilteringMarkup()
    {
        List<InlineKeyboardButton[]> lstWithButtonsRows = new List<InlineKeyboardButton[]>();
        for (int i = 0; i < lstWithFilterHeaders.Count; i += 2)
        {   
            InlineKeyboardButton[] curButtons;
            if (i + 1 < lstWithFilterHeaders.Count)
            {
                curButtons = new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(text: lstWithFilterHeaders[i], callbackData: lstWithFilterHeaders[i]),
                    InlineKeyboardButton.WithCallbackData(text: lstWithFilterHeaders[i + 1], callbackData: lstWithFilterHeaders[i + 1]),
                };
            }
            else
            {
                curButtons = new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(text: lstWithFilterHeaders[i], callbackData: lstWithFilterHeaders[i]),
                };  
            }
            lstWithButtonsRows.Add(curButtons);
        }

        return new InlineKeyboardMarkup(lstWithButtonsRows);
    }
}