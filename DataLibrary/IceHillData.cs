using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace DataLibrary;

[Serializable]
public class IceHillData
{
    private readonly long _globalId;

    [JsonPropertyName("global_id")]
    public long GlobalId
    {
        get =>  _globalId;
        init => _globalId = value;
    }

    private readonly string? _objectName;
    public string ObjectName
    {
        get => _objectName!;
        init {_objectName = CheckStringFieldValue(value);}
    }

    private readonly string? _nameWinter;
    public string NameWinter
    {
        get => _nameWinter!;
        init {_nameWinter = CheckStringFieldValue(value);}
    }

    private readonly string? _photoWinter;
    public string PhotoWinter
    {
        get => _photoWinter!;
        init {_photoWinter = CheckStringFieldValue(value);}
    }

    private readonly string? _admArea;
    public string AdmArea
    {
        get => _admArea!;
        init {_admArea = CheckStringFieldValue(value);}
    }

    private readonly string? _district;
    public string District
    {
        get => _district!;
        init {_district = CheckStringFieldValue(value);}
    }

    private readonly string? _address;
    public string Address
    {
        get => _address!;
        init {_address = CheckStringFieldValue(value);}
    }

    private readonly string? _email;
    public string Email
    {
        get => _email!;
        init {_email = CheckStringFieldValue(value);}
    }

    private readonly string? _webSite;
    public string WebSite
    {
        get => _webSite!;
        init {_webSite = CheckStringFieldValue(value);}
    }

    private readonly string? _helpPhone;
    public string HelpPhone
    {
        get => _helpPhone!;
        init {_helpPhone = CheckStringFieldValue(value);}
    }

    private readonly string? _helpPhoneExtension;
    public string HelpPhoneExstencion
    {
        get => _helpPhoneExtension!;
        init {_helpPhoneExtension = CheckStringFieldValue(value);}
    }

    private readonly string? _workingHoursWinter;
    public string WorkingHoursWinter
    {
        get => _workingHoursWinter!;
        init {_workingHoursWinter = CheckStringFieldValue(value);}
    }

    private readonly string? _clarificationOfWorkingHoursWinter;
    public string ClarificationOfWorkingHoursWinter
    {
        get => _clarificationOfWorkingHoursWinter!;
        init {_clarificationOfWorkingHoursWinter = CheckStringFieldValue(value);}
    }

    private readonly string? _hasEquipmentRental;
    public string HasEquipmentRental
    {
        get => _hasEquipmentRental!;
        init {_hasEquipmentRental = CheckStringFieldValue(value);}
    }

    private readonly string? _equipmentRentalComments;
    public string EquipmentRentalComments
    {
        get => _equipmentRentalComments!;
        init {_equipmentRentalComments= CheckStringFieldValue(value);}
    }

    private readonly string? _hasTechService;
    public string HasTechService
    {
        get => _hasTechService!;
        init {_hasTechService= CheckStringFieldValue(value);}
    }

    private readonly string? _techServiceComments;
    public string TechServiceComments
    {
        get => _techServiceComments!;
        init {_techServiceComments= CheckStringFieldValue(value);}
    }

    private readonly string? _hasDressingRoom;
    public string HasDressingRoom
    {
        get => _hasDressingRoom!;
        init {_hasDressingRoom= CheckStringFieldValue(value);}
    }

    private readonly string? _hasEatery;
    public string HasEatery
    {
        get => _hasEatery!;
        init {_hasEatery= CheckStringFieldValue(value);}
    }

    private readonly string? _hasToilet;
    public string HasToilet
    {
        get => _hasToilet!;
        init {_hasToilet= CheckStringFieldValue(value);}
    }

    private readonly string? _hasWifi;
    public string HasWifi
    {
        get => _hasWifi!;
        init {_hasWifi= CheckStringFieldValue(value);}
    }

    private readonly string? _hasCashMachine;
    public string HasCashMachine
    {
        get => _hasCashMachine!;
        init {_hasCashMachine= CheckStringFieldValue(value);}
    }

    private readonly string? _hasFirstAidPost;
    public string HasFirstAidPost
    {
        get => _hasFirstAidPost!;
        init {_hasFirstAidPost= CheckStringFieldValue(value);}
    }

    private readonly string? _hasMusic;
    public string HasMusic
    {
        get => _hasMusic!;
        init {_hasMusic= CheckStringFieldValue(value);}
    }

    private readonly string? _usagePeriodWinter;
    public string UsagePeriodWinter
    {
        get => _usagePeriodWinter!;
        init {_usagePeriodWinter= CheckStringFieldValue(value);}
    }

    private readonly string? _dimensionsWinter;
    public string DimensionsWinter
    {
        get => _dimensionsWinter!;
        init {_dimensionsWinter= CheckStringFieldValue(value);}
    }

