using DataLibrary;
using Telegram.Bot.Types.ReplyMarkups;

namespace MoscowSleddingBot.Additional;

/// <summary>
/// A static class for processing sorting-related processes
/// </summary>
public static class SortHelper
{
    public static readonly List<string> lstWithSortingHeaders = new List<string>{
        "global_id.A","global_id.D","ObjectName.A","ObjectName.D","NameWinter.A","NameWinter.D",
        "PhotoWinter.A","PhotoWinter.D","AdmArea.A","AdmArea.D","District.A","District.D","Address.A",
        "Address.D","Email.A","Email.D","WebSite.A","WebSite.D","HelpPhone.A","HelpPhone.D","HelpPhoneExtension.A",
        "HelpPhoneExtension.D","WorkingHoursWinter.A","WorkingHoursWinter.D","ClarificationOfWorkingHoursWinter.A",
        "ClarificationOfWorkingHoursWinter.D","HasEquipmentRental.A","HasEquipmentRental.D","EquipmentRentalComments.A",
        "EquipmentRentalComments.D","HasTechService.A","HasTechService.D","TechServiceComments.A","TechServiceComments.D",
        "HasDressingRoom.A","HasDressingRoom.D","HasEatery.A","HasEatery.D","HasToilet.A","HasToilet.D","HasWifi.A","HasWifi.D",
        "HasCashMachine.A","HasCashMachine.D","HasFirstAidPost.A","HasFirstAidPost.D","HasMusic.A","HasMusic.D","UsagePeriodWinter.A",
        "UsagePeriodWinter.D","DimensionsWinter.A","DimensionsWinter.D","Lighting.A","Lighting.D","SurfaceTypeWinter.A","SurfaceTypeWinter.D",
        "Seats.A","Seats.D","Paid.A","Paid.D","PaidComments.A","PaidComments.D","DisabilityFriendly.A","DisabilityFriendly.D","ServicesWinter.A",
        "ServicesWinter.D","geoData.A","geoData.D","geodata_center.A","geodata_center.D","geoarea.A","geoarea.D"};

    /// <summary>
    /// a method for creating a keyboard from buttons with fields and sorting directions
    /// </summary>
    /// <returns>keyboard with text</returns>
    public static InlineKeyboardMarkup CreateSortingMarkup()
    {
        List<InlineKeyboardButton[]> lstWithButtonsRows = new List<InlineKeyboardButton[]>();
        for (int i = 0; i < lstWithSortingHeaders.Count; i += 2)
        {
            InlineKeyboardButton[] curButtons = new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData(text: $"{Char.ConvertFromUtf32(int.Parse("2B07", System.Globalization.NumberStyles.HexNumber))}{lstWithSortingHeaders[i][..^2]}", callbackData: lstWithSortingHeaders[i]),
                InlineKeyboardButton.WithCallbackData(text: $"{Char.ConvertFromUtf32(int.Parse("2B06", System.Globalization.NumberStyles.HexNumber))}{lstWithSortingHeaders[i + 1][..^2]}", callbackData: lstWithSortingHeaders[i + 1]),
            };
            lstWithButtonsRows.Add(curButtons);
        }

