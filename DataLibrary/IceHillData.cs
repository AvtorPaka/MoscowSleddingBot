using System.Text.Json.Serialization;
using CsvHelper.Configuration.Attributes;

namespace DataLibrary;

/// <summary>
/// A class for storing data
/// </summary>
[Serializable, CultureInfo("ru-RU")]
public class IceHillData
{   
    private long _globalId;

    [JsonPropertyName("global_id"), Name("global_id"), Index(0)]
    public long GlobalId
    {
        get =>  _globalId;
        set => _globalId = value;
    }

    private string? _objectName;
    [JsonPropertyName("ObjectName"), Name("ObjectName")]
    public string ObjectName
    {
        get => _objectName!;
        set {_objectName = CheckStringFieldValue(value);}
    }

    private string? _nameWinter;
    [JsonPropertyName("NameWinter"), Name("NameWinter")]
    public string NameWinter
    {
        get => _nameWinter!;
        set {_nameWinter = CheckStringFieldValue(value);}
    }

    private string? _photoWinter;
    [JsonPropertyName("PhotoWinter"), Name("PhotoWinter")]
    public string PhotoWinter
    {
        get => _photoWinter!;
        set {_photoWinter = CheckStringFieldValue(value);}
    }

    private string? _admArea;
    [JsonPropertyName("AdmArea"), Name("AdmArea")]
    public string AdmArea
    {
        get => _admArea!;
        set {_admArea = CheckStringFieldValue(value);}
    }

    private string? _district;
    [JsonPropertyName("District"), Name("District")]
    public string District
    {
        get => _district!;
        set {_district = CheckStringFieldValue(value);}
    }

    private string? _address;
    [JsonPropertyName("Address"), Name("Address")]
    public string Address
    {
        get => _address!;
        set {_address = CheckStringFieldValue(value);}
    }

    private string? _email;
    [JsonPropertyName("Email"), Name("Email")]
    public string Email
    {
        get => _email!;
        set {_email = CheckStringFieldValue(value);}
    }

    private string? _webSite;
    [JsonPropertyName("WebSite"), Name("WebSite")]
    public string WebSite
    {
        get => _webSite!;
        set {_webSite = CheckStringFieldValue(value);}
    }

    private string? _helpPhone;
    [JsonPropertyName("HelpPhone"), Name("HelpPhone")]
    public string HelpPhone
    {
        get => _helpPhone!;
        set {_helpPhone = CheckStringFieldValue(value);}
    }

    private string? _helpPhoneExtension;
    [JsonPropertyName("HelpPhoneExtension"), Name("HelpPhoneExtension")]
    public string HelpPhoneExtension
    {
        get => _helpPhoneExtension!;
        set {_helpPhoneExtension = CheckStringFieldValue(value);}
    }

    private string? _workingHoursWinter;
    [JsonPropertyName("WorkingHoursWinter"), Name("WorkingHoursWinter")]
    public string WorkingHoursWinter
    {
        get => _workingHoursWinter!;
        set {_workingHoursWinter = CheckStringFieldValue(value);}
    }

    private string? _clarificationOfWorkingHoursWinter;
    [JsonPropertyName("ClarificationOfWorkingHoursWinter"), Name("ClarificationOfWorkingHoursWinter")]
    public string ClarificationOfWorkingHoursWinter
    {
        get => _clarificationOfWorkingHoursWinter!;
        set {_clarificationOfWorkingHoursWinter = CheckStringFieldValue(value);}
    }

    private string? _hasEquipmentRental;
    [JsonPropertyName("HasEquipmentRental"), Name("HasEquipmentRental")]
    public string HasEquipmentRental
    {
        get => _hasEquipmentRental!;
        set {_hasEquipmentRental = CheckStringFieldValue(value);}
    }

    private string? _equipmentRentalComments;
    [JsonPropertyName("EquipmentRentalComments"), Name("EquipmentRentalComments")]
    public string EquipmentRentalComments
    {
        get => _equipmentRentalComments!;
        set {_equipmentRentalComments= CheckStringFieldValue(value);}
    }