    private readonly string? _lighting;
    public string Lighting
    {
        get => _lighting!;
        init {_lighting= CheckStringFieldValue(value);}
    }

    private readonly string? _surfaceTypeWinter;
    public string SurfaceTypeWinter
    {
        get => _surfaceTypeWinter!;
        init {_surfaceTypeWinter= CheckStringFieldValue(value);}
    }

    private readonly int _seats;
    public int Seats
    {
        get => _seats;
        init => _seats = value;
    }

    private readonly string? _piad;
    public string Paid
    {
        get => _piad!;
        init {_piad = CheckStringFieldValue(value);}
    }

    private readonly string? _paidComments;
    public string PaidComments
    {
        get => _paidComments!;
        init {_paidComments = CheckStringFieldValue(value);}
    }

    private readonly string? _disabilityFriendly;
    public string DisabilityFriendly
    {
        get => _disabilityFriendly!;
        init {_disabilityFriendly = CheckStringFieldValue(value);}
    }

    private readonly string? _servicesWinter;
    public string ServicesWinter
    {
        get => _servicesWinter!;
        init {_servicesWinter= CheckStringFieldValue(value);}
    }

    private readonly string? _geoData;
    [JsonPropertyName("geoData")]
    public string GeoData
    {
        get => _geoData!;
        init {_geoData= CheckStringFieldValue(value);}
    }

    private readonly string? _geoDataCenter;
    [JsonPropertyName("geodata_center")]
    public string GeoDataCenter
    {
        get => _geoDataCenter!;
        init {_geoDataCenter= CheckStringFieldValue(value);}
    }

    private readonly string? _geoArea;
    [JsonPropertyName("geoarea")]
    public string GeoArea
    {
        get => _geoArea!;
        init {_geoArea= CheckStringFieldValue(value);}
    }

    [JsonConstructor]
    public IceHillData(long globalId = 0, string objectName = "", string nameWinter = "", string photoWinter = "", string admArea = "", string district = "", string address = "", string email = "", string webSite = "",
    string helpPhone = "", string helpPhoneExtension = "", string workingHoursWinter = "", string clarificationOfWorkingHoursWinter = "", string hasEquipmentRental = "", string equipmentRentalComments = "", string hasTechService = "",
    string techServiceComments = "", string hasDressingRoom = "", string hasEatery = "", string hasToilet = "", string hasWifi = "", string hasCashMachine = "", string hasFirstAidPost = "", string hasMusic = "",
    string usagePeriodWinter = "", string dimensionsWinter = "", string lighting = "", string surfaceTypeWinter = "", int seats = 0, string paid = "", string paidComments = "", string disabilityFriendly = "", string servicesWinter = "",
    string geoData = "", string geoDataCenter = "", string geoArea = "")
    {
        GlobalId = globalId;
        ObjectName = objectName;
        NameWinter = nameWinter;
        PhotoWinter = photoWinter;
        AdmArea = admArea;
        District = district;
        Address = address;
        Email = email;
        WebSite = webSite;
        HelpPhone = helpPhone;
        HelpPhoneExstencion = helpPhoneExtension;
        WorkingHoursWinter = workingHoursWinter;
        ClarificationOfWorkingHoursWinter = clarificationOfWorkingHoursWinter;
        HasEquipmentRental = hasEquipmentRental;
        EquipmentRentalComments = equipmentRentalComments;
        HasTechService = hasTechService;
        TechServiceComments = techServiceComments;
        HasDressingRoom = hasDressingRoom;
        HasEatery = hasEatery;
        HasToilet = hasToilet;
        HasWifi = hasWifi;
        HasCashMachine = hasCashMachine;
        HasFirstAidPost = hasFirstAidPost;
        HasMusic = hasMusic;
        UsagePeriodWinter = usagePeriodWinter;
        DimensionsWinter = dimensionsWinter;
        Lighting = lighting;
        SurfaceTypeWinter = surfaceTypeWinter;
        Seats = seats;
        Paid = paid;
        PaidComments = paidComments;
        DisabilityFriendly = disabilityFriendly;
        ServicesWinter = servicesWinter;
        GeoData = geoData;
        GeoDataCenter = geoDataCenter;
        GeoArea = geoArea;
    }

    public string ConvertToJSON(out bool isConvertCorrect)
    {
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            AllowTrailingCommas = true
        };

        try
        {
            string jsonData = JsonSerializer.Serialize<IceHillData>(this, options);
            isConvertCorrect = true;
            return jsonData;
        }
        catch (Exception ex)
        {   
            Console.WriteLine($"{ex.Message}\nException occured while trying to Serialize IceHillData object - {this.GlobalId}");
            isConvertCorrect = false;
            return string.Empty;
        }
    }

    private static string CheckStringFieldValue(string value)
    {
        if (value == null) {return "";}
        else {return value;}
    }

}