        return new InlineKeyboardMarkup(lstWithButtonsRows);
    }

    /// <summary>
    /// A method for sorting data by the selected field
    /// </summary>
    /// <param name="lstWithData">a list with data</param>
    /// <param name="fieldName">sorting field</param>
    /// <param name="needToReverse">is reverse sorting necessary</param>
    /// <returns>sorted data</returns>
    public static List<IceHillData> SortDataByField(List<IceHillData> lstWithData, string fieldName, bool needToReverse)
    {
        List<IceHillData> lstSortedData = fieldName switch
        {
            "global_id" => lstWithData.OrderBy(n => n.GlobalId).ToList(),
            "ObjectName" => lstWithData.OrderBy(n => n.ObjectName ?? "").ToList(),
            "NameWinter" => lstWithData.OrderBy(n => n.NameWinter ?? "").ToList(),
            "PhotoWinter" => lstWithData.OrderBy(n => n.PhotoWinter ?? "").ToList(),
            "AdmArea" => lstWithData.OrderBy(n => n.AdmArea ?? "").ToList(),
            "District" => lstWithData.OrderBy(n => n.District ?? "").ToList(),
            "Address" => lstWithData.OrderBy(n => n.Address ?? "").ToList(),
            "Email" => lstWithData.OrderBy(n => n.Email ?? "").ToList(),
            "WebSite" => lstWithData.OrderBy(n => n.WebSite ?? "").ToList(),
            "HelpPhone" => lstWithData.OrderBy(n => n.HelpPhone ?? "").ToList(),
            "HelpPhoneExtension" => lstWithData.OrderBy(n => n.HelpPhoneExtension ?? "").ToList(),
            "WorkingHoursWinter" => lstWithData.OrderBy(n => n.WorkingHoursWinter ?? "").ToList(),
            "ClarificationOfWorkingHoursWinter" => lstWithData.OrderBy(n => n.ClarificationOfWorkingHoursWinter ?? "").ToList(),
            "HasEquipmentRental" => lstWithData.OrderBy(n => n.HasEquipmentRental ?? "").ToList(),
            "EquipmentRentalComments" => lstWithData.OrderBy(n => n.EquipmentRentalComments ?? "").ToList(),
            "HasTechService" => lstWithData.OrderBy(n => n.HasTechService ?? "").ToList(),
            "TechServiceComments" => lstWithData.OrderBy(n => n.TechServiceComments ?? "").ToList(),
            "HasDressingRoom" => lstWithData.OrderBy(n => n.HasDressingRoom ?? "").ToList(),
            "HasEatery" => lstWithData.OrderBy(n => n.HasEatery ?? "").ToList(),
            "HasToilet" => lstWithData.OrderBy(n => n.HasToilet ?? "").ToList(),
            "HasWifi" => lstWithData.OrderBy(n => n.HasWifi ?? "").ToList(),
            "HasCashMachine" => lstWithData.OrderBy(n => n.HasCashMachine ?? "").ToList(),
            "HasFirstAidPost" => lstWithData.OrderBy(n => n.HasFirstAidPost ?? "").ToList(),
            "HasMusic" => lstWithData.OrderBy(n => n.HasMusic ?? "").ToList(),

            "UsagePeriodWinter" => lstWithData.OrderBy(n => n.UsagePeriodWinter == null ? 0 : ((int.TryParse(n.UsagePeriodWinter.Split('-')[0].Split('.')[0], out int days) ? days : 0)
             + (int.TryParse(n.UsagePeriodWinter.Split('-')[0].Split('.')[^1], out int months) ? months * 32 : 0))).ToList(),

            "DimensionsWinter" => lstWithData.OrderBy(n => n.DimensionsWinter ?? "").ToList(),
            "Lighting" => lstWithData.OrderBy(n => n.Lighting ?? "").ToList(),
            "SurfaceTypeWinter" => lstWithData.OrderBy(n => n.SurfaceTypeWinter ?? "").ToList(),
            "Seats" => lstWithData.OrderBy(n => n.Seats).ToList(),
            "Paid" => lstWithData.OrderBy(n => n.Paid ?? "").ToList(),
            "PaidComments" => lstWithData.OrderBy(n => n.PaidComments?? "").ToList(),
            "DisabilityFriendly" => lstWithData.OrderBy(n => n.DisabilityFriendly ?? "").ToList(),
            "ServicesWinter" => lstWithData.OrderBy(n => n.ServicesWinter ?? "").ToList(),
            "geoData" => lstWithData.OrderBy(n => n.GeoData ?? "").ToList(),
            "geodata_center" => lstWithData.OrderBy(n => n.GeoDataCenter ?? "").ToList(),
            "geoarea" => lstWithData.OrderBy(n => n.GeoArea ?? "").ToList(),
            _ => lstWithData
        };

        if (needToReverse) { lstSortedData.Reverse();}

        return lstSortedData;
    }
}