    private string? _hasTechService;
    [JsonPropertyName("HasTechService"), Name("HasTechService")]
    public string HasTechService
    {
        get => _hasTechService!;
        set {_hasTechService= CheckStringFieldValue(value);}
    }

    private string? _techServiceComments;
    [JsonPropertyName("TechServiceComments"), Name("TechServiceComments")]
    public string TechServiceComments
    {
        get => _techServiceComments!;
        set {_techServiceComments= CheckStringFieldValue(value);}
    }

    private string? _hasDressingRoom;
    [JsonPropertyName("HasDressingRoom"), Name("HasDressingRoom")]
    public string HasDressingRoom
    {
        get => _hasDressingRoom!;
        set {_hasDressingRoom= CheckStringFieldValue(value);}
    }

    private string? _hasEatery;
    [JsonPropertyName("HasEatery"), Name("HasEatery")]
    public string HasEatery
    {
        get => _hasEatery!;
        set {_hasEatery= CheckStringFieldValue(value);}
    }

    private string? _hasToilet;
    [JsonPropertyName("HasToilet"), Name("HasToilet")]
    public string HasToilet
    {
        get => _hasToilet!;
        set {_hasToilet= CheckStringFieldValue(value);}
    }

    private string? _hasWifi;
    [JsonPropertyName("HasWifi"), Name("HasWifi")]
    public string HasWifi
    {
        get => _hasWifi!;
        set {_hasWifi= CheckStringFieldValue(value);}
    }

    private string? _hasCashMachine;
    [JsonPropertyName("HasCashMachine"), Name("HasCashMachine")]
    public string HasCashMachine
    {
        get => _hasCashMachine!;
        set {_hasCashMachine= CheckStringFieldValue(value);}
    }

    private string? _hasFirstAidPost;
    [JsonPropertyName("HasFirstAidPost"), Name("HasFirstAidPost")]
    public string HasFirstAidPost
    {
        get => _hasFirstAidPost!;
        set {_hasFirstAidPost= CheckStringFieldValue(value);}
    }

    private string? _hasMusic;
    [JsonPropertyName("HasMusic"), Name("HasMusic")]
    public string HasMusic
    {
        get => _hasMusic!;
        set {_hasMusic= CheckStringFieldValue(value);}
    }

    private string? _usagePeriodWinter;
    [JsonPropertyName("UsagePeriodWinter"), Name("UsagePeriodWinter")]
    public string UsagePeriodWinter
    {
        get => _usagePeriodWinter!;
        set {_usagePeriodWinter= CheckStringFieldValue(value);}
    }

    private string? _dimensionsWinter;
    [JsonPropertyName("DimensionsWinter"), Name("DimensionsWinter")]
    public string DimensionsWinter
    {
        get => _dimensionsWinter!;
        set {_dimensionsWinter= CheckStringFieldValue(value);}
    }

    private string? _lighting;
    [JsonPropertyName("Lighting"), Name("Lighting")]
    public string Lighting
    {
        get => _lighting!;
        set {_lighting= CheckStringFieldValue(value);}
    }

    private string? _surfaceTypeWinter;
    [JsonPropertyName("SurfaceTypeWinter"), Name("SurfaceTypeWinter")]
    public string SurfaceTypeWinter
    {
        get => _surfaceTypeWinter!;
        set {_surfaceTypeWinter= CheckStringFieldValue(value);}
    }

    private int _seats;
    [JsonPropertyName("Seats"), Name("Seats")]
    public int Seats
    {
        get => _seats;
        set => _seats = value;
    }

    private string? _piad;
    [JsonPropertyName("Paid"), Name("Paid")]
    public string Paid
    {
        get => _piad!;
        set {_piad = CheckStringFieldValue(value);}
    }

    private string? _paidComments;
    [JsonPropertyName("PaidComments"), Name("PaidComments")]
    public string PaidComments
    {
        get => _paidComments!;
        set {_paidComments = CheckStringFieldValue(value);}
    }

