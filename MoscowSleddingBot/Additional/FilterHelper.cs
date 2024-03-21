using DataLibrary;
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

    public static ReplyKeyboardMarkup CreateValuesMarkup(List<IceHillData> lstWithData, string fieldName, out bool isNoData)
    {
        List<string> lstUniqueFields = FindUniqueValuesInField(lstWithData, fieldName, out isNoData);
        List<KeyboardButton[]> lstWithButtonsFieldValues = new List<KeyboardButton[]>();
        for (int i = 0; i < lstUniqueFields.Count; i+=2)
        {   
            KeyboardButton[] curButtons;
            if (i + 1 < lstUniqueFields.Count)
            {
                curButtons = new KeyboardButton[]
                {   
                    new KeyboardButton(lstUniqueFields[i]),
                    new KeyboardButton(lstUniqueFields[i + 1])
                };
            }
            else
            {
                curButtons = new KeyboardButton[]
                {   
                    new KeyboardButton(lstUniqueFields[i]),
                };
            }
            lstWithButtonsFieldValues.Add(curButtons);
        }
        
        return new ReplyKeyboardMarkup(lstWithButtonsFieldValues){ResizeKeyboard = true};
    }

    private static List<string> FindUniqueValuesInField(List<IceHillData> lstWithData, string fieldName, out bool isNoData)
    {
        List<string> lstUniqueFields = fieldName switch
        {
            "global_id" => lstWithData.Select(x => x.GlobalId.ToString()).Distinct().ToList(),
            "ObjectName" => lstWithData.Select(n => n.ObjectName ?? "").Distinct().ToList(),
            "NameWinter" => lstWithData.Select(n => n.NameWinter ?? "").Distinct().ToList(),
            "PhotoWinter" => lstWithData.Select(n => n.PhotoWinter ?? "").Distinct().ToList(),
            "AdmArea" => lstWithData.Select(n => n.AdmArea ?? "").Distinct().ToList(),
            "District" => lstWithData.Select(n => n.District ?? "").Distinct().ToList(),
            "Address" => lstWithData.Select(n => n.Address ?? "").Distinct().ToList(),
            "Email" => lstWithData.Select(n => n.Email ?? "").Distinct().ToList(),
            "WebSite" => lstWithData.Select(n => n.WebSite ?? "").Distinct().ToList(),
            "HelpPhone" => lstWithData.Select(n => n.HelpPhone ?? "").Distinct().ToList(),
            "HelpPhoneExtension" => lstWithData.Select(n => n.HelpPhoneExtension ?? "").Distinct().ToList(),
            "WorkingHoursWinter" => lstWithData.Select(n => n.WorkingHoursWinter ?? "").Distinct().ToList(),
            "ClarificationOfWorkingHoursWinter" => lstWithData.Select(n => n.ClarificationOfWorkingHoursWinter ?? "").Distinct().ToList(),
            "HasEquipmentRental" => lstWithData.Select(n => n.HasEquipmentRental ?? "").Distinct().ToList(),
            "EquipmentRentalComments" => lstWithData.Select(n => n.EquipmentRentalComments ?? "").Distinct().ToList(),
            "HasTechService" => lstWithData.Select(n => n.HasTechService ?? "").Distinct().ToList(),
            "TechServiceComments" => lstWithData.Select(n => n.TechServiceComments ?? "").Distinct().ToList(),
            "HasDressingRoom" => lstWithData.Select(n => n.HasDressingRoom ?? "").Distinct().ToList(),
            "HasEatery" => lstWithData.Select(n => n.HasEatery ?? "").Distinct().ToList(),
            "HasToilet" => lstWithData.Select(n => n.HasToilet ?? "").Distinct().ToList(),
            "HasWifi" => lstWithData.Select(n => n.HasWifi ?? "").Distinct().ToList(),
            "HasCashMachine" => lstWithData.Select(n => n.HasCashMachine ?? "").Distinct().ToList(),
            "HasFirstAidPost" => lstWithData.Select(n => n.HasFirstAidPost ?? "").Distinct().ToList(),
            "HasMusic" => lstWithData.Select(n => n.HasMusic ?? "").Distinct().ToList(),
            "UsagePeriodWinter" => lstWithData.Select(n => n.UsagePeriodWinter ?? "").Distinct().ToList(),
            "DimensionsWinter" => lstWithData.Select(n => n.DimensionsWinter ?? "").Distinct().ToList(),
            "Lighting" => lstWithData.Select(n => n.Lighting ?? "").Distinct().ToList(),
            "SurfaceTypeWinter" => lstWithData.Select(n => n.SurfaceTypeWinter ?? "").Distinct().ToList(),
            "Seats" => lstWithData.Select(n => n.Seats.ToString()).Distinct().ToList(),
            "Paid" => lstWithData.Select(n => n.Paid ?? "").Distinct().ToList(),
            "PaidComments" => lstWithData.Select(n => n.PaidComments?? "").Distinct().ToList(),
            "DisabilityFriendly" => lstWithData.Select(n => n.DisabilityFriendly ?? "").Distinct().ToList(),
            "ServicesWinter" => lstWithData.Select(n => n.ServicesWinter ?? "").Distinct().ToList(),
            "geoData" => lstWithData.Select(n => n.GeoData ?? "").Distinct().ToList(),
            "geodata_center" => lstWithData.Select(n => n.GeoDataCenter ?? "").Distinct().ToList(),
            "geoarea" => lstWithData.Select(n => n.GeoArea ?? "").Distinct().ToList(),
            _ => lstWithData.Select(x => x.GlobalId.ToString()).Distinct().ToList(),
        };

        if (lstUniqueFields.Count ==  1 && lstUniqueFields[0] == "")
        {
            isNoData = true;
        }
        else {isNoData = false;}
        return lstUniqueFields;
    }

    public static List<IceHillData> FilterDataByFieldAndParam(List<IceHillData> lstWithData, string fieldName, string filterParam)
    {
         List<IceHillData> lstFilteredData  = fieldName switch
        {
            "global_id" => lstWithData.Where(x => x.GlobalId.ToString() == filterParam).ToList(),
            "ObjectName" => lstWithData.Where(n => (n.ObjectName ?? "") == filterParam).ToList(),
            "NameWinter" => lstWithData.Where(n => (n.NameWinter ?? "") == filterParam).ToList(),
            "PhotoWinter" => lstWithData.Where(n => (n.PhotoWinter ?? "") == filterParam).ToList(),
            "AdmArea" => lstWithData.Where(n => (n.AdmArea ?? "") == filterParam).ToList(),
            "District" => lstWithData.Where(n => (n.District ?? "") == filterParam).ToList(),
            "Address" => lstWithData.Where(n => (n.Address ?? "") == filterParam).ToList(),
            "Email" => lstWithData.Where(n => (n.Email ?? "") == filterParam).ToList(),
            "WebSite" => lstWithData.Where(n => (n.WebSite ?? "") == filterParam).ToList(),
            "HelpPhone" => lstWithData.Where(n => (n.HelpPhone ?? "") == filterParam).ToList(),
            "HelpPhoneExtension" => lstWithData.Where(n => (n.HelpPhoneExtension ?? "") == filterParam).ToList(),
            "WorkingHoursWinter" => lstWithData.Where(n => (n.WorkingHoursWinter ?? "") == filterParam).ToList(),
            "ClarificationOfWorkingHoursWinter" => lstWithData.Where(n => (n.ClarificationOfWorkingHoursWinter ?? "") == filterParam).ToList(),
            "HasEquipmentRental" => lstWithData.Where(n => (n.HasEquipmentRental ?? "") == filterParam).ToList(),
            "EquipmentRentalComments" => lstWithData.Where(n => (n.EquipmentRentalComments ?? "") == filterParam).ToList(),
            "HasTechService" => lstWithData.Where(n => (n.HasTechService ?? "") == filterParam).ToList(),
            "TechServiceComments" => lstWithData.Where(n => (n.TechServiceComments ?? "") == filterParam).ToList(),
            "HasDressingRoom" => lstWithData.Where(n => (n.HasDressingRoom ?? "") == filterParam).ToList(),
            "HasEatery" => lstWithData.Where(n => (n.HasEatery ?? "") == filterParam).ToList(),
            "HasToilet" => lstWithData.Where(n => (n.HasToilet ?? "") == filterParam).ToList(),
            "HasWifi" => lstWithData.Where(n => (n.HasWifi ?? "") == filterParam).ToList(),
            "HasCashMachine" => lstWithData.Where(n => (n.HasCashMachine ?? "") == filterParam).ToList(),
            "HasFirstAidPost" => lstWithData.Where(n => (n.HasFirstAidPost ?? "") == filterParam).ToList(),
            "HasMusic" => lstWithData.Where(n => (n.HasMusic ?? "") == filterParam).ToList(),
            "UsagePeriodWinter" => lstWithData.Where(n => (n.UsagePeriodWinter ?? "") == filterParam).ToList(),
            "DimensionsWinter" => lstWithData.Where(n => (n.DimensionsWinter ?? "") == filterParam).ToList(),
            "Lighting" => lstWithData.Where(n => (n.Lighting ?? "") == filterParam).ToList(),
            "SurfaceTypeWinter" => lstWithData.Where(n => (n.SurfaceTypeWinter ?? "") == filterParam).ToList(),
            "Seats" => lstWithData.Where(n => n.Seats.ToString() == filterParam).ToList(),
            "Paid" => lstWithData.Where(n => (n.Paid ?? "") == filterParam).ToList(),
            "PaidComments" => lstWithData.Where(n => (n.PaidComments?? "") == filterParam).ToList(),
            "DisabilityFriendly" => lstWithData.Where(n => (n.DisabilityFriendly ?? "") == filterParam).ToList(),
            "ServicesWinter" => lstWithData.Where(n => (n.ServicesWinter ?? "") == filterParam).ToList(),
            "geoData" => lstWithData.Where(n => (n.GeoData ?? "") == filterParam).ToList(),
            "geodata_center" => lstWithData.Where(n => (n.GeoDataCenter ?? "") == filterParam).ToList(),
            "geoarea" => lstWithData.Where(n => (n.GeoArea ?? "") == filterParam).ToList(),
            _ => lstWithData,
        };

        return lstFilteredData;
    }
}