    private string? _disabilityFriendly;
    [JsonPropertyName("DisabilityFriendly"), Name("DisabilityFriendly")]
    public string DisabilityFriendly
    {
        get => _disabilityFriendly!;
        set {_disabilityFriendly = CheckStringFieldValue(value);}
    }

    private string? _servicesWinter;
    [JsonPropertyName("ServicesWinter"), Name("ServicesWinter")]
    public string ServicesWinter
    {
        get => _servicesWinter!;
        set {_servicesWinter= CheckStringFieldValue(value);}
    }

    private string? _geoData;
    [JsonPropertyName("geoData"), Name("geoData")]
    public string GeoData
    {
        get => _geoData!;
        set {_geoData= CheckStringFieldValue(value);}
    }

    private string? _geoDataCenter;
    [JsonPropertyName("geodata_center"), Name("geodata_center")]
    public string GeoDataCenter
    {
        get => _geoDataCenter!;
        set {_geoDataCenter= CheckStringFieldValue(value);}
    }

    private string? _geoArea;
    [JsonPropertyName("geoarea"), Name("geoarea")]
    public string GeoArea
    {
        get => _geoArea!;
        set {_geoArea= CheckStringFieldValue(value);}
    }

    public IceHillData() {}

    [JsonConstructor]
    public IceHillData(long globalid, string objectname, string namewinter, string photowinter, string admarea, string district, string address, string email, string website,
    string helpphone, string helpPhoneExtension, string workingHoursWinter, string clarificationOfWorkingHoursWinter, string hasEquipmentRental, string equipmentRentalComments, string hasTechService,
    string techServiceComments, string hasDressingRoom, string hasEatery, string hasToilet, string hasWifi, string hasCashMachine, string hasFirstAidPost, string hasMusic,
    string usagePeriodWinter, string dimensionsWinter, string lighting, string surfaceTypeWinter, int seats, string paid, string paidComments, string disabilityFriendly, string servicesWinter,
    string geoData, string geoDataCenter, string geoArea)
    {
        GlobalId = globalid;
        ObjectName = objectname ?? "";
        NameWinter = namewinter ?? "";
        PhotoWinter = photowinter ?? "";
        AdmArea = admarea ?? "";
        District = district ?? "";
        Address = address ?? "";
        Email = email ?? "";
        WebSite = website ?? "";
        HelpPhone = helpphone ?? "";
        HelpPhoneExtension = helpPhoneExtension ?? "";
        WorkingHoursWinter = workingHoursWinter ?? "";
        ClarificationOfWorkingHoursWinter = clarificationOfWorkingHoursWinter ?? "";
        HasEquipmentRental = hasEquipmentRental ?? "";
        EquipmentRentalComments = equipmentRentalComments ?? "";
        HasTechService = hasTechService ?? "";
        TechServiceComments = techServiceComments ?? "";
        HasDressingRoom = hasDressingRoom ?? "";
        HasEatery = hasEatery ?? "";
        HasToilet = hasToilet ?? "";
        HasWifi = hasWifi ?? "";
        HasCashMachine = hasCashMachine ?? "";
        HasFirstAidPost = hasFirstAidPost ?? "";
        HasMusic = hasMusic ?? "";
        UsagePeriodWinter = usagePeriodWinter ?? "";
        DimensionsWinter = dimensionsWinter ?? "";
        Lighting = lighting ?? "";
        SurfaceTypeWinter = surfaceTypeWinter ?? "";
        Seats = seats;
        Paid = paid ?? "";
        PaidComments = paidComments ?? "";
        DisabilityFriendly = disabilityFriendly ?? "";
        ServicesWinter = servicesWinter ?? "";
        GeoData = geoData ?? "";
        GeoDataCenter = geoDataCenter ?? "";
        GeoArea = geoArea ?? "";
    }

    /// <summary>
    /// Method for checking a string for null
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private static string CheckStringFieldValue(string value)
    {
        if (value == null) {return "";}
        else {return value.Replace("\u0022", "");}
    }
}
